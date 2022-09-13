using Au;
using Au.Logs;
using NUnit.Framework;
using System.IO;
using UnityEngine;
using UnityEngine.TestTools;

public class LoggerTest : IPrebuildSetup, IPostBuildCleanup
{
    private string testBuff = new string('a', 1023);

    [Test]
    public void WriteLog()
    {
        var logger = Au.Logger.GetLogger("Test");
        logger.Info(Application.persistentDataPath);
        logger.Info("Test info");
        logger.Warn("Test Warn");
        logger.Error("Test error");
        LogAssert.Expect(LogType.Error, "[Test] Test error");
    }

    [Test]
    public void TestLoop()
    {
        var logger = Au.Logger.GetLogger("Loop");
        logger.Info("1" + testBuff);
        Assert.IsTrue(File.Exists(Path.Combine(Application.persistentDataPath, "logs", "test.log.1")));
        logger.Info("2" + testBuff);
        Assert.IsTrue(File.Exists(Path.Combine(Application.persistentDataPath, "logs", "test.log.2")));

        string l2 = File.ReadAllText(Path.Combine(Application.persistentDataPath, "logs", "test.log.2"));
        Assert.IsTrue(l2.Contains("1aa"), "log.2 should start with 1");
        string l1 = File.ReadAllText(Path.Combine(Application.persistentDataPath, "logs", "test.log.1"));
        Assert.IsTrue(l1.Contains("2aa"), "log.1 should start with 2");
    }

    public void Setup()
    {
        var dir = Path.Combine(Application.persistentDataPath, "logs");
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        var files = Directory.GetFiles(dir);
        foreach (var item in files)
        {
            File.Delete(item);
        }
        Au.Logger.AddFile("test.log", 1024, 3);
    }

    public void Cleanup()
    {
        Au.Logger.Stop();
    }
}

