using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

class AutoBuilder
{
    [MenuItem ("File/AutoBuilder/WIN64")]
    static void PerformBuild ()
    {
        string[] scenes = { "Assets/Scenes/Main.unity" };

        string buildDirectory = "./Build/WIN64";
        string buildPath = "./Build/WIN64/PTBeatmapEditor.exe";

        // Create build folder if not yet exists
        Directory.CreateDirectory(buildDirectory);

        var report = BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.StandaloneWindows64, BuildOptions.None);

        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
}