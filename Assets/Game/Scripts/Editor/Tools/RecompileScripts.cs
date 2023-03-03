using UnityEditor;
using UnityEditor.Compilation;

namespace Game.Editor.Tools
{
    public static class RecompileScripts
    {
        private const string MENU_PATH = "Tools/Editor/Force Recompile";

        [MenuItem(MENU_PATH, true)]
        public static bool Validate()
            => !EditorApplication.isPlaying;

        [MenuItem(MENU_PATH, false)]
        public static void OpenFolderPersistentDataPath()
            => CompilationPipeline.RequestScriptCompilation();
    }
}