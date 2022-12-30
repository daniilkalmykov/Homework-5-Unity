using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Signalization _signalization;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Robber robber))
        {
            robber.Reach();

            _signalization.InvokeEvent();
            StartCoroutine(_signalization.Activate(robber.WaitingTime, robber));
        }
    }
}
