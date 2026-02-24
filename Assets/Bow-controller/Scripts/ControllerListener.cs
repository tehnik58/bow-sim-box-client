using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerListener : MonoBehaviour
{
    public InputActionReference moveActionReference;
    private Vector2 moveInput;
    private float d_moveInput;

    void OnEnable()
    {
        if (moveActionReference != null)
        {
            moveActionReference.action.Enable();
            // Подписываемся на событие performed, чтобы получать данные стика
            moveActionReference.action.performed += OnMovePerformed;
            moveActionReference.action.canceled += OnMoveCanceled;
        }
    }

    void OnDisable()
    {
        if (moveActionReference != null)
        {
            moveActionReference.action.performed -= OnMovePerformed;
            moveActionReference.action.canceled -= OnMoveCanceled;
            moveActionReference.action.Disable();
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        // Читаем значение Vector2 (позиция стика: X и Y от -1 до 1)
        moveInput = context.ReadValue<Vector2>();
        Debug.Log($"Стик двигается: {moveInput}");
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // Сброс значения, когда стик возвращается в центр
        moveInput = Vector2.zero;
        Debug.Log("Стик в центре");
    }
}
