# C# Style Guide (.NET 8)

## General Principles

- **Clarity over cleverness** - Code should be self-documenting
- **Modern C# features** - Leverage C# 12 and .NET 8 capabilities  
- **Consistency** - Follow established patterns throughout the codebase
- **Type safety** - Leverage nullable reference types and strong typing
- **Performance** - Write efficient code using modern .NET patterns

## Naming Conventions

### Classes and Methods
- Use **PascalCase** for class names, method names, and properties
- Use **verbs** for methods that clearly describe the action
- Include context about what the method returns

```csharp
// ✅ Good
public class PaymentProcessor
{
    public async Task<ValidationResult<PaymentResult>> ProcessPaymentAsync(PaymentRequest request);
    public decimal CalculateShippingCost(decimal weight, decimal distance);
    public ValidationResult<User> ValidateUserRegistration(UserRegistration request);
}

// ❌ Avoid
public class processor
{
    public async Task<object> process(object input);
    public decimal calc(decimal w, decimal d);
    public bool validate(object input);
}
```

### Types and Interfaces
- Use **PascalCase** for types, interfaces, and enums
- Interface names should start with 'I'
- Use descriptive names that indicate purpose

```csharp
// ✅ Good
public interface IPaymentProcessor
{
    Task<ValidationResult<PaymentResult>> ProcessAsync(PaymentRequest request);
}

public record UserRegistration(string Email, string Name, string Password);

public enum PaymentStatus 
{ 
    Pending, 
    Completed, 
    Failed, 
    Refunded 
}

// ❌ Avoid
public interface PaymentProcessor { } // Missing 'I' prefix
public enum Status { P, C, F, R } // Unclear abbreviations
```

### Variables and Parameters
- Use **camelCase** for local variables, fields, and parameters
- Use descriptive names, avoid abbreviations

```csharp
// ✅ Good
public async Task<ValidationResult<PaymentResult>> ProcessPaymentAsync(
    PaymentRequest paymentRequest, 
    CancellationToken cancellationToken = default)
{
    var validationResult = await ValidateRequestAsync(paymentRequest);
    var processingFees = CalculateProcessingFees(paymentRequest.Amount);
}

// ❌ Avoid
public async Task<object> ProcessAsync(object req, CancellationToken ct = default)
{
    var val = await ValidateAsync(req);
    var fees = CalcFees(((PaymentRequest)req).Amount);
}
```

### Constants
- Use **PascalCase** for public constants
- Use **SCREAMING_SNAKE_CASE** for private constants when grouping is needed

```csharp
// ✅ Good
public class PaymentLimits
{
    public const decimal MinimumAmountCents = 500;
    public const decimal MaximumAmountCents = 1_000_000;
    
    private const int MAX_RETRY_ATTEMPTS = 3;
    private const int RATE_LIMIT_WINDOW_MINUTES = 15;
}
```

## Modern C# Patterns

### Primary Constructors (C# 12)
```csharp
// ✅ Preferred for dependency injection
public class PaymentService(
    IPaymentGateway paymentGateway, 
    ILogger<PaymentService> logger)
{
    public async Task<ValidationResult<PaymentResult>> ProcessAsync(PaymentRequest request)
    {
        logger.LogInformation("Processing payment for customer {CustomerId}", request.CustomerId);
        return await paymentGateway.ProcessAsync(request);
    }
}
```

### Record Types
```csharp
// ✅ Good for immutable data transfer objects
public record PaymentRequest(
    decimal Amount,
    string Currency,
    string CustomerId)
{
    public string? Description { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}

public record ValidationResult<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public string? Error { get; init; }
    public List<ValidationError>? Errors { get; init; }
    
    public static ValidationResult<T> SuccessResult(T data) => new() { Success = true, Data = data };
    public static ValidationResult<T> FailureResult(string error) => new() { Success = false, Error = error };
}
```

### Collection Expressions (C# 12)
```csharp
// ✅ Modern collection initialization
private static readonly string[] SupportedCurrencies = ["USD", "EUR", "GBP"];
private static readonly HashSet<string> AllowedFileExtensions = [".jpg", ".png", ".pdf", ".docx"];

// Method with collection parameter
public ValidationResult<List<string>> ProcessItems(string[] items)
{
    var validItems = items.Where(item => !string.IsNullOrWhiteSpace(item)).ToList();
    return ValidationResult<List<string>>.SuccessResult(validItems);
}
```

