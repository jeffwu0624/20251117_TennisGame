# Research & Decisions: Windows Forms Tennis Game

**Feature**: Windows Forms Tennis Game (Pong)
**Date**: 2025-11-23

## 1. Game Loop Implementation

### Decision
Use `System.Windows.Forms.Timer` with an interval of 16ms (approx. 60 FPS).

### Rationale
- **Simplicity**: Easiest to implement in WinForms.
- **Thread Safety**: Runs on the UI thread, so no cross-thread marshalling is needed for UI updates.
- **Performance**: Sufficient for a simple Pong game.

### Alternatives Considered
- `System.Threading.Timer`: More precise but requires `Invoke` to update UI.
- `Application.DoEvents` loop: High CPU usage, generally discouraged.
- `GameLoop` thread: Overkill for this requirement.

## 2. Rendering & Flicker Prevention

### Decision
Enable `DoubleBuffered = true` on the main Form and override `OnPaint`.

### Rationale
- **Flicker Free**: Double buffering is the standard way to prevent flickering in GDI+ animations.
- **Performance**: GDI+ is fast enough for simple 2D shapes.

## 3. Input Handling

### Decision
Use `KeyDown` and `KeyUp` events to toggle boolean flags in a `InputState` class or dictionary.

### Rationale
- **Responsiveness**: Allows handling multiple keys simultaneously (e.g., both players moving).
- **Smoothness**: Prevents the "stutter" caused by Windows key repeat delay.

## 4. Integration with TennisScoring

### Decision
Reference the `TennisScoring` project and use the `Game` class instance within the `PongEngine`.

### Rationale
- **Reusability**: Leverages the existing tested logic.
- **Compliance**: Meets FR-002.

## 5. Project Structure

### Decision
Create a new project `TennisScoring.WinForms` in `src/`.

### Structure
- `Program.cs`: Entry point.
- `Forms/MainForm.cs`: The game window.
- `Engine/PongEngine.cs`: Core game logic (physics, collision, state).
- `Entities/`: `Ball`, `Paddle`, `Player`.
