using System.IO;
using UnityEditor;

namespace SchoolYardArea.Editor
{
    public static class AndroidBuild
    {
        public static void BuildDebugApk()
        {
            PlayerSettings.companyName = "Tauris";
            PlayerSettings.productName = "SchoolYardArea";
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, "com.tauris.schoolyardarea");
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

            Directory.CreateDirectory("Builds/Android");

            var options = new BuildPlayerOptions
            {
                scenes = FindEnabledScenes(),
                locationPathName = "Builds/Android/SchoolYardArea-debug.apk",
                target = BuildTarget.Android,
                targetGroup = BuildTargetGroup.Android,
                options = BuildOptions.Development | BuildOptions.AllowDebugging
            };

            var report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                throw new BuildFailedException($"Android build failed: {report.summary.result}");
            }
        }

        private static string[] FindEnabledScenes()
        {
            var scenes = EditorBuildSettings.scenes;
            var enabledScenes = new System.Collections.Generic.List<string>();
            foreach (var scene in scenes)
            {
                if (scene.enabled)
                {
                    enabledScenes.Add(scene.path);
                }
            }

            return enabledScenes.ToArray();
        }
    }
}
