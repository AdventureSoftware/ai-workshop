using AIPairProgramming.Shared.Models;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace AIPairProgramming.Exercises;

/// <summary>
/// Exercise 4: Security Verification with AI
/// 
/// This exercise demonstrates security review of AI-generated code.
/// Based on presentation lines 430-438: "Add input validation & safe defaults. Flag any potential CWE-"
/// 
/// LEARNING OBJECTIVES:
/// 1. Identify common security vulnerabilities in AI-generated code
/// 2. Apply security-focused prompting techniques
/// 3. Use R.E.D. checklist with security emphasis
/// 4. Implement defensive coding patterns
/// 
/// WORKFLOW:
/// 1. Review AI-generated code examples below for security issues
/// 2. Use prompts like: "Add input validation & safe defaults. Flag any potential security issues"
/// 3. Apply security R.E.D. checklist
/// 4. Implement secure versions following OWASP guidelines
/// </summary>

// ❌ INSECURE AI-GENERATED CODE EXAMPLES
// These represent typical AI outputs that need security review

/// <summary>
/// EXAMPLE 1: SQL Injection Vulnerability
/// AI often generates code like this without proper parameterization
/// </summary>
public class InsecureUserDatabase
{
    // ❌ VULNERABLE: Direct string interpolation in SQL
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        // This is intentionally vulnerable code for educational purposes
        var query = $"SELECT * FROM users WHERE email = '{email}'";
        // return await database.QuerySingleOrDefaultAsync<User>(query);
        
        // SECURITY ISSUES:
        // - SQL injection: email could be "'; DROP TABLE users; --"
        // - No input validation
        // - Reveals database structure in errors
        // - Returns sensitive data (password hashes, etc.)
        
        throw new NotImplementedException("INSECURE CODE - Do not use in production");
    }

    // ❌ VULNERABLE: Dynamic query construction
    public async Task<List<User>> SearchUsersAsync(string searchTerm, string orderBy)
    {
        var query = $"SELECT * FROM users WHERE name LIKE '%{searchTerm}%' ORDER BY {orderBy}";
        
        // SECURITY ISSUES:
        // - SQL injection in both searchTerm and orderBy
        // - No input sanitization
        // - Information disclosure through error messages
        
        throw new NotImplementedException("INSECURE CODE - Review with security checklist");
    }
}

/// <summary>
/// EXAMPLE 2: Authentication Bypass
/// AI might generate weak authentication logic
/// </summary>
public class InsecureAuthService
{
    // ❌ VULNERABLE: Weak password validation
    public bool ValidatePassword(string password)
    {
        // AI might generate overly permissive validation
        return password.Length >= 6; // Too weak!
        
        // SECURITY ISSUES:
        // - No complexity requirements
        // - Common passwords allowed
        // - No rate limiting considerations
    }

    // ❌ VULNERABLE: Timing attack susceptible
    public async Task<bool> AuthenticateUserAsync(string email, string password)
    {
        // var user = await GetUserByEmailAsync(email);
        // if (user == null)
        // {
        //     return false; // Early return reveals if email exists
        // }
        // return user.Password == password; // Plain text comparison!
        
        // SECURITY ISSUES:
        // - Timing attacks possible
        // - No password hashing
        // - Information leakage about user existence
        // - No rate limiting
        
        throw new NotImplementedException("INSECURE CODE - Multiple vulnerabilities");
    }
}

/// <summary>
/// EXAMPLE 3: XSS and Input Validation Issues
/// AI often misses sanitization requirements
/// </summary>
public class InsecureDataHandler
{
    // ❌ VULNERABLE: No input sanitization
    public string ProcessUserInput(string userInput)
    {
        // AI might generate code that directly uses user input
        return $"<div>Welcome {userInput}!</div>";
        
        // SECURITY ISSUES:
        // - XSS vulnerability
        // - No HTML encoding
        // - Direct HTML injection possible
    }

    // ❌ VULNERABLE: Insufficient validation
    public ValidationResult<string> ProcessFileUpload(string filename, byte[] content)
    {
        // Basic validation without security considerations
        if (string.IsNullOrEmpty(filename))
        {
            return ValidationResult<string>.FailureResult("Filename required");
        }

        // var filepath = Path.Combine("./uploads", filename);
        // File.WriteAllBytes(filepath, content);
        
        // SECURITY ISSUES:
        // - Path traversal: "../../../etc/passwd"
        // - No file type validation
        // - No size limits
        // - No malware scanning
        
        throw new NotImplementedException("INSECURE CODE - Path traversal and other issues");
    }
}

