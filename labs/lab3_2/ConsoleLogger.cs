using System;

namespace lab3_2
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void LogError(string errorMessage)
        {
            Console.Error.WriteLine(errorMessage);
        }
    }
}