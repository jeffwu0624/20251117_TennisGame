# Data Model: Windows Forms Tennis Game

## Entities

### 1. Player (選手)
Represents a player in the game.

| Property | Type | Description |
| :--- | :--- | :--- |
| `Name` | `string` | Player's name. |
| `Side` | `TennisScoring.Side` | Player's side (PlayerA/Left or PlayerB/Right). |
| `Paddle` | `Paddle` | The paddle controlled by the player. |

### 2. Paddle (球拍)
Represents the visual representation of the player.

| Property | Type | Description |
| :--- | :--- | :--- |
| `Bounds` | `RectangleF` | Position and size (X, Y, Width, Height). |
| `Speed` | `float` | Movement speed (pixels per tick). |
| `Color` | `Color` | Visual color. |

### 3. Ball (球)
Represents the tennis ball.

| Property | Type | Description |
| :--- | :--- | :--- |
| `Position` | `PointF` | Current X, Y coordinates. |
| `Velocity` | `PointF` | Current X, Y velocity vector. |
| `Radius` | `float` | Radius of the ball. |
| `Speed` | `float` | Base speed magnitude. |

### 4. PongEngine (遊戲引擎)
Manages the game state and logic.

| Property | Type | Description |
| :--- | :--- | :--- |
| `PlayerA` | `Player` | Left player. |
| `PlayerB` | `Player` | Right player. |
| `Ball` | `Ball` | The game ball. |
| `ScoringGame` | `TennisScoring.Game` | Reference to the scoring logic. |
| `IsRunning` | `bool` | Game loop state. |
| `ServingSide` | `TennisScoring.Side` | Who is currently serving. |

## State Transitions

1.  **Initialization**:
    *   Input Names -> Create Players -> Randomize Server -> State: `Serving`.
2.  **Serving**:
    *   Ball attached to Server's Paddle.
    *   Space Key -> Ball Velocity set -> State: `Playing`.
3.  **Playing**:
    *   Ball moves.
    *   Collision with Paddle -> Bounce.
    *   Collision with Top/Bottom Wall -> Bounce.
    *   Collision with Left/Right Wall (Out) -> Score Update -> Reset Ball -> State: `Serving` (or `Finished`).
4.  **Finished**:
    *   Winner determined -> Show Message -> Stop Loop.
