using UnityEngine;

namespace SchoolYardArea.Combat
{
    public sealed class DamageFlash : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private SpriteRenderer[] renderers;
        [SerializeField] private float flashSeconds = 0.12f;

        private Color[] baseColors;
        private float flashUntil;
        private float lastCurrent;

        private void Awake()
        {
            if (health == null)
            {
                health = GetComponentInParent<Health>();
            }

            if (renderers == null || renderers.Length == 0)
            {
                renderers = GetComponentsInChildren<SpriteRenderer>();
            }

            baseColors = new Color[renderers.Length];
            for (var i = 0; i < renderers.Length; i++)
            {
                baseColors[i] = renderers[i].color;
            }

            if (health != null)
            {
                lastCurrent = health.Current;
            }
        }

        private void OnEnable()
        {
            if (health != null)
            {
                health.Changed += OnHealthChanged;
            }
        }

        private void OnDisable()
        {
            if (health != null)
            {
                health.Changed -= OnHealthChanged;
            }
        }

        private void Update()
        {
            if (Time.time < flashUntil)
            {
                return;
            }

            for (var i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null)
                {
                    renderers[i].color = baseColors[i];
                }
            }
        }

        private void OnHealthChanged(float current, float max)
        {
            if (current >= max)
            {
                lastCurrent = current;
                return;
            }

            if (current < lastCurrent)
            {
                SpawnFloatingText($"-{Mathf.CeilToInt(lastCurrent - current)}", new Color(1f, 0.28f, 0.18f));
            }

            lastCurrent = current;
            flashUntil = Time.time + flashSeconds;
            foreach (var spriteRenderer in renderers)
            {
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = Color.white;
                }
            }
        }

        private void SpawnFloatingText(string message, Color color)
        {
            var textObject = new GameObject("Damage Text");
            textObject.transform.position = transform.position + new Vector3(0f, 1.55f, 0f);
            textObject.AddComponent<FloatingText>().Configure(message, color, 60);
        }
    }
}
