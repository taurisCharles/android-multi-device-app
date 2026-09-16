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
        [SerializeField] private CharacterDefinition character;
        [SerializeField] private VirtualMoveInput input;

        private Rigidbody2D body;
        private Health health;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            health = GetComponent<Health>();

            if (character != null)
            {
                health.Configure(character.maxHealth);
            }
        }

        private void FixedUpdate()
        {
            if (input == null || character == null || health.IsKnockedOut)
            {
                body.velocity = Vector2.zero;
                return;
            }

            body.velocity = input.Move * character.moveSpeed;
        }
    }
}
