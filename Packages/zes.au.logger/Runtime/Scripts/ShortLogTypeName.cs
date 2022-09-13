using UnityEngine;

namespace Au.Logs
{
    internal static class ShortLogTypeName
    {
        public static string GetName(LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                    return "ERROR";
                case LogType.Assert:
                    return "ASSERT";
                case LogType.Warning:
                    return "WARN";
                case LogType.Log:
                    return "INFO";
                case LogType.Exception:
                    return "EXCEPTION";
                default:
                    return type.ToString();
            }
        }
    }
}
