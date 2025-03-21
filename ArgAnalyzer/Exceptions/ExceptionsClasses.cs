// classe di gestione dell'eccezione che l'opzione è già esistente
namespace ArgsAnalyzer.Exceptions;

public class OptionException : Exception {

    public OptionException (string message) : base(
        $"OptionError: {message}"
    ) { }
}

public class ArgumentException : Exception {

    public ArgumentException (string message) : base(
        $"OptionError: {message}"
    ) { }
}

public class CommandException : Exception {

    public CommandException (string message) : base(
        $"CommandError: {message}"
    ) { }
}

public class ParserException : Exception {

    public ParserException (string message) : base(
        $"ParserError: {message}"
    ) { }
}

public class BuilderException : Exception {

    public BuilderException (string message) : base(
        $"BuilderError: {message}"
    ) { }
}

#region Options
public class OptionAlreadyExistsException : OptionException {

    public OptionAlreadyExistsException (string optionName) : base(
        $"'{optionName}' already Exists"
    ) { }
}

public class OptionNullException : OptionException {

    public OptionNullException () : base(
        $"Option is Null"
    ) { }
    public OptionNullException (string optionAlias) : base(
        $"'{optionAlias}' is not an option of the command"
    ) { }
}
#endregion

#region Arguments
public class ArgumentAlreadyExistsException : ArgumentException {

    public ArgumentAlreadyExistsException (string argumentName) : base(
        $"'{argumentName}' already Exists"
    ) { }
}

public class ErrorCardinalityException : ArgumentException {

    public ErrorCardinalityException () : base(
        $"Cardinality must be among 0, 1, *"
    ) { }
}
#endregion

#region Commands
public class CommandNullException : CommandException {

    public CommandNullException () : base(
        $"Command is Null"
    ) { }
}

public class RootCommandNullException : CommandException {

    public RootCommandNullException () : base(
        $"Root Command is Null"
    ) { }
}
#endregion

#region ArgsParser
public class ArgsNullException : ParserException {

    public ArgsNullException () : base(
        $"Args list passed to the program is Null"
    ) { }
}
#endregion

#region CommandBuilder
public class ConfigPathNullException : BuilderException {

    public ConfigPathNullException () : base(
        $"Config path is Null Or Empty"
    ) { }
}


public class ConfigFileNotExistsException : BuilderException {

    public ConfigFileNotExistsException (string configPath) : base(
        $"Configuration's File '{configPath}' Not Exists "
    ) { }
}
#endregion