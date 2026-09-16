using UnityEngine;

namespace SchoolYardArea.Input
{
    public sealed class VirtualMoveInput : MonoBehaviour
    {
        [SerializeField] private float dragRadiusPixels = 120f;

        private bool isDragging;
        private Vector2 dragOrigin;

        public Vector2 Move { get; private set; }
        public Vector2 Aim { get; private set; } = Vector2.right;
        public bool AttackPressed { get; private set; }
        public bool SpecialPressed { get; private set; }

        private void Update()
        {
            if (UnityEngine.Input.touchCount > 0)
            {
                UpdateTouch(UnityEngine.Input.GetTouch(0));
                return;
            }

            UpdateMouse();
        }

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

        private void UpdateTouch(Touch touch)
        {
            if (touch.phase == TouchPhase.Began)
            {
                BeginDrag(touch.position);
                return;
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                EndDrag();
                return;
            }

            Drag(touch.position);
        }

        private void UpdateMouse()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                BeginDrag(UnityEngine.Input.mousePosition);
                return;
            }

            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                EndDrag();
                return;
            }

            if (isDragging)
            {
                Drag(UnityEngine.Input.mousePosition);
            }
        }

        private void BeginDrag(Vector2 position)
        {
            isDragging = true;
            dragOrigin = position;
            SetMove(Vector2.zero);
        }

        private void Drag(Vector2 position)
        {
            if (!isDragging)
            {
                return;
            }

            SetMove((position - dragOrigin) / dragRadiusPixels);
        }

        private void EndDrag()
        {
            isDragging = false;
            SetMove(Vector2.zero);
        }
    }
}
