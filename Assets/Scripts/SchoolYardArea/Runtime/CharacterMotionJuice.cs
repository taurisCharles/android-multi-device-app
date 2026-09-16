using UnityEngine;

namespace SchoolYardArea.Runtime
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class CharacterMotionJuice : MonoBehaviour
    {
        [SerializeField] private Transform visualRoot;
        [SerializeField] private float idleBob = 0.035f;
        [SerializeField] private float moveSquash = 0.08f;
        [SerializeField] private float bobSpeed = 8f;

        private Rigidbody2D body;
        private Vector3 startScale;
        private Vector3 startPosition;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            if (visualRoot == null)
            {
                visualRoot = transform;
            }

            startScale = visualRoot.localScale;
            startPosition = visualRoot.localPosition;
        }

        private void LateUpdate()
        {
            var speed = body.linearVelocity.magnitude;
            var moving01 = Mathf.Clamp01(speed / 4f);
            var wave = Mathf.Sin(Time.time * bobSpeed);
            var squash = Mathf.Abs(wave) * moveSquash * moving01;
            visualRoot.localScale = new Vector3(startScale.x * (1f + squash), startScale.y * (1f - squash * 0.55f), startScale.z);
            visualRoot.localPosition = startPosition + Vector3.up * (Mathf.Sin(Time.time * 3.6f) * idleBob * (0.35f + moving01));
        }
    }
}
