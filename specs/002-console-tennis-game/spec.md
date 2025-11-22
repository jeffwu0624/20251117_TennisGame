# Feature Specification: Interactive Tennis Game Console

**Feature Branch**: `002-console-tennis-game`  
**Created**: 2025-11-22  
**Status**: Draft  
**Input**: User description: "新增一個.net core console專案並加入參考TennisScoring,利用一問一答方式決定由哪位選手得分,直到一方選手勝出(win)為止,或輸入exit結束比賽, 開始比賽時需先輸入選手名字, 再每一回合詢問本次由哪個選手得分, 顯示本次得分結果, 如 Love-All"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Start New Tennis Match (Priority: P1)

Users need to start a new tennis match by providing player names before tracking any scores. This establishes the context for the entire game session.

**Why this priority**: This is the foundational requirement - without player names, there is no match to track. It's the minimal viable product that sets up the game state.

**Independent Test**: Can be fully tested by launching the application, entering two player names, and verifying that the system acknowledges the players and displays the initial score (Love-All).

**Acceptance Scenarios**:

1. **Given** the application starts, **When** the user is prompted to enter player names, **Then** the system should request the first player's name
2. **Given** the first player's name is entered, **When** the user presses enter, **Then** the system should request the second player's name
3. **Given** both player names are entered, **When** the second name is confirmed, **Then** the system should display the initial score as "Love-All" or "[Player1] 0 - 0 [Player2]"

---

### User Story 2 - Record Point and Display Score (Priority: P1)

During each round of the match, users need to indicate which player scored a point and see the updated score using standard tennis terminology (Love, 15, 30, 40, Deuce, Advantage, Game, Win).

**Why this priority**: This is the core gameplay loop - without the ability to record points and see updated scores, the application has no purpose. Together with Story 1, this forms the complete MVP.

**Independent Test**: Can be fully tested by starting a match with two players, recording several points for different players, and verifying that the score updates correctly according to tennis scoring rules for each input.

**Acceptance Scenarios**:

1. **Given** a match is in progress at Love-All, **When** the user indicates Player 1 scored, **Then** the system should display "15-Love" (or localized equivalent)
2. **Given** the score is 30-30, **When** the user indicates either player scored, **Then** the system should display "40-30" or "30-40" accordingly
3. **Given** the score is 40-40 (Deuce), **When** the user indicates a player scored, **Then** the system should display "Advantage [Player]"
4. **Given** a player has Advantage, **When** that same player scores again, **Then** the system should display "Game [Player]" and ask to continue or end
5. **Given** a player has Advantage, **When** the other player scores, **Then** the system should return to Deuce
6. **Given** each round completes, **When** score is displayed, **Then** the system should prompt for the next round's scoring player

---

### User Story 3 - Exit Match Gracefully (Priority: P2)

Users need the ability to exit the match at any time without completing the game, allowing them to abandon matches that cannot be finished.

**Why this priority**: This provides user control and prevents users from being forced to complete a match. It's important for usability but not essential for the core tennis scoring functionality.

**Independent Test**: Can be fully tested by starting a match, recording some points, then typing "exit" and verifying the application terminates gracefully with an appropriate message.

**Acceptance Scenarios**:

1. **Given** a match is in progress, **When** the user types "exit" at any prompt, **Then** the system should display a farewell message and terminate the match
2. **Given** a match is in progress, **When** the user types "exit", **Then** the current score should not be saved or carried over

---

### User Story 4 - Complete Match with Winner (Priority: P1)

When a player wins a game, users need clear indication of the winner and the option to start a new match or exit the application.

**Why this priority**: This provides closure to the match and is essential for the complete user experience. Without win detection, matches would continue indefinitely.

**Independent Test**: Can be fully tested by simulating a complete game where one player reaches win condition (winning by 2 points after deuce or reaching 4 points with 2-point lead), and verifying the system announces the winner and stops accepting score inputs for that game.

**Acceptance Scenarios**:

1. **Given** a player has won the game, **When** the win condition is met, **Then** the system should display "[Player] wins!" or similar victory message
2. **Given** a player has won, **When** the victory message is shown, **Then** the system should ask if the user wants to start a new match or exit
3. **Given** the victory message is displayed, **When** the user chooses to start a new match, **Then** the system should return to player name entry
4. **Given** the victory message is displayed, **When** the user chooses to exit, **Then** the application should terminate gracefully

---

### Edge Cases

- What happens when the user enters an empty string or whitespace-only text for player names?
- How does the system handle invalid input when asking which player scored (e.g., entering "3" when only players 1 and 2 exist)?
- What happens if the user enters "exit" with mixed case (e.g., "Exit", "EXIT", "eXiT")?
- How does the system respond to unexpected input types (e.g., special characters, very long names)?
- What happens if both players are given the same name?

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST prompt user to enter the first player's name at application start
- **FR-002**: System MUST prompt user to enter the second player's name after the first name is provided
- **FR-003**: System MUST initialize the game with both player names and display the starting score (Love-All)
- **FR-004**: System MUST prompt user each round to indicate which player scored the point
- **FR-005**: System MUST accept player selection input (player 1 or player 2) in a clear format
- **FR-006**: System MUST update the game score based on the scoring player according to standard tennis scoring rules
- **FR-007**: System MUST display the current score after each point using tennis terminology (Love, 15, 30, 40, Deuce, Advantage)
- **FR-008**: System MUST detect when a player has won the game (achieved win condition per tennis rules)
- **FR-009**: System MUST announce the winner when win condition is met
- **FR-010**: System MUST accept "exit" command at any prompt to terminate the match
- **FR-011**: System MUST handle "exit" command case-insensitively (exit, Exit, EXIT should all work)
- **FR-012**: System MUST terminate gracefully when exit command is received
- **FR-013**: System MUST continue prompting for points until either a player wins or user exits
- **FR-014**: System MUST reference and utilize the existing TennisScoring library for score calculation logic
- **FR-015**: System MUST display player names alongside scores when showing current game state
- **FR-016**: System MUST validate user input and handle invalid entries with appropriate error messages
- **FR-017**: System MUST allow starting a new match after a game is won

### Key Entities

- **Player**: Represents a tennis player with a name identifier, participates in the match and can score points
- **Match**: Represents the tennis game session containing two players, current score state, and game progression
- **Score State**: Represents the current point standing in the match following tennis scoring rules (Love-0, 15, 30, 40, Deuce, Advantage, Game/Win)
- **User Input**: Represents commands and responses from the user including player names, point scoring selections, and control commands (exit)

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Users can start a match by entering two player names in under 30 seconds
- **SC-002**: Users can complete a full tennis game (from Love-All to win) recording at least 8 points without errors
- **SC-003**: Score is displayed immediately after each point is recorded (within 1 second)
- **SC-004**: Win condition is correctly detected 100% of the time when a player reaches game point with the required lead
- **SC-005**: Users can exit the match at any time by typing "exit" and the application responds within 1 second
- **SC-006**: The application correctly handles and displays all standard tennis scores (Love through Win) in each test match
- **SC-007**: 95% of test users successfully complete their first match without needing help documentation

## Assumptions *(mandatory)*

- Users have basic familiarity with tennis scoring terminology (Love, 15, 30, 40, Deuce, Advantage, Game)
- Application is a single-game mode (one set with one game), not tracking multiple games or sets
- Player input will be primarily keyboard-based in a console environment
- The existing TennisScoring library correctly implements tennis scoring logic
- Console application will run on systems with .NET Core runtime installed
- Input language can be English or Chinese based on user preference for prompts
- Network connectivity is not required for this application
- The application is intended for casual tracking, not official match recording
