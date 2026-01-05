using UnityEngine;

namespace Controllers.Players
{
    public class PlayerMovement
    {
        private readonly Transform _transform;
        private readonly float _rotationSpeed;
        private float _baseMoveSpeed;
        private float _equipmentSpeedBonus;

        public float TotalMoveSpeed => _baseMoveSpeed + _equipmentSpeedBonus;

        public PlayerMovement(Transform transform, float baseMoveSpeed, float rotationSpeed)
        {
            _transform = transform;
            _baseMoveSpeed = baseMoveSpeed;
            _rotationSpeed = rotationSpeed;
        }

        public void Move(Vector3 direction, float speed, float deltaTime)
        {
            _transform.position += direction * (speed * deltaTime);
            RotateTowards(direction, deltaTime);
        }

        private void RotateTowards(Vector3 direction, float deltaTime)
        {
            if (direction == Vector3.zero)
                return;

            var targetRotation = Quaternion.LookRotation(direction);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, _rotationSpeed * deltaTime);
        }

        public void SetBaseMoveSpeed(float speed)
        {
            _baseMoveSpeed = speed;
        }

        public void SetEquipmentSpeedBonus(float bonus)
        {
            _equipmentSpeedBonus = Mathf.Max(0, bonus);
        }
    }
}
