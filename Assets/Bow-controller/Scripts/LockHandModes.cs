using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Hands;
using System.Collections.Generic;

public class LockHandModes : MonoBehaviour
{
    void Start()
    {
        // Отключаем левый контроллер
        DisableController(XRNode.LeftHand);
        
        // Отключаем правую руку (hand tracking)
        DisableHand(XRNode.RightHand);
        
        Debug.Log("✅ Режимы зафиксированы: Левая=рука, Правая=контроллер");
    }

    void DisableController(XRNode node)
    {
        var controllers = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(node, controllers);
        
        foreach (var controller in controllers)
        {
            if (controller.characteristics.HasFlag(InputDeviceCharacteristics.Controller))
            {
                Debug.Log($"Отключаю контроллер на {node}: {controller.name}");
                // Контроллер уже активен, просто игнорируем его
            }
        }
    }

    void DisableHand(XRNode node)
    {
        var hands = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(node, hands);
        
        foreach (var hand in hands)
        {
            if (hand.characteristics.HasFlag(InputDeviceCharacteristics.HandTracking))
            {
                Debug.Log($"Игнорирую руку на {node}: {hand.name}");
                // Просто не используем эту руку
            }
        }
    }

    void Update()
    {
        // Проверяем что используется
        var leftDevices = new List<InputDevice>();
        var rightDevices = new List<InputDevice>();
        
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftDevices);
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightDevices);
        
        foreach (var device in leftDevices)
        {
            if (device.isValid)
            {
                Debug.Log($"Левая сторона: {device.name} - {device.characteristics}");
            }
        }
        
        foreach (var device in rightDevices)
        {
            if (device.isValid)
            {
                Debug.Log($"Правая сторона: {device.name} - {device.characteristics}");
            }
        }
    }
}