# AI Pair-Programming Workshop - Multi-Language Starter

This repository contains examples and exercises for the AI pair-programming workshop focusing on GitHub Copilot best practices across different technology stacks.

## Workshop Tracks

Choose your preferred technology stack:

### 🟦 TypeScript Track
**Location**: `/typescript/`  
**Prerequisites**: Node.js 18+, VS Code with Copilot extension  
**Focus**: Modern TypeScript with strict typing, async/await patterns, comprehensive error handling

```bash
cd typescript
npm install
npm run test
```

### 🟣 .NET Track
**Location**: `/dotnet/`  
**Prerequisites**: .NET 8 SDK, VS Code or Visual Studio with Copilot extension  
**Focus**: C# with nullable reference types, async patterns, dependency injection, minimal APIs

```bash
cd dotnet
dotnet restore
dotnet test
```

## Repository Structure

```
ai-workshop/
├── README.md                    # This file
├── WORKSHOP_GUIDE.md           # General workshop instructions
├── nodejs/                 # TypeScript track
│   ├── README.md
│   ├── package.json
│   ├── src/
│   │   ├── examples/
│   │   ├── exercises/
│   │   └── types/
│   └── __tests__/
├── dotnet/                     # .NET track
│   ├── README.md
│   ├── AIPairProgramming.sln
│   ├── src/
│   │   ├── Examples/
│   │   ├── Exercises/
│   │   └── Types/
│   └── tests/
└── shared/                     # Cross-language resources
    ├── STYLE_GUIDES/
    ├── API_SCHEMAS/
    └── WORKSHOP_SLIDES/
```

## Workshop Concepts (Language Agnostic)

All tracks cover the same core AI pair-programming concepts:

### 1. Prompt Patterns
- **FCE Pattern**: Function-Constraints-Examples
- **Edge-Case Booster**: Explicit failure mode handling
- **Test-First Specification**: Using tests as requirements

### 2. Context Injection
- **Inline Comments**: Strategic placement for maximum AI context
- **Multi-File Context**: Using open tabs and workspace symbols
- **Documentation-Driven Development**: Using schemas and style guides

### 3. Verification Methods
- **R.E.D. Checklist**: Read-Execute-Diff verification process
- **Security Review**: Identifying AI-generated vulnerabilities
- **Performance Validation**: Ensuring efficient implementations

### 4. Team Integration
- **Code Review Practices**: AI-specific review criteria
- **Standards Development**: Creating team prompting guidelines
- **Measurement**: Tracking productivity and quality improvements

## Quick Start

1. **Choose your track** and navigate to the appropriate directory
2. **Follow track-specific setup** instructions in the track's README
3. **Install Copilot extension** for your IDE
4. **Start with Exercise 1** to practice FCE patterns
5. **Work through progressive exercises** building real features

## Workshop Flow

Each track follows the same learning progression:
1. **Basic Prompting** (20 min) - FCE pattern practice
2. **Context Strategies** (25 min) - Multi-file techniques
3. **Test-First Development** (20 min) - Using tests as specifications
4. **Security Verification** (15 min) - R.E.D. checklist application
5. **Advanced Techniques** (25 min) - Complex refactoring scenarios
6. **Prompt Iteration** (20 min) - Improving failed suggestions

## Language-Specific Adaptations

While core concepts remain the same, each track demonstrates:

### TypeScript Focus
- Strict typing for better AI inference
- Modern async/await patterns
- Comprehensive error handling with Result types
- Functional programming techniques

### .NET Focus
- Nullable reference types for safety
- Dependency injection patterns
- Minimal API development
- Entity Framework integration
- C# 12 features (primary constructors, collection expressions)

## Shared Resources

The `/shared/` directory contains:
- **Style Guides**: Language-specific coding standards
- **API Schemas**: Business domain definitions
- **Workshop Materials**: Presentation slides and reference docs

## Success Metrics

Track your improvement across any language:
- ⚡ **Speed**: Features implemented per sprint
- 🎯 **Quality**: Reduced code review iterations
- 🧠 **Learning**: New patterns and libraries adopted
- 👥 **Team**: Faster onboarding and knowledge sharing

## Contributing

To add a new language track:
1. Create directory: `/your-language/`
2. Follow the established pattern with examples and exercises
3. Adapt concepts to language-specific patterns
4. Update this root README

## Getting Help

- **Track-specific issues**: Check individual track READMEs
- **General concepts**: Review `WORKSHOP_GUIDE.md`
- **Copilot setup**: Refer to GitHub Copilot documentation

Ready to become an AI pair-programming expert in your favorite language! 🚀

---

**Workshop Duration**: 3-4 hours  
**Skill Level**: Intermediate developers familiar with chosen language  
**Tools Required**: IDE with GitHub Copilot extension