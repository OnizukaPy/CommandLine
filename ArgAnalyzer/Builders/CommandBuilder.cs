// classe per costruire il comando ed attivare i suoi metodi
namespace ArgsAnalyzer.Builders;

public class CommandBuilder {

    // proprietà
    RootCommand? _rootCommand { get; set; }

    #region Costruttori
    public CommandBuilder () {

        if (_rootCommand == null) {
            Build();
        } else {
            throw new RootCommandAlradyExistsException();
        }
    }
    public CommandBuilder (string configPath) {

        if (_rootCommand == null) {
            if (!String.IsNullOrEmpty(configPath)) {
                if(File.Exists(configPath)) {
                    LoadConfig(configPath);
                } else {
                throw new ConfigFileNotExistsException(configPath);
            }
            } else {
                throw new ConfigPathNullException();
            }
        } else {
            throw new RootCommandAlradyExistsException();
        }
    }
    #endregion

    // metodo per creare una configurazione di comandi
    public void Build () {
        // creiamo il root command
        var rootCommand = new RootCommand (
            name: "01_rimborso_spese.exe",
            description: "Refunds management program"
        );

        // aggiungiamo una opzione
        var inputOption = new Option (
            name: "Input Option",
            description: "Option to add the input file of expenses",
            aliases: ["--input", "-i"],
            flag: false
        );
        rootCommand.AddOption(inputOption);

        var outputOption = new Option (
            name: "Output Option",
            description: "Option to add the output file of report",
            aliases: ["--output", "-o"],
            flag: false
        );
        rootCommand.AddOption(outputOption);

        var verboseOption = new Option (
            name: "Verbose Option",
            description: "Option activate the verbose mode of the program",
            aliases: ["--verbose", "-V"],
            flag: true
        );
        rootCommand.AddOption(verboseOption);

        var logOption = new Option (
            name: "Logging Option",
            description: "Option activate the Logging mode of the program",
            aliases: ["--log"],
            flag: true
        );
        rootCommand.AddOption(logOption);

        var debugOption = new Option (
            name: "Debug Option",
            description: "Option activate the Debug mode of the program",
            aliases: ["--debug", "-d"],
            flag: true
        );
        rootCommand.AddOption(debugOption);

        var processFolderOption = new Option (
            name: "Process Folder Option",
            description: "Option to process a Folder with many expenses",
            aliases: ["--process-folder", "-pF"],
            flag: true
        );
        rootCommand.AddOption(processFolderOption);

        _rootCommand = rootCommand;
    }

    // metodo per salvare la configurazione
    public void SaveRootConfig () {
        if (_rootCommand != null) {
            var json = JsonConvert.SerializeObject(_rootCommand, Formatting.Indented);
            File.WriteAllText($"{_rootCommand.Name}_config.json", json);
        } else {
            throw new CommandNullException();
        }
    }

    // metodo per caricare la configurazione da un file config.json
    public void LoadConfig (string path) {

        if (!String.IsNullOrEmpty(path)) {
            // se uso un oggetto con il metodo, il json deve essere tra le []. Se il json è fatto di un solo elemento 
            // meglio fare il cast 
            // errore: Unexpected character encountered while parsing value: e. Path '', line 0, position 0.
            _rootCommand = (RootCommand?)JsonConvert.DeserializeObject(path);
            if (_rootCommand == null) throw new RootCommandNullException();
        } else {
            throw new ConfigPathNullException();
        }
    }

    // metodo per estrarre le opzioni dal comando
    public Option? GetOptionbyAlias (string alias) {

        if (_rootCommand != null) {
            var option = _rootCommand.options?.FirstOrDefault(o => o.Aliases.Contains(alias));
            if (option == null) throw new OptionNullException(alias);
            return option;
        } else {
            throw new CommandNullException();
        }
    }

    // metodo per estrarre gli argomenti dal comando
    public Argument? GetArgumentsbyAlias (string alias) {

        if (_rootCommand != null) {
            var argument = _rootCommand.arguments?.FirstOrDefault(a => a.Aliases.Contains(alias));
            if (argument == null) throw new OptionNullException(alias);
            return argument;
        } else {
            throw new CommandNullException();
        }
    }

    // metodo get per ottenere il comando di root
    public RootCommand GetRootCommand() {
        if (_rootCommand != null) {
            return _rootCommand;
        } else {
            throw new RootCommandNullException();
        }
    }
}