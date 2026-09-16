using System.IO;
using UnityEditor;
using UnityEditor.Android;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace SchoolYardArea.Editor
{
    public static class AndroidBuild
    {
        private const string AppIconPath = "Assets/Art/Icons/schoolyard_punch_icon.png";

        public static void BuildDebugApk()
        {
            PlayerSettings.companyName = "Tauris";
            PlayerSettings.productName = "SchoolYardArena";
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, "com.tauris.schoolyardarena");
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.Android.applicationEntry = AndroidApplicationEntry.Activity;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel35;
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            ConfigureAndroidExternalTools();
            ConfigureAppIcon();

            Directory.CreateDirectory("Builds/Android");

            var options = new BuildPlayerOptions
            {
                scenes = FindEnabledScenes(),
                locationPathName = "Builds/Android/SchoolYardArena-debug.apk",
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

        private static void ConfigureAndroidExternalTools()
        {
            AndroidExternalToolsSettings.jdkRootPath = @"C:\Dev\SchoolYardArea\tools\jdk17\jdk-17.0.20+8";
            AndroidExternalToolsSettings.sdkRootPath = @"C:\Users\cjlew\AppData\Local\Android\Sdk";
            AndroidExternalToolsSettings.ndkRootPath = @"C:\Users\cjlew\AppData\Local\Android\Sdk\ndk\27.2.12479018";
        }

        private static void ConfigureAppIcon()
        {
            var importer = AssetImporter.GetAtPath(AppIconPath) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.GUI;
                importer.alphaIsTransparency = true;
                importer.mipmapEnabled = false;
                importer.SaveAndReimport();
            }

            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(AppIconPath);
            if (icon == null)
            {
                throw new FileNotFoundException($"App icon not found at {AppIconPath}");
            }

            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Android, new[] { icon });

            foreach (var kind in PlayerSettings.GetSupportedIconKinds(NamedBuildTarget.Android))
            {
                var icons = PlayerSettings.GetPlatformIcons(BuildTargetGroup.Android, kind);
                for (var i = 0; i < icons.Length; i++)
                {
                    var layers = new Texture2D[icons[i].maxLayerCount];
                    for (var layer = 0; layer < layers.Length; layer++)
                    {
                        layers[layer] = icon;
                    }

                    icons[i].SetTextures(layers);
                }

                PlayerSettings.SetPlatformIcons(BuildTargetGroup.Android, kind, icons);
            }
        }
    }
}
