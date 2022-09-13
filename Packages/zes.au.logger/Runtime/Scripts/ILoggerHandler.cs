using UnityEngine;

namespace Au.Logs
{
    public interface ILoggerHandler
    {
        void Start();
        void Stop();
        void Log(LogType type, string message);
    }
}
