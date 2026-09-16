using System;
using UnityEngine;

namespace SchoolYardArea.Combat
{
    public sealed class Health : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;

        public event Action<Health> KnockedOut;
        public event Action<float, float> Changed;

        public float Current { get; private set; }
        public float Max => maxHealth;
        public bool IsKnockedOut => Current <= 0f;

        private void Awake()
        {
            Current = maxHealth;
        }

        public void Configure(float newMaxHealth)
        {
            maxHealth = Mathf.Max(1f, newMaxHealth);
            Current = maxHealth;
            Changed?.Invoke(Current, maxHealth);
        }

        public void TakeDamage(float amount)
        {
            if (IsKnockedOut || amount <= 0f)
            {
                return;
            }

            Current = Mathf.Max(0f, Current - amount);
            Changed?.Invoke(Current, maxHealth);

            if (IsKnockedOut)
            {
                KnockedOut?.Invoke(this);
            }
        }

        public void Heal(float amount)
        {
            if (IsKnockedOut || amount <= 0f)
            {
                return;
            }

            Current = Mathf.Min(maxHealth, Current + amount);
            Changed?.Invoke(Current, maxHealth);
        }
    }
}
