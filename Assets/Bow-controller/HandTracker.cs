using UnityEngine;
using UnityEngine.XR.Hands;

public class HandTracker : MonoBehaviour
{
    private XRHandTrackingEvents trackingEvents;

    void Start()
    {
        trackingEvents = GetComponent<XRHandTrackingEvents>();
        
        trackingEvents.trackingAcquired.AddListener(OnTrackingAcquired);
        trackingEvents.trackingLost.AddListener(OnTrackingLost);
    }

    // БЕЗ параметров!
    void OnTrackingAcquired()
    {
        Debug.Log("Рука обнаружена! Позиция: " + transform.position);
    }

    void OnTrackingLost()
    {
        Debug.Log("Рука потеряна");
    }

    void OnDestroy()
    {
        if (trackingEvents != null)
        {
            trackingEvents.trackingAcquired.RemoveListener(OnTrackingAcquired);
            trackingEvents.trackingLost.RemoveListener(OnTrackingLost);
        }
    }
}