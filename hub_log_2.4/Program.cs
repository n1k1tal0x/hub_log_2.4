using Serilog;

namespace hub_log_2._4
{
    internal class Program
    {
        private static ILogger Logger = new LoggerConfiguration().WriteTo.Console().WriteTo.File($"logs/log-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}").CreateLogger();
        private static IEnumerable<string> taskList;
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Task manager!");
                Logger.Information("App start");
                taskList = new List<string>();
                Logger.Information("Init seccuess");
                while (true)
                {
                    Console.WriteLine(@"Enter action:
    1 - Add task
    2 - Remove task
    3 - Show task list");
                    var input = Console.ReadLine();
                    Logger.Information($"User input: {input}");
                    switch (input.ToLower().Trim())
                    {
                        case "1":
                            break;
                        case "2":
                            break;
                        case "3":
                            break;
                    }
                }
            } catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
                Logger.Fatal(ex.StackTrace);
            }
            Logger.Information("App closed");
            Log.CloseAndFlush();
        }
    }
}
