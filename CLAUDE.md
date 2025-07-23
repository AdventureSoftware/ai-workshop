# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository Overview

This is an AI pair-programming workshop repository with multi-language tracks (TypeScript/Node.js and .NET) designed to teach GitHub Copilot best practices. The repository contains examples, exercises, and documentation for learning AI-assisted development patterns.

## Architecture

### Multi-Track Structure
- **Root level**: Workshop documentation and shared resources
- **nodejs/**: TypeScript track with modern Node.js patterns
- **dotnet/**: .NET track with C# 12 features and modern patterns
- Each track is self-contained with its own dependencies and tooling

### Key Patterns
- **FCE Pattern**: Function-Constraints-Examples prompting technique
- **Result Types**: Explicit error handling with success/failure patterns
- **Context Injection**: Strategic code comments and multi-file awareness for AI
- **R.E.D. Checklist**: Read-Execute-Diff verification method

## Development Commands

### TypeScript Track (nodejs/)
The package.json currently only has a placeholder test command, but the README documents these intended commands:
```bash
npm install              # Install dependencies
npm run test            # Run Jest tests
npm run lint            # ESLint checking  
npm run type-check      # TypeScript type checking
npm run dev             # Development server
npm run build           # Build project
npm run format          # Prettier formatting
```

### .NET Track (dotnet/)
```bash
dotnet restore          # Restore NuGet packages
dotnet build           # Build solution
dotnet test            # Run tests
dotnet format          # Format code
```

## Code Architecture Patterns

### TypeScript Track
- **Strict TypeScript**: Full strict mode with comprehensive null checks
- **Result Pattern**: Explicit success/failure handling instead of exceptions
- **Zod Validation**: Runtime type validation for external inputs  
- **Security-First**: Input validation, rate limiting, safe defaults
- **Jest Testing**: With ts-jest preset and coverage reporting

### .NET Track  
- **Modern C# 12**: Primary constructors, collection expressions, pattern matching
- **Nullable Reference Types**: Enabled for better null safety
- **Record Types**: Immutable data contracts
- **Dependency Injection**: Standard .NET DI patterns
- **xUnit Testing**: With FluentAssertions

## AI Context Files

Both tracks include essential context files for AI assistance:
- **STYLE_GUIDE.md**: Coding conventions and patterns
- **API_SCHEMA.md**: Business domain definitions
- **README.md**: Track-specific setup and development workflows

## Workshop Structure

The repository is organized around progressive learning exercises:
1. **Basic Prompting**: FCE pattern practice
2. **Context Injection**: Multi-file awareness techniques  
3. **Verification Methods**: R.E.D. checklist application
4. **Security Patterns**: Defensive coding practices
5. **Advanced Techniques**: Complex refactoring scenarios

## Key Implementation Notes

- Both tracks use Result<T> pattern for error handling instead of throwing exceptions
- Security validation is emphasized with input sanitization and rate limiting
- Tests are designed to be AI-generated using specification-driven development
- Style guides are crafted specifically to provide AI context for consistent code generation
- The codebase demonstrates defensive security practices throughout