### Pattern Matching
```csharp
// ✅ Use pattern matching for business logic
public decimal CalculateProcessingFees(PaymentRequest request) => request switch
{
    { Amount: < 500 } => throw new ArgumentException("Amount below minimum"),
    { Amount: > 1_000_000 } => CalculateLargeTransactionFee(request),
    { Currency: not "USD" } => CalculateInternationalFee(request),
    _ => CalculateStandardFee(request)
};

// ✅ Pattern matching in validation
public ValidationResult<PaymentRequest> ValidatePayment(PaymentRequest request) => request switch
{
    { Amount: <= 0 } => ValidationResult<PaymentRequest>.FailureResult("Amount must be positive"),
    { CustomerId: null or "" } => ValidationResult<PaymentRequest>.FailureResult("Customer ID required"),
    { Currency: not "USD" } => ValidationResult<PaymentRequest>.FailureResult("Only USD supported"),
    _ => ValidationResult<PaymentRequest>.SuccessResult(request)
};
```

## Error Handling Patterns

### ValidationResult Pattern
```csharp
// ✅ Preferred over exceptions for business logic errors
public record ValidationResult<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public string? Error { get; init; }
    public List<ValidationError>? Errors { get; init; }
}

// Usage example
public async Task<ValidationResult<User>> CreateUserAsync(UserRegistration registration)
{
    // Validate input
    var emailValidation = ValidateEmail(registration.Email);
    if (!emailValidation.Success)
        return ValidationResult<User>.FailureResult(emailValidation.Error!);
    
    // Business logic
    try 
    {
        var user = new User(
            Id: Guid.NewGuid().ToString(),
            Email: registration.Email,
            Name: registration.Name,
            Role: UserRole.User,
            CreatedAt: DateTime.UtcNow,
            LastLoginAt: null,
            IsActive: true
        );
        
        await _repository.SaveAsync(user);
        return ValidationResult<User>.SuccessResult(user);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Failed to create user with email {Email}", registration.Email);
        return ValidationResult<User>.FailureResult("User creation failed");
    }
}
```

### Async Methods
```csharp
// ✅ Always use ConfigureAwait(false) in libraries
public async Task<ValidationResult<PaymentResult>> ProcessPaymentAsync(PaymentRequest request)
{
    var validation = await ValidateRequestAsync(request).ConfigureAwait(false);
    if (!validation.Success)
        return ValidationResult<PaymentResult>.FailureResult(validation.Error!);
        
    return await _paymentGateway.ProcessAsync(request).ConfigureAwait(false);
}

// ✅ Use CancellationToken for long-running operations
public async Task<ValidationResult<List<User>>> SearchUsersAsync(
    string searchTerm, 
    CancellationToken cancellationToken = default)
{
    using var scope = _logger.BeginScope("SearchUsers: {SearchTerm}", searchTerm);
    
    try
    {
        var users = await _repository
            .SearchAsync(searchTerm, cancellationToken)
            .ConfigureAwait(false);
            
        return ValidationResult<List<User>>.SuccessResult(users);
    }
    catch (OperationCanceledException)
    {
        _logger.LogInformation("User search was cancelled");
        throw; // Re-throw cancellation
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Failed to search users");
        return ValidationResult<List<User>>.FailureResult("Search failed");
    }
}
```

## Function Structure

### Parameters
- **Maximum 3 parameters** - use options object for more
- **Required parameters first**, optional parameters last
- Use **object destructuring** in primary constructors

```csharp
// ✅ Good - options object for multiple parameters
public record CreateUserOptions(
    string Email,
    string Name,
    UserRole Role = UserRole.User,
    bool SendWelcomeEmail = true,
    string? PreferredLanguage = null
);

public async Task<ValidationResult<User>> CreateUserAsync(
    CreateUserOptions options, 
    CancellationToken cancellationToken = default)
{
    // Implementation
}

// ❌ Avoid - too many parameters
public async Task<ValidationResult<User>> CreateUserAsync(
    string email, 
    string name, 
    UserRole? role, 
    bool? sendEmail, 
    string? locale, 
    DateTime? createdAt,
    CancellationToken cancellationToken = default)
{
    // Too many parameters
}
```

### Return Types
- **Always specify explicit return types** for public methods
- Use **ValidationResult<T>** for operations that can fail
- **Document edge cases** with XML documentation

```csharp
/// <summary>
/// Calculates shipping cost based on weight and distance
/// </summary>
/// <param name="weight">Package weight in kg (must be positive)</param>
/// <param name="distance">Shipping distance in km (must be positive)</param>
/// <returns>Shipping cost in cents, or error for invalid inputs</returns>
public ValidationResult<decimal> CalculateShippingCost(decimal weight, decimal distance)
{
    // Validation
    if (weight <= 0 || distance <= 0)
        return ValidationResult<decimal>.FailureResult("Weight and distance must be positive");
    
    // Business logic
    var baseCost = weight * 50; // 50 cents per kg
    var distanceCost = distance * 10; // 10 cents per km
    
    return ValidationResult<decimal>.SuccessResult(Math.Round(baseCost + distanceCost, 2));
}
```

