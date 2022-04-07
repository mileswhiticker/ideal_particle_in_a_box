using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public static class Log
{
    public static string logID = "unknown NA";
    private static string LogDirPath = "Logs/SimLogs";
    public static void AddLine(string aTextLine)
    {
        string filePath = LogDirPath + "/Sim " + logID + ".txt";
        if (!Directory.Exists(LogDirPath))
        {
            UnityEngine.Debug.Log("Creating " + LogDirPath);
            Directory.CreateDirectory(LogDirPath);
        }
        WriteText(filePath, aTextLine + "\r\n");
    }
    public static void WriteText(string aFilePath, string aText)
    {
        StreamWriter writer = System.IO.File.AppendText(aFilePath);
        writer.WriteLine(aText);
        writer.Close();
    }
}
