using System.ComponentModel.DataAnnotations;

namespace AIPairProgramming.Shared.Models;

/// <summary>
/// Authentication types for the workshop examples
/// Provides context for AI Copilot when working with user management
/// </summary>

public record User(
    string Id,
    string Email,
    string Name,
    UserRole Role,
    DateTime CreatedAt,
    DateTime? LastLoginAt,
    bool IsActive
);

public enum UserRole
{
    User,
    Admin
}

public record LoginCredentials(
    string Email,
    string Password
);

public record AuthToken(
    string Token,
    DateTime ExpiresAt,
    string UserId
);

public record ValidationResult<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public string? Error { get; init; }
    public List<ValidationError>? Errors { get; init; }

    public static ValidationResult<T> SuccessResult(T data) => new()
    {
        Success = true,
        Data = data
    };

    public static ValidationResult<T> FailureResult(string error) => new()
    {
        Success = false,
        Error = error
    };

    public static ValidationResult<T> FailureResult(List<ValidationError> errors) => new()
    {
        Success = false,
        Errors = errors
    };
}

public record ValidationError(
    string Field,
    string Code,
    string Message
);

public enum AuthErrorCode
{
    InvalidCredentials,
    RateLimited,
    AccountDisabled,
    InvalidEmailFormat,
    WeakPassword
}

public record UserRegistration(
    string Email,
    string Name,
    string Password
);

// Rate limiting configuration for AI context
public record RateLimitConfig(
    int MaxAttempts,
    TimeSpan Window,
    TimeSpan BlockDuration
);

// Audit event for security logging
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
    SecurityViolation
}