// Classe di modelle della opzione
public class Option {

    // Proprietà
    public string Name { get; set; } = null!;
    public string[] Aliases { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool Flag { get; set; }
    public string? Value { get; set; } 

    // Costruttore
    public Option (string name, string description, string[] aliases, bool flag) {

        if (!String.IsNullOrEmpty(name)) Name = name;
        if (!String.IsNullOrEmpty(description)) Description = description;
        // se aliases non è nulla e ogni suo componente non è nullo
        if (aliases != null && !aliases.ToList().Any(a => String.IsNullOrEmpty(a))) Aliases = aliases;
        // se flag è true value è null
        Flag = flag;
        if (Flag) Value = null;

    }

    // metodo di override ToString
    public string ToHelpString() {
        
        string aliases = $"{String.Join(", ", Aliases)} {(Flag ? "" : "<value>")}";
        return $"\t{String.Join(", ", aliases), -30} {Description}";
    }
}