using SchoolYardArea.Runtime;
using UnityEngine;

namespace SchoolYardArea.Combat
{
    public sealed class HealthPickup : MonoBehaviour
    {
        [SerializeField] private float healAmount = 28f;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponentInParent<PlayerController2D>() == null)
            {
                return;
            }

            var health = other.GetComponentInParent<Health>();
            if (health == null || health.IsKnockedOut)
            {
                return;
            }

            health.Heal(healAmount);
            SpawnFloatingText($"+{Mathf.CeilToInt(healAmount)}", new Color(0.35f, 1f, 0.42f), other.transform.position);
            gameObject.SetActive(false);
        }

        private static void SpawnFloatingText(string message, Color color, Vector3 position)
        {
            var textObject = new GameObject("Heal Text");
            textObject.transform.position = position + new Vector3(0f, 1.55f, 0f);
            textObject.AddComponent<FloatingText>().Configure(message, color, 62);
        }
    }
}
