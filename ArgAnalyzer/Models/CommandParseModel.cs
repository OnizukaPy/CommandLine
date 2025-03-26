namespace ArgsAnalyzer.Models;

public class CommandParseModel {

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? CommandName { get; set; } = null;
    public List<string>? optionsValues { get; set; } = new();
    public List<string>? argumentValues { get; set; } = new();
}