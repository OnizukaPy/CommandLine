// CommandLineController.cs
namespace CommandLine.Controllers;

public class CommandLineController {

    public static RootCommand BuildCommandLine(CommandLineConfig config) {

        var rootCommand = new RootCommand("My CLI application");

        if (config.Commands == null) {
            throw new ArgumentNullException(nameof(config.Commands), "Commands cannot be null");
        }

        foreach (var commandConfig in config.Commands.Values) {
            if (string.IsNullOrEmpty(commandConfig.Command)) {
                throw new ArgumentNullException(nameof(commandConfig.Command), "Command name cannot be null or empty");
            }
            var command = new Command(commandConfig.Command, commandConfig.Description);

            if (commandConfig.Options == null) {
                throw new ArgumentNullException(nameof(commandConfig.Options), "Options cannot be null");
            }
            foreach (var optionConfig in commandConfig.Options) {
                var option = CreateOption(optionConfig);
                command.AddOption(option);
            }


            command.SetHandler((context) => {

                var inputOption = new Option<string>("--input", "Input file path");
                var outputOption = new Option<string>("--output", "Output file path");

                string? inputFilePath = context.ParseResult.GetValueForOption(inputOption);
                string? outputFilePath = context.ParseResult.GetValueForOption(outputOption);

                if (string.IsNullOrEmpty(inputFilePath) || string.IsNullOrEmpty(outputFilePath)) {
                    Console.WriteLine("Errore: sia --input che --output devono essere specificati.");
                    return;
                }
                // logica del comando
                // ...
                Console.WriteLine($"File copiato da '{inputFilePath}' a '{outputFilePath}'.");

            });

            // aggiunge il comando alla radice
            rootCommand.AddCommand(command);
        }

        return rootCommand;
    }

    private static Option CreateOption(OptionConfig optionConfig) {

        Option option;

        // disriminiamo la cardinalità solo per le stringhe, mentre in caso di booleani la cardinalità è sempre 0 o 1
        if (optionConfig.Type == "boolean" && string.IsNullOrEmpty(optionConfig.Name)) {

            option = new Option<bool>(optionConfig.Name, optionConfig.Description);
        } else {
            if (optionConfig.Arity == "+") {
                option = new Option<string[]>(optionConfig.Name, optionConfig.Description);
            } else if (optionConfig.Arity == "*") {
                option = new Option<string[]>(optionConfig.Name, optionConfig.Description);
            } else {
                option = new Option<string>(optionConfig.Name, optionConfig.Description);
            }

            if (optionConfig.Choices != null && optionConfig.Choices.Any()) {
                if (option is Option<string[]> stringArrayOption) {
                    stringArrayOption.FromAmong(optionConfig.Choices.ToArray());
                } else if (option is Option<string> stringOption) {
                    stringOption.FromAmong(optionConfig.Choices.ToArray());
                }
            }
        }

        if (optionConfig.DefaultValue != null) {
            option.SetDefaultValue(optionConfig.DefaultValue);
        }
        return option;
    }
}