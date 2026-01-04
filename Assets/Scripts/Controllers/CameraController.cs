using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        [Header("Target Settings")] [SerializeField]
        private Transform target;

        [Header("Camera Position")] [SerializeField]
        private Vector3 offset = new(0f, 5f, -6f);

        [Header("Follow Settings")] [SerializeField]
        private float smoothSpeed = 10f;

        private void LateUpdate()
        {
            if (!target)
                return;

            FollowTarget();
        }

        private void FollowTarget()
        {
            var desiredPosition = target.position + offset;
            var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;

            transform.LookAt(target);
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }
}