// Classe di definizione del comando di Root
public class RootCommand : Command {

    // proprietà
    public List<Command> Commands { get; set; }
    public RootCommand(string name, string description) : base(name, description) {

        Commands = new();

        // aggiungiamo l'opzione di default --version
        AddOption(new Option (
            name: "Program Version",
            description: "Option to show del version of the program",
            aliases: ["--version", "-v"],
            flag: true
            )
        );
    }

    // metodo per aggiungere un argomento
    public void AddSubCommand(Command command) {
        if (command != null && Commands != null) {
            // se l'opzione non è già contenuta e non c'è n'è una con lo stesso nome
            if (!Commands.Contains(command) && !Commands.Any(cmd => {
                return cmd.CommandName != null && cmd.CommandName.Equals(command.CommandName);
                })) {
                Commands.Add(command);
            } else {
                throw new ArgumentAlreadyExistsException($"{command.CommandName}");
            }
        }
    }

    // metodo di override ToString
    public string ToHelpString() {
        StringBuilder stringBuilder = new();
        string syntax = $"Sintax: {Name} {(options?.Count() > 0 ? "[options]" : "")} {(arguments?.Count() > 0 ? "[arguments]" : "")}";

        stringBuilder.AppendLine(new string('=', syntax.Length));
        stringBuilder.AppendLine($"{Name}");
        stringBuilder.AppendLine(new string('=', syntax.Length));
        if (options?.Count() > 0 || arguments?.Count() > 0) {
            stringBuilder.AppendLine(syntax);
        }

        stringBuilder.AppendLine($"\n{Description}");
        stringBuilder.AppendLine("");

        // Options
        stringBuilder.AppendLine($"\nOptions: ");
        if (options?.Count() > 0) {
            options.ForEach(o => stringBuilder.AppendLine(o.ToHelpString()));
        }

        // Arguments
        stringBuilder.AppendLine($"\nArguments: ");
        if (arguments?.Count() > 0) {
            arguments.ForEach(a => stringBuilder.AppendLine(a.ToHelpString()));
        }

        stringBuilder.AppendLine($"\nCommands: ");
        if (Commands?.Count() > 0) {
            stringBuilder.Append("");
            Commands.ForEach(cmd => stringBuilder.AppendLine(cmd.ToHelpString(Name)));
        }
        return stringBuilder.ToString();
    }
}