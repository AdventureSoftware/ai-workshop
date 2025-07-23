/**
 * Basic Prompting Examples for AI Workshop
 *
 * This file demonstrates the difference between vague and specific prompts.
 * Use these as starting points for Copilot demonstrations.
 */

// ❌ VAGUE PROMPT EXAMPLE (DON'T DO THIS)
// Comment: "Create a login function"
// Result: AI will create basic, generic code with no business logic

// ✅ GOOD PROMPT EXAMPLE (FCE PATTERN)
// Function: Create secure user authentication with JWT tokens
// Constraints: Rate limiting (5 attempts/15min), bcrypt hashing, email validation
// Examples: {email: "user@example.com", password: "SecurePass123!"} → {success: true, token: "jwt_token"}

import { ValidationResult, LoginCredentials, AuthToken } from '../types/auth';

/**
 * Authenticates user with email and password
 *
 * Constraints:
 * - Email must be valid format (RFC 5322)
 * - Password must meet security requirements
 * - Rate limiting: max 5 attempts per 15 minutes per IP
 * - Uses bcrypt for password verification
 * - Returns JWT token valid for 24 hours
 *
 * Examples:
 * - Valid: {email: "user@example.com", password: "SecurePass123!"} → {success: true, token: "eyJ0..."}
 * - Invalid email: {email: "invalid-email", password: "password"} → {success: false, error: "Invalid email format"}
 * - Wrong password: {email: "user@example.com", password: "wrong"} → {success: false, error: "Invalid credentials"}
 */
async function authenticateUser(credentials: LoginCredentials): Promise<ValidationResult<AuthToken>> {
  // Let Copilot complete this function based on the detailed prompt above
  throw new Error('Not implemented - use Copilot to complete this function');
}

// ❌ VAGUE PROMPT EXAMPLE
// Comment: "Calculate shipping cost"
function calculateShipping(weight: number, distance: number): number {
  // AI will make assumptions about rates, currency, etc.
  throw new Error('Vague prompt - results will vary');
}

// ✅ SPECIFIC PROMPT WITH FCE PATTERN
// Function: Calculate shipping cost based on weight and distance
// Constraints:
//   - Weight in kg (positive only), distance in km (positive only)
//   - Base rate: $0.50/kg + $0.10/km
//   - Return amount in cents (not dollars)
//   - Handle edge cases: negative/zero inputs should return error
// Examples:
//   - calculateShipping(2.5, 100) → 1125 cents ($11.25)
//   - calculateShipping(-1, 50) → throw error "Weight must be positive"
//   - calculateShipping(0, 100) → throw error "Weight must be positive"
function calculateShippingCost(weight: number, distance: number): number {
  // Let Copilot complete this based on the detailed constraints above
  throw new Error('Not implemented - use Copilot to complete');
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
async function processPayment(amount: number, cardToken: string): Promise<ValidationResult> {
  // Copilot should generate robust error handling based on edge cases listed above
  throw new Error('Not implemented - demonstrate edge-case handling');
}

// TEST-FIRST SPECIFICATION EXAMPLE
// Write tests first, then let Copilot implement the function

describe('Email validation', () => {
  it('should accept valid emails: user@domain.com, test+tag@example.org');
  it('should reject invalid: missing@.com, double@@domain.com, spaces in email');
  it('should handle edge cases: unicode domains, 64+ char local parts');
  it('should normalize input: trim whitespace, convert to lowercase');
});

// Now prompt: "Implement validateEmail function to pass these tests"
function validateEmail(email: string): ValidationResult<string> {
  throw new Error('Implement based on test specifications above');
}

export {
  authenticateUser,
  calculateShipping,
  calculateShippingCost,
  processPayment,
  validateEmail
};
