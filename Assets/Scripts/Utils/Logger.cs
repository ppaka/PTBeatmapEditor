// System
using System;
using System.IO;
using System.Diagnostics;

// Unity
using UnityEngine;

// Project
// Alias

public class Logger
{
    // public readonly variables
    private static readonly string FilePath = Application.persistentDataPath + "/log.txt";
    private static readonly string TimeFormat = "yyyy/MM/dd HH:mm:ss";

    // private cached variables
    private static string currentTime = string.Empty;
    private static string callerName = string.Empty;
    private static string logContentMerged = string.Empty;

    public static void Log(object content)
    {
        currentTime = GetCurrentTimeFormatted();
        callerName = GetCallerName();
        logContentMerged = $"[{currentTime}] {callerName}\n                      Message : {content}";

        UnityEngine.Debug.Log(logContentMerged);
        SaveToFile(logContentMerged);
    }

    // TODO : Set delete

    private static void SaveToFile(string content)
    {
        using (StreamWriter writer = new StreamWriter(FilePath, true))
        {
            writer.WriteLine(logContentMerged);
        }
    }

    private static string GetCallerName()
    {
        StackTrace stackTrace = new StackTrace(true);
        StackFrame stackFrame = stackTrace.GetFrame(2);
        string className = stackFrame.GetMethod().DeclaringType.Name;
        string methodName = stackFrame.GetMethod().Name;
        int lineNumber = stackFrame.GetFileLineNumber();

        return $"{className}.{methodName} (line:{lineNumber})";
    }

    private static string GetCurrentTimeFormatted()
    {
        return DateTime.Now.ToString(TimeFormat);
    }
}