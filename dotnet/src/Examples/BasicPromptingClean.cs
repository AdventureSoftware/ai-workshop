using AIPairProgramming.Shared.Models;

namespace AIPairProgramming.Examples;

public class BasicPromptingClean
{
    public async Task<ValidationResult<AuthToken>> AuthenticateUserAsync(LoginCredentials credentials)
    {
        throw new NotImplementedException();
    }

    public decimal CalculateDiscount(string loyaltyTier, decimal orderTotal)
    {
        throw new NotImplementedException();
    }

    public decimal GetDiscount(string tier, decimal total)
    {
        throw new NotImplementedException();
    }

    public class DiscountCalculator(IPricingStrategy pricingStrategy)
    {
        public async Task<ValidationResult<decimal>> CalculateCustomerDiscountAsync(string customerId, decimal orderAmount)
        {
            throw new NotImplementedException();
        }
    }

    public async Task<ValidationResult<User>> RegisterUserAsync(UserRegistration userData)
    {
        throw new NotImplementedException();
    }

    public async Task<ValidationResult<ShippingQuote>> GenerateShippingQuoteAsync(ShippingRequest request)
    {
        throw new NotImplementedException();
    }

    public decimal CalculateShipping(decimal weight, decimal distance)
    {
        throw new NotImplementedException();
    }

    public int CalculateShippingCost(decimal weight, decimal distance)
    {
        throw new NotImplementedException();
    }

    public async Task<ValidationResult<string>> ProcessPaymentAsync(decimal amount, string cardToken)
    {
        throw new NotImplementedException();
    }

    public ValidationResult<string> ValidateEmail(string email)
    {
        throw new NotImplementedException();
    }

    public class PaymentService(IPaymentGateway gateway, ILogger<PaymentService> logger)
    {
        public async Task<ValidationResult<string>> ProcessAsync(decimal amount)
        {
            throw new NotImplementedException();
        }
    }

    public record PaymentRequest(decimal Amount, string Currency, string CustomerId)
    {
        public string? Description { get; init; }
    }

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

    public ValidationResult<string> ValidateEmailAI(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return ValidationResult<string>.FailureResult("Email is required");
        }

        var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");
        if (!emailRegex.IsMatch(email))
        {
            return ValidationResult<string>.FailureResult("Invalid email format");
        }

        return ValidationResult<string>.SuccessResult(email.ToLower().Trim());
    }

    public ValidationResult<string> ValidateEmailImproved(string email)
    {
        if (email is null)
        {
            return ValidationResult<string>.FailureResult("Email must not be null");
        }

        var trimmed = email.Trim();

        if (trimmed.Length == 0)
        {
            return ValidationResult<string>.FailureResult("Email is required");
        }

        if (trimmed.Length > 254)
        {
            return ValidationResult<string>.FailureResult("Email too long (max 254 characters)");
        }

        var emailRegex = new System.Text.RegularExpressions.Regex(
            @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");

        if (!emailRegex.IsMatch(trimmed))
        {
            return ValidationResult<string>.FailureResult("Invalid email format");
        }

        var localPart = trimmed.Split('@')[0];
        if (localPart.Length > 64)
        {
            return ValidationResult<string>.FailureResult("Email local part too long (max 64 characters)");
        }

        return ValidationResult<string>.SuccessResult(trimmed.ToLower());
    }

    public void CheckForWrongApiUsage()
    {
    }

    public bool CheckBusinessLogic(int age)
    {
        return age >= 13;
    }

    public string[] CheckPerformance(string[] items)
    {
        var result = new List<string>(items.Length);
        foreach (var item in items)
        {
            if (!string.IsNullOrEmpty(item))
            {
                result.Add(item.ToUpper());
            }
        }
        return result.ToArray();
    }

    public ValidationResult<decimal> SafeDivision(decimal dividend, decimal divisor)
    {
        if (divisor == 0)
        {
            return ValidationResult<decimal>.FailureResult("Cannot divide by zero");
        }

        return ValidationResult<decimal>.SuccessResult(dividend / divisor);
    }

    public async Task<ValidationResult<PaymentConfirmation?>> ProcessPaymentAsync(
        PaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private static readonly string[] SupportedCurrencies = ["USD", "EUR", "GBP"];

    public record PaymentConfirmation(string Id, decimal Amount, string Status);
    public record ShippingRequest(decimal Weight, decimal Distance, string Priority);
    public record ShippingQuote(decimal Cost, int EstimatedDays, string Carrier);

    public class PromptIterationExample
    {
        public bool ValidateInputV1(string input)
        {
            return !string.IsNullOrEmpty(input);
        }

        public ValidationResult<string> ValidateInputV2(string input)
        {
            if (string.IsNullOrEmpty(input))
                return ValidationResult<string>.FailureResult("Input required");
            
            return ValidationResult<string>.SuccessResult(input);
        }

        public ValidationResult<UserRegistration> ValidateRegistrationInput(string name, string email, string password)
        {
            var errors = new List<ValidationError>();

            if (string.IsNullOrWhiteSpace(name))
                errors.Add(new ValidationError(nameof(name), "REQUIRED", "Name is required"));
            else if (name.Length < 2 || name.Length > 100)
                errors.Add(new ValidationError(nameof(name), "LENGTH", "Name must be 2-100 characters"));
            else if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
                errors.Add(new ValidationError(nameof(name), "FORMAT", "Name can only contain letters and spaces"));

            var passwordErrors = ValidatePasswordStrength(password);
            errors.AddRange(passwordErrors);

            if (errors.Count > 0)
                return ValidationResult<UserRegistration>.FailureResult(errors);

            return ValidationResult<UserRegistration>.SuccessResult(new UserRegistration(email, name, password));
        }

        private List<ValidationError> ValidatePasswordStrength(string password)
        {
            var errors = new List<ValidationError>();
            
            if (string.IsNullOrEmpty(password))
                errors.Add(new ValidationError("password", "REQUIRED", "Password is required"));
            else
            {
                if (password.Length < 12)
                    errors.Add(new ValidationError("password", "TOO_SHORT", "Password must be at least 12 characters"));
                
                if (!password.Any(char.IsUpper))
                    errors.Add(new ValidationError("password", "NO_UPPERCASE", "Password must contain uppercase letter"));
                
                if (!password.Any(char.IsLower))
                    errors.Add(new ValidationError("password", "NO_LOWERCASE", "Password must contain lowercase letter"));
                
                if (!password.Any(char.IsDigit))
                    errors.Add(new ValidationError("password", "NO_DIGIT", "Password must contain number"));
                
                if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                    errors.Add(new ValidationError("password", "NO_SYMBOL", "Password must contain special character"));
            }
            
            return errors;
        }
    }
}

public interface IPricingStrategy
{
    Task<decimal> CalculateDiscountAsync(string customerId, decimal orderAmount);
}

public interface IPaymentGateway
{
    Task<ValidationResult<string>> ProcessPaymentAsync(decimal amount, string cardToken);
}

public interface ILogger<T>
{
}