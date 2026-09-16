using UnityEngine;

namespace SchoolYardArea.Input
{
    public sealed class VirtualMoveInput : MonoBehaviour
    {
        public Vector2 Move { get; private set; }
        public Vector2 Aim { get; private set; } = Vector2.right;
        public bool AttackPressed { get; private set; }
        public bool SpecialPressed { get; private set; }

        public void SetMove(Vector2 move)
        {
            Move = Vector2.ClampMagnitude(move, 1f);

            if (Move.sqrMagnitude > 0.01f)
            {
                Aim = Move.normalized;
            }
        }

        public void PressAttack()
        {
            AttackPressed = true;
        }

        public void PressSpecial()
        {
            SpecialPressed = true;
        }

        public void ConsumeButtons()
        {
            AttackPressed = false;
            SpecialPressed = false;
        }
    }
}
