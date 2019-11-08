using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System;

#if UNITY_EDITOR
class BatchModePlayer
{
    static string sPrjPath;
    static bool[] IsPlayerRunning = new bool[] { false, false, false, false, false };

    static void RunPlayer()
    {
        var editorWindows = Resources.FindObjectsOfTypeAll<EditorWindow>();
        foreach (var editorWindow in editorWindows)
        {
            var editorWindowName = editorWindow.ToString();
            if (!editorWindowName.Contains("GameView"))
                editorWindow.Close();
        }

        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var go = new GameObject("LoadMain");
        go.AddComponent<LoadMainProxy>();

        EditorApplication.isPlaying = true;
    }

    [MenuItem("DotsNetKit/Run Pseudo Player/#1")]
    static void RunPlayer1()
    {
        CreateHardLinkPrj(1);
#if UNITY_EDITOR_WIN
        DoRunPlayer(1);
#endif
    }
    [MenuItem("DotsNetKit/Run Pseudo Player/#1", true)]
    static bool RunPlayer1Valid()
    {
        return !IsPlayerRunning[1];
    }

    [MenuItem("DotsNetKit/Run Pseudo Player/#2")]
    static void RunPlayer2()
    {
        CreateHardLinkPrj(2);
#if UNITY_EDITOR_WIN
        DoRunPlayer(2);
#endif
    }
    [MenuItem("DotsNetKit/Run Pseudo Player/#2", true)]
    static bool RunPlayer2Valid()
    {
        return !IsPlayerRunning[2];
    }

    [MenuItem("DotsNetKit/Run Pseudo Player/#3")]
    static void RunPlayer3()
    {
        CreateHardLinkPrj(3);
#if UNITY_EDITOR_WIN
        DoRunPlayer(3);
#endif
    }
    [MenuItem("DotsNetKit/Run Pseudo Player/#3", true)]
    static bool RunPlayer3Valid()
    {
        return !IsPlayerRunning[3];
    }

    [MenuItem("DotsNetKit/Run Pseudo Player/#4")]
    static void RunPlayer4()
    {
        CreateHardLinkPrj(4);
#if UNITY_EDITOR_WIN
        DoRunPlayer(4);
#endif
    }
    [MenuItem("DotsNetKit/Run Pseudo Player/#4", true)]
    static bool RunPlayer4Valid()
    {
        return !IsPlayerRunning[4];
    }

    // The range of i is [1, 4]
    static void DoRunPlayer(int i)
    {
        string playerPrjRoot = Path.Combine(sPrjPath, "DotsNetKitPseudoPlayers");
        string playerPrjPath = Path.Combine(playerPrjRoot, string.Format("PseudoPlayer{0}", i));

        string unityArgs = string.Format("-disable-assembly-updater -projectPath {0} -executeMethod BatchModePlayer.RunPlayer", playerPrjPath);

        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
        startInfo.FileName = EditorApplication.applicationPath;
        startInfo.Arguments = unityArgs;
        process.StartInfo = startInfo;
        process.EnableRaisingEvents = true;
        switch (i)
        {
            case 1:
                process.Exited += Player1Exited;
                break;
            case 2:
                process.Exited += Player2Exited;
                break;
            case 3:
                process.Exited += Player3Exited;
                break;
            case 4:
                process.Exited += Player4Exited;
                break;
        }
        process.Start();
        IsPlayerRunning[i] = true;
    }

    static void Player1Exited(object sender, System.EventArgs e)
    {
        RemoveHardLinkPrj(1);
        IsPlayerRunning[1] = false;
    }

    static void Player2Exited(object sender, System.EventArgs e)
    {
        RemoveHardLinkPrj(2);
        IsPlayerRunning[2] = false;
    }

    static void Player3Exited(object sender, System.EventArgs e)
    {
        RemoveHardLinkPrj(3);
        IsPlayerRunning[3] = false;
    }
    static void Player4Exited(object sender, System.EventArgs e)
    {
        RemoveHardLinkPrj(4);
        IsPlayerRunning[4] = false;
    }

