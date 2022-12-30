using UnityEngine;

public class Door : MonoBehaviour
{
    private bool _detected;

    public bool Detected => _detected;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Robber robber))
        {
            _detected = true;
            
            robber.Reach();
        }
    }

    public void NotDetect()
    {
        _detected = false;
    }
}
