/**
 * Context Injection Examples
 *
 * This file demonstrates how to provide rich context for AI Copilot.
 * Open multiple related files when working on these examples.
 */

import { ValidationResult } from '../types/auth';
import { PricingStrategy } from '../types/pricing';

// INLINE COMMENTS EXAMPLE
// Place intent right above cursor for maximum weight in AI context

// ✅ GOOD: Specific context with constraints
// Calculate customer discount percentage based on loyalty tier and order total
// Constraints: decimal precision to 2 places, min 0%, max 50%
// Business rules: Gold = 15%, Silver = 10%, Bronze = 5%, orders >$200 get +5%
function calculateDiscount(loyaltyTier: string, orderTotal: number): number {
  // Copilot will understand the business logic from comments above
  throw new Error('Not implemented');
}

// ❌ POOR: Vague context
// Calculate discount
function getDiscount(tier: string, total: number): number {
  // AI has to guess the business rules
  throw new Error('Not implemented');
}

// MULTI-FILE CONTEXT STRATEGY
// To demonstrate: Open pricing-strategy.ts, customer.ts, and order.ts files
// Then reference them in Chat: "Refactor #calculateDiscount to use #PricingStrategy pattern"

/**
 * Advanced discount calculator that uses dependency injection
 *
 * Context clues for AI:
 * - Uses Strategy pattern (open strategy files for context)
 * - Implements business rules from API_SCHEMA.md
 * - Follows error handling patterns from STYLE_GUIDE.md
 */
class DiscountCalculator {
  constructor(private pricingStrategy: PricingStrategy) {}

  // AI can infer implementation based on open strategy files
  calculateCustomerDiscount(customerId: string, orderAmount: number): Promise<ValidationResult<number>> {
    throw new Error('Use Copilot with #PricingStrategy context');
  }
}

// WORKSPACE SYMBOL CONTEXT
// These functions reference types/interfaces defined elsewhere
// Copilot will use workspace index to understand relationships

/**
 * Processes user registration with full validation
 * References: User, ValidationResult, AuthError types from workspace
 *
 * Constraints:
 * - Email uniqueness check required
 * - Password strength validation per STYLE_GUIDE.md
 * - Rate limiting: max 3 registrations per IP per hour
 */
async function registerUser(userData: unknown): Promise<ValidationResult> {
  // AI can reference User type definition and validation patterns
  throw new Error('Implement using workspace type context');
}

// EXTERNAL DOCS CONTEXT EXAMPLE
// This function should follow patterns defined in API_SCHEMA.md
// Copilot reads schema documentation to understand data structures

/**
 * Creates shipping quote based on package details
 *
 * Implementation notes:
 * - Follow ShippingRequest/ShippingQuote interfaces from API_SCHEMA.md
 * - Use business rules defined in schema (rates, limits, etc.)
 * - Handle all edge cases listed in validation section
 */
function generateShippingQuote(request: unknown): Promise<ValidationResult> {
  // AI should reference API_SCHEMA.md for data structures and business rules
  throw new Error('Implement following API_SCHEMA.md patterns');
}

// NEIGHBOR FILES TECHNIQUE
// Open these related files before prompting Copilot:
// - src/types/auth.ts (for type definitions)
// - src/utils/validation.ts (for validation helpers)
// - src/services/email.ts (for email handling)
// - __tests__/auth.test.ts (for expected behavior)

export {
  calculateDiscount,
  getDiscount,
  DiscountCalculator,
  registerUser,
  generateShippingQuote
};
