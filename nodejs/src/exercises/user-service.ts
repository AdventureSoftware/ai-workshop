/**
 * Exercise 2: Context Injection Practice
 * 
 * This file demonstrates how to provide rich context for AI Copilot.
 * Based on presentation: OAuth2 + API keys, rate limiting, audit logging
 * 
 * SETUP INSTRUCTIONS:
 * 1. Open these files simultaneously for context:
 *    - types/auth.ts (User, AuthToken, ValidationResult interfaces)
 *    - STYLE_GUIDE.md (error handling patterns)
 *    - API_SCHEMA.md (business rules and validation requirements)
 * 
 * 2. Keep files open while working - Copilot uses open tabs as context
 * 3. Use Copilot Chat with # references: "Refactor #registerUser to use #ValidationResult pattern"
 */

import { ValidationResult, User, LoginCredentials, AuthToken, UserRegistration } from '../types/auth';
import bcryptjs from 'bcryptjs';
import jwt from 'jsonwebtoken';

// Rate limiting interface - demonstrates context for AI
interface RateLimitConfig {
  maxAttempts: number;
  windowMinutes: number;
  blockDurationMinutes: number;
}

// Audit log interface - follows security patterns from presentation
interface AuditEvent {
  userId?: string;
  action: 'login_attempt' | 'login_success' | 'registration' | 'rate_limit_exceeded';
  ipAddress: string;
  timestamp: Date;
  metadata?: Record<string, any>;
}

// TODO: Exercise 2a - Context Injection with Multi-File Awareness
// 
// CONTEXT FOR AI:
// - This service implements OAuth2-style authentication per API_SCHEMA.md
// - Uses bcrypt for password hashing (already imported)
// - Follows ValidationResult pattern from types/auth.ts (already open)
// - Rate limiting: 5 login attempts per 15 minutes per IP
// - All auth events must be audited for security compliance
//
// Function: Register new user with comprehensive validation
// Constraints: 
//   - Email must be unique and RFC 5322 compliant
//   - Password requirements: 8+ chars, 1 uppercase, 1 lowercase, 1 number, 1 symbol
//   - Rate limiting: max 3 registrations per IP per hour
//   - Hash passwords with bcrypt (cost factor 12)
//   - Return JWT token valid for 24 hours
// Examples:
//   - Valid: {email: "user@example.com", name: "John Doe", password: "SecurePass1!"} 
//     → {success: true, data: {user: User, token: AuthToken}}
//   - Duplicate email: → {success: false, error: "Email already registered"}
//   - Weak password: → {success: false, error: "Password must contain uppercase, lowercase, number, and symbol"}
async function registerUser(userData: UserRegistration, ipAddress: string): Promise<ValidationResult<{user: User, token: AuthToken}>> {
  // TODO: Let Copilot implement using context from open files
  // Reference STYLE_GUIDE.md error handling patterns
  // Use ValidationResult type from types/auth.ts
  // Follow business rules from API_SCHEMA.md
  throw new Error('Not implemented - use Copilot with multi-file context');
}

// TODO: Exercise 2b - Advanced Context with Rate Limiting
//
// CONTEXT CLUES FOR AI:
// This function demonstrates the "Edge-Case Booster" pattern from presentation
// Open tabs should include: types/auth.ts, STYLE_GUIDE.md for error patterns
//
// Function: Authenticate user with comprehensive security measures
// Constraints:
//   - Verify password using bcrypt.compare()
//   - Rate limiting per RateLimitConfig above (5 attempts/15min)
//   - Generate JWT with 24-hour expiry
//   - Log all authentication events for audit trail
// Edge cases to handle:
//   - null/undefined inputs (return validation error)
//   - account disabled/locked (return specific error, don't reveal if email exists)
//   - rate limit exceeded (return 429-style error)
//   - bcrypt comparison failures (handle gracefully)
//   - JWT signing failures (log error, return generic failure)
// Examples:
//   - Success: {email: "user@example.com", password: "correct"} 
//     → {success: true, data: {token: "jwt_token", expiresAt: Date}}
//   - Wrong password: → {success: false, error: "Invalid credentials"}
//   - Rate limited: → {success: false, error: "Too many login attempts. Try again in 15 minutes."}
async function authenticateUser(credentials: LoginCredentials, ipAddress: string): Promise<ValidationResult<AuthToken>> {
  // TODO: AI should generate robust error handling based on edge cases above
  // Use the RateLimitConfig and AuditEvent interfaces for context
  throw new Error('Not implemented - demonstrate edge-case handling with context');
}

