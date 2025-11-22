<!--
SYNC IMPACT REPORT
===================
Version change: 1.1.0 → 1.2.0
Modified principles:
  - Principle IV: 'Test-First Development' → 'Testability & Atomic Commits'
Added sections: N/A
Removed sections: N/A
Templates requiring updates:
  ✅ .specify/templates/tasks-template.md (updated to reflect atomic commit guidance)
  ✅ .github/agents/speckit.implement.agent.md (updated to enforce commit-per-task)
  ✅ .specify/memory/constitution.md (this file)
Follow-up TODOs: N/A
-->

# TennisGame Constitution

## Core Principles

### I. Single Responsibility Principle (SRP)

Each class, method, or module MUST have one and only one reason to change. This means:

- Classes focus on a single concern or responsibility
- Methods perform one well-defined task
- Modules encapsulate one cohesive set of related functionality
- Dependencies are minimized and explicit

**Rationale**: SRP ensures maintainability by isolating change impact, improving testability through focused components, and enhancing code comprehension by reducing cognitive load.

### II. Open/Closed & Liskov Substitution Principles (OCP + LSP)

Code MUST be open for extension but closed for modification. Abstractions MUST be substitutable without breaking behavior:

- Use interfaces and abstract classes for extensibility
- Favor composition over inheritance
- Derived types MUST honor base type contracts (pre/post-conditions, invariants)
- Avoid breaking substitutability with unexpected exceptions or side effects
- New functionality added via new classes, not by modifying existing stable code

**Rationale**: OCP reduces regression risk and enables safe feature addition. LSP ensures polymorphism works correctly, preventing runtime surprises and maintaining type safety guarantees.

### III. Interface Segregation & Dependency Inversion Principles (ISP + DIP)

Interfaces MUST be client-specific and cohesive. High-level modules MUST NOT depend on low-level modules:

- Design small, focused interfaces (avoid "fat" interfaces forcing unused method implementations)
- Depend on abstractions (interfaces/abstract classes), not concrete implementations
- Use dependency injection (DI) for all cross-cutting and service dependencies
- Register services in DI container with appropriate lifetimes (Singleton, Scoped, Transient)
- Constructor injection is preferred; property/method injection only when justified

**Rationale**: ISP prevents interface pollution and reduces coupling. DIP inverts control flow, enabling testability, flexibility, and decoupling from infrastructure details.

### IV. Testability & Atomic Commits (NON-NEGOTIABLE)

All production code MUST be designed for testability and committed incrementally:

- **Test-First**: Follow the Red-Green-Refactor TDD cycle. Write a failing test before writing production code.
- **Design for Testability**: Write clean, loosely-coupled code (using SRP, DIP) that is inherently easy to test. Avoid static methods and tight coupling where possible.
- **Atomic Commits**: Each task (and its corresponding test) from `tasks.md` MUST be completed in a single, atomic commit. This creates a clean, auditable, and revertible project history.
- **Coverage**: Aim for >80% code coverage for business logic and services.
- **Frameworks**: Use xUnit as the primary test framework and Moq/NSubstitute for mocking.

**Rationale**: TDD ensures every line of code is justified by a test. Designing for testability leads to better architecture. Atomic commits provide a step-by-step, safe, and understandable development process, enabling risk-free iteration.

### V. Explicit Configuration & Observability

Systems MUST be transparent, debuggable, and configurable without code changes:

- Use `appsettings.json` and environment-specific overrides (`appsettings.Development.json`, etc.)
- Store secrets in Azure Key Vault, AWS Secrets Manager, or User Secrets (development)
- NEVER commit secrets, connection strings, or API keys to source control
- Implement structured logging using Serilog or Microsoft.Extensions.Logging
- Log at appropriate levels: Trace (verbose), Debug (diagnostics), Information (milestones), Warning (recoverable issues), Error (failures), Critical (system-level failures)
- Include correlation IDs for distributed tracing
- Expose health check endpoints (`/health`, `/ready`) for orchestrators

**Rationale**: Explicit configuration enables environment portability. Observability ensures rapid troubleshooting, performance monitoring, and production incident resolution.

### VI. Traditional Chinese Documentation (正體中文文件規範)

All project documentation MUST be written in Traditional Chinese (正體中文):

- All specification documents (spec.md, plan.md, tasks.md) MUST use Traditional Chinese
- Code comments for complex business logic MUST be in Traditional Chinese
- README files, architecture decision records (ADRs), and design documents MUST be in Traditional Chinese
- User-facing documentation, API documentation, and quickstart guides MUST be in Traditional Chinese
- Commit messages SHOULD use Traditional Chinese for consistency (English acceptable for technical terms)
- Exception messages and log output MAY use English for technical compatibility

**Exceptions**:
- Source code (classes, methods, variables) MUST follow English naming conventions per .NET standards
- Technical terms without clear Traditional Chinese equivalents MAY use English (e.g., "Dependency Injection", "Repository Pattern")
- Third-party library documentation references remain in original language

**Rationale**: Using Traditional Chinese as the primary documentation language ensures clear communication within the team, reduces misunderstandings in requirements and design discussions, and aligns with the team's native language for more precise technical discourse. This improves collaboration efficiency and knowledge transfer among team members.

## .NET Core Standards

### Technology Stack

