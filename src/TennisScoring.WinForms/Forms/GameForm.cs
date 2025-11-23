using System.Windows.Forms;
using System.Drawing;
using TennisScoring.WinForms.Engine;

namespace TennisScoring.WinForms.Forms;

public class GameForm : Form
{
    private TextBox _txtPlayerA = null!;
    private TextBox _txtPlayerB = null!;
    private Button _btnStart = null!;
    private Panel _pnlSetup = null!;
    private PongEngine? _gameEngine;
    private InputState _inputState = new InputState();
    private System.Windows.Forms.Timer _gameTimer = null!;

    public GameForm()
    {
        // Basic Form Setup
        Text = "Tennis Pong";
        ClientSize = new Size(1600, 1200);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        // Enable Double Buffering to prevent flicker
        DoubleBuffered = true;
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        UpdateStyles();

        InitializeSetupControls();
        InitializeGameTimer();
    }

    private void InitializeGameTimer()
    {
        _gameTimer = new System.Windows.Forms.Timer();
        _gameTimer.Interval = 16; // ~60 FPS
        _gameTimer.Tick += GameLoop_Tick;
    }

    private void GameLoop_Tick(object? sender, EventArgs e)
    {
        if (_gameEngine != null && _gameEngine.IsRunning)
        {
            _gameEngine.Update(0.016f); // Fixed time step for simplicity
            Invalidate();
        }
    }

    private void InitializeSetupControls()
    {
        _pnlSetup = new Panel { Dock = DockStyle.Fill };
        
        int centerX = ClientSize.Width / 2;
        int centerY = ClientSize.Height / 2;

        var lblA = new Label { Text = "Player A Name:", Location = new Point(centerX - 200, centerY - 100), AutoSize = true, Font = new Font("Arial", 12) };
        _txtPlayerA = new TextBox { Location = new Point(centerX + 60, centerY - 100), Width = 150, Text = "PlayerA", Font = new Font("Arial", 12) };
        
        var lblB = new Label { Text = "Player B Name:", Location = new Point(centerX - 200, centerY - 60), AutoSize = true, Font = new Font("Arial", 12) };
        _txtPlayerB = new TextBox { Location = new Point(centerX + 60, centerY - 60), Width = 150, Text = "PlayerB", Font = new Font("Arial", 12) };
        
        _btnStart = new Button { Text = "Start Game", Location = new Point(centerX - 50, centerY), Width = 100, Height = 40, Font = new Font("Arial", 12, FontStyle.Bold) };
        _btnStart.Click += BtnStart_Click;

        _pnlSetup.Controls.Add(lblA);
        _pnlSetup.Controls.Add(_txtPlayerA);
        _pnlSetup.Controls.Add(lblB);
        _pnlSetup.Controls.Add(_txtPlayerB);
        _pnlSetup.Controls.Add(_btnStart);

        Controls.Add(_pnlSetup);
    }

    private void BtnStart_Click(object? sender, EventArgs e)
    {
        string nameA = _txtPlayerA.Text;
        string nameB = _txtPlayerB.Text;
        
        if (string.IsNullOrWhiteSpace(nameA) || string.IsNullOrWhiteSpace(nameB))
        {
            MessageBox.Show("Please enter names for both players.");
            return;
        }

        StartGame(nameA, nameB);
    }

    private void StartGame(string nameA, string nameB)
    {
        _pnlSetup.Visible = false;
        
        // Initialize Engine
        _gameEngine = new PongEngine(nameA, nameB, ClientSize);
        _gameEngine.GameEnded += GameEngine_GameEnded;
        _gameEngine.Start();
        _gameTimer.Start();

        Focus(); // Ensure form has focus for key events
    }

    private void GameEngine_GameEnded(object? sender, GameEndedEventArgs e)
    {
        _gameTimer.Stop();
        Invalidate(); // Draw final state
        MessageBox.Show($"{e.Message}\nWinner: {e.WinnerName}", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
        // Optional: Reset to setup screen?
        // For now, just stop.
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        if (_gameEngine == null) return;

        var state = _gameEngine.GetState();
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        // Draw Background (Tennis Court Green)
        g.Clear(Color.FromArgb(34, 139, 34)); // ForestGreen

        // Draw Center Line
        using (var pen = new Pen(Color.White, 4) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
        {
            g.DrawLine(pen, ClientSize.Width / 2, 0, ClientSize.Width / 2, ClientSize.Height);
        }

        // Draw Paddles
        using (var brushPaddle = new SolidBrush(Color.White))
        {
            g.FillRectangle(brushPaddle, state.PlayerAPaddle);
            g.FillRectangle(brushPaddle, state.PlayerBPaddle);
        }

        // Draw Ball
        using (var brushBall = new SolidBrush(Color.Yellow))
        {
            float r = 10; 
            g.FillEllipse(brushBall, state.BallPosition.X - r, state.BallPosition.Y - r, r * 2, r * 2);
        }

        // Draw Score
        if (!string.IsNullOrEmpty(state.ScoreText))
        {
            using (var font = new Font("Arial", 24, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.White))
            {
                var size = g.MeasureString(state.ScoreText, font);
                g.DrawString(state.ScoreText, font, brush, (ClientSize.Width - size.Width) / 2, 50);
            }
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        UpdateInputState(e.KeyCode, true);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);
        UpdateInputState(e.KeyCode, false);
    }

    private void UpdateInputState(Keys key, bool isPressed)
    {
        if (_gameEngine == null) return;

        switch (key)
        {
            case Keys.Q: _inputState.PlayerAUp = isPressed; break;
            case Keys.A: _inputState.PlayerADown = isPressed; break;
            case Keys.Up: _inputState.PlayerBUp = isPressed; break;
            case Keys.Down: _inputState.PlayerBDown = isPressed; break;
            case Keys.Space: _inputState.Serve = isPressed; break;
            case Keys.Escape: if (isPressed) Application.Exit(); break;
        }

        _gameEngine.HandleInput(_inputState);
    }
}