## Security Patterns

### Input Validation
- **Validate all inputs** at method boundaries
- Use **FluentValidation** for complex validation scenarios
- **Sanitize** inputs that will be stored or displayed

```csharp
// ✅ FluentValidation example
public class UserRegistrationValidator : AbstractValidator<UserRegistration>
{
    public UserRegistrationValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(254); // RFC 5322 limit
            
        RuleFor(x => x.Password)
            .MinimumLength(12)
            .Matches(@"[A-Z]").WithMessage("Must contain uppercase letter")
            .Matches(@"[a-z]").WithMessage("Must contain lowercase letter")
            .Matches(@"\d").WithMessage("Must contain number")
            .Matches(@"[^\w]").WithMessage("Must contain special character");
    }
}

// Usage in service
public async Task<ValidationResult<User>> RegisterUserAsync(UserRegistration registration)
{
    var validator = new UserRegistrationValidator();
    var validationResult = await validator.ValidateAsync(registration);
    
    if (!validationResult.IsValid)
    {
        var errors = validationResult.Errors.Select(e => 
            new ValidationError(e.PropertyName, e.ErrorCode, e.ErrorMessage)).ToList();
        return ValidationResult<User>.FailureResult(errors);
    }
    
    // Process valid registration
}
```

### Safe Defaults
- **Fail closed** - default to most restrictive settings
- **Rate limiting** - implement by default for external APIs
- **Timeouts** - set reasonable timeouts for all network calls

```csharp
// ✅ Safe defaults with options pattern
public class ApiClientOptions
{
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(30);
    public int MaxRetryAttempts { get; init; } = 3;
    public int RateLimitPerMinute { get; init; } = 60;
    public bool EnableLogging { get; init; } = true;
}

public class ApiClient(ApiClientOptions options, ILogger<ApiClient> logger)
{
    private readonly ApiClientOptions _options = options;
    
    public async Task<ValidationResult<TResponse>> CallAsync<TResponse>(
        string endpoint, 
        CancellationToken cancellationToken = default)
    {
        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(_options.Timeout);
        
        // Implementation with safe defaults
    }
}
```

## Testing Patterns

- **One assertion per test** when possible
- **Descriptive test names** that explain the scenario
- **Test edge cases** explicitly
- **Use FluentAssertions** for readable assertions

```csharp
public class ShippingCalculatorTests
{
    [Fact]
    public void CalculateShippingCost_ValidWeightAndDistance_ReturnsCorrectCost()
    {
        // Arrange
        var calculator = new ShippingCalculator();
        
        // Act
        var result = calculator.CalculateShippingCost(2.5m, 100m);
        
        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().Be(375m); // (2.5 * 50) + (100 * 10) = 375 cents
    }
    
    [Theory]
    [InlineData(-1, 100, "Weight and distance must be positive")]
    [InlineData(2.5, -50, "Weight and distance must be positive")]
    [InlineData(0, 100, "Weight and distance must be positive")]
    public void CalculateShippingCost_InvalidInputs_ReturnsError(
        decimal weight, 
        decimal distance, 
        string expectedError)
    {
        // Arrange
        var calculator = new ShippingCalculator();
        
        // Act
        var result = calculator.CalculateShippingCost(weight, distance);
        
        // Assert
        result.Success.Should().BeFalse();
        result.Error.Should().Contain(expectedError);
    }
}
```

## AI Copilot Integration Tips

When using GitHub Copilot with this codebase:

1. **Reference this style guide** in prompts: "Follow the patterns in StyleGuide.md"
2. **Use FCE pattern**: Function intent, Constraints (error cases, limits), Examples (input/output)
3. **Open related files** before prompting to give Copilot context
4. **Be specific about modern C# features**: "Use C# 12 primary constructors and collection expressions"
5. **Mention ValidationResult pattern**: "Return ValidationResult<T> following established patterns"

### Good Prompts for C#
```csharp
// ✅ Specific C# context
// Function: Create JWT token generator using System.IdentityModel.Tokens.Jwt
// Constraints: 24-hour expiry, include user claims, use symmetric security key from configuration
// Modern C# features: Use primary constructor for DI, nullable reference types
// Examples: GenerateTokenAsync(user) → "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9..."
public class JwtTokenGenerator(IConfiguration configuration, ILogger<JwtTokenGenerator> logger)
```

### Context Files for AI
- **ApiSchema.md**: Business rules and data structures
- **StyleGuide.md**: This file with coding conventions
- **.editorconfig**: Code formatting preferences
- **Open related interfaces**: For dependency injection context