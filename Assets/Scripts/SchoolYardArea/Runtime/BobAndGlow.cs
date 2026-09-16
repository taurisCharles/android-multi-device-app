using UnityEngine;

namespace SchoolYardArea.Runtime
{
    public sealed class BobAndGlow : MonoBehaviour
    {
        [SerializeField] private float bobHeight = 0.08f;
        [SerializeField] private float bobSpeed = 3f;
        [SerializeField] private float pulseAmount = 0.08f;

        private Vector3 startLocalPosition;
        private Vector3 startLocalScale;
        private float phase;

        private void Awake()
        {
            startLocalPosition = transform.localPosition;
            startLocalScale = transform.localScale;
            phase = Random.value * Mathf.PI * 2f;
        }

        private void Update()
        {
            var wave = Mathf.Sin(Time.time * bobSpeed + phase);
            transform.localPosition = startLocalPosition + Vector3.up * (wave * bobHeight);
            transform.localScale = startLocalScale * (1f + Mathf.Abs(wave) * pulseAmount);
        }
    }
}
