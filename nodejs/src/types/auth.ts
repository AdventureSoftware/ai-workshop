/**
 * Authentication types for the workshop examples
 */

export interface User {
  id: string;
  email: string;
  name: string;
  role: 'user' | 'admin';
  createdAt: Date;
  lastLoginAt: Date | null;
  isActive: boolean;
}

export interface LoginCredentials {
  email: string;
  password: string;
}

export interface AuthToken {
  token: string;
  expiresAt: Date;
  userId: string;
}

export interface ValidationResult<T = any> {
  success: boolean;
  data?: T;
  error?: string;
  errors?: ValidationError[];
}

export interface ValidationError {
  field: string;
  code: string;
  message: string;
}

export type AuthError =
  | 'INVALID_CREDENTIALS'
  | 'RATE_LIMITED'
  | 'ACCOUNT_DISABLED'
  | 'INVALID_EMAIL_FORMAT'
  | 'WEAK_PASSWORD';

export interface UserRegistration {
  email: string;
  name: string;
  password: string;
}
