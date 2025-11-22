# Quickstart: Windows Forms Tennis Game

## Prerequisites
- .NET 8.0 SDK
- Windows OS (for Windows Forms support)

## Running the Game

1.  **Build the Solution**:
    ```powershell
    dotnet build
    ```

2.  **Run the Application**:
    Navigate to the project directory and run:
    ```powershell
    cd src/TennisScoring.WinForms
    dotnet run
    ```
    *Alternatively, set `TennisScoring.WinForms` as the startup project in Visual Studio and press F5.*

## Controls

| Action | Player A (Left) | Player B (Right) |
| :--- | :--- | :--- |
| **Move Up** | `Q` | `Up Arrow` |
| **Move Down** | `J` | `Down Arrow` |
| **Serve** | `Space` | `Space` |
| **Quit** | `Esc` | `Esc` |

## Game Rules
1.  Enter player names at the start.
2.  The server is chosen randomly.
3.  Press **Space** to serve.
4.  Use controls to move paddles and hit the ball.
5.  If the ball passes your paddle, the opponent scores.
6.  First to win according to Tennis rules wins the match.
