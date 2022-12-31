using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    public event Action Detected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Robber robber))
        {
            Detected?.Invoke();
        }
    }
}