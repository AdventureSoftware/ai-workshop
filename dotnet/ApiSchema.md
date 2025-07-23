# API Schema & Business Rules (.NET)

This file provides domain-specific context for AI Copilot when generating C# code. Referenced throughout the workshop exercises to ensure AI suggestions align with business requirements and .NET best practices.

Based on presentation: "Domain artefacts – schema.md, api.yml, Confluence export"

## User Management System

### User Entity (.NET Record)
```csharp
public record User(
    string Id,                  // GUID format
    string Email,               // RFC 5322 compliant, unique
    string Name,                // 2-100 characters, letters and spaces
    UserRole Role,              // Enum: User, Admin
    DateTime CreatedAt,
    DateTime? LastLoginAt,
    bool IsActive               // Account status
);

public enum UserRole
{
    User,
    Admin
}
```

### Authentication Rules (.NET Specific)
- **Password Requirements**: 12+ characters, uppercase, lowercase, number, special character
- **Rate Limiting**: 5 login attempts per 15 minutes per IP (using IMemoryCache)
- **Account Lockout**: 5 failed attempts locks account for 30 minutes  
- **JWT Tokens**: 24-hour expiry, HS256 algorithm (System.IdentityModel.Tokens.Jwt)
- **API Keys**: 90-day expiry, base64 encoded, 32-byte entropy (System.Security.Cryptography)
- **OAuth2 Support**: Google OAuth2 flow with state validation
- **Password Hashing**: BCrypt.Net-Next with cost factor 12

### User Registration API (.NET Web API Style)
```csharp
[HttpPost("auth/register")]
public async Task<ActionResult<ApiResponse<UserRegistrationResponse>>> RegisterAsync(
    [FromBody] UserRegistration request,
    CancellationToken cancellationToken)
{
    // Implementation follows ValidationResult<T> pattern
}

public record UserRegistration(
    string Email,
    string Name,
    string Password
);

// Success Response (201 Created)
public record UserRegistrationResponse(User User, AuthToken Token);

public record AuthToken(
    string Token,
    DateTime ExpiresAt,
    string UserId
);

// Error Response (400 Bad Request)
public record ApiError(
    string Code,
    string Message,
    List<ValidationError>? Details = null
);
```

### Authentication API (.NET Controller)
```csharp
[HttpPost("auth/login")]
public async Task<ActionResult<ApiResponse<AuthToken>>> LoginAsync(
    [FromBody] LoginCredentials credentials,
    CancellationToken cancellationToken)

// Success (200 OK)
// Error (401 Unauthorized): "Invalid credentials"  
// Error (429 Too Many Requests): "Too many login attempts. Try again in 15 minutes."
```

## Payment Processing System (.NET)

### Payment Request Schema
```csharp
public record PaymentRequest(
    decimal Amount,             // Amount in cents (decimal for precision)
    string Currency,            // "USD" only supported
    string CustomerId,          // GUID format
    string? Description = null  // Optional, max 200 characters
);

public record PaymentResult(
    string TransactionId,       // GUID format
    decimal Amount,             // Confirmed amount in cents
    decimal Fees,               // Processing fees in cents
    PaymentStatus Status,       // Enum
    DateTime Timestamp
);

public enum PaymentStatus
{
    Pending,
    Completed,
    Failed,
    Refunded
}
```

### Business Rules - Payments (.NET Implementation)
- **Amount Limits**: Minimum 500 cents ($5.00), Maximum 1,000,000 cents ($10,000)
- **Processing Fees**: 
  - Standard: 2.9% + 30 cents
  - Premium customers: 1.9% + 20 cents  
  - International cards: +1.5%
- **Retry Logic**: Up to 3 retries with exponential backoff using Polly library
- **No Retries**: Insufficient funds, expired cards, fraud detection
- **Large Transactions**: Amounts >5,000,000 cents ($50k) require manual approval

### Payment Processing API (.NET)
```csharp
[HttpPost("payments")]
[Authorize] // JWT Bearer token required
public async Task<ActionResult<ApiResponse<PaymentResult>>> ProcessPaymentAsync(
    [FromBody] PaymentRequest request,
    CancellationToken cancellationToken)

// Success (200 OK)
// Error (400 Bad Request): "Amount below minimum limit"
// Error (402 Payment Required): "Insufficient funds"
```

## Shipping Calculator System (.NET)

