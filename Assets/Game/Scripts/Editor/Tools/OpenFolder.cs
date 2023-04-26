using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace Game.Editor.Tools
{
    public static class OpenFolder
    {
        [MenuItem("Tools/Open Folder/Data Path", false, 10)]
        public static void OpenFolderDataPath()
            => Execute(Application.dataPath);

        [MenuItem("Tools/Open Folder/Persistent Data Path", false, 11)]
        public static void OpenFolderPersistentDataPath()
            => Execute(Application.persistentDataPath);

        [MenuItem("Tools/Open Folder/Streaming Assets Path", false, 12)]
        public static void OpenFolderStreamingAssetsPath()
            => Execute(Application.streamingAssetsPath);

        [MenuItem("Tools/Open Folder/Temporary Cache Path", false, 13)]
        public static void OpenFolderTemporaryCachePath()
            => Execute(Application.temporaryCachePath);

        private static void Execute(string folder)
        {
            folder = $"\"{folder}\"";

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    Process.Start("Explorer.exe", folder.Replace('/', '\\'));
                    break;

                case RuntimePlatform.OSXEditor:
                    Process.Start("open", folder);
                    break;

                default:
                    throw new Exception($"Not support open folder on '{Application.platform}' platform.");
            }
        }
    }
}