using UnityEngine;

public class Signalization : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Robber robber))
        {
            robber.Reach();
        }
    }
}
