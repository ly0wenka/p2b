using UnityEditor;
using static UnityEditor.BuildPipeline;

namespace Editor
{
    public static class Builder
    {
        [MenuItem("Build/Android/APK")]
        public static void BuildAndroidAPK()
        {
            EditorUserBuildSettings.buildAppBundle = false;
            BuildPlayer(
                new BuildPlayerOptions
                {
                    target = BuildTarget.Android,
                    locationPathName = @"S:\repos\Unity.Builds\Match3\NotMatch3.apk",
                    scenes = new[] { @"Assets\Scenes\SampleScene.unity" }
                });
        }

        [MenuItem("Build/Android/Aab")]
        public static void BuildAndroidAab()
        {
            EditorUserBuildSettings.buildAppBundle = true;
            BuildPlayer(
                new BuildPlayerOptions
                {
                    target = BuildTarget.Android,
                    locationPathName = @"S:\repos\Unity.Builds\Match3\NotMatch3.aab",
                    scenes = new[] { @"Assets\Scenes\SampleScene.unity" }
                });
        }
    }
}
