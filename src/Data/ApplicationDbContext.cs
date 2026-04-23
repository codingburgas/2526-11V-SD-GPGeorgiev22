using Microsoft.EntityFrameworkCore;
using WarehouseManagementSchool.Models;

namespace WarehouseManagementSchool.Data;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Answer> Answers => Set<Answer>();
    public DbSet<TestResult> TestResults => Set<TestResult>();
    public DbSet<TestResultItem> TestResultItems => Set<TestResultItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<Lesson>()
            .HasOne(l => l.CategoryNavigation)
            .WithMany(c => c.Lessons)
            .HasForeignKey(l => l.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Question>()
            .HasOne(q => q.CategoryNavigation)
            .WithMany(c => c.Questions)
            .HasForeignKey(q => q.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Answer>()
            .HasOne(a => a.QuestionNavigation)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TestResultItem>()
            .HasOne(ri => ri.TestResultNavigation)
            .WithMany(r => r.TestResultItems)
            .HasForeignKey(ri => ri.TestResultId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TestResultItem>()
            .HasOne(ri => ri.QuestionNavigation)
            .WithMany()
            .HasForeignKey(ri => ri.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TestResultItem>()
            .HasIndex(ri => new { ri.TestResultId, ri.QuestionId })
            .IsUnique();

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Warehouse Fundamentals" },
            new Category { Id = 2, Name = "Systems and Data" },
            new Category { Id = 3, Name = "Design and Experience" },
            new Category { Id = 4, Name = "Inventory Logic" },
            new Category { Id = 5, Name = "Database Relations" },
            new Category { Id = 6, Name = "UI Design" });

        modelBuilder.Entity<Lesson>().HasData(
            new Lesson
            {
                Id = 1,
                Title = "Inventory Lifecycle",
                Content = "Understand inventory flow from procurement to storage, picking, shipping, and returns.",
                Category = "Warehouse Fundamentals",
                CategoryId = 1
            },
            new Lesson
            {
                Id = 2,
                Title = "Warehouse Operations",
                Content = "Learn receiving, put-away strategies, cycle counting, and safety-driven daily operations.",
                Category = "Warehouse Fundamentals",
                CategoryId = 1
            },
            new Lesson
            {
                Id = 3,
                Title = "Logistics Basics",
                Content = "Explore transportation planning, lead times, service levels, and fulfillment constraints.",
                Category = "Warehouse Fundamentals",
                CategoryId = 1
            },
            new Lesson
            {
                Id = 4,
                Title = "Database Relations",
                Content = "See how relational modeling connects lessons, categories, and users with proper integrity.",
                Category = "Systems and Data",
                CategoryId = 2
            },
            new Lesson
            {
                Id = 5,
                Title = "UI Design Basics",
                Content = "Review layout composition, readability, and navigation patterns for operational software.",
                Category = "Design and Experience",
                CategoryId = 3
            });

        var questionSeedDefinitions = BuildQuestionSeedDefinitions();
        var seededQuestions = BuildQuestionSeeds(questionSeedDefinitions);
        var seededAnswers = BuildAnswerSeeds(questionSeedDefinitions);

        modelBuilder.Entity<Question>().HasData(seededQuestions);
        modelBuilder.Entity<Answer>().HasData(seededAnswers);
    }

    private static List<Question> BuildQuestionSeeds(
        IReadOnlyList<(string Text, int CategoryId, DifficultyLevel Difficulty, string[] Answers, int CorrectIndex)> definitions)
    {
        var questions = new List<Question>(definitions.Count);

        for (var i = 0; i < definitions.Count; i++)
        {
            var definition = definitions[i];
            questions.Add(new Question
            {
                Id = i + 1,
                Text = definition.Text,
                CategoryId = definition.CategoryId,
                Difficulty = definition.Difficulty,
                Points = GetPointsByDifficulty(definition.Difficulty)
            });
        }

        return questions;
    }

    private static List<Answer> BuildAnswerSeeds(
        IReadOnlyList<(string Text, int CategoryId, DifficultyLevel Difficulty, string[] Answers, int CorrectIndex)> definitions)
    {
        var answers = new List<Answer>(definitions.Count * 4);
        var answerId = 1;

        for (var i = 0; i < definitions.Count; i++)
        {
            var definition = definitions[i];
            var questionId = i + 1;

            for (var optionIndex = 0; optionIndex < definition.Answers.Length; optionIndex++)
            {
                answers.Add(new Answer
                {
                    Id = answerId++,
                    QuestionId = questionId,
                    Text = definition.Answers[optionIndex],
                    IsCorrect = optionIndex == definition.CorrectIndex
                });
            }
        }

        return answers;
    }

    private static int GetPointsByDifficulty(DifficultyLevel difficulty)
    {
        return difficulty switch
        {
            DifficultyLevel.Easy => 1,
            DifficultyLevel.Medium => 2,
            DifficultyLevel.Hard => 3,
            _ => 1
        };
    }

    private static List<(string Text, int CategoryId, DifficultyLevel Difficulty, string[] Answers, int CorrectIndex)> BuildQuestionSeedDefinitions()
    {
        return
        [
            ("What inventory method consumes older stock first?", 4, DifficultyLevel.Easy, ["FIFO", "LIFO", "ABC", "JIT"], 0),
            ("What does safety stock primarily protect against?", 4, DifficultyLevel.Easy, ["Demand and lead-time variability", "Low warehouse rent", "Barcode failure", "Packaging damage"], 0),
            ("What metric shows how often inventory is sold and replaced?", 4, DifficultyLevel.Easy, ["Inventory turnover", "Fill color", "Record lock", "Index scan"], 0),
            ("Which operation places goods into storage locations after receiving?", 4, DifficultyLevel.Easy, ["Put-away", "Picking", "Packing", "Kitting"], 0),
            ("Cycle counting is mainly used to:", 4, DifficultyLevel.Medium, ["Improve stock accuracy continuously", "Eliminate purchase orders", "Replace receiving", "Skip audits forever"], 0),
            ("Which replenishment trigger starts a new order at a threshold?", 4, DifficultyLevel.Medium, ["Reorder point", "Gross margin", "Dock door index", "Heat map"], 0),
            ("ABC analysis classifies items by:", 4, DifficultyLevel.Medium, ["Value and consumption impact", "Box color", "Shelf height", "Supplier logo"], 0),
            ("Which formula best estimates available-to-promise?", 4, DifficultyLevel.Hard, ["On-hand + scheduled receipts - allocated demand", "On-hand - unit cost", "Lead time x margin", "Orders / warehouse area"], 0),
            ("A negative inventory balance usually indicates:", 4, DifficultyLevel.Hard, ["Transaction timing or process errors", "Excellent demand forecast", "Too much safety stock", "Perfect cycle counting"], 0),
            ("What is the strongest reason to standardize location naming?", 4, DifficultyLevel.Hard, ["Reduce picking ambiguity and errors", "Increase SKU price", "Avoid user accounts", "Disable scanners"], 0),

            ("What key type uniquely identifies each row in a table?", 5, DifficultyLevel.Easy, ["Primary key", "Foreign key", "Alias key", "Display key"], 0),
            ("A foreign key is used to:", 5, DifficultyLevel.Easy, ["Link related tables", "Encrypt passwords", "Render HTML", "Create backups"], 0),
            ("What relationship means one category has many lessons?", 5, DifficultyLevel.Easy, ["One-to-many", "Many-to-many", "One-to-one", "Zero-to-one"], 0),
            ("Normalization primarily helps reduce:", 5, DifficultyLevel.Easy, ["Data redundancy", "Network speed", "Color contrast", "Thread count"], 0),
            ("What does a unique index guarantee?", 5, DifficultyLevel.Medium, ["No duplicate values in indexed columns", "Faster internet", "Larger memory", "Lower CPU temperature"], 0),
            ("Which join returns only matching rows from both tables?", 5, DifficultyLevel.Medium, ["INNER JOIN", "LEFT JOIN", "RIGHT JOIN", "FULL JOIN"], 0),
            ("Why use `AsNoTracking` for read-only queries?", 5, DifficultyLevel.Medium, ["It reduces change-tracking overhead", "It creates indexes automatically", "It enforces transactions", "It renames columns"], 0),
            ("Referential integrity ensures that:", 5, DifficultyLevel.Hard, ["Foreign keys reference valid parent rows", "All rows are sorted", "Every table has ten columns", "Indexes are always clustered"], 0),
            ("Which delete behavior prevents removing a category with dependent lessons?", 5, DifficultyLevel.Hard, ["Restrict", "Cascade", "SetNull", "NoAction in memory"], 0),
            ("A composite unique index on (A, B) allows:", 5, DifficultyLevel.Hard, ["Repeated A values when B differs", "No rows in the table", "Only numeric columns", "Only one row per table"], 0),

            ("What improves readability most for long forms?", 6, DifficultyLevel.Easy, ["Clear labels and spacing", "Random font sizes", "Hidden submit button", "All caps paragraphs"], 0),
            ("A good primary action button should be:", 6, DifficultyLevel.Easy, ["Visually distinct", "Invisible until hover", "Smaller than links", "Unlabeled"], 0),
            ("Which technique helps mobile layout adapt correctly?", 6, DifficultyLevel.Easy, ["Responsive grid and breakpoints", "Fixed 1920px width", "Absolute positioning only", "Disable viewport meta"], 0),
            ("Validation messages should appear:", 6, DifficultyLevel.Easy, ["Near the related field", "Only in server logs", "Inside CSS files", "In browser title"], 0),
            ("Why should forms include anti-forgery tokens in MVC?", 6, DifficultyLevel.Medium, ["To prevent CSRF attacks", "To compress HTML", "To speed up SQL joins", "To hide nav links"], 0),
            ("Which pattern improves test-taking UX for many questions?", 6, DifficultyLevel.Medium, ["Card layout with grouped answers", "One giant paragraph", "No question numbers", "Randomly moving controls"], 0),
            ("What is the best fallback when no data is available?", 6, DifficultyLevel.Medium, ["Show an informative empty state", "Throw raw exception", "Render blank page", "Disable routing"], 0),
            ("A semantic heading hierarchy improves:", 6, DifficultyLevel.Hard, ["Accessibility and scanability", "Database indexing", "File compression", "Cookie expiration"], 0),
            ("Using both client and server validation is important because:", 6, DifficultyLevel.Hard, ["Client-side checks can be bypassed", "Server-side cannot parse strings", "Browsers ignore CSS", "HTTP forbids validation"], 0),
            ("Which UX decision best reduces accidental wrong submissions?", 6, DifficultyLevel.Hard, ["Require explicit choice per question and confirm submit", "Auto-submit on first click", "Hide selected answers", "Remove question numbering"], 0)
        ];
    }
}
