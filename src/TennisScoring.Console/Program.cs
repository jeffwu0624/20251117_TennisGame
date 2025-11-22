using TennisScoring.Console;
using SystemConsoleAdapter = TennisScoring.Console.Console.SystemConsoleAdapter;

var consoleAdapter = new SystemConsoleAdapter();
var consoleUi = new ConsoleUI(consoleAdapter);
var validator = new InputValidator();
var controller = new MatchController(consoleUi, validator);

await controller.RunAsync();
