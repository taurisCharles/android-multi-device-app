using UnityEngine;

namespace SchoolYardArea.Combat
{
    public sealed class FloatingText : MonoBehaviour
    {
        [SerializeField] private float lifetime = 0.72f;
        [SerializeField] private float riseSpeed = 1.25f;

        private TextMesh text;
        private Color startColor;
        private float bornAt;

        public void Configure(string message, Color color, int sortingOrder)
        {
            text = gameObject.AddComponent<TextMesh>();
            text.text = message;
            text.anchor = TextAnchor.MiddleCenter;
            text.alignment = TextAlignment.Center;
            text.fontSize = 42;
            text.color = color;
            text.characterSize = 0.085f;
            text.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            startColor = color;
            bornAt = Time.time;
        }

        private void Update()
        {
            var age = Time.time - bornAt;
            transform.position += Vector3.up * (riseSpeed * Time.deltaTime);

            if (text != null)
            {
                var alpha = Mathf.Clamp01(1f - age / lifetime);
                text.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            }

            if (age >= lifetime)
            {
                Destroy(gameObject);
            }
        }
    }
}
