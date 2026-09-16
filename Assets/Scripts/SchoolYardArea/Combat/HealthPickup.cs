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
            gameObject.SetActive(false);
        }
    }
}
