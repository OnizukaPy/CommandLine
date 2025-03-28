// classe per analizzare l'array args passato dal main
namespace ArgsAnalyzer.Parsers;

public class ArgsParser {

    // proprietà
    string[]? _args { set; get; }
    // comando di root
    RootCommand? _rootCommand { get; set; }
    // RootOptions List
    // RootArgumentList
    // SubCommandList
    List<Command>? _subCommands { get; set; }

    // costruttore
    public ArgsParser (string[] args, RootCommand rootCommand) {

        _args = args != null ? args : null;
        if (_args == null) throw new ArgsNullException();

        _rootCommand = rootCommand != null ? rootCommand : null;
        if (_rootCommand == null) throw new CommandNullException();

        // get subcommand List
        _subCommands = _rootCommand.Commands != null ? _rootCommand.Commands : null;

    }

        // metodo per ottenere il valore da un comando passato
    public string GetOptionValue(string[] optionAliases, List<string> argsList) {
        //List<string> argList = _args != null ? _args.ToList() : new ();
        string value = String.Empty;
        for (int i = 0; i < argsList.Count(); i++) {
            if (optionAliases.Contains(argsList[i])) {
                if (i + 1 < argsList.Count())
                    if (!argsList[i+1].StartsWith("-"))
                        value =  argsList[i+1];
            }
        }
        return value;
    }

    // metodo per verificare se ci sono comandi inattesi
    public bool FindStrangers(List<string> argsList, out string value) {

        if( _rootCommand !=  null && 
            _rootCommand.options != null) {
            
            List<Option> rootOption = _rootCommand.options;
            List<string> rootOptionsAliases = new();
            rootOption.ForEach(ro => rootOptionsAliases.AddRange(ro.Aliases.ToList()));

            // cicliamo la lista
            for (int i = 0; i < argsList.Count(); i++) {
                
                string arg = argsList[i];
                bool check = false;

                if (arg.StartsWith("-") && !rootOptionsAliases.Contains(arg)) {
                    check = true;
                    value = arg;
                    Console.WriteLine($"Argomento in esame: {arg}, {check}");
                    return true;
                }
            }
        }
        value = String.Empty;
        return false;
    }
}

