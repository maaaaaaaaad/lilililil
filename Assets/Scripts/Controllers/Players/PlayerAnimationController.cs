using UnityEngine;

namespace Controllers.Players
{
    public class PlayerAnimationController
    {
        private readonly Animator _animator;
        private readonly float _walkSpeedThreshold;
        private readonly float _runDelayTime;
        private float _moveStartTime;
        private bool _wasMoving;

        public PlayerAnimationController(Animator animator, float walkSpeedThreshold, float runDelayTime = 1f)
        {
            _animator = animator;
            _walkSpeedThreshold = walkSpeedThreshold;
            _runDelayTime = runDelayTime;
        }

        public float UpdateAnimation(float totalSpeed, bool isMoving)
        {
            if (!_animator)
                return 0f;

            if (!_wasMoving && isMoving)
                _moveStartTime = Time.time;

            var actualSpeed = isMoving ? totalSpeed : 0f;

            if (isMoving && Time.time - _moveStartTime < _runDelayTime)
                actualSpeed = Mathf.Min(totalSpeed, _walkSpeedThreshold);

            _animator.SetFloat("Speed", actualSpeed);
            _wasMoving = isMoving;

            return actualSpeed;
        }
    }
}
