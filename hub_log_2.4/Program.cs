using System.Diagnostics;

namespace hub_log_2._4
{
    internal static class Tracer
    {
        public static readonly TraceSource TaskManagerTrace = CreateTaskManagerTrace();

        private static TraceSource CreateTaskManagerTrace()
        {
            Directory.CreateDirectory("logs");

            var traceSource = new TraceSource("TaskManagerTrace", SourceLevels.All);
            traceSource.Listeners.Clear();
            traceSource.Listeners.Add(new TextWriterTraceListener(Path.Combine("logs", $"trace_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.txt")));

            Trace.AutoFlush = true;

            return traceSource;
        }

        public static void Close()
        {
            TaskManagerTrace.Flush();
            TaskManagerTrace.Close();
        }
    }

    internal class Program
    {
        private static ICollection<string> taskList = new List<string>();
        private static bool _appCicle = true;

        private static void AddTaskEvent()
        {
            Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Start, 1000, "Добавления задачи.");

            var stopwatch = Stopwatch.StartNew();

            try
            {
                while (true)
                {
                    Console.WriteLine("Enter task text:");
                    string? input = Console.ReadLine();

                    Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Verbose, 1001, $"Ввод пользователя при добавлении задачи: {input}");

                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        taskList.Add(input);
                        stopwatch.Stop();

                        Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Information, 1002, $"Задача добавлена за {stopwatch.ElapsedMilliseconds} мс.");
                        return;
                    }

                    Console.WriteLine("Bad input");
                    Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Warning, 1003, "Попытка добавить задачу с пустым названием.");
                }
            }
            catch (Exception ex)
            {
                Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Error, 1004, $"Ошибка при добавлении задачи: {ex}");
            }
            finally
            {
                stopwatch.Stop();
                Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Stop, 1005, $"Сценарий добавления задачи завершён за {stopwatch.ElapsedMilliseconds} мс.");
            }
        }

        private static void RemoveTaskEvent()
        {
            Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Start, 2000, "Запущен сценарий удаления задачи.");

            var stopwatch = Stopwatch.StartNew();

            try
            {
                while (true)
                {
                    Console.WriteLine("Enter task index:");
                    string? input = Console.ReadLine();

                    Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Verbose, 2001, $"Ввод пользователя при удалении задачи: {input}");

                    if (int.TryParse(input, out int index))
                    {
                        var target = taskList.ElementAtOrDefault(index);
                        if (target != null)
                        {
                            taskList.Remove(target);
                            stopwatch.Stop();

                            Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Information, 2002, $"Задача с индексом {index} удалена за {stopwatch.ElapsedMilliseconds} мс.");
                            return;
                        }

                        Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Warning, 2003, $"Попытка удалить несуществующую задачу с индексом {index}.");
                    }
                    else
                    {
                        Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Warning, 2004, $"Введено нечисловое значение индекса: {input}");
                    }

                    Console.WriteLine("Bad input");
                    Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Warning, 2005, $"Некорректный ввод при удалении задачи: {input}");
                }
            }
            catch (Exception ex)
            {
                Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Error, 2006, $"Ошибка при удалении задачи: {ex}");
            }
            finally
            {
                stopwatch.Stop();
                Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Stop, 2007, $"Сценарий удаления задачи завершён за {stopwatch.ElapsedMilliseconds} мс.");
            }
        }

        private static void GetTasksEvent()
        {
            Console.WriteLine("Task list (index, text):");
            Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Start, 3000, "Запущен сценарий просмотра списка задач.");

            var stopwatch = Stopwatch.StartNew();
            int index = 0;

            foreach (var task in taskList)
            {
                Console.WriteLine($"\t{index} - {task}");
                Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Information, 3001, $"Задача [{index}]: {task}");
                index++;
            }

            stopwatch.Stop();
            Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Stop, 3002, $"Просмотр списка задач завершён за {stopwatch.ElapsedMilliseconds} мс.");
        }

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Task manager!");
                Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Start, 4000, "Приложение запущено.");

                taskList = new List<string>();
                Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Information, 4001, "Инициализация списка задач выполнена.");

                while (_appCicle)
                {
                    Console.WriteLine(@"Enter action:
    1 - Add task
    2 - Remove task
    3 - Show task list
    4 - Close app");

                    string? input = Console.ReadLine();
                    Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Verbose,4002,$"Команда пользователя: {input}");

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Warning, 4003, "Получена пустая команда в главном меню.");
                        continue;
                    }

                    switch (input.Trim())
                    {
                        case "1":
                            Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Information,4004, "Переход к сценарию добавления задачи.");
                            AddTaskEvent();
                            break;
                        case "2":
                            Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Information, 4005, "Переход к сценарию удаления задачи.");
                            RemoveTaskEvent();
                            break;
                        case "3":
                            Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Information, 4006, "Переход к сценарию просмотра задач.");
                            GetTasksEvent();
                            break;
                        case "4":
                            Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Information,4007, "Получена команда на завершение приложения.");
                            _appCicle = false;
                            break;
                        default:
                            Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Warning, 4008, $"Неизвестная команда пользователя: {input}");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Tracer.TaskManagerTrace.TraceEvent(TraceEventType.Critical,4009,$"Критическая ошибка в Main: {ex}");
            }
            finally
            {
                Tracer.TaskManagerTrace.TraceEvent(
                    TraceEventType.Stop,
                    4010,
                    "Приложение закрыто.");
                Tracer.Close();
            }
        }
    }
}
