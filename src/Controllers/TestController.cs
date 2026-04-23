using Microsoft.AspNetCore.Mvc;
using WarehouseManagementSchool.DTOs;
using WarehouseManagementSchool.Interfaces;
using WarehouseManagementSchool.ViewModels;

namespace WarehouseManagementSchool.Controllers;

public sealed class TestController : Controller
{
    private readonly ITestService _testService;

    public TestController(ITestService testService)
    {
        _testService = testService;
    }

    [HttpGet]
    public async Task<IActionResult> Start()
    {
        var generatedTest = await _testService.GenerateTest();

        if (generatedTest.Questions.Count == 0)
        {
            return View(new TestStartViewModel
            {
                ErrorMessage = "No questions are currently available. Please seed the database and try again."
            });
        }

        var model = new TestStartViewModel
        {
            Questions = generatedTest.Questions
                .Select(question => new TestQuestionViewModel
                {
                    Id = question.Id,
                    Text = question.Text,
                    CategoryName = question.CategoryName,
                    Difficulty = question.Difficulty,
                    Points = question.Points,
                    Answers = question.Answers
                        .Select(answer => new TestAnswerViewModel
                        {
                            Id = answer.Id,
                            Text = answer.Text
                        })
                        .ToList()
                })
                .ToList(),
            TotalPoints = generatedTest.TotalPoints,
            ErrorMessage = TempData["TestError"] as string
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit([FromForm] List<int> questionIds, [FromForm] Dictionary<int, int> answers)
    {
        if (questionIds.Count == 0)
        {
            TempData["TestError"] = "Invalid test submission. Please start a new test.";
            return RedirectToAction(nameof(Start));
        }

        var distinctQuestionIds = questionIds
            .Where(id => id > 0)
            .Distinct()
            .ToList();

        var isValidSubmission = distinctQuestionIds.Count == questionIds.Count &&
                                distinctQuestionIds.All(questionId => answers.TryGetValue(questionId, out var answerId) && answerId > 0);

        if (!isValidSubmission)
        {
            TempData["TestError"] = "Please answer every question before submitting the test.";
            return RedirectToAction(nameof(Start));
        }

        var submission = new TestSubmissionDto
        {
            Answers = distinctQuestionIds
                .Select(questionId => new SubmittedAnswerDto
                {
                    QuestionId = questionId,
                    AnswerId = answers[questionId]
                })
                .ToList()
        };

        var evaluation = await _testService.EvaluateTest(submission);
        return RedirectToAction(nameof(Result), new { id = evaluation.TestResultId });
    }

    [HttpGet]
    public async Task<IActionResult> Result(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        var result = await _testService.GetResult(id);
        if (result is null)
        {
            return NotFound();
        }

        var model = new TestResultViewModel
        {
            Id = result.TestResultId,
            Score = result.Score,
            MaximumScore = result.MaximumScore,
            Grade = result.Grade,
            Percentage = result.Percentage,
            AnsweredQuestions = result.AnsweredQuestions,
            DateTaken = result.DateTaken
        };

        return View(model);
    }
}
