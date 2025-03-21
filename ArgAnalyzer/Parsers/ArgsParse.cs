// classe per analizzare l'array args passato dal main
namespace ArgsAnalyzer.Parsers;

public class ArgsParser {

    // proprietà
    string[]? _args { set; get; }

    // comando di root
    Command? _rootCommand { get; set; }

    // costruttore
    public ArgsParser (string[] args, Command rootCommand) {

        if (args != null) {
            _args = args;
        } else {
            _args = null;
            throw new ArgsNullException();
        }

        if (rootCommand != null) {
            _args = args;
        } else {
            _rootCommand = null;
            throw new CommandNullException();
        }

    }
}