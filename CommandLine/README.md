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

# [Guida](https://youtu.be/MOweq3EttPU)

Questa libreria include diversi accorgimenti già implementati, come la gestione dei comandi errati, il comando di `--help` e la `--version`. Posso inserire la versione del programma richiamata dal comando di default `--version` dentro il csproj

```xml
<PropertyGroup>
    <!-- ... -->
    <Version>0.0.1</Version>
</PropertyGroup>
```

Step:

## 1. La prima cosa da fare è creare il `rootCommand` che rappresenta il comando principale del programma. Questo comando non ha nome e non ha azioni associate. Il `rootCommand` è il padre di tutti i comandi.

La tipologia di `Main` che useremo sarà `async Task Main<int> (string[] args)` che prevede un ritorno di un Task. Questo ci permette di utilizzare il comando `await` per aspettare il completamento di un'operazione asincrona.

La struttura base da cui partiremo sarà queste:

```csharp
namespace CommandLine;

public class Program {

    public static async Task<int> Main(string[] args) {

        // Creo il comando principale a cui passo un testo di descrizione
        var rootCommand = new RootCommand(description: "Il mio programma da linea di comando") {

        };

        // Setto una azione (Handler) che il comando andrà a fare, passando come argomento del metodo
        // un metodo definito altrove
        rootCommand.SetHandler(OnHandle);

        // Dichiaro un builder del commandLine e gli passo come argomento il rootCommand
        var commandLineBulder = new CommandLineBuilder(rootCommand)
                                .UseDefaults();
        
        // Dichiaro un parse a cui passare la build del bulder
        var parser = commandLineBulder.Build();

        // Restituisco un invocazione asincrona         
        return await parser.InvokeAsync(args).ConfigureAwait(false);
    }

    public static void OnHandle() {

    }
}
```

Quando lancerò il comando di default `--help` mi restituirà la descrizione del programma e i comandi disponibili.

```bash
.\CommandLine.exe --help   
```

mi restituirà:

```bash

Description:
  Il mio programma da linea di comando

Usage:
  CommandLine [options]

Options:
  --version       Show version information
  -?, -h, --help  Show help and usage information
```

Le options sono i comandi che posso dare (come --help) i quali possono avare delle azioni collegate.

## 2. Creo una opzione per passare un `<int>` mediante l'opzione `--port` da uisare poi nella funzione `OnHandle`:

```csharp
// L'opzione sarà di tipo int
var portOption = new Option<int>(
    aliases: ["--port", "-p"],                      // alias che può avere l'opzione
    description: "Il nome della porta"              // descrizione dell'opzione
);
```

Questa poi va aggiunta nel `rootCommand`:

```csharp
var rootCommand = new RootCommand(description: "Il mio programma da linea di comando") {
    portOption
};
```

Posso anche aggiungere una option con `rootCommand.AddOption(portOption);`.

La funzione `OnHandle` dovrà avere come argomento un `int`:

```csharp
public static void OnHandle(int port) {
    Console.WriteLine($"Porta: {port}");
}
```

e il SetHandler dovrà passare come argomento il metodo `OnHandle`, il quale accetterà un argomento port, passatogli dal comando `--port`. Per fare questo si crea una arrow function che passa l'argomento `port` al metodo `OnHandle`:

```csharp
rootCommand.SetHandler((port) => OnHandle(port), portOption);
// possiamo anche scriverlo in maniera più compatta
// rootCommand.SetHandler(OnHandle, portOption);
```

## 3. Aggiungere un sub commando al comando di root, come ad esempio `test`:

L'aggiunta di un comando la si fa al `rootCommand`, mediante il metodo `Add`. Prima di fare questo si deve dichiarare un `Command` che rappresenta il sub comando, al quale dare un nome e una descrizione. A questo comando posso assegnargli anche un metodo `OnTest` che verrà eseguito quando il sub comando `test` verrà chiamato.

```csharp
// Dichiaro il comando a cui do nome e descrizione
var testCommand = new Command(
    name: "test",
    description: "comando di test dell'applicazione"
);
testCommand.SetHandler(OnTest);
// aggiuingo al comando root
rootCommand.Add(testCommand);
```

Il metodo `OnTest` dovrà avere la stessa firma del metodo `OnHandle`:

```csharp
public static void OnTest() {

    // definiamo una logica semplice al metodo
    Console.WriteLine($"Handle test");

}
```

Ora l'help si è arricchito di un nuovo comando:

```bash
.\CommandLine.exe --help
Description:
  Il mio programma da linea di comando

Usage:
  CommandLine [command] [options]

Options:
  -p, --port <port>  Il nome della porta
  --version          Show version information
  -?, -h, --help     Show help and usage information

Commands:
  test  comando di test dell'applicazione
```

Se lancio il --help con il comando `test` avrà una descrizione di cosa può fare il comando test:

```bash
.\CommandLine.exe test --help
Description:
  comando di test dell'applicazione

Usage:
  CommandLine test [options]

Options:
  -?, -h, --help  Show help and usage information
```

Il comando test è ora implementabile con una opzione e un handler che sfruttino questa opzione, esattamente come abbiamo fatto con il rootComando con l'opzione  `--port`:

```csharp
// definisco le sue opzioni
var nameOption = new Option<string>(
    aliases: ["--name", "-n"],
    description: "comando di inserimento del nome utente"
);
// Dichiaro il comando a cui do nome e descrizione
var testCommand = new Command(
    name: "test",
    description: "comando di test dell'applicazione"
) {
    // inserisco qui l'opzione che mi serve
    nameOption,
};
testCommand.SetHandler(OnTest, nameOption);
// aggiuingo al comando root
rootCommand.Add(testCommand);
```

Il metodo `OnTest` dovrà avere la stessa firma del metodo `OnHandle`:

```csharp
public static void OnTest(string name) {

    // definiamo una logica semplice al metodo
    Console.WriteLine($"Handle test: Hello {name}");

}
```

Le options possono avere un solo argomento. Per fare un comando che accetta più argomenti è possibile creare invece della `Option` un `Argument`, che accetta più argomenti.

```csharp
var nameArguments = new Argument<string[]>(
    name: "Names",
    description: "Nomi da aggiungere"
);
```

Il comando `test` sarò implemtato con un `Argument`:

```csharp
testCommand.AddArgument(nameArguments);
testCommand.SetHandler(OnTest, nameArguments);
```

E l'Handler dovrà avere come argomento un array di stringhe:

```csharp
public static void OnTest(string[] name) {

    // definiamo una logica semplice al metodo
    Console.WriteLine($"Handle test: Hello {String.Join(", ", name)}");

}
```

Per salvare la configurazione dei comandi creati in Json posso serializzare il comando root in un file Json:

```csharp
var json = JsonSerializer.Serialize(rootCommand, new JsonSerializerOptions { WriteIndented = true });
File.WriteAllText("command.json", json);
```