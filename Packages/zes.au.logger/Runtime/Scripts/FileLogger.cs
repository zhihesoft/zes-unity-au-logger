using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Au.Logs
{
    public class FileLogger : ILoggerHandler
    {
        public FileLogger(string name, int maxSize, int maxCount)
        {
            this.name = name;
            this.maxSize = maxSize;
            this.maxCount = maxCount;
        }

        const string logsDir = "logs";
        readonly string name;
        readonly int maxSize;
        readonly int maxCount;
        StreamWriter writer;

        public void Log(LogType type, string message)
        {
            writer.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] [{type}] {message}");
            if (writer.BaseStream.Length > maxSize)
            {
                Loop();
            }
        }


        public void Start()
        {
            var dir = Path.Combine(Application.persistentDataPath, logsDir);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            GetWriter();
        }

        public void Stop()
        {
            writer?.Close();
        }

        TextWriter GetWriter()
        {
            if (writer != null)
            {
                return writer;
            }

            writer = new StreamWriter(GetLogFilename(0), true, Encoding.UTF8);
            return writer;
        }

        void Loop()
        {
            writer.Close();
            writer = null;

            string filename = GetLogFilename(maxCount - 1);
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            for (int i = maxCount - 2; i >= 0; i--)
            {
                string nextfilename = GetLogFilename(i);
                if (File.Exists(nextfilename))
                {
                    File.Move(nextfilename, GetLogFilename(i + 1));
                }
            }

            GetWriter();
        }

        string GetLogFilename(int index)
        {
            return Path.Combine(Application.persistentDataPath, logsDir, name + (index > 0 ? ("." + index.ToString()) : ""));
        }
    }
}
