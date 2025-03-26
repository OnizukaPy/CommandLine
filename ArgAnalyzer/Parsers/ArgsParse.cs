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
}

