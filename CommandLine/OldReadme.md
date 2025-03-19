# Generiamo una onsole App command line

```bash
dotnet new console -o CommandLine
dotnet add package System.CommandLine --version 2.0.0-beta4.22272.1
```

# Aggiungiamo un file README.md

```bash
cd CommandLine
echo "# Command Line App" > README.md
```

# Aggiungiamo un file .gitignore

```bash
echo "bin/" > .gitignore
echo "obj/" >> .gitignore
```

# [Guida](https://g.co/gemini/share/73ed3b789286)

## Step 1

- `Obiettivo`: Creare una app command Line che necessita di due comandi obbligatori per accettare un nome di file come input e un nome file come output. Per entrambi è ammesso un solo valore. Genera il file di configurazione json, Genera il modello, Genera il program.cs.

Il file di configurazione json deve avere la seguente struttura:

```json
{
  "commands": {
    "run": {
      "description": "Copia un file",
      "command": "run",
      "options": [
        {
          "name": "--input",
          "description": "File di input",
          "arity": "1"
        },
        {
          "name": "--output",
          "description": "File di output",
          "arity": "1"
        }
      ]
    }
  },
  "globalOptions": []
}
```

I modelli che servono sono 3:

- `CommandLineConfig`: rappresenta il file di configurazione json

```csharp
// CommandLineConfig.cs
using System.Collections.Generic;

public class CommandLineConfig {
    // il comando è la chiave e il valore è il CommandConfig
    public Dictionary<string, CommandConfig> Commands { get; set; }
    // le opzioni globali sono una lista di OptionConfig
    public List<OptionConfig> GlobalOptions { get; set; }
}
```

- `CommandConfig`: rappresenta un comando

```csharp
// CommandConfig.cs
public class CommandConfig {

    // la desrizione del comando
    public string Description { get; set; }
    // il comando
    public string Command { get; set; }
    // le opzioni del comando
    public List<OptionConfig> Options { get; set; }
}
```

- `OptionConfig`: rappresenta un'opzione. Le opzioni possono avere i seguenti campi:

  - `Name`: il nome dell'opzione
  - `Description`: la descrizione dell'opzione
  - `DefaultValue`: il valore di default
  - `Type`: il tipo dell'opzione. Il tipo è una stringa e può essere `boolean` o `string`. Sono valori escludenti, se il tipo è `boolean` l'opzione non può avere scelte possibili
  - `Choices`: le scelte possibili. Se l'opzione è di tipo `string` e ha delle scelte possibili, l'opzione può assumere solo i valori specificati
  - `Arity`: la cardinalità dell'opzione. Se l'opzione ha cardinalità `1` significa che l'opzione può essere specificata una sola volta. Se l'opzione ha cardinalità `+` significa che l'opzione può essere specificata una o più volte. Se l'opzione ha cardinalità `*` significa che l'opzione può essere specificata zero o più volte. Può avere cardinalità `1`, `+` o `*` se il tipo è `string`. Se il tipo è `boolean` la cardinalità deve essere `0` o `1`

```csharp
// OptionConfig.cs
public class OptionConfig {

    // il nome dell'opzione
    public string Name { get; set; }
    // la descrizione dell'opzione
    public string Description { get; set; }
    // il valore di default
    public string DefaultValue { get; set; }
    // il tipo dell'opzione
    public string Type { get; set; }
    // le scelte possibili
    public List<string> Choices { get; set; }
    // la cardinalità dell'opzione
    public string Arity { get; set; }
}
```

Le classi da redarre sono:

- `Repository.cs`: contiene i metodi per leggere e scrivere il file di configurazione

```csharp
public static class Repository {

    public static CommandLineConfig LoadConfig(string filePath) {
        string jsonString = File.ReadAllText(filePath);
        var config = JsonSerializer.Deserialize<CommandLineConfig>(jsonString);
        if (config == null) {
            throw new InvalidOperationException("Deserialization returned null.");
        }
        return config;
    }
}
```

- `CommandLineController.cs`: contiene il codice per costruire la linea di comando

```csharp
// CommandLineController.cs
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;

public class CommandLineController {

    public static RootCommand BuildCommandLine(CommandLineConfig config) {
        var rootCommand = new RootCommand("My CLI application");

        foreach (var commandConfig in config.Commands.Values) {
            var command = new Command(commandConfig.Command, commandConfig.Description);

            foreach (var optionConfig in commandConfig.Options) {
                var option = CreateOption(optionConfig);
                command.AddOption(option);
            }


            command.SetHandler((context) => {

                string inputFilePath = context.ParseResult.GetValueForOption<string>("--input");
                string outputFilePath = context.ParseResult.GetValueForOption<string>("--output");

                if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(output)) {
                    Console.WriteLine("Errore: sia --input che --output devono essere specificati.");
                    return;
                }
                // logica del comando
                // ...
                Console.WriteLine($"File copiato da '{input}' a '{output}'.");

            }, command.GetOption("--input"), command.GetOption("--output"));

            // aggiunge il comando alla radice
            rootCommand.AddCommand(command);
        }

        return rootCommand;
    }

    private static Option CreateOption(OptionConfig optionConfig) {

        Option option;

        // disriminiamo la cardinalità solo per le stringhe, mentre in caso di booleani la cardinalità è sempre 0 o 1
        if (optionConfig.Type == "boolean") {
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
```

- `Program.cs`: contiene il codice per costruire la linea di comando

```csharp
// Program.cs
using System;
using System.CommandLine;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public class Program {

    // il metodo Main è asincrono, restituisce un Task<int> e accetta un array di stringhe
    public static async Task<int> Main(string[] args) {

        // carica il file di configurazione
        var config = Repository.LoadConfig("config.json");
        // costruisce la linea di comando
        var rootCommand = CommandLineController.BuildCommandLine(config);

        // esegue la linea di comando
        return await rootCommand.InvokeAsync(args);
    }
}
```