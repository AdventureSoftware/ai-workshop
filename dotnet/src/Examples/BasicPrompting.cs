using AIPairProgramming.Shared.Models;

namespace AIPairProgramming.Examples;

/// <summary>
/// Basic Prompting Examples for AI Workshop
/// 
/// This file demonstrates the difference between vague and specific prompts.
/// Use these as starting points for Copilot demonstrations.
/// </summary>
public class BasicPrompting
{
    // ❌ VAGUE PROMPT EXAMPLE (DON'T DO THIS)
    // Comment: "Create a login function"
    // Result: AI will create basic, generic code with no business logic

    // ✅ GOOD PROMPT EXAMPLE (FCE PATTERN)
    // Function: Create secure user authentication with JWT tokens
    // Constraints: Rate limiting (5 attempts/15min), BCrypt hashing, email validation
    // Examples: {Email: "user@example.com", Password: "SecurePass123!"} → {Success: true, Token: "jwt_token"}

    /// <summary>
    /// Authenticates user with email and password using modern C# patterns
    /// 
    /// Constraints:
    /// - Email must be valid format (RFC 5322)
    /// - Password must meet security requirements
    /// - Rate limiting: max 5 attempts per 15 minutes per IP
    /// - Uses BCrypt for password verification
    /// - Returns JWT token valid for 24 hours
    /// 
    /// Examples:
    /// - Valid: {Email: "user@example.com", Password: "SecurePass123!"} → {Success: true, Token: "eyJ0..."}
    /// - Invalid email: {Email: "invalid-email", Password: "password"} → {Success: false, Error: "Invalid email format"}
    /// - Wrong password: {Email: "user@example.com", Password: "wrong"} → {Success: false, Error: "Invalid credentials"}
    /// </summary>
    public async Task<ValidationResult<AuthToken>> AuthenticateUserAsync(LoginCredentials credentials)
    {
        // Let Copilot complete this function based on the detailed prompt above
        throw new NotImplementedException("Not implemented - use Copilot to complete this function");
    }

    // ❌ VAGUE PROMPT EXAMPLE
    // Comment: "Calculate shipping cost"
    public decimal CalculateShipping(decimal weight, decimal distance)
    {
        // AI will make assumptions about rates, currency, etc.
        throw new NotImplementedException("Vague prompt - results will vary");
    }

    // ✅ SPECIFIC PROMPT WITH FCE PATTERN
    // Function: Calculate shipping cost based on weight and distance
    // Constraints:
    //   - Weight in kg (positive only), distance in km (positive only)
    //   - Base rate: $0.50/kg + $0.10/km
    //   - Return amount in cents (not dollars)
    //   - Handle edge cases: negative/zero inputs should return error
    // Examples:
    //   - CalculateShippingCost(2.5m, 100m) → 1125 cents ($11.25)
    //   - CalculateShippingCost(-1m, 50m) → throw ArgumentException("Weight must be positive")
    //   - CalculateShippingCost(0m, 100m) → throw ArgumentException("Weight must be positive")
    public int CalculateShippingCost(decimal weight, decimal distance)
    {
        // Let Copilot complete this based on the detailed constraints above
        throw new NotImplementedException("Not implemented - use Copilot to complete");
    }

    // EDGE-CASE BOOSTER PATTERN EXAMPLE
    // Function: Process payment with comprehensive error handling
    // Handle edge cases:
    //   - null/undefined inputs (return validation error)
    //   - amount below $5 or above $10,000 (return limit error)
    //   - invalid credit card (return card error)
    //   - network timeouts (retry 3x with exponential backoff)
    //   - insufficient funds (return specific error, don't retry)
    //   - rate limiting exceeded (return 429 error)
    public async Task<ValidationResult<string>> ProcessPaymentAsync(decimal amount, string cardToken)
    {
        // Copilot should generate robust error handling based on edge cases listed above
        throw new NotImplementedException("Not implemented - demonstrate edge-case handling");
    }

    // TEST-FIRST SPECIFICATION EXAMPLE
    // Write tests first, then let Copilot implement the function
    // 
    // Tests should cover:
    // - Valid emails: user@domain.com, test+tag@example.org
    // - Invalid emails: missing@.com, double@@domain.com, spaces in email
    // - Edge cases: unicode domains, 64+ char local parts
    // - Normalization: trim whitespace, convert to lowercase
    //
    // Now prompt: "Implement ValidateEmail method to pass these test specifications"
    public ValidationResult<string> ValidateEmail(string email)
    {
        throw new NotImplementedException("Implement based on test specifications above");
    }

    // MODERN C# PATTERNS FOR AI CONTEXT
    
    // Primary constructors provide clear context for AI
    public class PaymentService(IPaymentGateway gateway, ILogger<PaymentService> logger)
    {
        // Copilot understands DI patterns from constructor
        public async Task<ValidationResult<string>> ProcessAsync(decimal amount)
        {
            throw new NotImplementedException("AI can infer DI usage from primary constructor");
        }
    }

    // Record types for immutable data contracts
    public record PaymentRequest(decimal Amount, string Currency, string CustomerId)
    {
        public string? Description { get; init; }
    }

    // Pattern matching for business logic - AI generates cleaner code
    public decimal CalculateFees(PaymentRequest request) => request switch
    {
        { Amount: < 5 } => throw new ArgumentException("Minimum amount is $5"),
        { Amount: > 10000 } => CalculateLargeTransactionFee(request),
        { Currency: not "USD" } => CalculateInternationalFee(request),
        _ => CalculateStandardFee(request)
    };

    private decimal CalculateLargeTransactionFee(PaymentRequest request) => 0m;
    private decimal CalculateInternationalFee(PaymentRequest request) => 0m;
    private decimal CalculateStandardFee(PaymentRequest request) => 0m;
}

// Interface for AI context - shows expected dependency injection patterns
public interface IPaymentGateway
{
    Task<ValidationResult<string>> ProcessPaymentAsync(decimal amount, string cardToken);
}