// ✅ SECURE IMPLEMENTATIONS
// These demonstrate how to prompt AI for security-focused code

/// <summary>
/// SECURE PROMPTING EXAMPLE:
/// "Create secure user database class with:
/// - Parameterized queries to prevent SQL injection
/// - Input validation using FluentValidation
/// - Error handling that doesn't leak information
/// - Rate limiting considerations
/// - Audit logging for security events"
/// </summary>
public class SecureUserDatabase(ILogger<SecureUserDatabase> logger)
{
    // TODO: Exercise 4a - Secure Database Operations
    // 
    // PROMPT: "Implement secure user lookup with SQL injection prevention:
    // - Use parameterized queries with Dapper or EF Core only
    // - Validate email format with FluentValidation
    // - Return minimal user data (no sensitive fields)
    // - Log failed lookup attempts for monitoring
    // - Handle database errors without information leakage
    // Examples:
    //   - Valid: GetUserByEmailAsync("user@example.com") → {Id, Email, Name, Role}
    //   - Invalid email: → {Success: false, Error: "Invalid email format"}
    //   - User not found: → {Success: false, Error: "User not found"} (same as invalid email)
    //   - Database error: → {Success: false, Error: "Service unavailable"}"
    public async Task<ValidationResult<User?>> GetUserByEmailAsync(string email)
    {
        // TODO: AI should generate secure database operations
        throw new NotImplementedException("Implement with security-focused prompting");
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
    //   - orderBy: must be one of ['Name', 'Email', 'CreatedAt']
    //   - maxResults: limit to 50 per query
    // Examples:
    //   - SearchUsersAsync('john', 'Name') → paginated results
    //   - SearchUsersAsync('john<script>', 'Name') → validation error
    //   - SearchUsersAsync('john', 'DROP TABLE') → validation error"
    public async Task<ValidationResult<List<User>>> SearchUsersAsync(
        string searchTerm, 
        string orderBy, 
        int page = 1)
    {
        // TODO: AI should generate secure search with comprehensive validation
        throw new NotImplementedException("Implement with input sanitization and SQL injection prevention");
    }
}

/// <summary>
/// SECURITY-FIRST AUTH SERVICE
/// </summary>
public class SecureAuthService(ILogger<SecureAuthService> logger)
{
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
    //   - 'Password123!' → {Valid: true, Strength: 'good'}
    //   - 'password' → {Valid: false, Errors: ['too_short', 'no_uppercase', 'common_password']}
    //   - '123456789012' → {Valid: false, Errors: ['no_letters', 'predictable_pattern']}"
    public ValidationResult<PasswordStrengthResult> ValidatePasswordSecurity(string password)
    {
        // TODO: AI should generate OWASP-compliant password validation
        throw new NotImplementedException("Implement with security-focused requirements");
    }

