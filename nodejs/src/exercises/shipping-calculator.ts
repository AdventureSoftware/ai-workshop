/**
 * Exercise 3: Test-First Development with AI
 * 
 * This exercise demonstrates the "Test-First Specification" pattern from the presentation.
 * Based on presentation lines 274-282: Write comprehensive tests first, then let AI implement.
 * 
 * WORKFLOW:
 * 1. Write detailed test specifications below
 * 2. Run tests (they should fail initially)
 * 3. Prompt Copilot: "Implement shipping calculator functions to pass these test specifications"
 * 4. Apply R.E.D. verification to AI-generated code
 * 5. Iterate until all tests pass
 */

import { ValidationResult } from '../types/auth';

// Business domain types for shipping calculator
export interface ShippingRequest {
  weight: number;           // Weight in kilograms
  dimensions: {
    length: number;         // Length in cm
    width: number;          // Width in cm  
    height: number;         // Height in cm
  };
  origin: string;           // Origin postal code
  destination: string;      // Destination postal code
  serviceType: 'standard' | 'express' | 'overnight';
  declaredValue: number;    // Value in cents for insurance
}

export interface ShippingQuote {
  baseRate: number;         // Base shipping cost in cents
  fuelSurcharge: number;    // Fuel surcharge in cents
  insuranceFee: number;     // Insurance fee in cents
  totalCost: number;        // Total cost in cents
  estimatedDays: number;    // Estimated delivery days
  serviceLevel: string;     // Human-readable service description
}

// TEST SPECIFICATIONS - Write These First!
// These tests serve as executable requirements for AI code generation

describe('Shipping Calculator - Weight-Based Pricing', () => {
  // Happy path tests
  it('should calculate standard shipping for 2.5kg package', async () => {
    const request: ShippingRequest = {
      weight: 2.5,
      dimensions: { length: 30, width: 20, height: 15 },
      origin: '10001',
      destination: '90210', 
      serviceType: 'standard',
      declaredValue: 5000 // $50.00 in cents
    };
    
    const result = await calculateShipping(request);
    
    expect(result.success).toBe(true);
    if (result.success) {
      expect(result.data.totalCost).toBeGreaterThan(0);
      expect(result.data.estimatedDays).toBeGreaterThan(0);
      expect(result.data.serviceLevel).toBe('Standard Ground');
    }
  });

  // Edge case: minimum weight
  it('should handle minimum weight (0.1kg) with base rate', async () => {
    const request: ShippingRequest = {
      weight: 0.1,
      dimensions: { length: 10, width: 10, height: 5 },
      origin: '10001',
      destination: '10002',
      serviceType: 'standard', 
      declaredValue: 1000
    };

    const result = await calculateShipping(request);
    expect(result.success).toBe(true);
    if (result.success) {
      expect(result.data.totalCost).toBeGreaterThanOrEqual(500); // Minimum $5.00
    }
  });

  // Edge case: maximum weight
  it('should reject packages over 50kg weight limit', async () => {
    const request: ShippingRequest = {
      weight: 51,
      dimensions: { length: 100, width: 100, height: 100 },
      origin: '10001', 
      destination: '90210',
      serviceType: 'standard',
      declaredValue: 10000
    };

    const result = await calculateShipping(request);
    expect(result.success).toBe(false);
    expect(result.error).toContain('weight limit');
  });
});

describe('Shipping Calculator - Service Type Pricing', () => {
  it('should price express shipping 50% higher than standard', async () => {
    const baseRequest: ShippingRequest = {
      weight: 5,
      dimensions: { length: 40, width: 30, height: 20 },
      origin: '10001',
      destination: '90210',
      serviceType: 'standard',
      declaredValue: 7500
    };

    const expressRequest = { ...baseRequest, serviceType: 'express' as const };

    const standardResult = await calculateShipping(baseRequest);
    const expressResult = await calculateShipping(expressRequest);

    expect(standardResult.success).toBe(true);
    expect(expressResult.success).toBe(true);
    
    if (standardResult.success && expressResult.success) {
      const expectedExpressCost = Math.round(standardResult.data.totalCost * 1.5);
      expect(expressResult.data.totalCost).toBeCloseTo(expectedExpressCost, -1); // Within $0.10
      expect(expressResult.data.estimatedDays).toBeLessThan(standardResult.data.estimatedDays);
    }
  });

  it('should price overnight shipping 200% higher than standard', async () => {
    const request: ShippingRequest = {
      weight: 3,
      dimensions: { length: 25, width: 25, width: 15 },
      origin: '10001',
      destination: '90210', 
      serviceType: 'overnight',
      declaredValue: 5000
    };

    const result = await calculateShipping(request);
    expect(result.success).toBe(true);
    if (result.success) {
      expect(result.data.estimatedDays).toBe(1);
      expect(result.data.serviceLevel).toBe('Overnight Express');
    }
  });
});

