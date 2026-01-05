using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers.Players
{
    public class PlayerMouseMoveController
    {
        private readonly Camera _camera;
        private readonly LayerMask _groundLayer;
        private readonly GameObject _clickMarker;
        private readonly float _stopDistance;
        private Vector3? _targetPosition;

        public PlayerMouseMoveController(Camera camera, LayerMask groundLayer, GameObject clickMarker, float stopDistance)
        {
            _camera = camera;
            _groundLayer = groundLayer;
            _clickMarker = clickMarker;
            _stopDistance = stopDistance;
        }

        public void ProcessInput()
        {
            if (Mouse.current is null || !Mouse.current.leftButton.isPressed)
                return;

            if (!TryGetClickPosition(out var clickPosition))
                return;

            _targetPosition = clickPosition;
            ShowMarker(clickPosition);
        }

        public Vector3? GetMoveDirection(Vector3 currentPosition)
        {
            if (!_targetPosition.HasValue)
                return null;

            var direction = (_targetPosition.Value - currentPosition);
            direction.y = 0;

            if (direction.magnitude <= _stopDistance)
            {
                _targetPosition = null;
                HideMarker();
                return null;
            }

            return direction.normalized;
        }

        public void ClearTarget()
        {
            _targetPosition = null;
            HideMarker();
        }

        private bool TryGetClickPosition(out Vector3 position)
        {
            position = Vector3.zero;

            if (_camera == null)
                return false;

            var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, _groundLayer))
                return false;

            position = hit.point;
            return true;
        }

        private void ShowMarker(Vector3 position)
        {
            if (!_clickMarker)
                return;

            _clickMarker.transform.position = position;
            _clickMarker.SetActive(true);
        }

        private void HideMarker()
        {
            if (!_clickMarker)
                return;

            _clickMarker.SetActive(false);
        }
    }
}
