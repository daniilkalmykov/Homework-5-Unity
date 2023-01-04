using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    public event Action Detected;
    public event Action UnDetected;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Robber robber))
        {
            Detected?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Robber robber))
        {
            UnDetected?.Invoke();
        }
    }
}