// TODO: Exercise 2c - Documentation-Driven Development
// 
// CONTEXT FROM PRESENTATION (lines 352-368):
// User Authentication Service Requirements:
// - Support OAuth2 + API keys  
// - Rate limiting: 100 req/min per user
// - Audit log all auth events
// - API Design: POST /auth/login -> {token, expires_at}
// - Error Handling: 429 for rate limits, 401 for invalid credentials
//
// Function: Generate API key for authenticated users (OAuth2 pattern)
// Constraints:
//   - API keys should be cryptographically secure (32 bytes, base64 encoded)
//   - Rate limit: 100 requests per minute per user
//   - Store key hash, not plain key (for security)
//   - Keys expire after 90 days unless refreshed
//   - Audit log key generation events
// Examples:
//   - Success: generateApiKey("user123") → {success: true, data: {apiKey: "ak_...", expiresAt: Date}}
//   - Rate limited: → {success: false, error: "API key generation rate limited"}
async function generateApiKey(userId: string): Promise<ValidationResult<{apiKey: string, expiresAt: Date}>> {
  // TODO: Implement following documentation-driven development pattern
  // Use crypto.randomBytes for secure key generation
  throw new Error('Implement using OAuth2 patterns from presentation context');
}

// TODO: Exercise 2d - Workspace Symbol Context
//
// These functions reference types/interfaces defined elsewhere in workspace
// Copilot should use workspace index to understand relationships
//
// Function: Refresh expired JWT tokens
// Context: References AuthToken type from workspace, follows JWT patterns
// Constraints:
//   - Only refresh tokens within 7 days of expiry
//   - Generate new token with same user claims
//   - Invalidate old token (add to blacklist)
//   - Rate limit: 1 refresh per hour per user
async function refreshAuthToken(existingToken: string): Promise<ValidationResult<AuthToken>> {
  // TODO: AI should reference AuthToken type definition and JWT patterns
  throw new Error('Implement using workspace symbol context');
}

// TODO: Exercise 2e - Real-World Integration Example
//
// CONTEXT: This mirrors the "Real-World AI-Assisted Projects" from presentation
// Should integrate with external services while maintaining security
//
// Function: Validate OAuth2 authorization code and exchange for tokens
// Constraints:
//   - Support Google OAuth2 flow
//   - Validate state parameter to prevent CSRF
//   - Exchange code for access token via Google API
//   - Create or update user record based on Google profile
//   - Generate internal JWT for session management
// Security considerations:
//   - Verify OAuth2 state matches session
//   - Validate Google token signature
//   - Handle OAuth2 errors gracefully
//   - Rate limit OAuth attempts
async function handleOAuthCallback(
  authorizationCode: string, 
  state: string, 
  sessionState: string
): Promise<ValidationResult<{user: User, token: AuthToken}>> {
  // TODO: Complex OAuth2 integration with security best practices
  throw new Error('Implement OAuth2 flow with security context');
}

// HELPER FUNCTIONS FOR CONTEXT
// These provide additional context clues for AI about expected patterns

/**
 * Rate limiting helper - demonstrates expected error handling pattern
 * AI can reference this for consistent rate limiting across functions
 */
async function checkRateLimit(key: string, config: RateLimitConfig): Promise<ValidationResult<boolean>> {
  // TODO: Implement using Redis or in-memory cache
  throw new Error('Rate limiting implementation needed');
}

/**
 * Audit logging helper - shows expected audit pattern
 * AI can reference this for consistent logging across auth functions
 */
async function logAuditEvent(event: AuditEvent): Promise<void> {
  // TODO: Log to secure audit storage (database, log service)
  throw new Error('Audit logging implementation needed');
}

/**
 * Password validation helper - demonstrates validation patterns
 * AI can use this context for consistent password requirements
 */
function validatePassword(password: string): ValidationResult<string> {
  // TODO: Implement password strength validation
  throw new Error('Password validation implementation needed');
}

// VERIFICATION CHECKLIST FOR AI-GENERATED CODE
//
// After AI generates implementations, apply R.E.D. verification:
//
// READ:
// ✅ Does the code solve the OAuth2 + API key requirements?
// ✅ Are rate limiting constraints properly implemented?
// ✅ Does error handling follow ValidationResult patterns?
// ✅ Are security best practices applied (hashing, validation)?
// ✅ Are audit events logged for compliance?
//
// EXECUTE:
// ✅ Run tests with valid/invalid inputs
// ✅ Test rate limiting behavior
// ✅ Verify password hashing works correctly
// ✅ Check JWT token generation and validation
// ✅ Test OAuth2 flow with mocked Google API
//
// DIFF-REVIEW:
// ✅ Compare with presentation requirements (OAuth2, rate limiting, audit)
// ✅ Check against security best practices
// ✅ Verify business logic matches API_SCHEMA.md rules
// ✅ Ensure integration with existing auth types

export {
  registerUser,
  authenticateUser,
  generateApiKey,
  refreshAuthToken,
  handleOAuthCallback,
  checkRateLimit,
  logAuditEvent,
  validatePassword,
  RateLimitConfig,
  AuditEvent
};