public class Program {

    public static void Main (string[] args) {

        try {
            var commandBuilder = new CommandBuilder();
            var rootCommand = commandBuilder.GetRootCommand();
            Console.WriteLine(rootCommand.ToHelpString());
            commandBuilder.SaveRootConfig();
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }
}