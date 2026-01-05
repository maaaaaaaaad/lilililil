using UnityEngine;

namespace Controllers.Players
{
    public class PlayerGamepadMoveController
    {
        private readonly JoystickController _joystick;
        private readonly float _deadzone;

        public PlayerGamepadMoveController(JoystickController joystick, float deadzone = 0.1f)
        {
            _joystick = joystick;
            _deadzone = deadzone;
        }

        public Vector3? GetMoveDirection()
        {
            if (!_joystick || !_joystick.IsActive)
                return null;

            var input = _joystick.InputDirection;

            if (input.magnitude < _deadzone)
                return null;

            return new Vector3(input.x, 0f, input.y).normalized;
        }
    }
}
