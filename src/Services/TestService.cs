using Microsoft.EntityFrameworkCore;
using WarehouseManagementSchool.Data;
using WarehouseManagementSchool.DTOs;
using WarehouseManagementSchool.Interfaces;
using WarehouseManagementSchool.Models;

namespace WarehouseManagementSchool.Services;

public sealed class TestService : ITestService
{
    private const int DefaultQuestionCount = 20;
    private readonly ApplicationDbContext _context;
    private readonly IGradingService _gradingService;

    public TestService(ApplicationDbContext context, IGradingService gradingService)
    {
        _context = context;
        _gradingService = gradingService;
    }

    public async Task<GeneratedTestDto> GenerateTest()
    {
        /*
         * Mathematical documentation:
         * If total questions = 30 and selected questions = 20,
         * then the number of unique test combinations is:
         * C(30,20) = 30! / (20! * 10!).
         * This represents the number of unique test variations.
         */

        var allQuestions = await _context.Questions
            .AsNoTracking()
            .Include(q => q.CategoryNavigation)
            .Include(q => q.Answers)
            .Where(q => q.Answers.Count == 4)
            .ToListAsync();

        if (allQuestions.Count == 0)
        {
            return new GeneratedTestDto();
        }

        var selectedQuestions = new List<Question>();

        // Ensure a mix by taking one random question per category first.
        var groupedByCategory = allQuestions
            .GroupBy(q => q.CategoryId)
            .Select(group => group.OrderBy(_ => Random.Shared.Next()).First())
            .ToList();

        selectedQuestions.AddRange(groupedByCategory);

        var maxCount = Math.Min(DefaultQuestionCount, allQuestions.Count);
        var remaining = allQuestions
            .Where(q => selectedQuestions.All(selected => selected.Id != q.Id))
            .OrderBy(_ => Random.Shared.Next())
            .Take(maxCount - selectedQuestions.Count)
            .ToList();

        selectedQuestions.AddRange(remaining);

        var randomized = selectedQuestions
            .OrderBy(_ => Random.Shared.Next())
            .ToList();

        var mappedQuestions = randomized
            .Select(question => new GeneratedQuestionDto
            {
                Id = question.Id,
                Text = question.Text,
                CategoryName = question.CategoryNavigation?.Name ?? "Uncategorized",
                Difficulty = question.Difficulty.ToString(),
                Points = question.Points,
                Answers = question.Answers
                    .OrderBy(answer => answer.Id)
                    .Select(answer => new GeneratedAnswerDto
                    {
                        Id = answer.Id,
                        Text = answer.Text
                    })
                    .ToList()
            })
            .ToList();

        return new GeneratedTestDto
        {
            Questions = mappedQuestions,
            TotalPoints = mappedQuestions.Sum(question => question.Points)
        };
    }

    public async Task<TestEvaluationDto> EvaluateTest(TestSubmissionDto submission)
    {
        if (submission is null)
        {
            throw new ArgumentNullException(nameof(submission));
        }

        var distinctAnswers = submission.Answers
            .Where(answer => answer.QuestionId > 0 && answer.AnswerId > 0)
            .GroupBy(answer => answer.QuestionId)
            .Select(group => group.First())
            .ToList();

        var questionIds = distinctAnswers.Select(answer => answer.QuestionId).ToList();

        var questions = await _context.Questions
            .Include(question => question.Answers)
            .Where(question => questionIds.Contains(question.Id))
            .ToListAsync();

        var score = 0;
        var maxScore = 0;
        var resultItems = new List<TestResultItem>();

        foreach (var question in questions)
        {
            maxScore += question.Points;

            var selected = distinctAnswers.FirstOrDefault(answer => answer.QuestionId == question.Id);
            var isCorrect = selected is not null &&
                            question.Answers.Any(answer => answer.Id == selected.AnswerId && answer.IsCorrect);

            if (isCorrect)
            {
                score += question.Points;
            }

            resultItems.Add(new TestResultItem
            {
                QuestionId = question.Id,
                IsCorrect = isCorrect
            });
        }

        var percentage = maxScore == 0 ? 0 : (double)score / maxScore * 100;
        var grade = _gradingService.CalculateGrade(percentage);

        var testResult = new TestResult
        {
            Score = score,
            MaximumScore = maxScore,
            Grade = grade,
            DateTaken = DateTime.UtcNow,
            TestResultItems = resultItems
        };

        _context.TestResults.Add(testResult);
        await _context.SaveChangesAsync();

        return new TestEvaluationDto
        {
            TestResultId = testResult.Id,
            Score = score,
            MaximumScore = maxScore,
            Grade = grade,
            Percentage = Math.Round(percentage, 2),
            AnsweredQuestions = questions.Count,
            DateTaken = testResult.DateTaken
        };
    }

    public async Task<TestEvaluationDto?> GetResult(int id)
    {
        if (id <= 0)
        {
            return null;
        }

        var result = await _context.TestResults
            .AsNoTracking()
            .FirstOrDefaultAsync(entry => entry.Id == id);

        if (result is null)
        {
            return null;
        }

        var percentage = result.MaximumScore == 0
            ? 0
            : Math.Round((double)result.Score / result.MaximumScore * 100, 2);

        var answeredQuestions = await _context.TestResultItems
            .AsNoTracking()
            .CountAsync(item => item.TestResultId == id);

        return new TestEvaluationDto
        {
            TestResultId = result.Id,
            Score = result.Score,
            MaximumScore = result.MaximumScore,
            Grade = result.Grade,
            Percentage = percentage,
            AnsweredQuestions = answeredQuestions,
            DateTaken = result.DateTaken
        };
    }
}
