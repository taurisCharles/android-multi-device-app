using SchoolYardArea.Characters;
using SchoolYardArea.Combat;
using SchoolYardArea.Input;
using UnityEngine;

namespace SchoolYardArea.Runtime
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Health))]
    public sealed class PlayerController2D : MonoBehaviour
    {
        [SerializeField] private float fallbackMoveSpeed = 5f;
        [SerializeField] private CharacterDefinition character;
        [SerializeField] private VirtualMoveInput input;

        private Rigidbody2D body;
        private Health health;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            health = GetComponent<Health>();
            input ??= GetComponent<VirtualMoveInput>();

            if (character != null)
            {
                health.Configure(character.maxHealth);
            }
        }

        private void FixedUpdate()
        {
            if (input == null || health.IsKnockedOut)
            {
                body.linearVelocity = Vector2.zero;
                return;
            }

            var moveSpeed = character != null ? character.moveSpeed : fallbackMoveSpeed;
            body.linearVelocity = input.Move * moveSpeed;
        }
    }
}
