using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Hands;
using System.Collections.Generic;

public class ForceMultimodal : MonoBehaviour
{
    void Start()
    {
        var ovrManager = FindObjectOfType<OVRManager>();
        if (ovrManager != null)
        {
            ovrManager.SimultaneousHandsAndControllersEnabled = true;
            Debug.Log("✅ Мультимодальный режим включен!");
        }
        else
        {
            Debug.LogWarning("⚠️ OVRManager не найден. Добавьте его на XR Origin");
        }
    }
}