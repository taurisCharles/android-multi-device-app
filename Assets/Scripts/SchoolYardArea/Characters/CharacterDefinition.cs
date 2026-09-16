using UnityEngine;

namespace SchoolYardArea.Characters
{
    [CreateAssetMenu(menuName = "SchoolYardArea/Character Definition")]
    public sealed class CharacterDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string characterName = "Helena";
        public CharacterRole role = CharacterRole.Balanced;
        public int unlockWins;
        public Sprite portrait;
        public Color accentColor = Color.white;

        [Header("Stats")]
        public float maxHealth = 100f;
        public float moveSpeed = 5f;
        public float primaryDamage = 12f;
        public float primaryCooldown = 0.45f;
        public float specialCooldown = 8f;

        [Header("Ability Labels")]
        public string primaryName = "Straight Shot";
        public string specialName = "Power Combo";
        [TextArea]
        public string specialDescription = "A simple starter special.";
    }
}
