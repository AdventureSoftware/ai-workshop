/**
 * Exercise 1: FCE Pattern Practice
 *
 * Complete this payment processor using the Function-Constraints-Examples pattern.
 * Each function should be implemented by providing clear context to Copilot.
 */

import { ValidationResult } from '../types/auth';

interface PaymentRequest {
  amount: number;        // In cents
  currency: 'USD';
  customerId: string;
  description?: string;
}

interface PaymentResult {
  transactionId: string;
  amount: number;
  fees: number;         // Processing fees in cents
  status: 'completed' | 'failed' | 'pending';
  timestamp: Date;
}

// TODO: Exercise 1a - Use FCE pattern to implement payment validation
// Function: Validate payment request before processing
// Constraints:
//   - Amount must be between 500 cents ($5) and 1,000,000 cents ($10,000)
//   - Currency must be 'USD' (only supported currency)
//   - Customer ID must be valid UUID format
//   - Description optional, max 200 characters if provided
// Examples:
//   - {amount: 2999, currency: 'USD', customerId: 'valid-uuid'} → {success: true}
//   - {amount: 100, currency: 'USD', customerId: 'valid-uuid'} → {success: false, error: 'Amount below minimum'}
//   - {amount: 5000, currency: 'EUR', customerId: 'valid-uuid'} → {success: false, error: 'Currency not supported'}
function validatePaymentRequest(request: PaymentRequest): ValidationResult<PaymentRequest> {
  // TODO: Let Copilot implement based on FCE pattern above

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
//   - Negative amounts (throw error)
//   - Very large amounts >$50k (require manual approval, return special code)
// Examples:
//   - calculateFees(1000, 'standard', false) → 59 cents (29 + 30)
//   - calculateFees(1000, 'premium', false) → 39 cents (19 + 20)
//   - calculateFees(1000, 'standard', true) → 74 cents (29 + 30 + 15)
function calculateProcessingFees(
  amount: number,
  customerTier: 'standard' | 'premium' | null,
  isInternationalCard: boolean
): number {
  // TODO: Let Copilot handle edge cases based on detailed constraints
  throw new Error('Not implemented - demonstrate edge-case handling');
}

// TODO: Exercise 1c - Complex business logic with FCE
// Function: Process payment with retry logic and comprehensive error handling
// Constraints:
//   - Must validate request first using validatePaymentRequest
//   - Calculate fees using calculateProcessingFees
//   - Retry failed payments up to 3 times with exponential backoff (1s, 2s, 4s)
//   - Only retry on network errors, not validation/business rule failures
//   - Return detailed error information for debugging
//   - Must handle: insufficient funds, expired cards, fraud detection, network timeouts
// Examples:
//   - Success: processPayment(validRequest) → {success: true, data: PaymentResult}
//   - Validation error: processPayment(invalidRequest) → {success: false, error: 'validation details'}
//   - Retriable error: network timeout → retry up to 3 times
//   - Non-retriable error: insufficient funds → fail immediately, don't retry
async function processPayment(request: PaymentRequest): Promise<ValidationResult<PaymentResult>> {
  // TODO: Complex implementation with retry logic and error handling
  throw new Error('Not implemented - demonstrate complex business logic handling');
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
// Then prompt: "Implement refundPayment function to pass these test specifications"

describe('Payment refund system', () => {
  // TODO: Write comprehensive tests first
  it('should process full refunds within 30 days');
  it('should calculate partial refund fees correctly');
  it('should reject refunds after 30 day limit');
  it('should handle already-refunded transactions');
  it('should require manager approval for refunds over $1000');
});

async function refundPayment(
  transactionId: string,
  refundAmount?: number,  // Full refund if not specified
  reason?: string
): Promise<ValidationResult<PaymentResult>> {
  // TODO: Implement based on test specifications above
  throw new Error('Write tests first, then implement with Copilot');
}

// VERIFICATION CHECKLIST APPLICATION
// After implementing each function with Copilot, apply R.E.D.:
//
// READ:
// ✅ Does the code solve the actual problem described?
// ✅ Are variable names consistent with our TypeScript conventions?
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

export {
  validatePaymentRequest,
  calculateProcessingFees,
  processPayment,
  refundPayment,
  PaymentRequest,
  PaymentResult
};
