using AIPairProgramming.Shared.Models;

namespace AIPairProgramming.Exercises;

/// <summary>
/// Exercise 3: Test-First Development with AI
/// 
/// This exercise demonstrates the "Test-First Specification" pattern from the presentation.
/// Based on presentation lines 274-282: Write comprehensive tests first, then let AI implement.
/// 
/// WORKFLOW:
/// 1. Write detailed test specifications below
/// 2. Run tests (they should fail initially)
/// 3. Prompt Copilot: "Implement shipping calculator methods to pass these test specifications"
/// 4. Apply R.E.D. verification to AI-generated code
/// 5. Iterate until all tests pass
/// </summary>

// Business domain types for shipping calculator
public record ShippingRequest(
    decimal Weight,           // Weight in kilograms
    Dimensions Dimensions,
    string Origin,            // Origin postal code
    string Destination,       // Destination postal code
    ServiceType ServiceType,
    decimal DeclaredValue     // Value in cents for insurance
);

public record Dimensions(
    decimal Length,           // Length in cm
    decimal Width,            // Width in cm  
    decimal Height            // Height in cm
);

public enum ServiceType
{
    Standard,
    Express,
    Overnight
}

public record ShippingQuote(
    decimal BaseRate,         // Base shipping cost in cents
    decimal FuelSurcharge,    // Fuel surcharge in cents
    decimal InsuranceFee,     // Insurance fee in cents
    decimal TotalCost,        // Total cost in cents
    int EstimatedDays,        // Estimated delivery days
    string ServiceLevel       // Human-readable service description
);

/// <summary>
/// Shipping Calculator with Test-First Development approach
/// Tests serve as executable specifications for AI implementation
/// </summary>
public class ShippingCalculator
{
    // TEST SPECIFICATIONS - Write These First!
    // These tests serve as executable requirements for AI code generation

    /*
     * TEST SPECIFICATION GROUP 1: Weight-Based Pricing
     * 
     * [Test] Should_CalculateStandardShippingFor2_5KgPackage()
     * {
     *     var request = new ShippingRequest(
     *         Weight: 2.5m,
     *         Dimensions: new Dimensions(30, 20, 15),
     *         Origin: "10001",
     *         Destination: "90210", 
     *         ServiceType: ServiceType.Standard,
     *         DeclaredValue: 5000 // $50.00 in cents
     *     );
     *     
     *     var result = await CalculateShippingAsync(request);
     *     
     *     result.Success.Should().BeTrue();
     *     result.Data.TotalCost.Should().BeGreaterThan(0);
     *     result.Data.EstimatedDays.Should().BeGreaterThan(0);
     *     result.Data.ServiceLevel.Should().Be("Standard Ground");
     * }
     *
     * [Test] Should_HandleMinimumWeight0_1KgWithBaseRate()
     * {
     *     var request = new ShippingRequest(0.1m, new Dimensions(10, 10, 5), "10001", "10002", ServiceType.Standard, 1000);
     *     var result = await CalculateShippingAsync(request);
     *     
     *     result.Success.Should().BeTrue();
     *     result.Data.TotalCost.Should().BeGreaterThanOrEqual(500); // Minimum $5.00
     * }
     *
     * [Test] Should_RejectPackagesOver50KgWeightLimit()
     * {
     *     var request = new ShippingRequest(51m, new Dimensions(100, 100, 100), "10001", "90210", ServiceType.Standard, 10000);
     *     var result = await CalculateShippingAsync(request);
     *     
     *     result.Success.Should().BeFalse();
     *     result.Error.Should().Contain("weight limit");
     * }
     */

    /*
     * TEST SPECIFICATION GROUP 2: Service Type Pricing
     * 
     * [Test] Should_PriceExpressShipping50PercentHigherThanStandard()
     * {
     *     var baseRequest = new ShippingRequest(5m, new Dimensions(40, 30, 20), "10001", "90210", ServiceType.Standard, 7500);
     *     var expressRequest = baseRequest with { ServiceType = ServiceType.Express };
     *
     *     var standardResult = await CalculateShippingAsync(baseRequest);
     *     var expressResult = await CalculateShippingAsync(expressRequest);
     *
     *     standardResult.Success.Should().BeTrue();
     *     expressResult.Success.Should().BeTrue();
     *     
     *     var expectedExpressCost = Math.Round(standardResult.Data.TotalCost * 1.5m, 0);
     *     expressResult.Data.TotalCost.Should().BeApproximately(expectedExpressCost, 10); // Within $0.10
     *     expressResult.Data.EstimatedDays.Should().BeLessThan(standardResult.Data.EstimatedDays);
     * }
     *
     * [Test] Should_PriceOvernightShipping200PercentHigherThanStandard()
     * {
     *     var request = new ShippingRequest(3m, new Dimensions(25, 25, 15), "10001", "90210", ServiceType.Overnight, 5000);
     *     var result = await CalculateShippingAsync(request);
     *     
     *     result.Success.Should().BeTrue();
     *     result.Data.EstimatedDays.Should().Be(1);
     *     result.Data.ServiceLevel.Should().Be("Overnight Express");
     * }
     */