- **Framework**: .NET Core 10 (or latest LTS if 10 not yet released)
- **Language**: C# 12+ with nullable reference types enabled
- **API Framework**: ASP.NET Core (Minimal APIs or Controllers based on complexity)
- **ORM**: Entity Framework Core (Code-First preferred)
- **Testing**: xUnit + FluentAssertions + Moq/NSubstitute
- **Build**: .NET CLI (`dotnet build`, `dotnet test`, `dotnet publish`)
- **Package Management**: NuGet with central package management (Directory.Packages.props)

### Code Quality Standards

- **Nullable Reference Types**: MUST be enabled project-wide (`<Nullable>enable</Nullable>`)
- **Warnings as Errors**: Treat warnings as errors in CI/CD (`<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`)
- **Code Analysis**: Enable .NET analyzers and StyleCop for style consistency
- **Async/Await**: Use async methods for I/O-bound operations; suffix with `Async` (e.g., `GetUserAsync`)
- **Naming Conventions**:
  - PascalCase for classes, methods, properties, constants
  - camelCase for parameters, local variables
  - Prefix interfaces with `I` (e.g., `IUserRepository`)
  - Suffix async methods with `Async`
- **File Organization**: One primary class per file, file name matches class name

### Project Structure

```
src/
├── [ProjectName].Api/          # API layer (controllers, minimal APIs)
├── [ProjectName].Core/         # Domain models, interfaces, business logic
├── [ProjectName].Infrastructure/ # Data access, external services, implementations
└── [ProjectName].Shared/       # Cross-cutting concerns (logging, utilities)

tests/
├── [ProjectName].UnitTests/    # Unit tests (isolated, fast)
├── [ProjectName].IntegrationTests/ # Integration tests (database, APIs)
└── [ProjectName].FunctionalTests/  # End-to-end tests (optional)
```

**Justification**: Clean Architecture / Onion Architecture pattern ensures domain logic independence from infrastructure, enabling testability and technology substitution.

### Dependency Management

- Use central package management (`Directory.Packages.props`) to avoid version conflicts
- Keep dependencies updated; review security advisories weekly
- Avoid transitive dependency bloat; reference only what you use
- Document non-obvious dependency choices in architecture decision records (ADRs)

## Development Workflow

### Branch Strategy

- **main**: Production-ready code; protected, requires PR + passing tests
- **develop**: Integration branch for ongoing work (if using Gitflow)
- **feature/[###-feature-name]**: Feature branches from main or develop
- **hotfix/[issue-id]**: Critical production fixes

### Pull Request Requirements

Before merging, every PR MUST:

1. Pass all automated tests (unit, integration, functional)
2. Maintain or improve code coverage (no coverage drops >2%)
3. Pass static analysis (no new warnings/errors)
4. Include XML documentation comments for public APIs
5. Update relevant documentation (README, ADRs, API docs)
6. Receive at least one approving review from a team member
7. Demonstrate compliance with this constitution (checklist in PR template)

### Code Review Checklist

Reviewers MUST verify:

- [ ] SOLID principles applied appropriately
- [ ] No hardcoded secrets or configuration
- [ ] Proper exception handling (specific exceptions, meaningful messages)
- [ ] Async/await used correctly (no blocking calls like `.Result` or `.Wait()`)
- [ ] Nullable reference types handled (no null-forgiving operator `!` without justification)
- [ ] Tests are present, meaningful, and pass
- [ ] Commits are atomic and correspond to individual tasks from `tasks.md`.
- [ ] Logging includes sufficient context for troubleshooting
- [ ] No unnecessary complexity (YAGNI - You Aren't Gonna Need It)
- [ ] Documentation in Traditional Chinese (spec, comments, ADRs)

### Continuous Integration

CI pipeline MUST include:

1. Build validation (`dotnet build`)
2. Unit test execution (`dotnet test`)
3. Code coverage report (>80% threshold for new code)
4. Static analysis (Roslyn analyzers, security scanners)
5. Dependency vulnerability scanning
6. Docker image build (if applicable)

Builds MUST complete in <10 minutes for rapid feedback.

## Governance

### Authority & Amendments

This constitution is the ultimate authority for all development decisions. When conflicts arise between this document and other guidelines, this constitution prevails.

**Amendment Procedure**:

1. Propose change via PR to `.specify/memory/constitution.md`
2. Include rationale, impact analysis, and migration plan
3. Require unanimous approval from technical leads
4. Update `CONSTITUTION_VERSION` per semantic versioning:
   - **MAJOR**: Breaking changes to principles (e.g., removing SRP requirement)
   - **MINOR**: New principles or substantial expansions
   - **PATCH**: Clarifications, typo fixes, non-semantic refinements
5. Synchronize dependent templates (plan, spec, tasks templates)
6. Announce changes in team meeting and update training materials

### Compliance & Enforcement

- All team members MUST review this constitution during onboarding
- PR reviews MUST include constitution compliance verification
- Quarterly constitution audits to identify and address drift
- Non-compliance requires either justification (captured in Complexity Tracking table in plan.md) or remediation

### Complexity Justification

When violating a principle is necessary, document in `Complexity Tracking` table:

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| Example: Multiple responsibilities in `Startup.cs` | ASP.NET Core convention requires configuration in single location | Splitting would obscure framework integration points |

### Versioning Policy

- Version format: `MAJOR.MINOR.PATCH`
- Track changes in Sync Impact Report (HTML comment at top of this file)
- Link version history to commit SHAs for auditability

**Version**: 1.2.0 | **Ratified**: 2025-11-18 | **Last Amended**: 2025-11-22