### Shipping Request Schema
```csharp
public record ShippingRequest(
    decimal Weight,             // Weight in kilograms, max 50kg
    Dimensions Dimensions,
    string Origin,              // US postal code (5 digits or ZIP+4)
    string Destination,         // US postal code (5 digits or ZIP+4)
    ServiceType ServiceType,
    decimal DeclaredValue       // Value in cents for insurance
);

public record Dimensions(
    decimal Length,             // Length in cm
    decimal Width,              // Width in cm  
    decimal Height              // Height in cm
);

public enum ServiceType
{
    Standard,
    Express,
    Overnight
}

public record ShippingQuote(
    decimal BaseRate,           // Base shipping cost in cents
    decimal FuelSurcharge,      // Fuel surcharge in cents
    decimal InsuranceFee,       // Insurance fee in cents
    decimal TotalCost,          // Total cost in cents
    int EstimatedDays,          // Estimated delivery days
    string ServiceLevel         // Human-readable service description
);
```

### Business Rules - Shipping (.NET)
- **Weight Limits**: 0.1kg minimum, 50kg maximum
- **Dimensional Weight**: (L × W × H) ÷ 5000 = kg (use if higher than actual weight)
- **Service Multipliers**:
  - Standard: 1.0x (5-7 business days) → "Standard Ground"
  - Express: 1.5x (2-3 business days) → "Express Shipping"
  - Overnight: 3.0x (next business day) → "Overnight Express"
- **Base Rates**: $0.50 per kg + $0.10 per km distance
- **Insurance**: 1% of declared value for packages over 50,000 cents ($500)
- **Fuel Surcharge**: 15% of base rate
- **Minimum Cost**: 500 cents ($5.00)

### Shipping Quote API (.NET)
```csharp
[HttpPost("shipping/quote")]
public async Task<ActionResult<ApiResponse<ShippingQuote>>> GetQuoteAsync(
    [FromBody] ShippingRequest request,
    CancellationToken cancellationToken)

// Success (200 OK)
// Error (400 Bad Request): Validation errors
```

## Data Validation Rules (.NET)

### Email Validation (FluentValidation)
```csharp
public class EmailValidator : AbstractValidator<string>
{
    public EmailValidator()
    {
        RuleFor(email => email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(254) // RFC 5322
            .Must(email => email.Split('@')[0].Length <= 64); // Local part max 64
    }
}
```

### Password Validation (FluentValidation)
```csharp
public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(password => password)
            .MinimumLength(12)
            .Matches(@"[A-Z]").WithMessage("Must contain uppercase letter")
            .Matches(@"[a-z]").WithMessage("Must contain lowercase letter")
            .Matches(@"\d").WithMessage("Must contain number")
            .Matches(@"[^\w]").WithMessage("Must contain special character")
            .Must(NotBeCommonPassword).WithMessage("Password is too common");
    }
    
    private static bool NotBeCommonPassword(string password) => 
        !CommonPasswords.Contains(password.ToLower());
}
```

### Postal Code Validation (.NET Regex)
```csharp
private static readonly Regex UsPostalCodeRegex = new(
    @"^\d{5}(-\d{4})?$", 
    RegexOptions.Compiled
);

public static bool IsValidUsPostalCode(string postalCode) =>
    UsPostalCodeRegex.IsMatch(postalCode);
```

### Input Sanitization (.NET)
```csharp
// Maximum string length: 1000 characters
// HTML encode all output: System.Net.WebUtility.HtmlEncode
// SQL parameterization: Dapper or Entity Framework Core
// File uploads: whitelist extensions, magic byte validation with file signatures
```

## Rate Limiting Policies (.NET)

### Authentication Endpoints (IMemoryCache)
```csharp
public class RateLimitConfig
{
    public int MaxAttempts { get; init; }
    public TimeSpan Window { get; init; }
    public TimeSpan BlockDuration { get; init; }
}

// Login: 5 attempts per 15 minutes per IP
// Registration: 3 attempts per hour per IP
// Password reset: 3 attempts per hour per email
```

### API Endpoints (ASP.NET Core Rate Limiting)
```csharp
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("Standard", limiterOptions =>
    {
        limiterOptions.PermitLimit = 100;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
    });
});
```

### Payment Processing
- Payment creation: 10 per minute per user
- Refund requests: 5 per hour per user
- Large transactions (>100,000 cents): 1 per hour per user

## Error Handling Standards (.NET)

