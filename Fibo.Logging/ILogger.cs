namespace Fibo.Logging
{
    public interface ILogger
    {
        void Log(string message, LogEventType eventType);
    }
}
