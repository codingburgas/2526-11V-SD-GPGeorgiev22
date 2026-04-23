# Warehouse Management e-Learning System

## Overview

Warehouse Management e-Learning System is a web-based educational platform built with ASP.NET Core MVC.
It functions as an electronic school focused on warehouse management and logistics.

The system provides:

- Learning materials (lessons)
- Automated test system
- Performance evaluation
- Statistics and analytics

---

## Project Goals

- Teach core concepts of warehouse management
- Provide structured educational content
- Automatically assess student knowledge
- Analyze results and performance

---

## Technologies

- ASP.NET Core MVC
- C#
- Entity Framework Core
- SQLite database
- Bootstrap 5

---

## Architecture

The project follows a layered architecture:

- Controllers: Handle HTTP requests
- Services: Business logic
- Models: Data layer
- Views: UI layer
- Interfaces: Contracts for dependency inversion

---

## Project Structure

```bash
/src
  /Controllers
  /Models
  /Services
  /Interfaces
  /Data
  /DTOs
  /ViewModels
  /Helpers
  /Views
    /Shared
    /Lessons
    /Test
    /Statistics
  /wwwroot
    /css
    /js
  /Tests
```

---

## Features

### Learning Module

- Lessons grouped by categories
- Detailed lesson pages
- Easy navigation

---

### Test System

- 30+ questions stored in database
- 3 categories:
  - Inventory Logic
  - Database Relations
  - UI Design
- Each generated test includes:
  - 20 randomly selected questions
  - No duplicate questions
  - Mixed categories
  - Multiple choice answers
  - Difficulty-based scoring

---

### Grading System

Uses the Bulgarian 6-point scale:

| Percentage | Grade |
| ---------- | ----- |
| 90-100%    | 6     |
| 75-89%     | 5     |
| 60-74%     | 4     |
| 45-59%     | 3     |
| <45%       | 2     |

Displayed after submission:

- Score (points)
- Percentage
- Final grade
- Feedback message

---

### Statistics

- Highest score
- Lowest score
- Average score
- Category success rate
- Table view and chart view

---

## Mathematical Model

The number of possible test combinations is:

`C(30,20) = 30! / (20! * 10!)`

This represents the number of possible unique test combinations.

---

## How to Run

1. Clone the repository:

```bash
git clone https://github.com/codingburgas/2526-11V-SD-GPGeorgiev22.git
```

2. Navigate to project root:

```bash
cd 2526-11V-SD-GPGeorgiev22
```

3. Navigate to source folder:

```bash
cd src
```

4. Restore and run:

```bash
dotnet restore
dotnet run
```

5. Open in browser:

- https://localhost:7168
- or http://localhost:5168

---

## Validation and Security

- Input validation using Data Annotations
- Anti-forgery protection on form posts
- Safe EF Core operations via services
- Error handling for invalid states and edge cases

---

## Evaluation Criteria Coverage

- Code quality (OOP, structure, naming)
- Functional UI and navigation
- Test system implementation
- Statistics module
- GitHub usage and documentation

---

## Future Improvements

- Role-based authentication and authorization
- Admin panel for managing questions and lessons
- Advanced analytics dashboards
- Timed tests and question review mode

---

## Author

Student project for ASP.NET Core course.

Topic: Warehouse Management e-Learning System

---

## Final Result

A fully functional educational web application ready for demonstration and evaluation.