### HTTP Status Codes (ASP.NET Core)
```csharp
public enum ApiErrorCode
{
    ValidationFailed,
    Unauthorized,
    Forbidden,
    NotFound,
    Conflict,
    RateLimited,
    InternalError
}

// 200: Success
// 400: Bad request (validation errors)
// 401: Unauthorized (invalid/missing token)
// 403: Forbidden (insufficient permissions)  
// 409: Conflict (duplicate resource)
// 429: Too many requests (rate limited)
// 500: Internal server error
```

### Error Response Format (.NET)
```csharp
public record ApiResponse<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public ApiError? Error { get; init; }
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
    public string RequestId { get; init; } = Guid.NewGuid().ToString();
}

public record ApiError(
    string Code,
    string Message,
    List<ValidationError>? Details = null
);

public record ValidationError(
    string Field,
    string Code,
    string Message
);
```

## Security Requirements (.NET)

### Authentication (.NET Libraries)
```csharp
// JWT tokens: Microsoft.IdentityModel.Tokens.Jwt
// Password hashing: BCrypt.Net-Next
// OAuth2: Microsoft.AspNetCore.Authentication.Google
// API keys: System.Security.Cryptography.RandomNumberGenerator
```

### Authorization (.NET Core)
```csharp
[Authorize(Roles = "Admin")]
[Authorize(Policy = "RequireManagerRole")]

public class ResourceBasedAuthorizationHandler : 
    AuthorizationHandler<ResourceRequirement, ResourceEntity>
```

### Data Protection (.NET Core Data Protection)
```csharp
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@".\keys"))
    .ProtectKeysWithCertificate(cert);

// All passwords hashed with BCrypt (cost factor 12)
// Sensitive data encrypted with Data Protection API
// PII handling follows GDPR guidelines
```

### Input Validation (.NET Security)
```csharp
// All inputs validated with FluentValidation
// SQL injection prevention with EF Core or Dapper parameterized queries
// XSS prevention with System.Net.WebUtility.HtmlEncode
// File upload restrictions and virus scanning hooks
// CSRF protection with [ValidateAntiForgeryToken]
```

## Monitoring & Observability (.NET)

### Audit Events (ILogger<T>)
```csharp
public record AuditEvent(
    string? UserId,
    AuditAction Action,
    string IpAddress,
    DateTime Timestamp,
    Dictionary<string, object>? Metadata = null
);

public enum AuditAction
{
    LoginAttempt,
    LoginSuccess,
    Registration,
    RateLimitExceeded,
    SecurityViolation,
    PaymentProcessed,
    AdminAction
}
```

### Performance Metrics (.NET Diagnostics)
```csharp
// Response time percentiles with System.Diagnostics.Activity
// Error rates by endpoint using ILogger
// Database query performance with EF Core logging
// Cache hit rates with IMemoryCache diagnostics
// External API latency tracking
```

### Alerting Thresholds (.NET Health Checks)  
```csharp
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database")
    .AddCheck<ExternalApiHealthCheck>("payment-gateway");

// Error rate >5% for 5 minutes
// Response time >2 seconds for p95
// Authentication failures >100/minute
// Failed payment rate >10%
// Memory usage >80%
```

## Modern C# Features for AI Context

### Primary Constructors (C# 12)
```csharp
public class PaymentService(IPaymentGateway gateway, ILogger<PaymentService> logger)
{
    // AI understands DI from primary constructor
}
```

### Collection Expressions (C# 12)
```csharp
private static readonly string[] SupportedCurrencies = ["USD", "EUR", "GBP"];
```

### Pattern Matching
```csharp
public decimal CalculateFees(PaymentRequest request) => request switch
{
    { Amount: < 500 } => throw new ArgumentException("Minimum amount is $5"),
    { Amount: > 1000000 } => CalculateLargeTransactionFee(request),
    { Currency: not "USD" } => CalculateInternationalFee(request),
    _ => CalculateStandardFee(request)
};
```

### Record Types with Init Properties
```csharp
public record PaymentRequest(decimal Amount, string Currency, string CustomerId)
{
    public string? Description { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
```

### Nullable Reference Types
```csharp
#nullable enable

public async Task<ValidationResult<User?>> GetUserAsync(string? userId)
{
    if (userId is null)
        return ValidationResult<User?>.FailureResult("User ID is required");
    
    // AI generates null-safe code
}
```

This schema provides the .NET-specific business context that AI tools need to generate appropriate, production-ready C# code that follows modern .NET practices and your specific requirements.