// OptionConfig.cs
public class OptionConfig {

    // il nome dell'opzione
    public string? Name { get; set; }
    // la descrizione dell'opzione
    public string? Description { get; set; }
    // il valore di default
    public string? DefaultValue { get; set; }
    // il tipo dell'opzione
    public string? Type { get; set; }
    // le scelte possibili
    public List<string>? Choices { get; set; }
    // la cardinalità dell'opzione
    public string? Arity { get; set; }
}