using UnityEngine;

namespace SchoolYardArea.Combat
{
    public sealed class HealthBar2D : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private Transform fill;

        private void Awake()
        {
            if (health == null)
            {
                health = GetComponentInParent<Health>();
            }
        }

        private void OnEnable()
        {
            if (health != null)
            {
                health.Changed += UpdateBar;
                UpdateBar(health.Current, health.Max);
            }
        }

        private void OnDisable()
        {
            if (health != null)
            {
                health.Changed -= UpdateBar;
            }
        }

        private void UpdateBar(float current, float max)
        {
            if (fill == null)
            {
                return;
            }

            var percent = max <= 0f ? 0f : Mathf.Clamp01(current / max);
            fill.localScale = new Vector3(percent, 1f, 1f);
            fill.localPosition = new Vector3((percent - 1f) * 0.5f, 0f, 0f);
        }
    }
}
