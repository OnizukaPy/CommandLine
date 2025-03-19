using System.Text.Json;

namespace CommandLine;

public class Program {

    public static async Task<int> Main(string[] args) {

        var portOption = new Option<int>(
            aliases: ["--port", "-p"],
            description: "Il nome della porta" 
        );

        var verboseOption = new Option<bool>(
            aliases: ["--verbose", "-V"],
            description: "Attivare la modalità verbose"
        );
        /* verboseOption.Arity = ArgumentArity.Zero; */

        // Creo il comando principale a cui passo un testo di descrizione
        var rootCommand = new RootCommand(description: "Il mio programma da linea di comando") {
            // aggiungo qui le options
            portOption,
            verboseOption
        };

        // Setto una azione (Handler) che il comando andrà a fare, passando come argomento del metodo
        // un metodo definito altrove
        rootCommand.SetHandler(OnHandle, portOption);
        rootCommand.SetHandler((bool verbose) => OnVerbose(verbose, rootCommand), verboseOption);

        #region Comando test
        // definisco le sue opzioni
        var nameOption = new Option<string[]>(
            aliases: ["--name", "-n"],
            description: "comando di inserimento del nome utente"
        );

        var nameArguments = new Argument<string[]>(
            name: "Names",
            description: "Nomi da aggiungere"
        );

        // Dichiaro il comando a cui do nome e descrizione
        var testCommand = new Command(
            name: "--test",
            description: "comando di test dell'applicazione"
        );
        testCommand.AddOption(nameOption);
        testCommand.AddArgument(nameArguments);
        testCommand.SetHandler(OnTest, nameArguments);
        // aggiuingo al comando root
        rootCommand.Add(testCommand);
        #endregion


        // Dichiaro un builder del commandLine e gli passo come argomento il rootCommand
        var commandLineBulder = new CommandLineBuilder(rootCommand)
                                .UseDefaults();
        
        // Dichiaro un parse a cui passare la build del bulder
        var parser = commandLineBulder.Build();

        var json = JsonSerializer.Serialize(rootCommand);
        File.WriteAllText("command.json", json);

        // Restituisco un invocazione asincrona         
        return await parser.InvokeAsync(args).ConfigureAwait(false);
    }

    public static void OnHandle(int port) {

        // definiamo una logica semplice al metodo
        Console.WriteLine($"Handle On port {port}");

    }

    public static void OnTest(string[] name) {

        // definiamo una logica semplice al metodo
        Console.WriteLine($"Handle test: Hello {String.Join(", ", name)}");

    }

    public static void OnVerbose(bool verbose,RootCommand rootCommand) {
        Console.WriteLine($"Verbose On {verbose}");
        var json = JsonSerializer.Serialize(rootCommand, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("command.json", json);
    }
}
