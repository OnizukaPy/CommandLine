// CommandLineConfig.cs
namespace CommandLine.Models;

public class CommandLineConfig {
    // il comando è la chiave e il valore è il CommandConfig
    public Dictionary<string, CommandConfig>? Commands { get; set; }
    // le opzioni globali sono una lista di OptionConfig
    public List<OptionConfig>? GlobalOptions { get; set; }
}