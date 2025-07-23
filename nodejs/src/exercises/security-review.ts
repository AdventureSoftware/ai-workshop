/**
 * Exercise 4: Security Verification with AI
 * 
 * This exercise demonstrates security review of AI-generated code.
 * Based on presentation lines 430-438: "Add input validation & safe defaults. Flag any potential CWE-"
 * 
 * LEARNING OBJECTIVES:
 * 1. Identify common security vulnerabilities in AI-generated code
 * 2. Apply security-focused prompting techniques
 * 3. Use R.E.D. checklist with security emphasis
 * 4. Implement defensive coding patterns
 * 
 * WORKFLOW:
 * 1. Review AI-generated code examples below for security issues
 * 2. Use prompts like: "Add input validation & safe defaults. Flag any potential security issues"
 * 3. Apply security R.E.D. checklist
 * 4. Implement secure versions following OWASP guidelines
 */

import { ValidationResult } from '../types/auth';
import { z } from 'zod';

// ❌ INSECURE AI-GENERATED CODE EXAMPLES
// These represent typical AI outputs that need security review

/**
 * EXAMPLE 1: SQL Injection Vulnerability
 * AI often generates code like this without proper parameterization
 */
class InsecureUserDatabase {
  // ❌ VULNERABLE: Direct string concatenation in SQL
  async getUserByEmail(email: string): Promise<any> {
    // This is intentionally vulnerable code for educational purposes
    const query = `SELECT * FROM users WHERE email = '${email}'`;
    // return await database.query(query);
    
    // SECURITY ISSUES:
    // - SQL injection: email could be "'; DROP TABLE users; --"
    // - No input validation
    // - Reveals database structure in errors
    // - Returns sensitive data (password hashes, etc.)
    
    throw new Error('INSECURE CODE - Do not use in production');
  }

  // ❌ VULNERABLE: Dynamic query construction
  async searchUsers(searchTerm: string, orderBy: string): Promise<any[]> {
    const query = `SELECT * FROM users WHERE name LIKE '%${searchTerm}%' ORDER BY ${orderBy}`;
    
    // SECURITY ISSUES:
    // - SQL injection in both searchTerm and orderBy
    // - No input sanitization
    // - Information disclosure through error messages
    
    throw new Error('INSECURE CODE - Review with security checklist');
  }
}

/**
 * EXAMPLE 2: Authentication Bypass
 * AI might generate weak authentication logic
 */
class InsecureAuthService {
  // ❌ VULNERABLE: Weak password validation
  validatePassword(password: string): boolean {
    // AI might generate overly permissive validation
    return password.length >= 6; // Too weak!
    
    // SECURITY ISSUES:
    // - No complexity requirements
    // - Common passwords allowed
    // - No rate limiting considerations
  }

  // ❌ VULNERABLE: Timing attack susceptible
  async authenticateUser(email: string, password: string): Promise<boolean> {
    // const user = await this.getUserByEmail(email);
    // if (!user) {
    //   return false; // Early return reveals if email exists
    // }
    // return user.password === password; // Plain text comparison!
    
    // SECURITY ISSUES:
    // - Timing attacks possible
    // - No password hashing
    // - Information leakage about user existence
    // - No rate limiting
    
    throw new Error('INSECURE CODE - Multiple vulnerabilities');
  }
}

/**
 * EXAMPLE 3: XSS and Input Validation Issues
 * AI often misses sanitization requirements
 */
class InsecureDataHandler {
  // ❌ VULNERABLE: No input sanitization
  processUserInput(userInput: string): string {
    // AI might generate code that directly uses user input
    return `<div>Welcome ${userInput}!</div>`;
    
    // SECURITY ISSUES:
    // - XSS vulnerability
    // - No HTML encoding
    // - Direct HTML injection possible
  }

  // ❌ VULNERABLE: Insufficient validation
  processFileUpload(filename: string, content: Buffer): ValidationResult<string> {
    // Basic validation without security considerations
    if (!filename) {
      return { success: false, error: 'Filename required' };
    }

    // const filepath = `./uploads/${filename}`;
    // fs.writeFileSync(filepath, content);
    
    // SECURITY ISSUES:
    // - Path traversal: "../../../etc/passwd"
    // - No file type validation
    // - No size limits
    // - No malware scanning
    
    throw new Error('INSECURE CODE - Path traversal and other issues');
  }
}

// ✅ SECURE IMPLEMENTATIONS
// These demonstrate how to prompt AI for security-focused code

/**
 * SECURE PROMPTING EXAMPLE:
 * "Create secure user database class with:
 * - Parameterized queries to prevent SQL injection
 * - Input validation using Zod schemas
 * - Error handling that doesn't leak information
 * - Rate limiting considerations
 * - Audit logging for security events"
 */
