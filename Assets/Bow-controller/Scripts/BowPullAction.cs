using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Events;

public class BowPullAction : MonoBehaviour
{
    // Ссылка на стик контроллера
    public InputActionReference stickActionReference;
    
    [Header("Настройки натяжения")]
    public float pullThreshold = 0.2f;        // Минимальное отклонение для начала натяжения
    public float maxPullTime = 1.5f;          // Время для полного натяжения
    
    // Событие для запуска стрелы (передаёт скорость от 0 до 1)
    public UnityEvent<float> OnArrowReleased;
    
    // Текущее состояние
    public bool IsPulling { get; private set; }
    public float CurrentPullStrength { get; private set; }
    
    // Приватные переменные
    private Vector2 stickInput;
    private float pullStartTime;
    private float lastStickMagnitude = 0f;
    
    void OnEnable()
    {
        if (stickActionReference != null)
        {
            stickActionReference.action.Enable();
            stickActionReference.action.performed += OnStickMoved;
            stickActionReference.action.canceled += OnStickReleased;
        }
    }
    
    void OnDisable()
    {
        if (stickActionReference != null)
        {
            stickActionReference.action.performed -= OnStickMoved;
            stickActionReference.action.canceled -= OnStickReleased;
            stickActionReference.action.Disable();
        }
    }
    
    private void OnStickMoved(InputAction.CallbackContext context)
    {
        stickInput = context.ReadValue<Vector2>();
        float stickMagnitude = stickInput.magnitude;
        
        // Начало натяжения
        if (!IsPulling && stickMagnitude > pullThreshold)
        {
            StartPull();
        }
        
        // Обновление натяжения
        if (IsPulling)
        {
            // Сила зависит от времени удержания
            float pullDuration = Time.time - pullStartTime;
            CurrentPullStrength = Mathf.Clamp01(pullDuration / maxPullTime);
        }
        
        // Проверка на выстрел (резкий возврат стика)
        if (IsPulling && stickMagnitude < pullThreshold * 0.3f && lastStickMagnitude > pullThreshold)
        {
            ReleaseArrow();
        }
        
        lastStickMagnitude = stickMagnitude;
    }
    
    private void OnStickReleased(InputAction.CallbackContext context)
    {
        // Стик отпущен - выстрел
        if (IsPulling)
        {
            ReleaseArrow();
        }
        
        stickInput = Vector2.zero;
        lastStickMagnitude = 0f;
    }
    
    private void StartPull()
    {
        IsPulling = true;
        pullStartTime = Time.time;
        CurrentPullStrength = 0f;
    }
    
    private void ReleaseArrow()
    {
        if (CurrentPullStrength > 0.1f) // Игнорируем слишком слабые натяжения
        {
            OnArrowReleased?.Invoke(CurrentPullStrength*20f);
        }
        
        ResetPull();
    }
    
    private void ResetPull()
    {
        IsPulling = false;
        CurrentPullStrength = 0f;
    }
}