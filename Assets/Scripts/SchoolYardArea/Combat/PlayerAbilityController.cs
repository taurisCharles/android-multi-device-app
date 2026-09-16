using System.Collections.Generic;
using SchoolYardArea.Input;
using UnityEngine;

namespace SchoolYardArea.Combat
{
    [RequireComponent(typeof(VirtualMoveInput))]
    public sealed class PlayerAbilityController : MonoBehaviour
    {
        [SerializeField] private float primaryDamage = 34f;
        [SerializeField] private float primaryRadius = 1.45f;
        [SerializeField] private float primaryCooldown = 0.45f;
        [SerializeField] private float specialDamage = 60f;
        [SerializeField] private float specialRadius = 2.6f;
        [SerializeField] private float specialCooldown = 7f;
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
                TryStrike(primaryDamage, primaryRadius, ref nextPrimaryAt, primaryCooldown);
            }

            if (input.SpecialPressed)
            {
                TryStrike(specialDamage, specialRadius, ref nextSpecialAt, specialCooldown);
            }

            input.ConsumeButtons();
        }

        private void TryStrike(float damage, float radius, ref float nextReadyAt, float cooldown)
        {
            if (Time.time < nextReadyAt)
            {
                return;
            }

            nextReadyAt = Time.time + cooldown;
            var hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, radius, hits, targetLayers);
            var damaged = new HashSet<Health>();

            for (var i = 0; i < hitCount; i++)
            {
                var hit = hits[i];
                if (hit == null || hit.transform == transform)
                {
                    continue;
                }

                var health = hit.GetComponentInParent<Health>();
                if (health != null && damaged.Add(health))
                {
                    health.TakeDamage(damage);
                }
            }
        }
    }
}
