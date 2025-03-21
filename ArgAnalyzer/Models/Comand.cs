// Classe di definizione dell'oggetto Comand
namespace ArgsAnalyzer.Models;
public class Command {

    // propeità
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? CommandName { get; set; } = null;
    public List<Option>? options { get; set; }
    public List<Argument>? arguments { get; set; }

    public Command (string name, string description) {

        if (!String.IsNullOrEmpty(name)) Name = name;
        if (!String.IsNullOrEmpty(description)) Description = description;

        // aggiungiamo le opzione di default
        options = new();
        AddOption(new Option (
            name: "Help Option",
            description: "Option to read command's details",
            aliases: ["--help", "-h", "?"],
            flag: true
            )
        );

        arguments = new();
    }

    // costruttore
    public Command (string name, string description, string commandName) {

        if (!String.IsNullOrEmpty(name)) Name = name;
        if (!String.IsNullOrEmpty(description)) Description = description;
        if (!String.IsNullOrEmpty(description)) CommandName = commandName;

        // aggiungiamo le opzione di default
        options = new();
        AddOption(new Option (
            name: "Help Option",
            description: "Option to read command's details",
            aliases: ["--help", "-h", "?"],
            flag: true
            )
        );

        arguments = new();
    }

    // metodi di classe
    // metodo per aggiungere una opzione
    public void AddOption(Option option) {
        if (option != null && options != null) {
            // se l'opzione non è già contenuta e non c'è n'è una con lo stesso nome
            if (!options.Contains(option) && !options.Any(o => o.Name.Equals(option.Name))) {
                options.Add(option);
            } else {
                throw new OptionAlreadyExistsException($"{option.Name}");
            }
        }
    }

    // metodo per aggiungere un argomento
    public void AddArgument(Argument argument) {
        if (argument != null && arguments != null) {
            // se l'opzione non è già contenuta e non c'è n'è una con lo stesso nome
            if (!arguments.Contains(argument) && !arguments.Any(a => a.Name.Equals(argument.Name))) {
                arguments.Add(argument);
            } else {
                throw new ArgumentAlreadyExistsException($"{argument.Name}");
            }
        }
    }

    // metodo di override ToString
    public string ToHelpString(string rootName) {
        StringBuilder stringBuilder = new();
        string syntax = $"Sintax: {rootName} {CommandName} {(options?.Count() > 0 ? "[options]" : "")} {(arguments?.Count() > 0 ? "[arguments]" : "")}";
        stringBuilder.AppendLine(new string('-', syntax.Length));
        stringBuilder.AppendLine($"{CommandName}");
        stringBuilder.AppendLine(new string('-', syntax.Length));
        stringBuilder.AppendLine(syntax);
        stringBuilder.AppendLine($"\n{Description}");
        stringBuilder.AppendLine("");
        if (options?.Count() > 0) {
            stringBuilder.AppendLine("Options: ");
            options.ForEach(o => stringBuilder.AppendLine(o.ToHelpString()));
        }
        stringBuilder.AppendLine("");
        if (arguments?.Count() > 0) {
            stringBuilder.AppendLine("Arguments: ");
            arguments.ForEach(a => stringBuilder.AppendLine(a.ToHelpString()));
        }
        return stringBuilder.ToString();
    }
}