class SecureUserDatabase {
  // TODO: Exercise 4a - Secure Database Operations
  // 
  // PROMPT: "Implement secure user lookup with SQL injection prevention:
  // - Use parameterized queries only
  // - Validate email format with Zod schema
  // - Return minimal user data (no sensitive fields)
  // - Log failed lookup attempts for monitoring
  // - Handle database errors without information leakage
  // Examples:
  //   - Valid: getUserByEmail('user@example.com') → {id, email, name, role}
  //   - Invalid email: → {success: false, error: 'Invalid email format'}
  //   - User not found: → {success: false, error: 'User not found'} (same as invalid email)
  //   - Database error: → {success: false, error: 'Service unavailable'}"
  async getUserByEmail(email: string): Promise<ValidationResult<any>> {
    // TODO: AI should generate secure database operations
    throw new Error('Implement with security-focused prompting');
  }

  // TODO: Exercise 4b - Secure Search with Input Sanitization
  //
  // PROMPT: "Create secure user search function:
  // - Sanitize search terms to prevent injection
  // - Validate orderBy against whitelist of allowed columns
  // - Use parameterized queries exclusively  
  // - Implement pagination to prevent large data exposure
  // - Rate limit search operations per user
  // - Log suspicious search patterns
  // Security constraints:
  //   - searchTerm: alphanumeric + spaces only, max 100 chars
  //   - orderBy: must be one of ['name', 'email', 'created_at']
  //   - maxResults: limit to 50 per query
  // Examples:
  //   - searchUsers('john', 'name') → paginated results
  //   - searchUsers('john<script>', 'name') → validation error
  //   - searchUsers('john', 'DROP TABLE') → validation error"
  async searchUsers(
    searchTerm: string, 
    orderBy: string, 
    page: number = 1
  ): Promise<ValidationResult<any[]>> {
    // TODO: AI should generate secure search with comprehensive validation
    throw new Error('Implement with input sanitization and SQL injection prevention');
  }
}

/**
 * SECURITY-FIRST AUTH SERVICE
 */
class SecureAuthService {
  // TODO: Exercise 4c - Secure Password Validation
  //
  // PROMPT: "Create secure password validator following OWASP guidelines:
  // - Minimum 12 characters length
  // - Require uppercase, lowercase, number, special character
  // - Block common passwords (integrate with known password lists)
  // - Check for dictionary words and patterns
  // - Rate limit validation attempts to prevent brute force
  // - Return detailed feedback for UX while avoiding enumeration
  // Security considerations:
  //   - Constant-time comparison where possible
  //   - No password logging or storage in plain text
  //   - Memory-safe string handling
  // Examples:
  //   - 'Password123!' → {valid: true, strength: 'good'}
  //   - 'password' → {valid: false, errors: ['too_short', 'no_uppercase', 'common_password']}
  //   - '123456789012' → {valid: false, errors: ['no_letters', 'predictable_pattern']}"
  validatePasswordSecurity(password: string): ValidationResult<{strength: string, feedback: string[]}> {
    // TODO: AI should generate OWASP-compliant password validation
    throw new Error('Implement with security-focused requirements');
  }

  // TODO: Exercise 4d - Timing-Attack Resistant Authentication
  //
  // PROMPT: "Create secure user authentication resistant to timing attacks:
  // - Use bcrypt for password hashing and verification
  // - Implement constant-time comparison for user lookup
  // - Apply same processing time whether user exists or not
  // - Rate limiting: 5 attempts per IP per 15 minutes
  // - Account lockout: 5 failed attempts locks account for 30 minutes
  // - Audit logging for all authentication events
  // - No information leakage about user existence
  // Security measures:
  //   - Always perform bcrypt comparison even for non-existent users
  //   - Return same error message for invalid credentials vs non-existent user
  //   - Implement progressive delays for repeated failures
  //   - Log suspicious patterns (multiple usernames from same IP)
  // Examples:
  //   - Valid credentials → {success: true, token: 'jwt_token'}
  //   - Invalid password → {success: false, error: 'Invalid credentials'} (after same processing time)
  //   - Non-existent user → {success: false, error: 'Invalid credentials'} (after same processing time)
  //   - Rate limited → {success: false, error: 'Too many attempts. Try again later.'}"
  async authenticateUserSecurely(
    email: string, 
    password: string, 
    ipAddress: string
  ): Promise<ValidationResult<{token: string}>> {
    // TODO: AI should generate timing-attack resistant authentication
    throw new Error('Implement with constant-time operations and rate limiting');
  }
}

/**
 * SECURE DATA HANDLING
 */
class SecureDataHandler {
  // TODO: Exercise 4e - XSS Prevention and Input Sanitization
  //
  // PROMPT: "Create secure user input processor with XSS prevention:
  // - HTML encode all user input before display
  // - Validate input against strict schema (alphanumeric + basic punctuation)
  // - Content Security Policy headers support
  // - Input length limits (prevent DoS)
  // - Character encoding validation (UTF-8 only)
  // - Sanitize against script injection patterns
  // Security constraints:
  //   - Maximum input length: 1000 characters
  //   - Allowed characters: letters, numbers, spaces, basic punctuation
  //   - Block HTML tags, script content, data URLs
  //   - Normalize Unicode to prevent bypass attempts
  // Examples:
  //   - 'Hello John' → '<div>Welcome Hello John!</div>'
  //   - '<script>alert(1)</script>' → Validation error
  //   - 'Hello <b>John</b>' → 'Hello &lt;b&gt;John&lt;/b&gt;' → '<div>Welcome Hello &lt;b&gt;John&lt;/b&gt;!</div>'"
  processUserInputSecurely(userInput: string): ValidationResult<string> {
    // TODO: AI should generate XSS-safe input processing
    throw new Error('Implement with HTML encoding and input validation');
  }

