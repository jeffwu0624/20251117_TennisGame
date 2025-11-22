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
        return false;
    }

    public bool IsExitCommand(string? input)
    {
        return false;
    }

    public bool TryParseRestartCommand(string? input, out bool startNewMatch)
    {
        startNewMatch = false;
        return false;
    }
}
