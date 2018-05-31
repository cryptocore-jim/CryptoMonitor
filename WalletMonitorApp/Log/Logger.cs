using log4net;
using System;
using System.Globalization;

namespace WalletMonitorApp.Log
{    
    public class Logger
    {
        private ILog log = null;

        static Logger() 
        {            
            log4net.Config.XmlConfigurator.Configure();
            log4net.GlobalContext.Properties["host"] = Environment.MachineName;
        }

        public Logger(Type logClass)
        {
            log = LogManager.GetLogger(logClass);
        }

        public void LogException(Exception exception)
        {
            if (log.IsErrorEnabled)
                log.Error(string.Format(CultureInfo.InvariantCulture, "{0}", exception.Message), exception);
        }
        public void LogError(string message)
        {
            if (log.IsErrorEnabled)
                log.Error(string.Format(CultureInfo.InvariantCulture, "{0}", message));
        }
        public void LogWarningMessage(string message)
        {
            if (log.IsWarnEnabled)
                log.Warn(string.Format(CultureInfo.InvariantCulture, "{0}", message));
        }
        public void LogInfoMessage(string message)
        {
            if (log.IsInfoEnabled)
                log.Info(string.Format(CultureInfo.InvariantCulture, "{0}", message));
        }
    }
}