    /*
     * TEST SPECIFICATION GROUP 3: Dimensional Weight
     * 
     * [Test] Should_UseDimensionalWeightWhenHigherThanActualWeight()
     * {
     *     // Large, light package (dimensional weight should be used)
     *     var request = new ShippingRequest(
     *         Weight: 1m, // 1kg actual weight
     *         Dimensions: new Dimensions(50, 40, 30), // Dimensional weight ~10kg  
     *         Origin: "10001",
     *         Destination: "90210",
     *         ServiceType: ServiceType.Standard,
     *         DeclaredValue: 3000
     *     );
     *
     *     var result = await CalculateShippingAsync(request);
     *     result.Success.Should().BeTrue();
     *     
     *     // Should be priced based on dimensional weight, not actual weight
     *     result.Data.TotalCost.Should().BeGreaterThan(2000); // Higher cost due to dimensional weight
     * }
     *
     * [Test] Should_UseActualWeightWhenHigherThanDimensionalWeight()
     * {
     *     // Small, heavy package (actual weight should be used)  
     *     var heavyRequest = new ShippingRequest(10m, new Dimensions(15, 15, 10), "10001", "90210", ServiceType.Standard, 8000);
     *     var lightRequest = new ShippingRequest(1.5m, new Dimensions(15, 15, 10), "10001", "90210", ServiceType.Standard, 8000);
     *
     *     var heavyResult = await CalculateShippingAsync(heavyRequest);
     *     var lightResult = await CalculateShippingAsync(lightRequest);
     *
     *     heavyResult.Success.Should().BeTrue();
     *     lightResult.Success.Should().BeTrue();
     *     
     *     heavyResult.Data.TotalCost.Should().BeGreaterThan(lightResult.Data.TotalCost);
     * }
     */

    /*
     * TEST SPECIFICATION GROUP 4: Distance-Based Pricing
     * 
     * [Test] Should_CostMoreForCrossCountryShippingThanLocal()
     * {
     *     var localRequest = new ShippingRequest(2m, new Dimensions(20, 20, 10), "10001", "10002", ServiceType.Standard, 4000); // NYC to NYC
     *     var crossCountryRequest = localRequest with { Destination = "90210" }; // NYC to LA
     *
     *     var localResult = await CalculateShippingAsync(localRequest);
     *     var crossCountryResult = await CalculateShippingAsync(crossCountryRequest);
     *
     *     localResult.Success.Should().BeTrue();
     *     crossCountryResult.Success.Should().BeTrue();
     *
     *     crossCountryResult.Data.TotalCost.Should().BeGreaterThan(localResult.Data.TotalCost);
     *     crossCountryResult.Data.EstimatedDays.Should().BeGreaterThanOrEqualTo(localResult.Data.EstimatedDays);
     * }
     */

    /*
     * TEST SPECIFICATION GROUP 5: Insurance and Surcharges
     * 
     * [Test] Should_AddInsuranceFeeForHighValuePackages()
     * {
     *     var highValueRequest = new ShippingRequest(1m, new Dimensions(20, 15, 10), "10001", "90210", ServiceType.Standard, 100000); // $1000.00
     *     var lowValueRequest = highValueRequest with { DeclaredValue = 5000 }; // $50.00
     *
     *     var highValueResult = await CalculateShippingAsync(highValueRequest);
     *     var lowValueResult = await CalculateShippingAsync(lowValueRequest);
     *
     *     highValueResult.Success.Should().BeTrue();
     *     lowValueResult.Success.Should().BeTrue();
     *
     *     highValueResult.Data.InsuranceFee.Should().BeGreaterThan(0);
     *     lowValueResult.Data.InsuranceFee.Should().Be(0);
     *     highValueResult.Data.TotalCost.Should().BeGreaterThan(lowValueResult.Data.TotalCost);
     * }
     *
     * [Test] Should_ApplyFuelSurchargeToAllShipments()
     * {
     *     var request = new ShippingRequest(3m, new Dimensions(25, 20, 15), "10001", "90210", ServiceType.Standard, 6000);
     *     var result = await CalculateShippingAsync(request);
     *     
     *     result.Success.Should().BeTrue();
     *     result.Data.FuelSurcharge.Should().BeGreaterThan(0);
     *     result.Data.TotalCost.Should().Be(result.Data.BaseRate + result.Data.FuelSurcharge + result.Data.InsuranceFee);
     * }
     */

    /*
     * TEST SPECIFICATION GROUP 6: Input Validation
     * 
     * [Test] Should_RejectNegativeWeight()
     * {
     *     var request = new ShippingRequest(-1m, new Dimensions(20, 15, 10), "10001", "90210", ServiceType.Standard, 3000);
     *     var result = await CalculateShippingAsync(request);
     *     
     *     result.Success.Should().BeFalse();
     *     result.Error.Should().Contain("Weight must be positive");
     * }
     *
     * [Test] Should_RejectInvalidPostalCodes()
     * {
     *     var request = new ShippingRequest(2m, new Dimensions(20, 15, 10), "INVALID", "90210", ServiceType.Standard, 3000);
     *     var result = await CalculateShippingAsync(request);
     *     
     *     result.Success.Should().BeFalse();
     *     result.Error.Should().Contain("postal code");
     * }
     *
     * [Test] Should_RejectZeroOrNegativeDimensions()
     * {
     *     var request = new ShippingRequest(2m, new Dimensions(0, 15, 10), "10001", "90210", ServiceType.Standard, 3000);
     *     var result = await CalculateShippingAsync(request);
     *     
     *     result.Success.Should().BeFalse();
     *     result.Error.Should().Contain("Dimensions must be positive");
     * }
     */

