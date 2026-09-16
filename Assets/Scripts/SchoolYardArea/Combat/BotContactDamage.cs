using SchoolYardArea.Runtime;
using UnityEngine;

namespace SchoolYardArea.Combat
{
    public sealed class BotContactDamage : MonoBehaviour
    {
        [SerializeField] private float damage = 10f;
        [SerializeField] private float cooldown = 0.8f;

        private float nextHitAt;

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (Time.time < nextHitAt)
            {
                return;
            }

            if (collision.collider.GetComponentInParent<PlayerController2D>() == null)
            {
                return;
            }

            var health = collision.collider.GetComponentInParent<Health>();
            if (health == null)
            {
                return;
            }

            nextHitAt = Time.time + cooldown;
            health.TakeDamage(damage);
        }
    }
}
