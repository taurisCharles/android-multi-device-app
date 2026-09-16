using SchoolYardArea.Input;
using UnityEngine;

namespace SchoolYardArea.Combat
{
    [RequireComponent(typeof(VirtualMoveInput))]
    public sealed class PlayerAbilityController : MonoBehaviour
    {
        [SerializeField] private float primaryDamage = 18f;
        [SerializeField] private float primaryRadius = 1.15f;
        [SerializeField] private float primaryCooldown = 0.75f;
        [SerializeField] private float primaryConeDegrees = 72f;
        [SerializeField] private float specialDamage = 48f;
        [SerializeField] private float specialRadius = 2.25f;
        [SerializeField] private float specialCooldown = 8.5f;
        [SerializeField] private LayerMask targetLayers = ~0;

        private readonly Collider2D[] hits = new Collider2D[16];
        private VirtualMoveInput input;
        private float nextPrimaryAt;
        private float nextSpecialAt;

        public float PrimaryReady01 => Mathf.Clamp01((Time.time - nextPrimaryAt + primaryCooldown) / primaryCooldown);
        public float SpecialReady01 => Mathf.Clamp01((Time.time - nextSpecialAt + specialCooldown) / specialCooldown);

        private void Awake()
        {
            input = GetComponent<VirtualMoveInput>();
        }

        private void Update()
        {
            if (input.AttackPressed)
            {
                TryPrimaryStrike();
            }

            if (input.SpecialPressed)
            {
                TryAreaStrike(specialDamage, specialRadius, ref nextSpecialAt, specialCooldown);
            }

            input.ConsumeButtons();
        }

        private void TryPrimaryStrike()
        {
            if (Time.time < nextPrimaryAt)
            {
                return;
            }

            nextPrimaryAt = Time.time + primaryCooldown;
            var hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, primaryRadius, hits, targetLayers);
            var aim = input.Aim.sqrMagnitude > 0.01f ? input.Aim.normalized : Vector2.up;
            var bestDot = Mathf.Cos(primaryConeDegrees * 0.5f * Mathf.Deg2Rad);
            Health bestTarget = null;
            var bestScore = -1f;

            for (var i = 0; i < hitCount; i++)
            {
                var hit = hits[i];
                if (hit == null || hit.transform == transform)
                {
                    continue;
                }

                var toTarget = (Vector2)hit.transform.position - (Vector2)transform.position;
                if (toTarget.sqrMagnitude < 0.01f)
                {
                    continue;
                }

                var dot = Vector2.Dot(aim, toTarget.normalized);
                if (dot < bestDot || dot <= bestScore)
                {
                    continue;
                }

                var health = hit.GetComponentInParent<Health>();
                if (health != null)
                {
                    bestTarget = health;
                    bestScore = dot;
                }
            }

            if (bestTarget != null)
            {
                bestTarget.TakeDamage(primaryDamage);
            }
        }

        private void TryAreaStrike(float damage, float radius, ref float nextReadyAt, float cooldown)
        {
            if (Time.time < nextReadyAt)
            {
                return;
            }

            nextReadyAt = Time.time + cooldown;
            var hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, radius, hits, targetLayers);

            for (var i = 0; i < hitCount; i++)
            {
                var hit = hits[i];
                if (hit == null || hit.transform == transform)
                {
                    continue;
                }

                var health = hit.GetComponentInParent<Health>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
            }
        }
    }
}
