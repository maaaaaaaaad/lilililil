using UnityEngine;

namespace Controllers.Players
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")] [SerializeField]
        private float baseMoveSpeed = 2f;

        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float stopDistance = 0.1f;
        [SerializeField] private float walkSpeedThreshold = 2f;

        [Header("Mouse Click Settings")] [SerializeField]
        private LayerMask groundLayer;

        [SerializeField] private GameObject clickMarker;

        [Header("Animation")] [SerializeField] private Animator animator;

        [Header("Mobile Input")] [SerializeField]
        private JoystickController joystick;

        private Camera _mainCamera;
        private bool _isMoving;
        private float _actualSpeed;

        private readonly PlayerKeyboardMoveController _keyboardController = new();
        private PlayerMouseMoveController _mouseController;
        private PlayerGamepadMoveController _gamepadController;
        private PlayerMovement _movement;
        private PlayerAnimationController _animationController;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _mouseController = new PlayerMouseMoveController(_mainCamera, groundLayer, clickMarker, stopDistance);
            _gamepadController = new PlayerGamepadMoveController(joystick);
            _movement = new PlayerMovement(transform, baseMoveSpeed, rotationSpeed);
            _animationController = new PlayerAnimationController(animator, walkSpeedThreshold);
        }

        private void Update()
        {
            _isMoving = false;
            _movement.SetBaseMoveSpeed(baseMoveSpeed);

            if (!joystick || !joystick.IsActive)
                _mouseController.ProcessInput();

            Vector3? moveDirection = null;

            var keyboardDirection = _keyboardController.GetMoveDirection();
            if (keyboardDirection.HasValue)
            {
                _mouseController.ClearTarget();
                moveDirection = keyboardDirection;
            }
            else
            {
                var gamepadDirection = _gamepadController.GetMoveDirection();
                if (gamepadDirection.HasValue)
                {
                    _mouseController.ClearTarget();
                    moveDirection = gamepadDirection;
                }
                else
                {
                    moveDirection = _mouseController.GetMoveDirection(transform.position);
                }
            }

            if (moveDirection.HasValue)
            {
                _isMoving = true;
                _actualSpeed = _animationController.UpdateAnimation(_movement.TotalMoveSpeed, _isMoving);
                _movement.Move(moveDirection.Value, _actualSpeed, Time.deltaTime);
            }
            else
            {
                _animationController.UpdateAnimation(_movement.TotalMoveSpeed, _isMoving);
            }
        }

        public void SetEquipmentSpeedBonus(float bonus)
        {
            _movement.SetEquipmentSpeedBonus(bonus);
        }

        public float GetCurrentSpeed() => _movement.TotalMoveSpeed;
    }
}