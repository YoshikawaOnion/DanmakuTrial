using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

public class BatchBuild
{
    [MenuItem("Tools/Build Project All Scene for iOS")]
    public static void IosDevelopmentBuild()
    {
        IosBuild(true);
    }

    private static bool IosBuild(bool isDebug)
	{
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);
		BuildOptions opt = BuildOptions.SymlinkLibraries;
        if (isDebug)
        {
            opt |= BuildOptions.Development
                               | BuildOptions.ConnectWithProfiler
                               | BuildOptions.AllowDebugging;
        }

        var scenes = GetScenes();
        string errorMessage = BuildPipeline.BuildPlayer(
            scenes,
            "bin/ios",
            BuildTarget.iOS,
            opt);

        if (string.IsNullOrEmpty(errorMessage))
        {
            Debug.Log("Build for iOS succeeded.");
            return true;
        }
        else
        {
            Debug.Log("Build iOS ERROR!");
            Debug.LogError(errorMessage);
            return false;
        }
    }

    private static string[] GetScenes()
    {
        var levels = new List<string>();
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                levels.Add(scene.path);
            }
        }
        return levels.ToArray();
    }
}
