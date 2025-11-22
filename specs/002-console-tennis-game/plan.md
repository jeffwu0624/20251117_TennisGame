# Implementation Plan: Interactive Tennis Game Console

**Branch**: `002-console-tennis-game` | **Date**: 2025-11-22 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/002-console-tennis-game/spec.md`

## Summary

Create a .NET Core console application that provides an interactive tennis match experience. Users enter two player names, record points round-by-round, see live score updates using tennis terminology, and can exit anytime. The application leverages the existing TennisScoring library for score calculation logic.

## Technical Context

**Language/Version**: C# / .NET 8.0  
**Primary Dependencies**: TennisScoring (existing library in solution)  
**Storage**: N/A (in-memory only, no persistence)  
**Testing**: xUnit (matching existing test project pattern)  
**Target Platform**: Cross-platform console (.NET 8.0 runtime)  
**Project Type**: Console application (new project in existing solution)  
**Performance Goals**: <1 second response time for user input processing  
**Constraints**: Must use existing TennisScoring library, text-based console UI  
**Scale/Scope**: Single-player session, no concurrent users, local execution only

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

✅ **Project Count**: Adding 1 new console project to existing solution (currently has library + test projects)  
✅ **Dependency Complexity**: Simple project reference to TennisScoring library  
✅ **Technology Stack**: Consistent with existing C#/.NET solution  
✅ **Architecture**: Simple console application, no additional patterns needed

## Project Structure

### Documentation (this feature)

```text
specs/002-console-tennis-game/
├── plan.md              # This file
├── spec.md              # Feature specification (already exists)
├── checklists/
│   └── requirements.md  # Quality checklist (already exists)
└── tasks.md             # Task breakdown (to be generated)
```

### Source Code (repository root)

```text
TennisScoring.sln                           # Existing solution file
├── src/
│   ├── TennisScoring/                      # Existing library project
│   │   ├── Game.cs                         # Tennis scoring logic
│   │   ├── Side.cs                         # Player enum (PlayerA, PlayerB)
│   │   └── TennisScoring.csproj
│   └── TennisScoring.Console/              # NEW: Console application
│       ├── Program.cs                      # Main entry point with game loop
│       ├── ConsoleUI.cs                    # Console I/O helpers
│       ├── MatchController.cs              # Match flow orchestration
│       └── TennisScoring.Console.csproj    # Project file with reference to TennisScoring
└── tests/
    ├── TennisScoring.Tests/                # Existing unit tests for library
    └── TennisScoring.Console.Tests/        # NEW: Console app integration tests
        ├── MatchControllerTests.cs         # Test match flow logic
        ├── ConsoleUITests.cs               # Test input validation
        └── TennisScoring.Console.Tests.csproj
```

**Structure Decision**: Adding a new console project (`TennisScoring.Console`) to the existing solution. The console project will reference the `TennisScoring` library and provide the interactive UI layer. This follows .NET solution conventions where the library contains business logic and the console project provides the user interface.

**Key Components**:
- **Program.cs**: Main entry point, application lifecycle, game loop
- **ConsoleUI.cs**: Console input/output helpers (prompts, validation, display)
- **MatchController.cs**: Orchestrates match flow, bridges UI and Game library

## Implementation Strategy

### Phase Breakdown

**Phase 1: Setup**
- Add TennisScoring.Console project to solution
- Add project reference to TennisScoring library
- Set up basic Program.cs structure

**Phase 2: Foundation (Blocking Prerequisites)**
- Create ConsoleUI helper class for input/output
- Implement input validation utilities
- Create MatchController for game flow orchestration

**Phase 3: User Story 1 - Start New Tennis Match (P1)**
- Implement player name input prompts
- Initialize Game instance with player names
- Display initial score (Love-All)

**Phase 4: User Story 2 - Record Point and Display Score (P1)**
- Implement point recording loop
- Player selection input and validation
- Score display after each point
- Integration with Game.PointWonBy() and Game.GetScoreText()

**Phase 5: User Story 4 - Complete Match with Winner (P1)**
- Win condition detection using Game.IsFinished
- Winner announcement using Game.Winner
- Post-game options (new match/exit)

**Phase 6: User Story 3 - Exit Match Gracefully (P2)**
- Exit command handling (case-insensitive)
- Graceful termination at any prompt
- Farewell message

**Phase 7: Polish & Testing**
- Edge case handling (empty names, invalid input)
- Integration tests for complete match flows
- User experience refinements

### Dependency Mapping

```text
Phase 1 (Setup) → Phase 2 (Foundation) → Phase 3, 4, 5, 6 (User Stories)
                                              ↓
                                         Phase 7 (Polish)
```

User Stories 3, 4, 5, 6 can be implemented in parallel after Phase 2 completes.

## Technical Decisions

### Using Existing TennisScoring Library
- **Game class**: Provides `PointWonBy(Side)`, `GetScoreText()`, `IsFinished`, `Winner`
- **Side enum**: `PlayerA` and `PlayerB` for player identification
- **Mapping**: Console will map player names to Side.PlayerA/PlayerB

### Console UI Pattern
- **ConsoleUI**: Static helper methods for prompts and validation
- **MatchController**: Instance-based controller managing one match session
- **Program**: Simple main loop calling MatchController

### Input Format
- Player names: Free-form text input
- Point scoring: "1" or "2" for player 1 or player 2
- Exit command: "exit" (case-insensitive)

### Error Handling
- Invalid input: Display error message, re-prompt
- Empty names: Treat as invalid, re-prompt
- Game state errors: Should not occur (library handles internally)

## Testing Strategy

### Unit Tests (ConsoleUI helpers)
- Input validation logic
- String parsing and normalization
- Error message generation

### Integration Tests (MatchController)
- Complete match flow from start to finish
- Win detection and announcement
- Exit command handling
- Edge cases (invalid input, empty names)

### Manual Testing Scenarios
- Complete match with standard scoring
- Deuce/Advantage scenarios
- Exit at various points
- Invalid input recovery
- Same player names

## Complexity Tracking

> No constitution violations - this section is not needed for this simple feature.
