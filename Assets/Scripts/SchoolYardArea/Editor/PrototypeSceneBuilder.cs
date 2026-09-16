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
        private const string ArenaArtPath = "Assets/Art/Arenas/schoolyard_arena.png";

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
            var appleSprite = CreateSpriteAsset("apple_pickup", new Color(0.9f, 0.05f, 0.04f), SpriteShape.Circle);
            var pulseSprite = CreateSpriteAsset("pulse_ring", Color.white, SpriteShape.Ring);
            var lightSprite = CreateSpriteAsset("soft_light", Color.white, SpriteShape.SoftCircle);

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            Physics2D.gravity = Vector2.zero;

            var camera = new GameObject("Main Camera");
            camera.tag = "MainCamera";
            var cameraComponent = camera.AddComponent<Camera>();
            cameraComponent.orthographic = true;
            cameraComponent.orthographicSize = 7.1f;
            cameraComponent.clearFlags = CameraClearFlags.SolidColor;
            cameraComponent.backgroundColor = new Color(0.23f, 0.36f, 0.39f);
            camera.transform.position = new Vector3(0f, 0f, -10f);

            CreateBackdrop(squareSprite);
            var importedArena = LoadSpriteAsset(ArenaArtPath, 128f);
            if (importedArena != null)
            {
                CreateArenaFloor(importedArena);
                CreateArenaLighting(squareSprite, lightSprite);
            }
            else
            {
                CreateArenaFloor(floorSprite);
                CreateCourtLines(lineSprite);
                CreateSchoolyardProps(squareSprite, bodySprite);
            }
            CreateBounds();
            var pickups = CreatePickups(appleSprite, squareSprite);

            var playerSpawn = new Vector3(0f, -4.15f, 0f);
            var player = CreateFighter("Helena", playerSpawn, 0.9f, playerSprite, bodySprite, squareSprite, pulseSprite, true);

            var botSpawns = new List<Vector3>
            {
                new(0f, 4.1f, 0f)
            };
            var botHealth = new List<Health>();
            var botTransforms = new List<Transform>();
            var botNames = new[] { "Owen Bot" };

            for (var i = 0; i < botSpawns.Count; i++)
            {
                var bot = CreateFighter(botNames[i], botSpawns[i], 0.84f, botSprite, bodySprite, squareSprite, pulseSprite, false);
                var chase = bot.GetComponent<BotChaseController>();
                chase.SetTarget(player.transform);
                chase.Configure(2.65f, 0.66f);
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
                botSpawns,
                pickups);

            new GameObject("Game Audio").AddComponent<GameAudio>();

            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateArenaFloor(Sprite sprite)
        {
            var floor = new GameObject("Schoolyard Arena Floor");
            floor.name = "Schoolyard Arena Floor";
            floor.transform.localScale = new Vector3(18.6f, 11.4f, 1f);
            floor.transform.position = Vector3.zero;
            var renderer = floor.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = -10;
        }

        private static void CreateArenaLighting(Sprite squareSprite, Sprite lightSprite)
        {
            CreateSpriteObject("Arena Wash", squareSprite, new Vector3(0f, 0f, 0f), new Vector3(18.6f, 11.4f, 1f), new Color(0.96f, 0.9f, 0.72f, 0.2f), -8);
            CreateSpriteObject("Upper Left Light", lightSprite, new Vector3(-4.7f, 3.2f, 0f), new Vector3(9.5f, 6.6f, 1f), new Color(1f, 0.95f, 0.72f, 0.22f), -7);
            CreateSpriteObject("Lower Right Shadow", lightSprite, new Vector3(5.8f, -3.3f, 0f), new Vector3(8.2f, 5.2f, 1f), new Color(0.06f, 0.08f, 0.1f, 0.16f), -7);
            CreateSpriteObject("Button Area Calm", squareSprite, new Vector3(7.3f, -4.65f, 0f), new Vector3(4.2f, 1.6f, 1f), new Color(0.08f, 0.1f, 0.1f, 0.12f), -6);
        }

        private static void CreateCourtLines(Sprite sprite)
        {
            CreateLine("Center Line", new Vector2(0f, 0f), new Vector2(18.6f, 0.08f), sprite);
            CreateLine("Left Lane", new Vector2(-5.8f, 0f), new Vector2(0.08f, 11.1f), sprite);
            CreateLine("Right Lane", new Vector2(5.8f, 0f), new Vector2(0.08f, 11.1f), sprite);
            CreateLine("Top Boundary Paint", new Vector2(0f, 5.55f), new Vector2(18.6f, 0.08f), sprite);
            CreateLine("Bottom Boundary Paint", new Vector2(0f, -5.55f), new Vector2(18.6f, 0.08f), sprite);
            CreateLine("Hopscotch Top", new Vector2(-7.6f, 2.2f), new Vector2(1.35f, 0.08f), sprite);
            CreateLine("Hopscotch Bottom", new Vector2(-7.6f, 1.25f), new Vector2(1.35f, 0.08f), sprite);
            CreateLine("Hopscotch Left", new Vector2(-8.25f, 1.72f), new Vector2(0.08f, 0.95f), sprite);
            CreateLine("Hopscotch Right", new Vector2(-6.95f, 1.72f), new Vector2(0.08f, 0.95f), sprite);
            CreateLine("Free Throw Arc", new Vector2(7.25f, -2.4f), new Vector2(2.3f, 0.08f), sprite);
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
            Sprite pulseSprite,
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
                var abilities = fighter.AddComponent<PlayerAbilityController>();
                var serialized = new SerializedObject(abilities);
                serialized.FindProperty("pulseSprite").objectReferenceValue = pulseSprite;
                serialized.ApplyModifiedPropertiesWithoutUndo();
            }
            else
            {
                fighter.AddComponent<BotChaseController>();
                fighter.AddComponent<BotContactDamage>();
            }

            var visualRoot = new GameObject("Visual Root");
            visualRoot.transform.SetParent(fighter.transform);
            visualRoot.transform.localPosition = Vector3.zero;
            visualRoot.transform.localScale = Vector3.one;

            CreateAvatar(name, visualRoot.transform, markerSprite, bodySprite, squareSprite, playerControlled);
            CreateHealthBar(fighter.transform, health, squareSprite, playerControlled);
            CreateNameLabel(name, fighter.transform);
            var motionJuice = fighter.AddComponent<CharacterMotionJuice>();
            var motionSerialized = new SerializedObject(motionJuice);
            motionSerialized.FindProperty("visualRoot").objectReferenceValue = visualRoot.transform;
            motionSerialized.ApplyModifiedPropertiesWithoutUndo();
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
            CreateWall("Top Wall", new Vector2(0f, 5.95f), new Vector2(19.2f, 0.4f));
            CreateWall("Bottom Wall", new Vector2(0f, -5.95f), new Vector2(19.2f, 0.4f));
            CreateWall("Left Wall", new Vector2(-9.55f, 0f), new Vector2(0.4f, 11.8f));
            CreateWall("Right Wall", new Vector2(9.55f, 0f), new Vector2(0.4f, 11.8f));
        }

        private static void CreateWall(string name, Vector2 position, Vector2 size)
        {
            var wall = new GameObject(name);
            wall.transform.position = position;
            var collider = wall.AddComponent<BoxCollider2D>();
            collider.size = size;
        }

        private static void CreateBackdrop(Sprite squareSprite)
        {
            CreateSpriteObject("Grass Left", squareSprite, new Vector3(-10.8f, 0f, 0f), new Vector3(2.8f, 13f, 1f), new Color(0.24f, 0.39f, 0.27f), -20);
            CreateSpriteObject("Grass Right", squareSprite, new Vector3(10.8f, 0f, 0f), new Vector3(2.8f, 13f, 1f), new Color(0.24f, 0.39f, 0.27f), -20);
            CreateSpriteObject("Top Asphalt", squareSprite, new Vector3(0f, 6.6f, 0f), new Vector3(24f, 1.8f, 1f), new Color(0.38f, 0.42f, 0.42f), -21);
            CreateSpriteObject("Bottom Asphalt", squareSprite, new Vector3(0f, -6.6f, 0f), new Vector3(24f, 1.8f, 1f), new Color(0.38f, 0.42f, 0.42f), -21);
        }

        private static void CreateSchoolyardProps(Sprite squareSprite, Sprite bodySprite)
        {
            CreateSpriteObject("Bench Seat", squareSprite, new Vector3(-7.5f, -4.65f, 0f), new Vector3(2.2f, 0.18f, 1f), new Color(0.45f, 0.24f, 0.12f), -2);
            CreateSpriteObject("Bench Leg Left", squareSprite, new Vector3(-8.25f, -4.9f, 0f), new Vector3(0.14f, 0.4f, 1f), new Color(0.17f, 0.17f, 0.16f), -1);
            CreateSpriteObject("Bench Leg Right", squareSprite, new Vector3(-6.75f, -4.9f, 0f), new Vector3(0.14f, 0.4f, 1f), new Color(0.17f, 0.17f, 0.16f), -1);
            CreateSpriteObject("Basketball Hoop Pole", squareSprite, new Vector3(8.15f, 4.35f, 0f), new Vector3(0.16f, 1.5f, 1f), new Color(0.18f, 0.18f, 0.18f), -1);
            CreateSpriteObject("Basketball Backboard", squareSprite, new Vector3(8.15f, 5.1f, 0f), new Vector3(1.05f, 0.52f, 1f), new Color(0.92f, 0.94f, 0.9f), -1);
            CreateSpriteObject("Basketball Rim", bodySprite, new Vector3(8.15f, 4.8f, 0f), new Vector3(0.55f, 0.18f, 1f), new Color(0.95f, 0.28f, 0.12f), 0);
            CreateSpriteObject("Lunch Ball", bodySprite, new Vector3(6.8f, -4.55f, 0f), new Vector3(0.38f, 0.38f, 1f), new Color(0.95f, 0.48f, 0.12f), 0);
            CreateSpriteObject("Cone Left Shadow", bodySprite, new Vector3(-2.8f, 4.75f, 0f), new Vector3(0.46f, 0.2f, 1f), new Color(0f, 0f, 0f, 0.18f), -1);
            CreateSpriteObject("Cone Left", squareSprite, new Vector3(-2.8f, 4.92f, 0f), new Vector3(0.34f, 0.42f, 1f), new Color(1f, 0.48f, 0.08f), 0);
            CreateSpriteObject("Cone Right Shadow", bodySprite, new Vector3(2.8f, -4.75f, 0f), new Vector3(0.46f, 0.2f, 1f), new Color(0f, 0f, 0f, 0.18f), -1);
            CreateSpriteObject("Cone Right", squareSprite, new Vector3(2.8f, -4.92f, 0f), new Vector3(0.34f, 0.42f, 1f), new Color(1f, 0.48f, 0.08f), 0);
            CreateSpriteObject("Lunch Table Top", squareSprite, new Vector3(-7.35f, 4.65f, 0f), new Vector3(1.7f, 0.55f, 1f), new Color(0.42f, 0.22f, 0.12f), -1);
            CreateSpriteObject("Lunch Table Highlight", squareSprite, new Vector3(-7.35f, 4.82f, 0f), new Vector3(1.55f, 0.08f, 1f), new Color(0.62f, 0.36f, 0.2f), 0);
            CreateSpriteObject("Tree Canopy Left", bodySprite, new Vector3(-10.75f, 4.75f, 0f), new Vector3(1.35f, 1.35f, 1f), new Color(0.16f, 0.48f, 0.22f), -5);
            CreateSpriteObject("Tree Trunk Left", squareSprite, new Vector3(-10.75f, 3.85f, 0f), new Vector3(0.26f, 0.72f, 1f), new Color(0.36f, 0.2f, 0.1f), -6);
            CreateSpriteObject("Tree Canopy Right", bodySprite, new Vector3(10.75f, -4.7f, 0f), new Vector3(1.25f, 1.25f, 1f), new Color(0.16f, 0.48f, 0.22f), -5);
            CreateSpriteObject("Tree Trunk Right", squareSprite, new Vector3(10.75f, -5.52f, 0f), new Vector3(0.24f, 0.62f, 1f), new Color(0.36f, 0.2f, 0.1f), -6);
        }

        private static List<GameObject> CreatePickups(Sprite appleSprite, Sprite squareSprite)
        {
            var pickups = new List<GameObject>();
            var positions = new[]
            {
                new Vector3(-6.8f, 3.95f, 0f),
                new Vector3(6.95f, 2.2f, 0f),
                new Vector3(-4.8f, -4.45f, 0f),
                new Vector3(5.55f, -3.85f, 0f)
            };

            foreach (var position in positions)
            {
                pickups.Add(CreateApplePickup(position, appleSprite, squareSprite));
            }

            return pickups;
        }

        private static GameObject CreateApplePickup(Vector3 position, Sprite appleSprite, Sprite squareSprite)
        {
            var pickup = new GameObject("Apple Health Pickup");
            pickup.transform.position = position;
            pickup.transform.localScale = Vector3.one * 0.72f;
            CreateSpriteChild("Apple Shadow", pickup.transform, appleSprite, new Vector3(0.04f, -0.08f, 0f), new Vector3(0.64f, 0.28f, 1f), new Color(0f, 0f, 0f, 0.2f), 3);
            var appleVisual = new GameObject("Apple Visual");
            appleVisual.transform.SetParent(pickup.transform);
            appleVisual.transform.localPosition = Vector3.zero;
            appleVisual.transform.localScale = Vector3.one;
            CreateSpriteChild("Apple Glow", appleVisual.transform, appleSprite, Vector3.zero, new Vector3(0.78f, 0.78f, 1f), new Color(1f, 0.25f, 0.12f, 0.22f), 3);
            CreateSpriteChild("Apple Body", appleVisual.transform, appleSprite, Vector3.zero, new Vector3(0.5f, 0.5f, 1f), new Color(0.92f, 0.08f, 0.05f), 4);
            CreateSpriteChild("Apple Leaf", appleVisual.transform, squareSprite, new Vector3(0.15f, 0.28f, 0f), new Vector3(0.22f, 0.1f, 1f), new Color(0.18f, 0.7f, 0.24f), 5);
            appleVisual.AddComponent<BobAndGlow>();
            var collider = pickup.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
            collider.radius = 0.42f;
            pickup.AddComponent<HealthPickup>();
            return pickup;
        }

        private static SpriteRenderer CreateSpriteObject(
            string name,
            Sprite sprite,
            Vector3 position,
            Vector3 scale,
            Color color,
            int sortingOrder)
        {
            var gameObject = new GameObject(name);
            gameObject.transform.position = position;
            gameObject.transform.localScale = scale;
            var renderer = gameObject.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.color = color;
            renderer.sortingOrder = sortingOrder;
            return renderer;
        }

        private static Sprite CreateSpriteAsset(string name, Color color, SpriteShape shape)
        {
            var path = $"{GeneratedArtPath}/{name}.png";
            const int size = 64;
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var pixels = new Color[size * size];
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var index = y * size + x;
                    var dx = x - (size - 1f) * 0.5f;
                    var dy = y - (size - 1f) * 0.5f;
                    var distance01 = Mathf.Sqrt(dx * dx + dy * dy) / (size * 0.5f);

                    if (shape == SpriteShape.Circle)
                    {
                        var lit = Color.Lerp(color * 0.78f, color * 1.18f, Mathf.Clamp01((y + x) / (float)(size * 2)));
                        pixels[index] = distance01 <= 0.9f ? lit : Color.clear;
                    }
                    else if (shape == SpriteShape.Ring)
                    {
                        var alpha = distance01 is > 0.62f and < 0.9f ? 0.7f : 0f;
                        pixels[index] = new Color(color.r, color.g, color.b, alpha);
                    }
                    else if (shape == SpriteShape.SoftCircle)
                    {
                        var alpha = Mathf.Clamp01(1f - distance01);
                        alpha *= alpha;
                        pixels[index] = new Color(color.r, color.g, color.b, alpha);
                    }
                    else
                    {
                        var noise = Mathf.PerlinNoise((x + name.Length * 17) * 0.21f, (y + name.Length * 11) * 0.21f);
                        var shade = Mathf.Lerp(0.9f, 1.08f, noise);
                        pixels[index] = new Color(color.r * shade, color.g * shade, color.b * shade, color.a);
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
                importer.spritePixelsPerUnit = 64f;
                importer.mipmapEnabled = false;
                importer.SaveAndReimport();
            }

            return AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }

        private static Sprite LoadSpriteAsset(string path, float pixelsPerUnit)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            AssetDatabase.ImportAsset(path);
            if (AssetImporter.GetAtPath(path) is TextureImporter importer)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spritePixelsPerUnit = pixelsPerUnit;
                importer.mipmapEnabled = false;
                importer.filterMode = FilterMode.Bilinear;
                importer.SaveAndReimport();
            }

            return AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }

        private enum SpriteShape
        {
            Square,
            Circle,
            Ring,
            SoftCircle
        }
    }
}
