using AIPairProgramming.Shared.Models;

namespace AIPairProgramming.Examples;

/// <summary>
/// Context Injection Examples
/// 
/// This file demonstrates how to provide rich context for AI Copilot.
/// Open multiple related files when working on these examples.
/// </summary>
public class ContextInjection
{
    // INLINE COMMENTS EXAMPLE
    // Place intent right above cursor for maximum weight in AI context

    // ✅ GOOD: Specific context with constraints
    // Calculate customer discount percentage based on loyalty tier and order total
    // Constraints: decimal precision to 2 places, min 0%, max 50%
    // Business rules: Gold = 15%, Silver = 10%, Bronze = 5%, orders >$200 get +5%
    public decimal CalculateDiscount(string loyaltyTier, decimal orderTotal)
    {
        // Copilot will understand the business logic from comments above
        throw new NotImplementedException("Not implemented");
    }

    // ❌ POOR: Vague context
    // Calculate discount
    public decimal GetDiscount(string tier, decimal total)
    {
        // AI has to guess the business rules
        throw new NotImplementedException("Not implemented");
    }

    // MULTI-FILE CONTEXT STRATEGY
    // To demonstrate: Open PricingStrategy.cs, Customer.cs, and Order.cs files
    // Then reference them in Chat: "Refactor #CalculateDiscount to use #IPricingStrategy pattern"

    /// <summary>
    /// Advanced discount calculator that uses dependency injection
    /// 
    /// Context clues for AI:
    /// - Uses Strategy pattern (open strategy files for context)
    /// - Implements business rules from ApiSchema.md
    /// - Follows error handling patterns from StyleGuide.md
    /// </summary>
    public class DiscountCalculator(IPricingStrategy pricingStrategy)
    {
        // AI can infer implementation based on open strategy files
        public async Task<ValidationResult<decimal>> CalculateCustomerDiscountAsync(string customerId, decimal orderAmount)
        {
            throw new NotImplementedException("Use Copilot with #IPricingStrategy context");
        }
    }

    // WORKSPACE SYMBOL CONTEXT
    // These functions reference types/interfaces defined elsewhere
    // Copilot will use workspace index to understand relationships

    /// <summary>
    /// Processes user registration with full validation
    /// References: User, ValidationResult, AuthErrorCode types from workspace
    /// 
    /// Constraints:
    /// - Email uniqueness check required
    /// - Password strength validation per StyleGuide.md
    /// - Rate limiting: max 3 registrations per IP per hour
    /// </summary>
    public async Task<ValidationResult<User>> RegisterUserAsync(UserRegistration userData)
    {
        // AI can reference User type definition and validation patterns
        throw new NotImplementedException("Implement using workspace type context");
    }

    // EXTERNAL DOCS CONTEXT EXAMPLE
    // This function should follow patterns defined in ApiSchema.md
    // Copilot reads schema documentation to understand data structures

    /// <summary>
    /// Creates shipping quote based on package details
    /// 
    /// Implementation notes:
    /// - Follow ShippingRequest/ShippingQuote interfaces from ApiSchema.md
    /// - Use business rules defined in schema (rates, limits, etc.)
    /// - Handle all edge cases listed in validation section
    /// </summary>
    public async Task<ValidationResult<ShippingQuote>> GenerateShippingQuoteAsync(ShippingRequest request)
    {
        // AI should reference ApiSchema.md for data structures and business rules
        throw new NotImplementedException("Implement following ApiSchema.md patterns");
    }

    // MODERN C# FEATURES FOR CONTEXT

    // Collection expressions provide clear data structures for AI
    private static readonly string[] SupportedCurrencies = ["USD", "EUR", "GBP"];

    // Nullable reference types help AI generate safer code
    public async Task<ValidationResult<PaymentConfirmation?>> ProcessPaymentAsync(
        PaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("AI respects nullable annotations");
    }

    // Record types for immutable data contracts that AI understands well
    public record PaymentRequest(decimal Amount, string Currency, string CustomerId)
    {
        public string? Description { get; init; }
    }

    public record PaymentConfirmation(string Id, decimal Amount, string Status);
    public record ShippingRequest(decimal Weight, decimal Distance, string Priority);
    public record ShippingQuote(decimal Cost, int EstimatedDays, string Carrier);

    // Pattern matching provides clear business logic context
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

// NEIGHBOR FILES TECHNIQUE
// Open these related files before prompting Copilot:
// - Models/AuthTypes.cs (for type definitions)
// - Services/IValidationService.cs (for validation helpers)
// - Services/IEmailService.cs (for email handling)
// - Tests/AuthTests.cs (for expected behavior)

// Interface definitions provide context for AI about expected patterns
public interface IPricingStrategy
{
    Task<decimal> CalculateDiscountAsync(string customerId, decimal orderAmount);
}

public interface IValidationService
{
    ValidationResult<T> Validate<T>(T model);
}

public interface IEmailService
{
    Task<bool> SendAsync(string to, string subject, string body);
}