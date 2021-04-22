namespace lab3_2
{
    public interface ILogger
    {
        void Log(string message);
        void LogError(string errorMessage);
    }
}