    // [MenuItem("DotsNetKit/Run Clients/CreateLink")]
    static void CreateHardLinkPrj(int i)
    {
        sPrjPath = Directory.GetParent(Application.dataPath).FullName;
        string playerPrjRoot = Path.Combine(sPrjPath, "DotsNetKitPseudoPlayers");
        string playerPrjName = string.Format("PseudoPlayer{0}", i);
        string playerPrjPath = Path.Combine(playerPrjRoot, playerPrjName);

#if UNITY_EDITOR_WIN
        CreateHardLinedPrjWin(playerPrjPath);
#endif
    }

    static void RemoveHardLinkPrj(int i)
    {
#if UNITY_EDITOR_WIN
        RemoveHardLinkPrjWin(i);
#endif
    }

#if UNITY_EDITOR_WIN
    static void RemoveHardLinkPrjWin(int i)
    {
        string playerPrjRoot = Path.Combine(sPrjPath, "DotsNetKitPseudoPlayers");
        string playerPrjName = string.Format("PseudoPlayer{0}", i);
        string playerPrjPath = Path.Combine(playerPrjRoot, playerPrjName);

        string linkToAssets = Path.Combine(playerPrjPath, "Assets");
        string linkToLibrary = Path.Combine(playerPrjPath, "Library");
        string linkToPackages = Path.Combine(playerPrjPath, "Packages");
        string linkToProjectSettings = Path.Combine(playerPrjPath, "ProjectSettings");
        string linkToAutoBuild = Path.Combine(playerPrjPath, "AutoBuild");

        ExecuteCommand("rmdir " + linkToAssets);
        ExecuteCommand("rmdir " + linkToLibrary);
        ExecuteCommand("rmdir " + linkToPackages);
        ExecuteCommand("rmdir " + linkToProjectSettings);
        ExecuteCommand("rmdir " + linkToAutoBuild);
        ExecuteCommand("rmdir /S /Q" + playerPrjPath);
    }

    static void CreateHardLinedPrjWin(string playerPrjPath)
    {
        Directory.CreateDirectory(playerPrjPath);
        // remove existing links
        string linkToAssets = Path.Combine(playerPrjPath, "Assets");
        string linkToLibrary = Path.Combine(playerPrjPath, "Library");
        string linkToPackages = Path.Combine(playerPrjPath, "Packages");
        string linkToProjectSettings = Path.Combine(playerPrjPath, "ProjectSettings");
        string linkToAutoBuild = Path.Combine(playerPrjPath, "AutoBuild");

        ExecuteCommand("rmdir " + linkToAssets);
        ExecuteCommand("rmdir " + linkToLibrary);
        ExecuteCommand("rmdir " + linkToPackages);
        ExecuteCommand("rmdir " + linkToProjectSettings);
        ExecuteCommand("rmdir " + linkToAutoBuild);

        string targetAssets = Path.Combine(sPrjPath, "Assets");
        string targetLibrary = Path.Combine(sPrjPath, "Library");
        string targetPackages = Path.Combine(sPrjPath, "Packages");
        string targetProjectSettings = Path.Combine(sPrjPath, "ProjectSettings");
        string targetAutoBuild = Path.Combine(sPrjPath, "AutoBuild");

        ExecuteCommand(string.Format("mklink /J {0} {1}", linkToAssets, targetAssets));
        ExecuteCommand(string.Format("mklink /J {0} {1}", linkToLibrary, targetLibrary));
        ExecuteCommand(string.Format("mklink /J {0} {1}", linkToPackages, targetPackages));
        ExecuteCommand(string.Format("mklink /J {0} {1}", linkToProjectSettings, targetProjectSettings));
        ExecuteCommand(string.Format("mklink /J {0} {1}", linkToAutoBuild, targetAutoBuild));
    }

    static void ExecuteCommand(string cmd)
    {
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = string.Format("/C {0}", cmd);
        process.StartInfo = startInfo;
        process.Start();
    }
#endif
}

#endif