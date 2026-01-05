using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers.Players
{
    public class PlayerKeyboardMoveController
    {
        public Vector3? GetMoveDirection()
        {
            if (Keyboard.current is null)
                return null;

            var horizontal = GetHorizontalInput();
            var vertical = GetVerticalInput();

            if (!HasMovementInput(horizontal, vertical))
                return null;

            return CalculateMoveDirection(horizontal, vertical);
        }

        private static float GetHorizontalInput()
        {
            if (Keyboard.current.aKey.isPressed) return -1f;
            return Keyboard.current.dKey.isPressed ? 1f : 0f;
        }

        private static float GetVerticalInput()
        {
            if (Keyboard.current.wKey.isPressed) return 1f;
            return Keyboard.current.sKey.isPressed ? -1f : 0f;
        }

        private static bool HasMovementInput(float horizontal, float vertical)
        {
            return horizontal != 0f || vertical != 0f;
        }

        private static Vector3 CalculateMoveDirection(float horizontal, float vertical)
        {
            return new Vector3(horizontal, 0f, vertical).normalized;
        }
    }
}