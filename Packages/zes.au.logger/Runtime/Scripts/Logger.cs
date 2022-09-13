using Au.Logs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Au
{
    /// <summary>
    /// AU Logger 
    /// </summary>
    public class Logger
    {
        private static Dictionary<string, Logger> loggers = new Dictionary<string, Logger>();
        private static List<ILoggerHandler> handlers = new List<ILoggerHandler>();

        private static void AddHandler(ILoggerHandler handler)
        {
            if (handlers.Count <= 0)
            {
                Application.logMessageReceivedThreaded += Application_logMessageReceivedThreaded;
            }
            handler.Start();
            handlers.Add(handler);
        }

        public static void AddFile(string name, int maxSize, int maxCount)
        {
            var handler = new FileLogger(name, maxSize, maxCount);
            AddHandler(handler);
        }

        public static void Stop()
        {
            if (handlers.Count > 0)
            {
                Application.logMessageReceivedThreaded -= Application_logMessageReceivedThreaded;
                foreach (var handler in handlers)
                {
                    handler.Stop();
                }
                handlers.Clear();
            }
        }

        private static void Application_logMessageReceivedThreaded(string condition, string stackTrace, LogType type)
        {
            foreach (var handler in handlers)
            {
                handler.Log(type, condition);
            }
        }

        /// <summary>
        /// Get a Logger from type
        /// </summary>
        /// <typeparam name="T">Class Type</typeparam>
        /// <returns>Logger</returns>
        public static Logger GetLogger<T>()
        {
            return GetLogger(typeof(T).FullName);
        }

        /// <summary>
        /// Get a Logger from name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Logger</returns>
        public static Logger GetLogger(string name)
        {
            if (loggers.TryGetValue(name, out Logger logger))
            {
                return logger;
            }
            logger = new Logger(name);
            loggers.Add(name, logger);
            return logger;
        }

        private Logger(string name)
        {
            this.name = name;
        }

        private string name;

        public void Info(object message)
        {
            Debug.Log($"[{name}] {message}");
        }

        public void Warn(object message)
        {
            Debug.LogWarning($"[{name}] {message}");
        }

        public void Error(object message)
        {
            Debug.LogError($"[{name}] {message}");
        }
    }
}