describe('Shipping Calculator - Dimensional Weight', () => {
  it('should use dimensional weight when higher than actual weight', async () => {
    // Large, light package (dimensional weight should be used)
    const request: ShippingRequest = {
      weight: 1, // 1kg actual weight
      dimensions: { length: 50, width: 40, height: 30 }, // Dimensional weight ~10kg  
      origin: '10001',
      destination: '90210',
      serviceType: 'standard',
      declaredValue: 3000
    };

    const result = await calculateShipping(request);
    expect(result.success).toBe(true);
    
    // Should be priced based on dimensional weight, not actual weight
    if (result.success) {
      expect(result.data.totalCost).toBeGreaterThan(2000); // Higher cost due to dimensional weight
    }
  });

  it('should use actual weight when higher than dimensional weight', async () => {
    // Small, heavy package (actual weight should be used)  
    const heavyRequest: ShippingRequest = {
      weight: 10, // 10kg actual weight
      dimensions: { length: 15, width: 15, height: 10 }, // Dimensional weight ~1.5kg
      origin: '10001', 
      destination: '90210',
      serviceType: 'standard',
      declaredValue: 8000
    };

    const lightRequest: ShippingRequest = {
      weight: 1.5, // Same as dimensional weight above
      dimensions: { length: 15, width: 15, height: 10 },
      origin: '10001',
      destination: '90210', 
      serviceType: 'standard',
      declaredValue: 8000
    };

    const heavyResult = await calculateShipping(heavyRequest);
    const lightResult = await calculateShipping(lightRequest);

    expect(heavyResult.success).toBe(true);
    expect(lightResult.success).toBe(true);
    
    if (heavyResult.success && lightResult.success) {
      expect(heavyResult.data.totalCost).toBeGreaterThan(lightResult.data.totalCost);
    }
  });
});

describe('Shipping Calculator - Distance-Based Pricing', () => {
  it('should cost more for cross-country shipping than local', async () => {
    const localRequest: ShippingRequest = {
      weight: 2,
      dimensions: { length: 20, width: 20, height: 10 },
      origin: '10001', // NYC  
      destination: '10002', // NYC
      serviceType: 'standard',
      declaredValue: 4000
    };

    const crossCountryRequest: ShippingRequest = {
      ...localRequest,
      destination: '90210' // LA
    };

    const localResult = await calculateShipping(localRequest);
    const crossCountryResult = await calculateShipping(crossCountryRequest);

    expect(localResult.success).toBe(true);
    expect(crossCountryResult.success).toBe(true);

    if (localResult.success && crossCountryResult.success) {
      expect(crossCountryResult.data.totalCost).toBeGreaterThan(localResult.data.totalCost);
      expect(crossCountryResult.data.estimatedDays).toBeGreaterThanOrEqual(localResult.data.estimatedDays);
    }
  });
});

describe('Shipping Calculator - Insurance and Surcharges', () => {
  it('should add insurance fee for high-value packages', async () => {
    const highValueRequest: ShippingRequest = {
      weight: 1,
      dimensions: { length: 20, width: 15, height: 10 },
      origin: '10001',
      destination: '90210',
      serviceType: 'standard', 
      declaredValue: 100000 // $1000.00 - should trigger insurance
    };

    const lowValueRequest: ShippingRequest = {
      ...highValueRequest,
      declaredValue: 5000 // $50.00 - should not trigger insurance
    };

    const highValueResult = await calculateShipping(highValueRequest);
    const lowValueResult = await calculateShipping(lowValueRequest);

    expect(highValueResult.success).toBe(true);
    expect(lowValueResult.success).toBe(true);

    if (highValueResult.success && lowValueResult.success) {
      expect(highValueResult.data.insuranceFee).toBeGreaterThan(0);
      expect(lowValueResult.data.insuranceFee).toBe(0);
      expect(highValueResult.data.totalCost).toBeGreaterThan(lowValueResult.data.totalCost);
    }
  });

  it('should apply fuel surcharge to all shipments', async () => {
    const request: ShippingRequest = {
      weight: 3,
      dimensions: { length: 25, width: 20, height: 15 },
      origin: '10001',
      destination: '90210',
      serviceType: 'standard',
      declaredValue: 6000
    };

    const result = await calculateShipping(request);
    expect(result.success).toBe(true);
    
    if (result.success) {
      expect(result.data.fuelSurcharge).toBeGreaterThan(0);
      expect(result.data.totalCost).toBe(
        result.data.baseRate + result.data.fuelSurcharge + result.data.insuranceFee
      );
    }
  });
});

