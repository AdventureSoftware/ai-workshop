using AIPairProgramming.Shared.Models;

namespace AIPairProgramming.Exercises;

/// <summary>
/// Exercise 1: FCE Pattern Practice
/// 
/// Complete this payment processor using the Function-Constraints-Examples pattern.
/// Each function should be implemented by providing clear context to Copilot.
/// </summary>

public record PaymentRequest(
    decimal Amount,         // In cents
    string Currency,
    string CustomerId,
    string? Description = null
);

public record PaymentResult(
    string TransactionId,
    decimal Amount,
    decimal Fees,           // Processing fees in cents
    PaymentStatus Status,
    DateTime Timestamp
);

public enum PaymentStatus
{
    Completed,
    Failed,
    Pending
}

public class PaymentProcessor
{
    // TODO: Exercise 1a - Use FCE pattern to implement payment validation
    // Function: Validate payment request before processing
    // Constraints:
    //   - Amount must be between 500 cents ($5) and 1,000,000 cents ($10,000)
    //   - Currency must be 'USD' (only supported currency)
    //   - Customer ID must be valid GUID format
    //   - Description optional, max 200 characters if provided
    // Examples:
    //   - new PaymentRequest(2999, "USD", "12345678-1234-1234-1234-123456789012") → {Success: true}
    //   - new PaymentRequest(100, "USD", "valid-guid") → {Success: false, Error: "Amount below minimum"}
    //   - new PaymentRequest(5000, "EUR", "valid-guid") → {Success: false, Error: "Currency not supported"}
    public ValidationResult<PaymentRequest> ValidatePaymentRequest(PaymentRequest request)
    {
        // TODO: Let Copilot implement based on FCE pattern above
        throw new NotImplementedException("Not implemented - use Copilot with FCE pattern");
    }

    // TODO: Exercise 1b - Use FCE with edge cases
    // Function: Calculate processing fees based on amount and customer tier
    // Constraints:
    //   - Standard rate: 2.9% + 30 cents
    //   - Premium customers (flagged in system): 1.9% + 20 cents
    //   - International cards: additional 1.5%
    //   - Round to nearest cent
    // Edge cases to handle:
    //   - Null/undefined customer tier (default to standard)
    //   - Negative amounts (throw ArgumentException)
    //   - Very large amounts >$50k (require manual approval, return special code)
    // Examples:
    //   - CalculateFees(1000, CustomerTier.Standard, false) → 59 cents (29 + 30)
    //   - CalculateFees(1000, CustomerTier.Premium, false) → 39 cents (19 + 20)
    //   - CalculateFees(1000, CustomerTier.Standard, true) → 74 cents (29 + 30 + 15)
    public decimal CalculateProcessingFees(
        decimal amount,
        CustomerTier? customerTier,
        bool isInternationalCard)
    {
        // TODO: Let Copilot handle edge cases based on detailed constraints
        throw new NotImplementedException("Not implemented - demonstrate edge-case handling");
    }

    // TODO: Exercise 1c - Complex business logic with FCE
    // Function: Process payment with retry logic and comprehensive error handling
    // Constraints:
    //   - Must validate request first using ValidatePaymentRequest
    //   - Calculate fees using CalculateProcessingFees
    //   - Retry failed payments up to 3 times with exponential backoff (1s, 2s, 4s)
    //   - Only retry on network errors, not validation/business rule failures
    //   - Return detailed error information for debugging
    //   - Must handle: insufficient funds, expired cards, fraud detection, network timeouts
    // Examples:
    //   - Success: ProcessPaymentAsync(validRequest) → {Success: true, Data: PaymentResult}
    //   - Validation error: ProcessPaymentAsync(invalidRequest) → {Success: false, Error: "validation details"}
    //   - Retriable error: network timeout → retry up to 3 times
    //   - Non-retriable error: insufficient funds → fail immediately, don't retry
    public async Task<ValidationResult<PaymentResult>> ProcessPaymentAsync(PaymentRequest request, CancellationToken cancellationToken = default)
    {
        // TODO: Complex implementation with retry logic and error handling
        throw new NotImplementedException("Not implemented - demonstrate complex business logic handling");
    }

    // TODO: Exercise 1d - Test-first specification
    // First write tests that describe the expected behavior, then implement
    //
    // Tests should cover:
    // - Happy path: valid payment processes successfully
    // - Validation errors: amount too low, unsupported currency, invalid customer ID
    // - Edge cases: very large amounts, international cards, premium customers
    // - Error handling: network failures with retries, non-retriable errors
    // - Business rules: fee calculations, processing limits
    //
    // Then prompt: "Implement RefundPaymentAsync method to pass these test specifications"

    // Example test specifications (write actual tests in test project):
    /*
    [Test]
    public void Should_ProcessFullRefundsWithin30Days() { }
    
    [Test] 
    public void Should_CalculatePartialRefundFeesCorrectly() { }
    
    [Test]
    public void Should_RejectRefundsAfter30DayLimit() { }
    
    [Test]
    public void Should_HandleAlreadyRefundedTransactions() { }
    
    [Test]
    public void Should_RequireManagerApprovalForRefundsOver1000() { }
    */

    public async Task<ValidationResult<PaymentResult>> RefundPaymentAsync(
        string transactionId,
        decimal? refundAmount = null,  // Full refund if not specified
        string? reason = null,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implement based on test specifications above
        throw new NotImplementedException("Write tests first, then implement with Copilot");
    }

    // VERIFICATION CHECKLIST APPLICATION
    // After implementing each function with Copilot, apply R.E.D.:
    //
    // READ:
    // ✅ Does the code solve the actual problem described?
    // ✅ Are variable names consistent with our C# conventions?
    // ✅ Any magic numbers that should be constants?
    // ✅ Does error handling follow our ValidationResult pattern?
    // ✅ Are there TODO comments or incomplete sections?
    //
    // EXECUTE:
    // ✅ Run the tests - do they pass?
    // ✅ Test edge cases manually
    // ✅ Check error scenarios work correctly
    //
    // DIFF-REVIEW:
    // ✅ Compare implementation with your mental model
    // ✅ Are business rules correctly implemented?
    // ✅ Does retry logic work as expected?
    // ✅ Is fee calculation accurate?

    // Helper methods for context
    private async Task<bool> CallPaymentGatewayAsync(PaymentRequest request, CancellationToken cancellationToken)
    {
        // TODO: Simulate payment gateway call
        await Task.Delay(100, cancellationToken);
        return true;
    }

    private CustomerTier GetCustomerTier(string customerId)
    {
        // TODO: Look up customer tier from database
        return CustomerTier.Standard;
    }
}

public enum CustomerTier
{
    Standard,
    Premium
}