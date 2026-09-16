using System.IO;
using SchoolYardArea.AI;
using SchoolYardArea.Combat;
using SchoolYardArea.Input;
using SchoolYardArea.Runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SchoolYardArea.Editor
{
    public static class PrototypeSceneBuilder
    {
        private const string ScenePath = "Assets/Scenes/ArenaPrototype.unity";

        [MenuItem("SchoolYardArea/Create Prototype Scene")]
        public static void CreatePrototypeScene()
        {
            Directory.CreateDirectory("Assets/Scenes");

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var camera = new GameObject("Main Camera");
            camera.tag = "MainCamera";
            var cameraComponent = camera.AddComponent<Camera>();
            cameraComponent.orthographic = true;
            cameraComponent.orthographicSize = 7f;
            cameraComponent.backgroundColor = new Color(0.16f, 0.32f, 0.34f);
            camera.transform.position = new Vector3(0f, 0f, -10f);

            CreateArenaFloor();
            var player = CreatePlayer();
            CreateBot(new Vector2(3.5f, 2.5f), player.transform);
            CreateBot(new Vector2(-3.5f, 2.5f), player.transform);
            CreateBot(new Vector2(0f, -3.2f), player.transform);

            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateArenaFloor()
        {
            var floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "Schoolyard Arena Floor";
            floor.transform.localScale = new Vector3(12f, 8f, 0.2f);
            floor.transform.position = Vector3.zero;
            var renderer = floor.GetComponent<Renderer>();
            renderer.sharedMaterial = CreateMaterial("ArenaFloor_Mat", new Color(0.74f, 0.62f, 0.45f));
        }

        private static GameObject CreatePlayer()
        {
            var player = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            player.name = "Player Helena Prototype";
            player.transform.position = Vector3.zero;
            player.transform.localScale = Vector3.one * 0.8f;
            player.GetComponent<Renderer>().sharedMaterial = CreateMaterial("Player_Mat", new Color(0.23f, 0.7f, 0.45f));
            Object.DestroyImmediate(player.GetComponent<Collider>());

            var body = player.AddComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
            player.AddComponent<CircleCollider2D>();
            player.AddComponent<Health>();
            player.AddComponent<VirtualMoveInput>();
            player.AddComponent<PlayerController2D>();

            return player;
        }

        private static void CreateBot(Vector2 position, Transform target)
        {
            var bot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bot.name = "Lunchyard Bot";
            bot.transform.position = new Vector3(position.x, position.y, 0f);
            bot.transform.localScale = Vector3.one * 0.7f;
            bot.GetComponent<Renderer>().sharedMaterial = CreateMaterial("Bot_Mat", new Color(0.9f, 0.25f, 0.2f));
            Object.DestroyImmediate(bot.GetComponent<Collider>());

            var body = bot.AddComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
            bot.AddComponent<CircleCollider2D>();
            bot.AddComponent<Health>();
            var chase = bot.AddComponent<BotChaseController>();
            chase.SetTarget(target);
        }

        private static Material CreateMaterial(string name, Color color)
        {
            var path = $"Assets/{name}.mat";
            var existing = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (existing != null)
            {
                return existing;
            }

            var material = new Material(Shader.Find("Sprites/Default"))
            {
                color = color
            };
            AssetDatabase.CreateAsset(material, path);
            return material;
        }
    }
}