    // FUNCTIONS TO IMPLEMENT - These are placeholders for AI to complete
    // Based on the test specifications above

    /// <summary>
    /// Main shipping calculator function
    /// 
    /// TODO: Implement based on test specifications above
    /// Business rules derived from tests:
    /// - Weight-based pricing with 50kg limit
    /// - Service type multipliers (standard 1x, express 1.5x, overnight 3x)  
    /// - Dimensional weight calculation (L×W×H÷5000 for kg)
    /// - Distance-based pricing using postal codes
    /// - Insurance for packages over $500 declared value (1% of value)
    /// - Fuel surcharge (current rate ~15% of base)
    /// - Minimum shipping cost $5.00
    /// </summary>
    public async Task<ValidationResult<ShippingQuote>> CalculateShippingAsync(ShippingRequest request)
    {
        // TODO: Let AI implement based on comprehensive test specifications above
        throw new NotImplementedException("Not implemented - use test-first approach with Copilot");
    }

    /// <summary>
    /// Helper: Calculate dimensional weight
    /// Formula: (L × W × H) ÷ 5000 = dimensional weight in kg
    /// </summary>
    private static decimal CalculateDimensionalWeight(Dimensions dimensions)
    {
        // TODO: Implement dimensional weight calculation
        throw new NotImplementedException("Implement based on test requirements");
    }

    /// <summary>
    /// Helper: Calculate distance between postal codes
    /// Returns distance multiplier for pricing
    /// </summary>
    private async Task<decimal> GetDistanceMultiplierAsync(string origin, string destination)
    {
        // TODO: Implement postal code distance calculation
        await Task.Delay(1); // Simulate async operation
        throw new NotImplementedException("Implement distance-based pricing");
    }

    /// <summary>
    /// Helper: Validate postal code format
    /// Supports US postal codes (5 digits or 5+4 format)
    /// </summary>
    private static bool ValidatePostalCode(string postalCode)
    {
        // TODO: Implement postal code validation
        throw new NotImplementedException("Implement postal code validation");
    }

    /// <summary>
    /// Helper: Get current fuel surcharge rate
    /// Returns current fuel surcharge as percentage
    /// </summary>
    private static decimal GetCurrentFuelSurchargeRate()
    {
        // TODO: Could integrate with external API for real rates
        return 0.15m; // 15% surcharge
    }

    /// <summary>
    /// Helper: Calculate service level description and delivery days
    /// </summary>
    private static (string ServiceLevel, int EstimatedDays) GetServiceDetails(ServiceType serviceType, bool isLocal)
    {
        return serviceType switch
        {
            ServiceType.Standard => ("Standard Ground", isLocal ? 2 : 5),
            ServiceType.Express => ("Express Shipping", isLocal ? 1 : 3),
            ServiceType.Overnight => ("Overnight Express", 1),
            _ => throw new ArgumentOutOfRangeException(nameof(serviceType))
        };
    }

    // PROMPT FOR COPILOT AFTER WRITING TESTS:
    //
    // "Implement the shipping calculator methods to pass all the test specifications above.
    // Use the business rules derived from the tests:
    // - Weight-based pricing with dimensional weight consideration (L×W×H÷5000)
    // - Service type multipliers (standard=1x, express=1.5x, overnight=3x)  
    // - Distance-based pricing using postal codes
    // - Insurance fees for high-value packages (1% over $500)
    // - 15% fuel surcharge on all shipments
    // - Comprehensive input validation for weight, dimensions, and postal codes
    // - Return ValidationResult<ShippingQuote> following the established pattern
    // - Use modern C# features: records, pattern matching, nullable reference types"

    // R.E.D. VERIFICATION CHECKLIST:
    //
    // READ:
    // ✅ Does implementation match all test requirements?
    // ✅ Are business rules correctly applied (weight limits, service multipliers)?
    // ✅ Is input validation comprehensive (weight, dimensions, postal codes)?
    // ✅ Does error handling follow ValidationResult pattern?
    // ✅ Are modern C# patterns used appropriately?
    //
    // EXECUTE:  
    // ✅ Run all tests - do they pass?
    // ✅ Test edge cases (min/max weights, invalid inputs)
    // ✅ Verify calculations with manual math
    // ✅ Test service type pricing differences
    // ✅ Validate dimensional weight calculations
    //
    // DIFF-REVIEW:
    // ✅ Compare pricing logic with real shipping services
    // ✅ Verify dimensional weight calculation is accurate  
    // ✅ Check that fuel surcharge and insurance are additive
    // ✅ Ensure postal code validation is realistic
    // ✅ Confirm service level descriptions match business requirements
}