# AI Workshop - TypeScript Starter

This repository contains examples and exercises for the AI pair-programming workshop focusing on GitHub Copilot best practices.

## Setup

1. **Install dependencies:**
   ```bash
   npm install
   ```

2. **Install GitHub Copilot extension** in your IDE (VS Code recommended)

3. **Verify setup:**
   ```bash
   npm run test
   npm run lint
   npm run type-check
   ```

## Repository Structure

```
src/
├── examples/           # Workshop demonstration examples
│   ├── prompting/      # FCE pattern examples
│   ├── context/        # Context injection examples
│   └── verification/   # R.E.D. checklist examples
├── exercises/          # Hands-on practice exercises
├── utils/              # Shared utilities and types
└── __tests__/          # Test files
```

## Workshop Sections

### 1. Prompt Patterns (`src/examples/prompting/`)
- FCE Pattern (Function-Constraints-Examples)
- Edge-case booster examples
- Test-first specifications

### 2. Context Injection (`src/examples/context/`)
- Inline comments and docstrings
- Multi-file context strategies
- External documentation usage

### 3. Verification (`src/examples/verification/`)
- R.E.D. checklist implementation
- Security validation examples
- Common failure patterns

### 4. Hands-on Exercises (`src/exercises/`)
- Payment processing system
- User authentication service
- Shipping calculator
- Data validation utilities

## AI Copilot Workshop Commands

During the workshop, you'll practice these Copilot commands:
- `/explain` - Understand complex code
- `/tests` - Generate unit tests
- `/fix` - Debug and fix issues
- `/optimize` - Performance improvements

## Key Files for AI Context

- `STYLE_GUIDE.md` - Team coding standards
- `API_SCHEMA.md` - Domain-specific schemas
- `.editorconfig` - Formatting rules (Copilot respects this!)
- TypeScript configurations for better inference

## Workshop Tips

1. **Open related files** - Copilot uses open tabs as context
2. **Use descriptive comments** - Place intent right above your cursor
3. **Reference specific functions** - Use `#functionName` in Copilot Chat
4. **Verify everything** - Apply the R.E.D. checklist to all AI code

## Development Commands

```bash
# Development
npm run dev          # Start development server
npm run build        # Build the project

# Testing
npm run test         # Run tests
npm run test:watch   # Run tests in watch mode
npm run test:coverage # Generate coverage report

# Code Quality
npm run lint         # Check for linting errors
npm run lint:fix     # Fix linting errors
npm run format       # Format code with Prettier
npm run type-check   # TypeScript type checking
```

## Workshop Flow

1. Start with basic inline suggestions
2. Practice FCE pattern prompting
3. Learn context injection techniques
4. Apply verification methods
5. Work through real-world exercises
6. Discuss team adoption strategies

Ready to become an AI pair-programming expert! 🚀
