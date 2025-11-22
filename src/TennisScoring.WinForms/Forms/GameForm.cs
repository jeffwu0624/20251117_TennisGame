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

    public GameForm()
    {
        // Basic Form Setup
        Text = "Tennis Pong";
        ClientSize = new Size(800, 600);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        // Enable Double Buffering to prevent flicker
        DoubleBuffered = true;
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        UpdateStyles();

        InitializeSetupControls();
    }

    private void InitializeSetupControls()
    {
        _pnlSetup = new Panel { Dock = DockStyle.Fill };
        
        var lblA = new Label { Text = "Player A Name:", Location = new Point(250, 200), AutoSize = true, Font = new Font("Arial", 12) };
        _txtPlayerA = new TextBox { Location = new Point(400, 200), Width = 150, Text = "PlayerA", Font = new Font("Arial", 12) };
        
        var lblB = new Label { Text = "Player B Name:", Location = new Point(250, 240), AutoSize = true, Font = new Font("Arial", 12) };
        _txtPlayerB = new TextBox { Location = new Point(400, 240), Width = 150, Text = "PlayerB", Font = new Font("Arial", 12) };
        
        _btnStart = new Button { Text = "Start Game", Location = new Point(350, 300), Width = 100, Height = 40, Font = new Font("Arial", 12, FontStyle.Bold) };
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
        _gameEngine.Start();

        Focus(); // Ensure form has focus for key events
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        if (_gameEngine == null) return;

        var state = _gameEngine.GetState();
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        // Draw Background (Black)
        g.Clear(Color.Black);

        // Draw Center Line
        using (var pen = new Pen(Color.Gray, 2) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
        {
            g.DrawLine(pen, ClientSize.Width / 2, 0, ClientSize.Width / 2, ClientSize.Height);
        }

        // Draw Paddles
        using (var brushA = new SolidBrush(Color.Blue))
        {
            g.FillRectangle(brushA, state.PlayerAPaddle);
        }
        using (var brushB = new SolidBrush(Color.Red))
        {
            g.FillRectangle(brushB, state.PlayerBPaddle);
        }

        // Draw Ball
        using (var brushBall = new SolidBrush(Color.Yellow))
        {
            float r = 10; // Assuming radius 10, but state has position. 
            // Wait, GameState has BallPosition (PointF). Ball radius is in Engine constants.
            // I should probably expose Radius in GameState or assume it.
            // Engine uses 10f.
            g.FillEllipse(brushBall, state.BallPosition.X - r, state.BallPosition.Y - r, r * 2, r * 2);
        }

        // Draw Score
        if (!string.IsNullOrEmpty(state.ScoreText))
        {
            using (var font = new Font("Arial", 24, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.White))
            {
                var size = g.MeasureString(state.ScoreText, font);
                g.DrawString(state.ScoreText, (ClientSize.Width - size.Width) / 2, 50, font, brush);
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
            case Keys.J: _inputState.PlayerADown = isPressed; break;
            case Keys.Up: _inputState.PlayerBUp = isPressed; break;
            case Keys.Down: _inputState.PlayerBDown = isPressed; break;
            case Keys.Space: _inputState.Serve = isPressed; break;
            case Keys.Escape: if (isPressed) Application.Exit(); break;
        }

        _gameEngine.HandleInput(_inputState);
    }
}
