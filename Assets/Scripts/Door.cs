using UnityEngine;

public class Door : MonoBehaviour
{
    public bool Detected { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Robber robber))
        {
            Detected = true;
            
            robber.Reach();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Deactivated!");
    }

    public void NotDetect()
    {
        Detected = false;
    }
}
