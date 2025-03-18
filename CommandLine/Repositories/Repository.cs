// Repository.cs
namespace CommandLine.Repositories;


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