    // TODO: Exercise 4d - Timing-Attack Resistant Authentication
    //
    // PROMPT: "Create secure user authentication resistant to timing attacks:
    // - Use BCrypt for password hashing and verification
    // - Implement constant-time comparison for user lookup
    // - Apply same processing time whether user exists or not
    // - Rate limiting: 5 attempts per IP per 15 minutes
    // - Account lockout: 5 failed attempts locks account for 30 minutes
    // - Audit logging for all authentication events
    // - No information leakage about user existence
    // Security measures:
    //   - Always perform BCrypt comparison even for non-existent users
    //   - Return same error message for invalid credentials vs non-existent user
    //   - Implement progressive delays for repeated failures
    //   - Log suspicious patterns (multiple usernames from same IP)
    // Examples:
    //   - Valid credentials → {Success: true, Token: 'jwt_token'}
    //   - Invalid password → {Success: false, Error: 'Invalid credentials'} (after same processing time)
    //   - Non-existent user → {Success: false, Error: 'Invalid credentials'} (after same processing time)
    //   - Rate limited → {Success: false, Error: 'Too many attempts. Try again later.'}"
    public async Task<ValidationResult<AuthToken>> AuthenticateUserSecurelyAsync(
        string email, 
        string password, 
        string ipAddress, 
        CancellationToken cancellationToken = default)
    {
        // TODO: AI should generate timing-attack resistant authentication
        throw new NotImplementedException("Implement with constant-time operations and rate limiting");
    }
}

/// <summary>
/// SECURE DATA HANDLING
/// </summary>
public class SecureDataHandler(ILogger<SecureDataHandler> logger)
{
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
    public ValidationResult<string> ProcessUserInputSecurely(string userInput)
    {
        // TODO: AI should generate XSS-safe input processing
        throw new NotImplementedException("Implement with HTML encoding and input validation");
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
    //   - 'document.pdf' + valid PDF → {Success: true, Filename: 'abc123.pdf'}
    //   - '../../../etc/passwd' → {Success: false, Error: 'Invalid filename'}
    //   - 'image.jpg' + non-JPEG data → {Success: false, Error: 'File type mismatch'}"
    public async Task<ValidationResult<FileUploadResult>> ProcessFileUploadSecurelyAsync(
        string originalFilename, 
        byte[] content, 
        string mimeType,
        CancellationToken cancellationToken = default)
    {
        // TODO: AI should generate secure file upload with comprehensive validation
        throw new NotImplementedException("Implement with path traversal prevention and file validation");
    }
}

// SECURITY VALIDATION RECORDS AND ENUMS
// These provide context for AI about expected security patterns

public record PasswordStrengthResult(
    bool IsValid,
    string Strength, // "weak", "fair", "good", "strong"
    List<string> Feedback
);

public record FileUploadResult(
    string Filename,
    long Size,
    string ContentType
);

// COMMON AI SECURITY ANTI-PATTERNS TO WATCH FOR:
//
// 1. String interpolation in SQL queries
// 2. Direct user input rendering without encoding
// 3. Weak password requirements
// 4. Information disclosure in error messages
// 5. Missing rate limiting on sensitive operations
// 6. Insecure file upload handling
// 7. Improper session management
// 8. Missing input validation
// 9. Hardcoded secrets or credentials
// 10. Insufficient logging of security events

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

/// <summary>
/// Example of iterative security prompt refinement
/// Shows how to improve AI suggestions for security-critical code
/// </summary>
public class SecurityPromptIteration
{
    // ❌ First try: "Validate user input"
    public bool ValidateInputV1(string input)
    {
        return !string.IsNullOrEmpty(input);
    }

    // ❌ Second try: "Validate user input with security"
    public ValidationResult<string> ValidateInputV2(string input)
    {
        if (string.IsNullOrEmpty(input))
            return ValidationResult<string>.FailureResult("Input required");
        
        if (input.Length > 1000)
            return ValidationResult<string>.FailureResult("Input too long");
            
        return ValidationResult<string>.SuccessResult(input);
    }

    // ✅ Third try: Security-focused specification
    // "Create secure input validator preventing XSS and injection attacks:
    // - HTML encode all output
    // - Validate against whitelist: alphanumeric + spaces + basic punctuation only
    // - Block script tags, event handlers, data URLs
    // - Limit length to 1000 characters
    // - Log suspicious input patterns
    // - Return detailed validation errors for legitimate failures
    // Security tests: input('<script>alert(1)</script>') should fail with specific error"
    public ValidationResult<string> ValidateInputSecure(string input)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrEmpty(input))
        {
            errors.Add(new ValidationError("input", "REQUIRED", "Input is required"));
            return ValidationResult<string>.FailureResult(errors);
        }

        if (input.Length > 1000)
        {
            errors.Add(new ValidationError("input", "TOO_LONG", "Input exceeds maximum length"));
        }

        // Security validation
        if (ContainsHtmlTags(input))
        {
            errors.Add(new ValidationError("input", "HTML_DETECTED", "HTML content not allowed"));
        }

        if (ContainsScriptContent(input))
        {
            errors.Add(new ValidationError("input", "SCRIPT_DETECTED", "Script content detected"));
        }

        if (!IsAllowedCharacters(input))
        {
            errors.Add(new ValidationError("input", "INVALID_CHARACTERS", "Contains disallowed characters"));
        }

        if (errors.Count > 0)
        {
            // Log suspicious input for security monitoring
            logger.LogWarning("Suspicious input detected from user: {InputLength} chars, {ErrorCount} violations", 
                input.Length, errors.Count);
            return ValidationResult<string>.FailureResult(errors);
        }

        // HTML encode the output for safe display
        var encodedInput = System.Net.WebUtility.HtmlEncode(input);
        return ValidationResult<string>.SuccessResult(encodedInput);
    }

    private static bool ContainsHtmlTags(string input) => 
        Regex.IsMatch(input, @"<[^>]*>", RegexOptions.IgnoreCase);

    private static bool ContainsScriptContent(string input) => 
        Regex.IsMatch(input, @"<script|javascript:|vbscript:|onload=|onerror=", RegexOptions.IgnoreCase);

    private static bool IsAllowedCharacters(string input) => 
        Regex.IsMatch(input, @"^[a-zA-Z0-9\s.,!?'""\-]+$");
}