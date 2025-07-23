# TypeScript Style Guide

## General Principles

- **Clarity over cleverness** - Code should be self-documenting
- **Consistency** - Follow established patterns throughout the codebase
- **Type safety** - Leverage TypeScript's type system fully
- **Error handling** - Always handle potential failures explicitly

## Naming Conventions

### Functions
- Use **camelCase** for function names
- Use **verbs** that clearly describe the action
- Include context about what the function returns

```typescript
// ✅ Good
function calculateShippingCost(weight: number, distance: number): number
function validateEmailFormat(email: string): ValidationResult
function fetchUserById(id: string): Promise<User>

// ❌ Avoid
function calc(w: number, d: number): number
function validate(input: string): boolean
function get(id: string): Promise<any>
```

### Types and Interfaces
- Use **PascalCase** for types and interfaces
- Interface names should NOT start with 'I'
- Use descriptive names that indicate purpose

```typescript
// ✅ Good
interface UserProfile {
  id: string;
  email: string;
  lastLoginAt: Date;
}

type PaymentStatus = 'pending' | 'completed' | 'failed';
type ValidationResult = {
  valid: boolean;
  errors: string[];
};

// ❌ Avoid
interface IUser { }
type Status = 'pending' | 'completed' | 'failed'; // too generic
```

### Constants
- Use **SCREAMING_SNAKE_CASE** for module-level constants
- Group related constants in enums when appropriate

```typescript
// ✅ Good
const MAX_RETRY_ATTEMPTS = 3;
const API_TIMEOUT_MS = 5000;

enum PaymentLimits {
  MIN_AMOUNT = 500, // cents
  MAX_AMOUNT = 1000000, // cents
}
```

## Error Handling Patterns

### Use Result Types
```typescript
type Result<T, E = Error> = {
  success: true;
  data: T;
} | {
  success: false;
  error: E;
};

// ✅ Preferred pattern
function processPayment(amount: number): Result<PaymentConfirmation> {
  if (amount < PaymentLimits.MIN_AMOUNT) {
    return {
      success: false,
      error: new Error('Amount below minimum limit')
    };
  }
  
  // Process payment logic
  return {
    success: true,
    data: { transactionId: 'tx_123', amount }
  };
}
```

### Async Error Handling
```typescript
// ✅ Handle errors explicitly
async function fetchUserProfile(id: string): Promise<Result<UserProfile>> {
  try {
    const response = await fetch(`/api/users/${id}`);
    
    if (!response.ok) {
      return {
        success: false,
        error: new Error(`HTTP ${response.status}: ${response.statusText}`)
      };
    }
    
    const user = await response.json();
    return { success: true, data: user };
    
  } catch (error) {
    return {
      success: false,
      error: error instanceof Error ? error : new Error('Unknown error')
    };
  }
}
```

## Function Structure

### Parameters
- **Maximum 3 parameters** - use options object for more
- **Required parameters first**, optional parameters last
- Use **destructuring** for options objects

```typescript
// ✅ Good
interface CreateUserOptions {
  email: string;
  name: string;
  role?: 'user' | 'admin';
  sendWelcomeEmail?: boolean;
}

function createUser({ email, name, role = 'user', sendWelcomeEmail = true }: CreateUserOptions): Promise<User> {
  // Implementation
}

// ❌ Avoid
function createUser(email: string, name: string, role?: string, sendEmail?: boolean, locale?: string): Promise<User> {
  // Too many parameters
}
```

### Return Types
- **Always specify explicit return types** for public functions
- Use **union types** to represent all possible outcomes
- **Document edge cases** in JSDoc comments

```typescript
/**
 * Calculates shipping cost based on weight and distance
 * 
 * @param weight - Package weight in kg (must be positive)
 * @param distance - Shipping distance in km (must be positive)
 * @returns Shipping cost in cents, or error for invalid inputs
 */
function calculateShippingCost(weight: number, distance: number): Result<number> {
  // Validation
  if (weight <= 0 || distance <= 0) {
    return {
      success: false,
      error: new Error('Weight and distance must be positive numbers')
    };
  }
  
  // Business logic
  const baseCost = weight * 50; // 50 cents per kg
  const distanceCost = distance * 10; // 10 cents per km
  
  return {
    success: true,
    data: Math.round(baseCost + distanceCost)
  };
}
```

## Security Patterns

### Input Validation
- **Validate all inputs** at function boundaries
- Use **Zod** or similar libraries for runtime validation
- **Sanitize** inputs that will be stored or displayed

```typescript
import { z } from 'zod';

const EmailSchema = z.string().email().min(5).max(254);
const PasswordSchema = z.string().min(8).regex(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])/);

function validateUserRegistration(input: unknown): Result<{ email: string; password: string }> {
  const schema = z.object({
    email: EmailSchema,
    password: PasswordSchema,
  });
  
  const result = schema.safeParse(input);
  
  if (!result.success) {
    return {
      success: false,
      error: new Error(`Validation failed: ${result.error.message}`)
    };
  }
  
  return { success: true, data: result.data };
}
```

### Safe Defaults
- **Fail closed** - default to most restrictive settings
- **Rate limiting** - implement by default for external APIs
- **Timeouts** - set reasonable timeouts for all network calls

```typescript
interface ApiClientOptions {
  timeout?: number;
  retryAttempts?: number;
  rateLimitPerMinute?: number;
}

class ApiClient {
  private readonly options: Required<ApiClientOptions>;
  
  constructor(options: ApiClientOptions = {}) {
    // ✅ Safe defaults
    this.options = {
      timeout: options.timeout ?? 5000, // 5 seconds
      retryAttempts: options.retryAttempts ?? 3,
      rateLimitPerMinute: options.rateLimitPerMinute ?? 60,
    };
  }
}
```

## Testing Patterns

- **One assertion per test** when possible
- **Descriptive test names** that explain the scenario
- **Test edge cases** explicitly
- **Mock external dependencies** consistently

```typescript
describe('calculateShippingCost', () => {
  it('should return cost for valid weight and distance', () => {
    const result = calculateShippingCost(2.5, 100);
    
    expect(result.success).toBe(true);
    if (result.success) {
      expect(result.data).toBe(1125); // (2.5 * 50) + (100 * 10)
    }
  });
  
  it('should reject negative weight', () => {
    const result = calculateShippingCost(-1, 100);
    
    expect(result.success).toBe(false);
    if (!result.success) {
      expect(result.error.message).toContain('positive numbers');
    }
  });
});
```

## AI Copilot Integration Tips

When using GitHub Copilot with this codebase:

1. **Reference this style guide** in your prompts: "Follow the patterns in STYLE_GUIDE.md"
2. **Use FCE pattern**: Function intent, Constraints (error cases, limits), Examples (input/output)
3. **Open related files** before prompting to give Copilot context
4. **Be specific about error handling**: Always mention Result types and validation requirements
