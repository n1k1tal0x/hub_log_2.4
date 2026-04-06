using Serilog;

namespace hub_log_2._4
{
    internal class Program
    {
        private static ILogger Logger = new LoggerConfiguration().WriteTo.File($"logs/log_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}").CreateLogger();
        private static ICollection<string> taskList;
        private static bool _appCicle = true;
        private static void AddTaskEvent()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Enter task text:");
                    string input = Console.ReadLine();
                    Logger.Information($"[AddTaskEvent] User input: {input}");
                    if (!String.IsNullOrEmpty(input))
                    {
                        taskList.Add(input);
                        Logger.Information($"[AddTaskEvent] Task was added");
                        return;
                    }
                    Console.WriteLine("Bad input");
                    Logger.Information($"[AddTaskEvent] Bad input");
                }
            } catch (Exception ex)
            {
                Logger.Error(ex.Message);
                Logger.Error(ex.StackTrace);
            }
        }
        private static void RemoveTaskEvent()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Enter task index:");
                    string input = Console.ReadLine();
                    Logger.Information($"[RemoveTaskEvent] User input: {input}");
                    int index;
                    if (int.TryParse(input, out index))
                    {
                        var target = taskList.ElementAtOrDefault(index);
                        if (target != null)
                        {
                            taskList.Remove(target);
                            Logger.Information($"[RemoveTaskEvent] Task was removed");
                            return;
                        }
                    }
                    Console.WriteLine("Bad input");
                    Logger.Information($"[RemoveTaskEvent] Bad input");
                }
            } catch (Exception ex)
            {
                Logger.Error(ex.Message);
                Logger.Error(ex.StackTrace);
            }
        }
        private static void GetTasksEvent()
        {
            Console.WriteLine("Task list (index, text):");
            Logger.Information("Task list:");
            int index = 0;
            foreach (var task in taskList)
            {
                Console.WriteLine($"\t{index} - {task}");
                Logger.Information($"\t{index} - {task}");
                index++;
            }
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
                            Logger.Information("Call AddTaskEvent");
                            AddTaskEvent();
                            break;
                        case "2":
                            Logger.Information("Call RemoveTaskEvent");
                            RemoveTaskEvent();
                            break;
                        case "3":
                            Logger.Information("Call GetTasksEvent");
                            GetTasksEvent();
                            break;
                        case "4":
                            Logger.Information("Call v");
                            _appCicle = false;
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
