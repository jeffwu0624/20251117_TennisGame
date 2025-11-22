namespace TennisScoring.Console;

internal sealed class InputValidator
{
    public bool TryNormalizePlayerName(string? input, out string normalized)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            normalized = string.Empty;
            return false;
        }

        normalized = input.Trim();
        return normalized.Length > 0;
    }

    public bool TryParseScoringSelection(string? input, out int playerIndex)
    {
        playerIndex = -1;
        if (string.IsNullOrWhiteSpace(input))
            return false;

        var trimmed = input.Trim();

        // Accept numeric selections: "1" => player 0, "2" => player 1
        if (int.TryParse(trimmed, out var n))
        {
            if (n == 1)
            {
                playerIndex = 0;
                return true;
            }

            if (n == 2)
            {
                playerIndex = 1;
                return true;
            }
        }

        return false;
    }

    public bool IsExitCommand(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        return string.Equals(input.Trim(), "exit", System.StringComparison.OrdinalIgnoreCase);
    }

    public bool TryParseRestartCommand(string? input, out bool startNewMatch)
    {
        startNewMatch = false;
        return false;
    }
}
