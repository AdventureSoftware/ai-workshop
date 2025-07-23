# AI Pair-Programming Workshop - .NET Track

This .NET track demonstrates AI pair-programming techniques using modern C# patterns with GitHub Copilot.

## Prerequisites

- **.NET 8 SDK** or later
- **Visual Studio 2022** or **VS Code** with C# extension
- **GitHub Copilot extension** installed
- Basic knowledge of C# and .NET ecosystem

## Quick Start

1. **Restore packages:**
   ```bash
   dotnet restore
   ```

2. **Build solution:**
   ```bash
   dotnet build
   ```

3. **Run tests:**
   ```bash
   dotnet test
   ```

4. **Open in IDE** and ensure Copilot is active

## Project Structure

```
dotnet/
├── AIPairProgramming.sln          # Solution file
├── src/
│   ├── Examples/                  # Workshop demonstration code
│   │   ├── BasicPrompting.cs      # FCE pattern examples
│   │   ├── ContextInjection.cs    # Multi-file context strategies
│   │   └── VerificationExamples.cs # R.E.D. checklist demos
│   └── Exercises/                 # Hands-on practice code
│       ├── PaymentProcessor.cs    # Exercise 1: FCE patterns
│       ├── UserService.cs         # Exercise 2: Context injection
│       └── ShippingCalculator.cs  # Exercise 3: Test-first development
├── tests/
│   ├── ExampleTests.cs           # Tests for demonstration code
│   └── ExerciseTests.cs          # Tests for exercise validation
└── Shared/
    ├── Models/                   # Domain models and DTOs
    ├── Services/                 # Service interfaces
    └── Validation/               # Validation helpers
```

## .NET-Specific AI Techniques

### 1. Leverage C# 12 Features
```csharp
// Primary constructors give Copilot clear intent
public class PaymentService(IPaymentGateway gateway, ILogger<PaymentService> logger)
{
    // Copilot understands DI patterns from constructor
}

// Collection expressions provide clear data structures
private static readonly string[] SupportedCurrencies = ["USD", "EUR", "GBP"];
```

### 2. Nullable Reference Types for Safety
```csharp
// Copilot respects nullable annotations for better suggestions
public async Task<Result<PaymentConfirmation?>> ProcessPaymentAsync(
    PaymentRequest request, 
    CancellationToken cancellationToken = default)
```

### 3. Record Types for Data Modeling
```csharp
// Records provide immutable data contracts that Copilot understands well
public record PaymentRequest(decimal Amount, string Currency, string CustomerId)
{
    public string? Description { get; init; }
}

public record Result<T>(bool IsSuccess, T? Data = default, string? Error = null);
```

### 4. Pattern Matching for Business Logic
```csharp
// Modern C# patterns help Copilot generate cleaner code
public decimal CalculateFees(PaymentRequest request) => request switch
{
    { Amount: < 5 } => throw new ArgumentException("Minimum amount is $5"),
    { Amount: > 10000 } => CalculateLargeTransactionFee(request),
    { Currency: not "USD" } => CalculateInternationalFee(request),
    _ => CalculateStandardFee(request)
};
```

## Workshop Exercises

### Exercise 1: FCE Pattern with Payment Processing (20 min)
**File**: `src/Exercises/PaymentProcessor.cs`

Practice the Function-Constraints-Examples pattern:
- **Function**: Process credit card payments
- **Constraints**: Amount limits, currency restrictions, validation rules
- **Examples**: Concrete input/output scenarios

### Exercise 2: Context Injection with User Management (25 min)
**File**: `src/Exercises/UserService.cs`

Learn multi-file context strategies:
- Open related interfaces and models
- Use XML documentation for AI context
- Reference dependency injection patterns

### Exercise 3: Test-First Development (20 min)
**File**: `src/Exercises/ShippingCalculator.cs`

Write comprehensive tests first, then implement:
- Use xUnit and FluentAssertions
- Cover edge cases in test names
- Let Copilot implement from test specifications

### Exercise 4: Advanced Patterns (25 min)
Practice dependency injection, async patterns, and error handling with modern .NET approaches.

## AI Context Files

### StyleGuide.md
Defines .NET-specific coding standards:
- Naming conventions (PascalCase, camelCase rules)
- Async/await patterns
- Exception handling strategies
- Dependency injection patterns

### ApiSchema.md
Business domain definitions:
- Payment processing rules
- User management workflows
- Shipping calculation logic
- Validation requirements

### .editorconfig
Code formatting rules that Copilot respects:
```ini
[*.cs]
indent_style = space
indent_size = 4
end_of_line = crlf
```

## Development Commands

```bash
# Build and test
dotnet build                    # Build solution
dotnet test                     # Run all tests
dotnet test --logger:trx        # Generate test results

# Code analysis  
dotnet format                   # Format code
dotnet build --verbosity normal # Check warnings

# Package management
dotnet add package <PackageName> # Add NuGet package
dotnet remove package <PackageName> # Remove package
```

## AI Prompting Tips for .NET

### 1. Reference .NET Conventions
```csharp
// ✅ Good prompt context
/// <summary>
/// Validates user registration following .NET naming conventions
/// Uses FluentValidation for rule definitions
/// Returns Result<T> pattern for error handling
/// </summary>
public async Task<Result<User>> ValidateUserRegistrationAsync(UserRegistrationRequest request)
```

### 2. Specify Framework Features
```csharp
// ✅ Be explicit about .NET features to use
// Function: Create JWT token using System.IdentityModel.Tokens.Jwt
// Constraints: 24-hour expiry, include user claims, use symmetric security key
// Examples: GenerateToken(user) → "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9..."
```

### 3. Include Dependency Injection Context
```csharp
// ✅ Show DI patterns in prompts
// Function: Payment service with proper DI constructor
// Constraints: IPaymentGateway dependency, ILogger for monitoring, async methods
// Examples: PaymentService(gateway, logger) → service with injected dependencies
```

## Common .NET AI Patterns

### Result Pattern Implementation
```csharp
public record Result<T>(bool IsSuccess, T? Data, string? Error)
{
    public static Result<T> Success(T data) => new(true, data, null);
    public static Result<T> Failure(string error) => new(false, default, error);
}
```

### Async Service Pattern
```csharp
public interface IPaymentService
{
    Task<Result<PaymentConfirmation>> ProcessPaymentAsync(
        PaymentRequest request, 
        CancellationToken cancellationToken = default);
}
```

### Validation Pattern with FluentValidation
```csharp
public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
{
    public PaymentRequestValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be positive");
        RuleFor(x => x.Currency).Must(BeValidCurrency).WithMessage("Invalid currency");
    }
}
```

## Success Metrics

Track your .NET AI pair-programming progress:
- ⚡ **Faster API development** with proper dependency injection
- 🔒 **Better null safety** with nullable reference types
- 🧪 **Improved test coverage** with AI-generated test cases
- 📋 **Consistent patterns** across team members
- 🚀 **Modern C# usage** (records, pattern matching, primary constructors)

## Troubleshooting

**Common Issues:**
- **Copilot not suggesting**: Ensure C# extension and Copilot are active
- **Build errors**: Check .NET 8 SDK installation with `dotnet --version`
- **Test failures**: Verify all packages restored with `dotnet restore`
- **Nullable warnings**: Enable nullable reference types in project file

Ready to master AI pair-programming with modern .NET! 🚀

---

**Next Steps**: Complete Exercise 1 in `src/Exercises/PaymentProcessor.cs`