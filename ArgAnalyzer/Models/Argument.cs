// Classe di modello di un Argomento
public class Argument {

    // Proprietà
    public string Name { get; set; } = null!;
    public string[] Aliases { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool Flag { get; set; }
    public string Cardinality { get; set; } = null!;
    public List<string>? Values { get; set; } 

    // costruttore
    public Argument (string name, string description, string[] aliases, string cardinality) {

        if (!String.IsNullOrEmpty(name)) Name = name;
        if (!String.IsNullOrEmpty(description)) Description = description;
        // se aliases non è nulla e ogni suo componente non è nullo
        if (aliases != null && !aliases.ToList().Any(a => String.IsNullOrEmpty(a))) Aliases = aliases;
        // se la cardinatilità non è nulla
        if (!String.IsNullOrEmpty(cardinality)) Cardinality = cardinality;
        // setrtiamo ora Flag e Values in base alla cardinalità
        if (Cardinality.Equals("0")) {
            Flag = true;
            Values = null;
        } else if (Cardinality.Equals("*")) {
            Flag = false;
            Values = new();
        } else if (Cardinality.Equals("1")) {
            Flag = false;
            Values = new(1);
        } else {
            throw new ErrorCardinalityException();
        }

    }

    // metodo di override ToString
    public string ToHelpString() {
        
        string aliases = $"{String.Join(", ", Aliases)}";
        if (Cardinality.Equals("*")) {
            aliases += " <[values]>";
        } else {
            aliases += " <value>";
        }
        return $"\t{aliases, -30} {Description}";
    }
}