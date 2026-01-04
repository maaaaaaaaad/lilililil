using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
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

        [Header("Animation")] [SerializeField]
        private Animator animator;

        private Camera _mainCamera;
        private Vector3? _targetPosition;
        private float _equipmentSpeedBonus;
        private float _currentSpeed;
        private float TotalMoveSpeed => baseMoveSpeed + _equipmentSpeedBonus;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            ProcessMouseInput();
            ProcessKeyboardInput();
            ProcessClickToMove();
            UpdateAnimator();
        }

        private void ProcessMouseInput()
        {
            if (Mouse.current is null || !Mouse.current.leftButton.isPressed)
                return;

            if (!TryGetClickPosition(out var clickPosition))
                return;

            _targetPosition = clickPosition;
            ShowMarker(clickPosition);
        }

        private bool TryGetClickPosition(out Vector3 position)
        {
            position = Vector3.zero;

            if (_mainCamera == null)
                return false;

            var ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, groundLayer))
                return false;

            position = hit.point;
            return true;
        }

        private void ProcessClickToMove()
        {
            if (!_targetPosition.HasValue)
            {
                _currentSpeed = 0f;
                return;
            }

            var direction = (_targetPosition.Value - transform.position);
            direction.y = 0;

            if (direction.magnitude <= stopDistance)
            {
                _targetPosition = null;
                _currentSpeed = 0f;
                HideMarker();
                return;
            }

            var moveDirection = direction.normalized;
            Move(moveDirection);
        }

        private void ProcessKeyboardInput()
        {
            if (Keyboard.current is null)
                return;

            var horizontal = GetHorizontalInput();
            var vertical = GetVerticalInput();

            if (!HasMovementInput(horizontal, vertical))
                return;

            _targetPosition = null;
            HideMarker();

            var moveDirection = CalculateMoveDirection(horizontal, vertical);
            Move(moveDirection);
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

        private void Move(Vector3 direction)
        {
            transform.position += direction * (TotalMoveSpeed * Time.deltaTime);
            RotateTowards(direction);
            _currentSpeed = TotalMoveSpeed;
        }

        private void RotateTowards(Vector3 direction)
        {
            if (direction == Vector3.zero)
                return;

            var targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        private void ShowMarker(Vector3 position)
        {
            if (!clickMarker)
                return;

            clickMarker.transform.position = position;
            clickMarker.SetActive(true);
        }

        private void HideMarker()
        {
            if (!clickMarker)
                return;

            clickMarker.SetActive(false);
        }

        private void UpdateAnimator()
        {
            if (!animator)
                return;

            animator.SetFloat("Speed", _currentSpeed);
        }

        public void SetEquipmentSpeedBonus(float bonus)
        {
            _equipmentSpeedBonus = Mathf.Max(0, bonus);
        }

        public float GetCurrentSpeed() => TotalMoveSpeed;
    }
}
