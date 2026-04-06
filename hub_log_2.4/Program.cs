using Serilog;

namespace hub_log_2._4
{
    internal class Program
    {
        private static ILogger Logger = new LoggerConfiguration().WriteTo.Console().WriteTo.File($"logs/log-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}").CreateLogger();
        private static IEnumerable<string> taskList;
        private static bool _appCicle = true;
        private static void AddTaskEvent()
        {

        }
        private static void RemoveTaskEvent()
        {

        }
        private static void GetTasksEvent()
        {

        }
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Task manager!");
                Logger.Information("App start");
                taskList = new List<string>();
                Logger.Information("Init seccuess");
                while (_appCicle)
                {
                    Console.WriteLine(@"Enter action:
    1 - Add task
    2 - Remove task
    3 - Show task list
    4 - Close app");
                    var input = Console.ReadLine();
                    Logger.Information($"User input: {input}");
                    switch (input.ToLower().Trim())
                    {
                        case "1":
                            AddTaskEvent();
                            break;
                        case "2":
                            break;
                        case "3":
                            break;
                        case "4":
                            _appCicle = true;
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
