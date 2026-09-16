using System.Collections.Generic;
using SchoolYardArea.Characters;
using SchoolYardArea.Combat;
using SchoolYardArea.Input;
using SchoolYardArea.Progression;
using UnityEngine;

namespace SchoolYardArea.Runtime
{
    public sealed class ArenaMatchController : MonoBehaviour
    {
        [SerializeField] private Health playerHealth;
        [SerializeField] private VirtualMoveInput playerInput;
        [SerializeField] private PlayerAbilityController abilities;
        [SerializeField] private List<Health> botHealth = new();
        [SerializeField] private List<Transform> botTransforms = new();
        [SerializeField] private List<Vector3> botSpawnPoints = new();
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Vector3 playerSpawnPoint;

        private readonly WinProgression progression = new();
        private bool roundEnded;
        private float resetAt;
        private GUIStyle titleStyle;
        private GUIStyle hudStyle;
        private GUIStyle smallStyle;

        public void Configure(
            Health player,
            VirtualMoveInput input,
            PlayerAbilityController playerAbilities,
            Transform playerObject,
            Vector3 playerSpawn,
            List<Health> bots,
            List<Transform> botObjects,
            List<Vector3> botSpawns)
        {
            playerHealth = player;
            playerInput = input;
            abilities = playerAbilities;
            playerTransform = playerObject;
            playerSpawnPoint = playerSpawn;
            botHealth = bots;
            botTransforms = botObjects;
            botSpawnPoints = botSpawns;
        }

        private void Start()
        {
            foreach (var bot in botHealth)
            {
                bot.KnockedOut += OnBotKnockedOut;
            }

            playerHealth.KnockedOut += OnPlayerKnockedOut;
        }

        private void Update()
        {
            if (roundEnded && Time.time >= resetAt)
            {
                ResetRound();
            }
        }

        private void OnGUI()
        {
            EnsureStyles();

            var scale = Mathf.Max(1f, Screen.height / 720f);
            GUI.Label(new Rect(24f, 18f, 520f, 44f), "SchoolYardArena", titleStyle);
            GUI.Label(new Rect(26f, 66f, 520f, 34f), $"Helena  HP {Mathf.CeilToInt(playerHealth.Current)}/{Mathf.CeilToInt(playerHealth.Max)}   Wins {progression.TotalWins}", hudStyle);
            GUI.Label(new Rect(26f, 104f, 620f, 32f), NextUnlockText(), smallStyle);
            GUI.Label(new Rect(26f, 136f, 840f, 32f), "Duel Owen. Get close, then tap Punch or Special.", smallStyle);

            var buttonSize = 112f * scale;
            var gap = 18f * scale;
            var y = Screen.height - buttonSize - 32f * scale;
            var attackRect = new Rect(Screen.width - buttonSize * 2f - gap - 32f * scale, y, buttonSize, buttonSize);
            var specialRect = new Rect(Screen.width - buttonSize - 28f * scale, y, buttonSize, buttonSize);

            GUI.enabled = abilities == null || abilities.PrimaryReady01 >= 1f;
            if (GUI.Button(attackRect, "Punch"))
            {
                playerInput.PressAttack();
            }

            GUI.enabled = abilities == null || abilities.SpecialReady01 >= 1f;
            if (GUI.Button(specialRect, "Special"))
            {
                playerInput.PressSpecial();
            }

            GUI.enabled = true;

            if (roundEnded)
            {
                var message = playerHealth.IsKnockedOut ? "Try again!" : "Win! New classmates unlock with wins.";
                GUI.Label(new Rect(Screen.width * 0.5f - 260f, Screen.height * 0.5f - 42f, 520f, 84f), message, titleStyle);
            }
        }

        private void OnBotKnockedOut(Health bot)
        {
            bot.gameObject.SetActive(false);

            foreach (var otherBot in botHealth)
            {
                if (!otherBot.IsKnockedOut)
                {
                    return;
                }
            }

            progression.AddWin();
            EndRound();
        }

        private void OnPlayerKnockedOut(Health _)
        {
            EndRound();
        }

        private void EndRound()
        {
            roundEnded = true;
            resetAt = Time.time + 2.25f;
        }

        private void ResetRound()
        {
            roundEnded = false;
            playerTransform.position = playerSpawnPoint;
            playerHealth.Configure(playerHealth.Max);

            for (var i = 0; i < botHealth.Count; i++)
            {
                var bot = botHealth[i];
                bot.gameObject.SetActive(true);
                bot.Configure(bot.Max);

                if (i < botTransforms.Count && i < botSpawnPoints.Count)
                {
                    botTransforms[i].position = botSpawnPoints[i];
                }
            }
        }

        private string NextUnlockText()
        {
            foreach (var entry in RosterDefaults.Entries)
            {
                if (!progression.IsUnlocked(entry.UnlockWins))
                {
                    return $"Next unlock: {entry.Name} at {entry.UnlockWins} wins";
                }
            }

            return "Full roster unlocked";
        }

        private void EnsureStyles()
        {
            if (titleStyle != null)
            {
                return;
            }

            titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 30,
                fontStyle = FontStyle.Bold,
                normal = { textColor = Color.white },
                alignment = TextAnchor.MiddleCenter
            };
            hudStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 22,
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(0.98f, 0.95f, 0.82f) }
            };
            smallStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 18,
                normal = { textColor = new Color(0.9f, 0.96f, 1f) }
            };
        }
    }
}
