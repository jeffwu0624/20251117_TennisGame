# Project Overview

This project is a .NET project that uses Spec-kit, BDD (Gherkin), and TDD to develop a tennis scoring library. The project is currently focused on a single game's scoring logic.

The project is structured around the concept of "features", where each feature has its own directory in the `specs` directory. Each feature directory contains a `spec.md` file that describes the feature in detail, including user stories, acceptance criteria, and technical specifications.

## Building and Running

This project does not have a traditional build process. Instead, it uses a set of PowerShell scripts to manage the development workflow.

### Creating a new feature

To create a new feature, run the following command:

```powershell
.specify/scripts/powershell/create-new-feature.ps1 -ShortName <feature-name> <feature-description>
```

This will create a new feature branch and a corresponding directory in the `specs` directory.

### Running tests

There are no tests in the project yet.

## Development Conventions

### Branching

The project uses a feature-based branching model. Each new feature is developed in its own branch. The branch name is a combination of a feature number and a short name for the feature, for example `001-tennis-scoring`.

### Specifications

Each feature has a detailed specification in the `specs/<feature-name>/spec.md` file. The specification is written in Markdown and includes user stories, acceptance criteria, and technical specifications.

### Commits

Commit messages should be clear and concise. They should describe the change and the reason for the change.
