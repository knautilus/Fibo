using log4net;
using log4net.Config;

namespace Fibo.Logging
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _log;

        public Log4NetLogger()
        {
            _log = LogManager.GetLogger("FiboLogger");
            XmlConfigurator.Configure();
        }

        void ILogger.Log(string message, LogEventType eventType)
        {
            switch(eventType)
            {
                case LogEventType.Error:
                    _log.Error(message);
                    break;
                case LogEventType.Warn:
                    _log.Warn(message);
                    break;
                case LogEventType.Info:
                    _log.Info(message);
                    break;
            }
        }
    }
}
