using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Signalization : MonoBehaviour
{
    [SerializeField] private UnityEvent _activated;
    [SerializeField] private float _minVolume;
    [SerializeField] private float _maxVolume;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _audioSource.volume = _minVolume;
    }

    private void OnValidate()
    {
        if (_minVolume > _maxVolume)
        {
            _minVolume = _maxVolume - 0.1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Robber robber))
        {
            robber.Reach();

            _activated?.Invoke();
            StartCoroutine(Activate(robber.WaitingTime, robber));
        }
    }

    private IEnumerator Activate(float playingTime, Robber robber)
    {
        while (true)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, robber.Reached ? _maxVolume : _minVolume, _maxVolume / playingTime);

            if (_audioSource.volume <= _minVolume) 
                break;
            
            var waitForOneSecond = new WaitForSeconds(1);
            yield return waitForOneSecond;
        }
        
        _audioSource.Stop();
    }
}