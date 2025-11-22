using System.Windows.Forms;
using System.Drawing;

namespace TennisScoring.WinForms.Forms;

public class GameForm : Form
{
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
    }
}
