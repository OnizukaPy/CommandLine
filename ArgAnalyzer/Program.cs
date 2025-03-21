public class Program {

    public static void Main () {

        // creiamo il root command
        var rootCommand = new RootCommand (
            name: "ArgAnalyzer",
            description: "Programma di gestione degli argomenti"
        );

        // aggiungiamo una opzione
        var nameOption = new Option (
            name: "Name Option",
            description: "Option to add name to the program",
            aliases: ["--name", "-n"],
            flag: false
        );
        rootCommand.AddOption(nameOption);

        // aggiungiamo un argomento
        var ageArgument = new Argument (
            name: "Age Argument",
            description: "Argument to add age to the program",
            aliases: ["age"],
            cardinality: "1"

        );
        rootCommand.AddArgument(ageArgument);

        // aggiugiamo un comando
        var subCommand = new Command (
            name: "Sub command",
            description: "Use sub to give new option to the program",
            commandName: "sub"
        );
        rootCommand.AddSubCommand(subCommand); 
        try {
            var commandBuilder = new CommandBuilder(rootCommand);
            Console.WriteLine(rootCommand.ToHelpString());
            commandBuilder.SaveRootConfig();
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }

        // istanziamo un builder
        /* try {
            var commandBuilder = new CommandBuilder(@"eArgAnalyzer_config.json");
            var rootCommand = commandBuilder.GetRootCommand();
            Console.WriteLine(rootCommand.ToHelpString());
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
        } */
    }
}