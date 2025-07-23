# API Schema & Business Rules

This file provides domain-specific context for AI Copilot when generating code. Referenced throughout the workshop exercises to ensure AI suggestions align with business requirements.

Based on presentation: "Domain artefacts – schema.md, api.yml, Confluence export"

## Core Types

### User Management

```typescript
interface User {
  id: string;              // UUID format
  email: string;           // Must be valid email format
  name: string;            // 2-50 characters
  role: 'user' | 'admin';  // Default: 'user'
  createdAt: Date;
  lastLoginAt: Date | null;
  isActive: boolean;       // Default: true
}

interface UserRegistrationRequest {
  email: string;           // Must be unique in system
  name: string;
  password: string;        // Min 8 chars, 1 upper, 1 lower, 1 number, 1 symbol
}

interface LoginCredentials {
  email: string;
  password: string;
}

interface AuthToken {
  token: string;           // JWT format
  expiresAt: Date;         // 24 hours from creation
  userId: string;
}
```

### Payment Processing

```typescript
interface PaymentRequest {
  amount: number;          // In cents, min: 500, max: 1000000
  currency: 'USD';         // Only USD supported currently
  description: string;     // Optional, max 200 chars
  customerId: string;      // Reference to User.id
}

interface PaymentConfirmation {
  id: string;              // Payment ID from provider
  amount: number;          // Confirmed amount in cents
  currency: 'USD';
  status: PaymentStatus;
  createdAt: Date;
  processingFeesCents: number; // Our fee calculation
}

type PaymentStatus = 'pending' | 'completed' | 'failed' | 'refunded';

interface PaymentError {
  code: PaymentErrorCode;
  message: string;
  retryable: boolean;      // Can this payment be retried?
}

type PaymentErrorCode = 
  | 'INSUFFICIENT_FUNDS'
  | 'INVALID_CARD'
  | 'PROCESSING_ERROR'
  | 'FRAUD_DETECTED'
  | 'LIMIT_EXCEEDED';
```

### Shipping System

```typescript
interface ShippingRequest {
  weight: number;          // In kilograms, must be positive
  distance: number;        // In kilometers, must be positive
  priority: ShippingPriority;
  destination: Address;
}

type ShippingPriority = 'standard' | 'express' | 'overnight';

interface Address {
  street: string;
  city: string;
  state: string;           // 2-letter code
  zipCode: string;         // 5 or 9 digit format
  country: 'US';           // Only US shipping currently
}

interface ShippingQuote {
  costCents: number;       // Total shipping cost
  estimatedDays: number;   // Delivery time estimate
  carrier: string;         // e.g., 'FedEx', 'UPS', 'USPS'
  trackingAvailable: boolean;
}
```

### Validation and Error Handling

```typescript
interface ValidationResult<T = any> {
  valid: boolean;
  data?: T;               // Present when valid = true
  errors: ValidationError[];
}

interface ValidationError {
  field: string;          // Which field failed validation
  code: string;           // Machine-readable error code
  message: string;        // Human-readable error message
}

// Common validation error codes
type ValidationErrorCode = 
  | 'REQUIRED'
  | 'INVALID_FORMAT'
  | 'TOO_SHORT'
  | 'TOO_LONG'
  | 'OUT_OF_RANGE'
  | 'INVALID_ENUM_VALUE';

// Standard API response wrapper
interface ApiResponse<T> {
  success: boolean;
  data?: T;               // Present when success = true
  error?: ApiError;       // Present when success = false
  timestamp: Date;
  requestId: string;      // For tracking/debugging
}

interface ApiError {
  code: string;
  message: string;
  details?: Record<string, any>;
}
```

## Business Rules

### Authentication
- **Password Requirements**: Minimum 8 characters, at least 1 uppercase, 1 lowercase, 1 number, 1 special character
- **Rate Limiting**: Maximum 5 login attempts per 15 minutes per IP
- **Token Expiry**: JWT tokens expire after 24 hours
- **Session Management**: Only one active session per user

### Payments
- **Amount Limits**: Minimum $5.00 (500 cents), Maximum $10,000.00 (1,000,000 cents)
- **Currency**: USD only
- **Retry Logic**: Failed payments can be retried up to 3 times with exponential backoff
- **Processing Fees**: 2.9% + 30¢ per transaction

### Shipping
- **Weight Limits**: 0.1kg minimum, 50kg maximum per package
- **Distance Calculation**: Straight-line distance between ZIP codes
- **Base Rates**:
    - Standard: $0.50/kg + $0.10/km
    - Express: $1.00/kg + $0.20/km (2-3 days)
    - Overnight: $2.00/kg + $0.50/km (next day)

### Data Validation
- **Email Format**: Must comply with RFC 5322
- **Phone Numbers**: US format only: (555) 123-4567 or +1-555-123-4567
- **ZIP Codes**: 5-digit or 9-digit (12345 or 12345-6789)
- **Names**: 2-50 characters, letters and spaces only

## Example Data

### Sample User
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "email": "john.doe@example.com",
  "name": "John Doe",
  "role": "user",
  "createdAt": "2024-01-15T10:30:00Z",
  "lastLoginAt": "2024-07-20T14:22:00Z",
  "isActive": true
}
```

### Sample Payment Request
```json
{
  "amount": 2999,
  "currency": "USD",
  "description": "Widget purchase",
  "customerId": "550e8400-e29b-41d4-a716-446655440000"
}
```

### Sample Shipping Request
```json
{
  "weight": 2.5,
  "distance": 150,
  "priority": "standard",
  "destination": {
    "street": "123 Main St",
    "city": "San Francisco",
    "state": "CA",
    "zipCode": "94102",
    "country": "US"
  }
}
```

## Error Response Examples

### Validation Error
```json
{
  "success": false,
  "error": {
    "code": "VALIDATION_FAILED",
    "message": "Request validation failed",
    "details": {
      "errors": [
        {
          "field": "email",
          "code": "INVALID_FORMAT",
          "message": "Email must be in valid format"
        },
        {
          "field": "password",
          "code": "TOO_SHORT",
          "message": "Password must be at least 8 characters"
        }
      ]
    }
  },
  "timestamp": "2024-07-22T15:30:00Z",
  "requestId": "req_abc123def456"
}
```

### Payment Error
```json
{
  "success": false,
  "error": {
    "code": "INSUFFICIENT_FUNDS",
    "message": "The payment method has insufficient funds",
    "details": {
      "retryable": false,
      "suggestedAction": "Try a different payment method"
    }
  },
  "timestamp": "2024-07-22T15:30:00Z",
  "requestId": "req_payment_789xyz"
}
```
