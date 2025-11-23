namespace LoggerService
{
    class ConsoleLoggerService : ILoggerService
    {
        public void LogInfo(string message)
        {
            Console.WriteLine(message);
        }
    }
}
