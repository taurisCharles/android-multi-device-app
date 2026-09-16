using SchoolYardArea.Combat;
using UnityEngine;

namespace SchoolYardArea.AI
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class BotChaseController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float moveSpeed = 3.5f;
        [SerializeField] private float stopDistance = 1.25f;

        private Rigidbody2D body;
        private Health health;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            health = GetComponent<Health>();
        }

        private void FixedUpdate()
        {
            if (target == null || health != null && health.IsKnockedOut)
            {
                body.linearVelocity = Vector2.zero;
                return;
            }

            Vector2 delta = target.position - transform.position;
            body.linearVelocity = delta.magnitude <= stopDistance ? Vector2.zero : delta.normalized * moveSpeed;
        }

        public void SetTarget(Transform nextTarget)
        {
            target = nextTarget;
        }
    }
}
