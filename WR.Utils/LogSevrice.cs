using System;
using System.Text;
using System.Threading;

using log4net;
using log4net.Config;
using log4net.Core;
using log4net.Repository;
using log4net.Repository.Hierarchy;

namespace WR.Utils
{
    public class LogService
    {
        /// <summary>
        /// 初始化系统日志器
        /// </summary>
        public static void InitializeService(string filename)
        {
            XmlConfigurator.Configure(new Uri(filename));
        }

        /// <summary>
        /// 完全打印异常信息
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetErrorMessage(Exception ex)
        {
            StringBuilder stb = new StringBuilder();
            if (ex != null)
            {
                stb.AppendLine("------");
                stb.AppendLine(ex.Message);
                stb.AppendLine(ex.Source);
                stb.AppendLine(ex.HelpLink);
                ex = ex.InnerException;
            }
            return stb.ToString();
        }


        /// <summary>
        /// 客户端其他代码自己访问这个函数 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static LoggerEx Getlog(Type t)
        {
            return new LoggerEx(log4net.LogManager.GetLogger(t));
        }
    }

    public class LoggerEx
    {
        private readonly log4net.ILog log;
        /// <summary>
        /// 5种级别，依次为DEBUG、INFO、WARN、ERROR和FATAL，当输出时，
        /// 只有级别高过配置中规定的级别的信息才能真正的输出
        /// </summary>
        /// <param name="log"></param>
        public LoggerEx(log4net.ILog log)
        {
            this.log = log;
        }

        public bool IsDebugEnabled { get { return this.log.IsDebugEnabled; } }
        public bool IsErrorEnabled { get { return this.log.IsErrorEnabled; } }
        public bool IsFatalEnabled { get { return this.log.IsFatalEnabled; } }
        public bool IsInfoEnabled { get { return this.log.IsInfoEnabled; } }
        public bool IsWarnEnabled { get { return this.log.IsWarnEnabled; } }

        public void Debug(object message)
        {
            this.log.Debug(message);
        }

        public void Debug(string p, Exception exception)
        {
            this.log.Debug(p, exception);
        }

        public void Error(object message)
        {
            this.log.Error(message);
        }

        public void Error(string p, Exception exception)
        {
            this.log.Error(p, exception);
        }

        public void Info(object message)
        {
            this.log.Info(message);
        }

        public void Info(string p, Exception exception)
        {
            this.log.Info(p, exception);
        }

        public void Fatal(object message)
        {
            this.log.Fatal(message);
        }

        public void Fatal(string p, Exception exception)
        {
            this.log.Fatal(p, exception);
        }

        public void Warn(object message)
        {
            this.log.Warn(message);
        }

        public void Warn(string p, Exception exception)
        {
            this.log.Warn(p, exception);
        }
    }

    public class LoggerWatch
    {
        private static readonly log4net.ILog log =
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool logWatching = true;
        private log4net.Appender.MemoryAppender append;
        private Thread logWatcher;
        private int sleep = 1000;
        public delegate void delegateShowLog(LoggerEvent[] events);
        private delegateShowLog view;
        private string libname;

        private log4net.Core.Level mylevel;

        public LoggerWatch(string libname, Level level, delegateShowLog view, int sleep)
        {
            log.Debug("开始日志 观察............");
            this.libname = libname;
            this.sleep = sleep;
            this.view = view;
            ILoggerRepository[] al = LogManager.GetAllRepositories();

            append = new log4net.Appender.MemoryAppender();
            append.Threshold = getLevel(level);

            Hierarchy h = LogManager.GetRepository() as Hierarchy;
            Logger loog = h.GetLogger(libname) as log4net.Repository.Hierarchy.Logger;

            loog.AddAppender(append);
            logWatcher = new Thread(new ThreadStart(LogWatcher));
            logWatcher.Start();
        }

        public void SetLevel(Level level)
        {
            this.mylevel = getLevel(level);
            append.Threshold = mylevel;
        }

        public void CloseLog()
        {
            this.logWatching = false;   
            lock (append)
            {
                append.Clear();
                append.Threshold = log4net.Core.Level.Off;
            }
        }

        private object moke = new object();

        /// <summary>
        /// 取日志
        /// </summary>
        private void LogWatcher()
        {
            LoggerEvent[] ksevents = null;
            LoggingEvent[] events = null;

            while (logWatching)
            {
                lock (append)
                {
                    events = append.GetEvents();
                    append.Clear();
                }

                if (events != null && events.Length > 0)
                {
                    ksevents = new LoggerEvent[events.Length];
                    for (int i = 0; i < events.Length; i++)
                    {
                        ksevents[i] = new LoggerEvent();
                        ksevents[i].Level = events[i].Level.Name;
                        ksevents[i].messageObj = events[i].MessageObject;
                        ksevents[i].TimeStamp = events[i].TimeStamp;
                    }

                    if (view != null)
                    {
                        this.view(ksevents);
                    }
                }
                events = null;
                Thread.Sleep(this.sleep);
            }
        }

        public static log4net.Core.Level getLevel(Level level)
        {
            switch (level)
            {
                case Level.Debug:
                    return log4net.Core.Level.Debug;
                case Level.Info:
                    return log4net.Core.Level.Info;
                case Level.Error:
                    return log4net.Core.Level.Error;
                case Level.Warn:
                    return log4net.Core.Level.Warn;
                case Level.Fatel:
                    return log4net.Core.Level.Fatal;
                default:
                    return log4net.Core.Level.Debug;
            }
        }
    }

    public class LoggerEvent
    {
        public DateTime TimeStamp;

        public object messageObj;

        public string Level;

    }

    /// <summary>
    /// 日志级别
    /// </summary>
    public enum Level
    {
        All = 0,
        Debug = 1,
        Info = 2,
        Warn = 3,
        Error = 4,
        Fatel = 5
    }
}
