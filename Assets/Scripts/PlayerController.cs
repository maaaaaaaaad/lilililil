using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")] [SerializeField]
    private float baseMoveSpeed = 2f;

    [SerializeField] private float rotationSpeed = 10f;

    private float _equipmentSpeedBonus;
    private float TotalMoveSpeed => baseMoveSpeed + _equipmentSpeedBonus;

    private void Update()
    {
        ProcessMovementInput();
    }

    private void ProcessMovementInput()
    {
        if (Keyboard.current == null)
            return;

        var horizontal = GetHorizontalInput();
        var vertical = GetVerticalInput();

        if (!HasMovementInput(horizontal, vertical))
            return;

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
        if (Camera.main == null)
            return Vector3.zero;

        var cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        var cameraRight = Camera.main.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        return (cameraRight * horizontal + cameraForward * vertical).normalized;
    }

    private void Move(Vector3 direction)
    {
        transform.position += direction * TotalMoveSpeed * Time.deltaTime;
        RotateTowards(direction);
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        var targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void SetEquipmentSpeedBonus(float bonus)
    {
        _equipmentSpeedBonus = Mathf.Max(0, bonus);
    }

    public float GetCurrentSpeed() => TotalMoveSpeed;
}