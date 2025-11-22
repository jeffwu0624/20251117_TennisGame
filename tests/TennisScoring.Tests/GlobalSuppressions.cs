using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores",
    Justification = "Test method names use underscores for readability (Given_When_Then pattern)",
    Scope = "namespaceanddescendants",
    Target = "~N:TennisScoring.Tests")]
