# Architecture Notes

This document summarizes key design decisions:
- Controllers remain thin and delegate business operations to services.
- Services depend on interfaces and EF Core context through dependency injection.
- DTOs and ViewModels isolate transport and UI concerns from domain models.
