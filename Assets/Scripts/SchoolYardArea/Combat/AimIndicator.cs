using SchoolYardArea.Input;
using UnityEngine;

namespace SchoolYardArea.Combat
{
    public sealed class AimIndicator : MonoBehaviour
    {
        [SerializeField] private VirtualMoveInput input;
        [SerializeField] private Transform indicator;
        [SerializeField] private float offset = 0.85f;

        private void Awake()
        {
            if (input == null)
            {
                input = GetComponentInParent<VirtualMoveInput>();
            }
        }

        private void LateUpdate()
        {
            if (input == null || indicator == null)
            {
                return;
            }

            var aim = input.Aim.sqrMagnitude > 0.01f ? input.Aim.normalized : Vector2.up;
            indicator.localPosition = aim * offset;
            indicator.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg);
        }
    }
}
