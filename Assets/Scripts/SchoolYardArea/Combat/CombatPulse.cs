using UnityEngine;

namespace SchoolYardArea.Combat
{
    public sealed class CombatPulse : MonoBehaviour
    {
        [SerializeField] private float lifetime = 0.18f;
        [SerializeField] private float startScale = 0.35f;
        [SerializeField] private float endScale = 1.4f;

        private SpriteRenderer spriteRenderer;
        private Color startColor;
        private float bornAt;

        public void Configure(Sprite sprite, Color color, float radius, int sortingOrder)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.color = color;
            spriteRenderer.sortingOrder = sortingOrder;
            startColor = color;
            startScale = radius * 0.35f;
            endScale = radius * 2f;
            bornAt = Time.time;
        }

        private void Update()
        {
            var age = Time.time - bornAt;
            var t = Mathf.Clamp01(age / lifetime);
            var scale = Mathf.Lerp(startScale, endScale, t);
            transform.localScale = new Vector3(scale, scale, 1f);

            if (spriteRenderer != null)
            {
                var alpha = Mathf.Lerp(startColor.a, 0f, t);
                spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            }

            if (age >= lifetime)
            {
                Destroy(gameObject);
            }
        }
    }
}
