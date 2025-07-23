/**
 * Jest test setup file
 * Configure global test utilities and mocks
 */

// Extend Jest matchers if needed
expect.extend({
  toBeValidationSuccess(received: any) {
    const pass = received && received.success === true && received.data !== undefined;
    return {
      message: () => `expected ${JSON.stringify(received)} to be a successful validation result`,
      pass,
    };
  },

  toBeValidationError(received: any, expectedError?: string) {
    const pass = received && received.success === false && received.error;
    const errorMatches = expectedError ? received.error.includes(expectedError) : true;

    return {
      message: () => `expected ${JSON.stringify(received)} to be a validation error${expectedError ? ` containing "${expectedError}"` : ''}`,
      pass: pass && errorMatches,
    };
  },
});

// Global test utilities
global.testHelpers = {
  createValidPaymentRequest: () => ({
    amount: 2999, // $29.99
    currency: 'USD' as const,
    customerId: '550e8400-e29b-41d4-a716-446655440000',
    description: 'Test payment'
  }),

  createValidUser: () => ({
    id: '550e8400-e29b-41d4-a716-446655440000',
    email: 'test@example.com',
    name: 'Test User',
    role: 'user' as const,
    createdAt: new Date(),
    lastLoginAt: new Date(),
    isActive: true
  })
};

// Mock console methods in tests by default
beforeEach(() => {
  jest.spyOn(console, 'log').mockImplementation(() => {});
  jest.spyOn(console, 'warn').mockImplementation(() => {});
  jest.spyOn(console, 'error').mockImplementation(() => {});
});

afterEach(() => {
  jest.restoreAllMocks();
});

// Type declarations for custom matchers
declare global {
  namespace jest {
    interface Matchers<R> {
      toBeValidationSuccess(): R;
      toBeValidationError(expectedError?: string): R;
    }
  }

  var testHelpers: {
    createValidPaymentRequest(): any;
    createValidUser(): any;
  };
}
