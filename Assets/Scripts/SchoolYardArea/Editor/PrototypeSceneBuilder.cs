using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SchoolYardArea.Editor
{
    public static class PrototypeSceneBuilder
    {
        private const string ScenePath = "Assets/Scenes/ArenaPrototype.unity";
        private const string GeneratedArtPath = "Assets/Art/Generated";

        [MenuItem("SchoolYardArena/Create Prototype Scene")]
        public static void CreatePrototypeScene()
        {
            Directory.CreateDirectory("Assets/Scenes");
            Directory.CreateDirectory(GeneratedArtPath);
            var floorSprite = CreateSpriteAsset("arena_floor", new Color(0.74f, 0.62f, 0.45f));
            var playerSprite = CreateSpriteAsset("player_marker", new Color(0.23f, 0.7f, 0.45f));
            var botSprite = CreateSpriteAsset("bot_marker", new Color(0.9f, 0.25f, 0.2f));

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var camera = new GameObject("Main Camera");
            camera.tag = "MainCamera";
            var cameraComponent = camera.AddComponent<Camera>();
            cameraComponent.orthographic = true;
            cameraComponent.orthographicSize = 7f;
            cameraComponent.backgroundColor = new Color(0.16f, 0.32f, 0.34f);
            camera.transform.position = new Vector3(0f, 0f, -10f);

            CreateArenaFloor(floorSprite);
            CreateMarker("Player Helena Prototype", Vector2.zero, 0.8f, playerSprite);
            CreateMarker("Lunchyard Bot", new Vector2(3.5f, 2.5f), 0.7f, botSprite);
            CreateMarker("Lunchyard Bot", new Vector2(-3.5f, 2.5f), 0.7f, botSprite);
            CreateMarker("Lunchyard Bot", new Vector2(0f, -3.2f), 0.7f, botSprite);

            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateArenaFloor(Sprite sprite)
        {
            var floor = new GameObject("Schoolyard Arena Floor");
            floor.name = "Schoolyard Arena Floor";
            floor.transform.localScale = new Vector3(12f, 8f, 1f);
            floor.transform.position = Vector3.zero;
            var renderer = floor.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = -10;
        }

        private static void CreateMarker(string name, Vector2 position, float scale, Sprite sprite)
        {
            var marker = new GameObject(name);
            marker.transform.position = new Vector3(position.x, position.y, 0f);
            marker.transform.localScale = Vector3.one * scale;
            var renderer = marker.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = 10;
        }

        private static Sprite CreateSpriteAsset(string name, Color color)
        {
            var path = $"{GeneratedArtPath}/{name}.png";
            var existing = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (existing != null)
            {
                return existing;
            }

            var texture = new Texture2D(32, 32, TextureFormat.RGBA32, false);
            var pixels = new Color[32 * 32];
            for (var i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            texture.SetPixels(pixels);
            texture.Apply();
            File.WriteAllBytes(path, texture.EncodeToPNG());
            Object.DestroyImmediate(texture);
            AssetDatabase.ImportAsset(path);

            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spritePixelsPerUnit = 32f;
                importer.mipmapEnabled = false;
                importer.SaveAndReimport();
            }

            return AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }
    }
}
