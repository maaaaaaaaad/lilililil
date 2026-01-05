using UnityEngine;
using UnityEngine.EventSystems;

namespace Controllers.Players
{
    public class JoystickController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform handle;
        [SerializeField] private float handleRange = 50f;

        private Vector2 _inputDirection;
        private bool _isActive;

        public Vector2 InputDirection => _inputDirection;
        public bool IsActive => _isActive;

        public void OnPointerDown(PointerEventData eventData)
        {
            _isActive = true;
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!background)
                return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background,
                eventData.position,
                eventData.pressEventCamera,
                out var localPoint
            );

            var clampedPosition = Vector2.ClampMagnitude(localPoint, handleRange);
            handle.anchoredPosition = clampedPosition;

            _inputDirection = clampedPosition / handleRange;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isActive = false;
            _inputDirection = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
        }
    }
}