describe('Shipping Calculator - Input Validation', () => {
  it('should reject negative weight', async () => {
    const request: ShippingRequest = {
      weight: -1,
      dimensions: { length: 20, width: 15, height: 10 },
      origin: '10001',
      destination: '90210',
      serviceType: 'standard',
      declaredValue: 3000
    };

    const result = await calculateShipping(request);
    expect(result.success).toBe(false);
    expect(result.error).toContain('Weight must be positive');
  });

  it('should reject invalid postal codes', async () => {
    const request: ShippingRequest = {
      weight: 2,
      dimensions: { length: 20, width: 15, height: 10 },
      origin: 'INVALID',
      destination: '90210',
      serviceType: 'standard', 
      declaredValue: 3000
    };

    const result = await calculateShipping(request);
    expect(result.success).toBe(false);
    expect(result.error).toContain('postal code');
  });

  it('should reject zero or negative dimensions', async () => {
    const request: ShippingRequest = {
      weight: 2,
      dimensions: { length: 0, width: 15, height: 10 },
      origin: '10001',
      destination: '90210',
      serviceType: 'standard',
      declaredValue: 3000  
    };

    const result = await calculateShipping(request);
    expect(result.success).toBe(false);
    expect(result.error).toContain('Dimensions must be positive');
  });
});

// FUNCTIONS TO IMPLEMENT - These are placeholders for AI to complete
// Based on the test specifications above

/**
 * Main shipping calculator function
 * 
 * TODO: Implement based on test specifications above
 * Business rules derived from tests:
 * - Weight-based pricing with 50kg limit
 * - Service type multipliers (standard 1x, express 1.5x, overnight 3x)  
 * - Dimensional weight calculation (L×W×H÷5000 for kg)
 * - Distance-based pricing using postal codes
 * - Insurance for packages over $500 declared value (1% of value)
 * - Fuel surcharge (current rate ~15% of base)
 * - Minimum shipping cost $5.00
 */
async function calculateShipping(request: ShippingRequest): Promise<ValidationResult<ShippingQuote>> {
  // TODO: Let AI implement based on comprehensive test specifications above
  throw new Error('Not implemented - use test-first approach with Copilot');
}

/**
 * Helper: Calculate dimensional weight
 * Formula: (L × W × H) ÷ 5000 = dimensional weight in kg
 */
function calculateDimensionalWeight(dimensions: ShippingRequest['dimensions']): number {
  // TODO: Implement dimensional weight calculation
  throw new Error('Implement based on test requirements');
}

/**
 * Helper: Calculate distance between postal codes
 * Returns distance multiplier for pricing
 */
async function getDistanceMultiplier(origin: string, destination: string): Promise<number> {
  // TODO: Implement postal code distance calculation
  throw new Error('Implement distance-based pricing');
}

/**
 * Helper: Validate postal code format
 * Supports US postal codes (5 digits or 5+4 format)
 */
function validatePostalCode(postalCode: string): boolean {
  // TODO: Implement postal code validation
  throw new Error('Implement postal code validation');
}

/**
 * Helper: Get current fuel surcharge rate
 * Returns current fuel surcharge as percentage
 */
function getCurrentFuelSurchargeRate(): number {
  // TODO: Could integrate with external API for real rates
  return 0.15; // 15% surcharge
}

// PROMPT FOR COPILOT AFTER WRITING TESTS:
//
// "Implement the shipping calculator functions to pass all the test specifications above.
// Use the business rules derived from the tests:
// - Weight-based pricing with dimensional weight consideration
// - Service type multipliers (standard=1x, express=1.5x, overnight=3x)  
// - Distance-based pricing using postal codes
// - Insurance fees for high-value packages (1% over $500)
// - 15% fuel surcharge on all shipments
// - Comprehensive input validation
// - Return ValidationResult<ShippingQuote> following the established pattern"

// R.E.D. VERIFICATION CHECKLIST:
//
// READ:
// ✅ Does implementation match all test requirements?
// ✅ Are business rules correctly applied (weight limits, service multipliers)?
// ✅ Is input validation comprehensive (weight, dimensions, postal codes)?
// ✅ Does error handling follow ValidationResult pattern?
//
// EXECUTE:  
// ✅ Run all tests - do they pass?
// ✅ Test edge cases (min/max weights, invalid inputs)
// ✅ Verify calculations with manual math
// ✅ Test service type pricing differences
//
// DIFF-REVIEW:
// ✅ Compare pricing logic with real shipping services
// ✅ Verify dimensional weight calculation is accurate  
// ✅ Check that fuel surcharge and insurance are additive
// ✅ Ensure postal code validation is realistic

export {
  calculateShipping,
  calculateDimensionalWeight,
  getDistanceMultiplier,
  validatePostalCode,
  getCurrentFuelSurchargeRate
};