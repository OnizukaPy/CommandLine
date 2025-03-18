// CommandConfig.cs
namespace CommandLine.Models;
public class CommandConfig {

    // la desrizione del comando
    public string? Description { get; set; }
    // il comando
    public string? Command { get; set; }
    // le opzioni del comando
    public List<OptionConfig>? Options { get; set; }
}