  // TODO: Exercise 4f - Secure File Upload with Path Traversal Prevention
  //
  // PROMPT: "Create secure file upload handler preventing path traversal and malware:
  // - Validate filename against strict whitelist (alphanumeric, dots, dashes only)
  // - Prevent directory traversal (../, .\\, absolute paths)
  // - File type validation using magic bytes, not just extension
  // - Size limits: max 10MB per file
  // - Generate secure random filename (don't trust user input)
  // - Virus scanning integration hooks
  // - Quarantine suspicious files
  // Security measures:
  //   - Whitelist allowed file extensions ['.jpg', '.png', '.pdf', '.docx']
  //   - Validate MIME types match extensions
  //   - Check file headers/magic bytes
  //   - Store files outside web root
  //   - Never execute uploaded files
  // Examples:
  //   - 'document.pdf' + valid PDF → {success: true, filename: 'abc123.pdf'}
  //   - '../../../etc/passwd' → {success: false, error: 'Invalid filename'}
  //   - 'image.jpg' + non-JPEG data → {success: false, error: 'File type mismatch'}"
  async processFileUploadSecurely(
    originalFilename: string, 
    content: Buffer, 
    mimeType: string
  ): Promise<ValidationResult<{filename: string, size: number}>> {
    // TODO: AI should generate secure file upload with comprehensive validation
    throw new Error('Implement with path traversal prevention and file validation');
  }
}

// SECURITY VALIDATION SCHEMAS
// These provide context for AI about expected security patterns

const EmailSchema = z.string()
  .email('Invalid email format')
  .min(5, 'Email too short')
  .max(254, 'Email too long') // RFC 5322 limit
  .toLowerCase()
  .trim();

const SecurePasswordSchema = z.string()
  .min(12, 'Password must be at least 12 characters')
  .regex(/[a-z]/, 'Password must contain lowercase letter')
  .regex(/[A-Z]/, 'Password must contain uppercase letter')
  .regex(/[0-9]/, 'Password must contain number')
  .regex(/[^a-zA-Z0-9]/, 'Password must contain special character');

const SafeTextInputSchema = z.string()
  .max(1000, 'Input too long')
  .regex(/^[a-zA-Z0-9\s.,!?-]+$/, 'Input contains invalid characters')
  .trim();

// SECURITY R.E.D. VERIFICATION CHECKLIST
//
// When reviewing AI-generated security code, apply this enhanced R.E.D. process:
//
// READ - Security Code Review:
// ✅ Input validation: Are all inputs validated against strict schemas?
// ✅ Output encoding: Is data properly encoded before display/storage?
// ✅ Authentication: Are timing attacks prevented?
// ✅ Authorization: Are permissions checked at every access point?
// ✅ Error handling: Do error messages avoid information leakage?
// ✅ Rate limiting: Are brute force attacks prevented?
// ✅ Logging: Are security events properly logged?
// ✅ Dependencies: Are security libraries used correctly?
//
// EXECUTE - Security Testing:
// ✅ Injection attacks: Test SQL injection, XSS, command injection
// ✅ Authentication bypass: Try invalid tokens, expired sessions
// ✅ Authorization bypass: Test accessing other users' data
// ✅ Rate limiting: Verify limits are enforced
// ✅ Input fuzzing: Test with malformed, oversized, malicious inputs
// ✅ Error conditions: Verify errors don't leak sensitive information
//
// DIFF-REVIEW - Security Architecture:
// ✅ Defense in depth: Multiple security layers implemented?
// ✅ Principle of least privilege: Minimal permissions granted?
// ✅ Fail secure: Does system fail to secure state?
// ✅ Security by design: Security built in, not bolted on?
// ✅ OWASP compliance: Follows security best practices?

// COMMON AI SECURITY ANTI-PATTERNS TO WATCH FOR:
//
// 1. String concatenation in SQL queries
// 2. Direct user input rendering without encoding
// 3. Weak password requirements
// 4. Information disclosure in error messages
// 5. Missing rate limiting on sensitive operations
// 6. Insecure file upload handling
// 7. Improper session management
// 8. Missing input validation
// 9. Hardcoded secrets or credentials
// 10. Insufficient logging of security events

export {
  InsecureUserDatabase,
  InsecureAuthService,
  InsecureDataHandler,
  SecureUserDatabase,
  SecureAuthService,
  SecureDataHandler,
  EmailSchema,
  SecurePasswordSchema,
  SafeTextInputSchema
};