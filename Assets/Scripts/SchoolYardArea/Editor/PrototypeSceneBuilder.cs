using System.IO;
using System.Collections.Generic;
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
        private const string GeneratedArtPath = "Assets/Art/Generated";

        [MenuItem("SchoolYardArena/Create Prototype Scene")]
        public static void CreatePrototypeScene()
        {
            Directory.CreateDirectory("Assets/Scenes");
            Directory.CreateDirectory(GeneratedArtPath);
            var floorSprite = CreateSpriteAsset("arena_floor", new Color(0.67f, 0.49f, 0.31f), SpriteShape.Square);
            var playerSprite = CreateSpriteAsset("player_marker", new Color(0.2f, 0.75f, 0.48f), SpriteShape.Circle);
            var botSprite = CreateSpriteAsset("bot_marker", new Color(0.9f, 0.2f, 0.18f), SpriteShape.Circle);
            var lineSprite = CreateSpriteAsset("court_line", new Color(0.95f, 0.88f, 0.63f), SpriteShape.Square);
            var bodySprite = CreateSpriteAsset("body_marker", Color.white, SpriteShape.Circle);
            var squareSprite = CreateSpriteAsset("ui_square", Color.white, SpriteShape.Square);

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            Physics2D.gravity = Vector2.zero;

            var camera = new GameObject("Main Camera");
            camera.tag = "MainCamera";
            var cameraComponent = camera.AddComponent<Camera>();
            cameraComponent.orthographic = true;
            cameraComponent.orthographicSize = 6.2f;
            cameraComponent.backgroundColor = new Color(0.18f, 0.31f, 0.34f);
            camera.transform.position = new Vector3(0f, 0f, -10f);

            CreateArenaFloor(floorSprite);
            CreateCourtLines(lineSprite);
            CreateBounds();

            var playerSpawn = new Vector3(0f, -2.8f, 0f);
            var player = CreateFighter("Helena", playerSpawn, 0.9f, playerSprite, bodySprite, squareSprite, true);

            var botSpawns = new List<Vector3>
            {
                new(-4.7f, 3f, 0f),
                new(0f, 1.6f, 0f),
                new(4.7f, 3f, 0f)
            };
            var botHealth = new List<Health>();
            var botTransforms = new List<Transform>();
            var botNames = new[] { "Owen Bot", "Zoe Bot", "Dylan Bot" };

            for (var i = 0; i < botSpawns.Count; i++)
            {
                var bot = CreateFighter(botNames[i], botSpawns[i], 0.84f, botSprite, bodySprite, squareSprite, false);
                var chase = bot.GetComponent<BotChaseController>();
                chase.SetTarget(player.transform);
                chase.Configure(2.8f + i * 0.25f, 0.66f);
                botHealth.Add(bot.GetComponent<Health>());
                botTransforms.Add(bot.transform);
            }

            var match = new GameObject("Arena Match Controller").AddComponent<ArenaMatchController>();
            match.Configure(
                player.GetComponent<Health>(),
                player.GetComponent<VirtualMoveInput>(),
                player.GetComponent<PlayerAbilityController>(),
                player.transform,
                playerSpawn,
                botHealth,
                botTransforms,
                botSpawns);

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

        private static void CreateCourtLines(Sprite sprite)
        {
            CreateLine("Center Line", new Vector2(0f, 0f), new Vector2(12f, 0.08f), sprite);
            CreateLine("Left Lane", new Vector2(-3.7f, 0f), new Vector2(0.08f, 7.8f), sprite);
            CreateLine("Right Lane", new Vector2(3.7f, 0f), new Vector2(0.08f, 7.8f), sprite);
            CreateLine("Top Boundary Paint", new Vector2(0f, 3.85f), new Vector2(12f, 0.08f), sprite);
            CreateLine("Bottom Boundary Paint", new Vector2(0f, -3.85f), new Vector2(12f, 0.08f), sprite);
        }

        private static void CreateLine(string name, Vector2 position, Vector2 scale, Sprite sprite)
        {
            var line = new GameObject(name);
            line.transform.position = new Vector3(position.x, position.y, -0.1f);
            line.transform.localScale = new Vector3(scale.x, scale.y, 1f);
            var renderer = line.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = -5;
        }

        private static GameObject CreateFighter(
            string name,
            Vector2 position,
            float scale,
            Sprite markerSprite,
            Sprite bodySprite,
            Sprite squareSprite,
            bool playerControlled)
        {
            var fighter = new GameObject(name);
            fighter.transform.position = new Vector3(position.x, position.y, 0f);
            fighter.transform.localScale = Vector3.one * scale;

            var body = fighter.AddComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.freezeRotation = true;
            body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            fighter.AddComponent<CircleCollider2D>().radius = 0.48f;

            var health = fighter.AddComponent<Health>();
            health.Configure(playerControlled ? 120f : 150f);

            if (playerControlled)
            {
                fighter.AddComponent<VirtualMoveInput>();
                fighter.AddComponent<PlayerController2D>();
                fighter.AddComponent<PlayerAbilityController>();
            }
            else
            {
                fighter.AddComponent<BotChaseController>();
                fighter.AddComponent<BotContactDamage>();
            }

            CreateAvatar(name, fighter.transform, markerSprite, bodySprite, squareSprite, playerControlled);
            CreateHealthBar(fighter.transform, health, squareSprite, playerControlled);
            CreateNameLabel(name, fighter.transform);
            fighter.AddComponent<DamageFlash>();
            return fighter;
        }

        private static void CreateAvatar(
            string name,
            Transform parent,
            Sprite markerSprite,
            Sprite bodySprite,
            Sprite squareSprite,
            bool playerControlled)
        {
            var shadow = CreateSpriteChild("Shadow", parent, bodySprite, new Vector3(0f, -0.1f, 0f), new Vector3(1.05f, 0.42f, 1f), new Color(0f, 0f, 0f, 0.22f), 8);
            shadow.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

            CreateSpriteChild("Leg Left", parent, squareSprite, new Vector3(-0.18f, -0.34f, 0f), new Vector3(0.16f, 0.34f, 1f), new Color(0.1f, 0.16f, 0.34f), 10);
            CreateSpriteChild("Leg Right", parent, squareSprite, new Vector3(0.18f, -0.34f, 0f), new Vector3(0.16f, 0.34f, 1f), new Color(0.1f, 0.16f, 0.34f), 10);

            var shirt = playerControlled ? new Color(0.1f, 0.62f, 0.86f) : new Color(0.9f, 0.18f, 0.16f);
            CreateSpriteChild("Shirt", parent, markerSprite, new Vector3(0f, -0.02f, 0f), new Vector3(0.78f, 0.68f, 1f), shirt, 12);

            var backpack = playerControlled ? new Color(0.96f, 0.76f, 0.22f) : new Color(0.22f, 0.22f, 0.25f);
            CreateSpriteChild("Backpack", parent, squareSprite, new Vector3(-0.46f, 0.02f, 0f), new Vector3(0.18f, 0.42f, 1f), backpack, 11);

            CreateSpriteChild("Head", parent, bodySprite, new Vector3(0f, 0.42f, 0f), new Vector3(0.42f, 0.42f, 1f), new Color(0.95f, 0.69f, 0.48f), 14);
            var hair = playerControlled ? new Color(0.35f, 0.18f, 0.08f) : new Color(0.08f, 0.08f, 0.08f);
            CreateSpriteChild("Hair", parent, bodySprite, new Vector3(0f, 0.56f, 0f), new Vector3(0.4f, 0.16f, 1f), hair, 15);

            if (playerControlled)
            {
                var arrowRoot = new GameObject("Punch Aim Indicator");
                arrowRoot.transform.SetParent(parent);
                arrowRoot.transform.localPosition = Vector3.zero;
                arrowRoot.transform.localScale = Vector3.one;

                CreateSpriteChild("Aim Shaft", arrowRoot.transform, squareSprite, new Vector3(0.28f, 0f, 0f), new Vector3(0.5f, 0.08f, 1f), new Color(1f, 0.95f, 0.35f, 0.72f), 5);
                CreateSpriteChild("Aim Tip", arrowRoot.transform, bodySprite, new Vector3(0.58f, 0f, 0f), new Vector3(0.18f, 0.18f, 1f), new Color(1f, 0.95f, 0.35f, 0.72f), 5);
                var aim = parent.gameObject.AddComponent<AimIndicator>();
                var serialized = new SerializedObject(aim);
                serialized.FindProperty("indicator").objectReferenceValue = arrowRoot.transform;
                serialized.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        private static SpriteRenderer CreateSpriteChild(
            string name,
            Transform parent,
            Sprite sprite,
            Vector3 localPosition,
            Vector3 localScale,
            Color color,
            int sortingOrder)
        {
            var child = new GameObject(name);
            child.transform.SetParent(parent);
            child.transform.localPosition = localPosition;
            child.transform.localScale = localScale;
            var renderer = child.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.color = color;
            renderer.sortingOrder = sortingOrder;
            return renderer;
        }

        private static void CreateHealthBar(Transform parent, Health health, Sprite squareSprite, bool playerControlled)
        {
            var bar = new GameObject("Health Bar");
            bar.transform.SetParent(parent);
            bar.transform.localPosition = new Vector3(0f, 1.08f, 0f);
            bar.transform.localScale = new Vector3(1.1f, 0.12f, 1f);

            CreateSpriteChild("Health Background", bar.transform, squareSprite, Vector3.zero, Vector3.one, new Color(0.05f, 0.04f, 0.04f, 0.86f), 35);
            var fill = CreateSpriteChild("Health Fill", bar.transform, squareSprite, Vector3.zero, new Vector3(0.94f, 0.62f, 1f), playerControlled ? new Color(0.18f, 0.95f, 0.42f) : new Color(1f, 0.28f, 0.2f), 36);

            var healthBar = bar.AddComponent<HealthBar2D>();
            var serialized = new SerializedObject(healthBar);
            serialized.FindProperty("health").objectReferenceValue = health;
            serialized.FindProperty("fill").objectReferenceValue = fill.transform;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void CreateNameLabel(string label, Transform parent)
        {
            var labelObject = new GameObject($"{label} Label");
            labelObject.transform.SetParent(parent);
            labelObject.transform.localPosition = new Vector3(0f, 1.28f, 0f);
            labelObject.transform.localScale = Vector3.one * 0.1f;

            var text = labelObject.AddComponent<TextMesh>();
            text.text = label.Replace(" Bot", "");
            text.anchor = TextAnchor.MiddleCenter;
            text.alignment = TextAlignment.Center;
            text.fontSize = 32;
            text.color = Color.white;
            text.GetComponent<MeshRenderer>().sortingOrder = 30;
        }

        private static void CreateBounds()
        {
            CreateWall("Top Wall", new Vector2(0f, 4.22f), new Vector2(12.5f, 0.4f));
            CreateWall("Bottom Wall", new Vector2(0f, -4.22f), new Vector2(12.5f, 0.4f));
            CreateWall("Left Wall", new Vector2(-6.22f, 0f), new Vector2(0.4f, 8.4f));
            CreateWall("Right Wall", new Vector2(6.22f, 0f), new Vector2(0.4f, 8.4f));
        }

        private static void CreateWall(string name, Vector2 position, Vector2 size)
        {
            var wall = new GameObject(name);
            wall.transform.position = position;
            var collider = wall.AddComponent<BoxCollider2D>();
            collider.size = size;
        }

        private static Sprite CreateSpriteAsset(string name, Color color, SpriteShape shape)
        {
            var path = $"{GeneratedArtPath}/{name}.png";
            var texture = new Texture2D(32, 32, TextureFormat.RGBA32, false);
            var pixels = new Color[32 * 32];
            for (var y = 0; y < 32; y++)
            {
                for (var x = 0; x < 32; x++)
                {
                    var index = y * 32 + x;
                    if (shape == SpriteShape.Circle)
                    {
                        var dx = x - 15.5f;
                        var dy = y - 15.5f;
                        var distance = Mathf.Sqrt(dx * dx + dy * dy);
                        pixels[index] = distance <= 14.5f ? color : Color.clear;
                    }
                    else
                    {
                        pixels[index] = color;
                    }
                }
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

        private enum SpriteShape
        {
            Square,
            Circle
        }
    }
}
