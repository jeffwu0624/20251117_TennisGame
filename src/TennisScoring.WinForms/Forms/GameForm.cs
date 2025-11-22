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
}
