using SchoolYardArea.Input;
using UnityEngine;

namespace SchoolYardArea.Combat
{
    [RequireComponent(typeof(VirtualMoveInput))]
    public sealed class PlayerAbilityController : MonoBehaviour
    {
        [SerializeField] private float primaryDamage = 24f;
        [SerializeField] private float primaryRadius = 1.55f;
        [SerializeField] private float primaryCooldown = 0.65f;
        [SerializeField] private float specialDamage = 48f;
        [SerializeField] private float specialRadius = 2.25f;
        [SerializeField] private float specialCooldown = 8.5f;
        [SerializeField] private Sprite pulseSprite;
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
            SpawnPulse(primaryRadius, new Color(1f, 0.92f, 0.24f, 0.45f));
            var hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, primaryRadius, hits, targetLayers);
            Health bestTarget = null;
            var bestDistance = float.MaxValue;

            for (var i = 0; i < hitCount; i++)
            {
                var hit = hits[i];
                if (hit == null || hit.transform == transform)
                {
                    continue;
                }

                var toTarget = (Vector2)hit.transform.position - (Vector2)transform.position;
                var distance = toTarget.sqrMagnitude;
                if (distance >= bestDistance)
                {
                    continue;
                }

                var health = hit.GetComponentInParent<Health>();
                if (health != null)
                {
                    bestTarget = health;
                    bestDistance = distance;
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
            SpawnPulse(radius, new Color(0.35f, 0.85f, 1f, 0.38f));
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

        private void SpawnPulse(float radius, Color color)
        {
            if (pulseSprite == null)
            {
                return;
            }

            var pulse = new GameObject("Combat Pulse");
            pulse.transform.position = transform.position + new Vector3(0f, 0f, 0.1f);
            pulse.AddComponent<CombatPulse>().Configure(pulseSprite, color, radius, 7);
        }
    }
}
