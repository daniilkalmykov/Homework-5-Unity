using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Signalization : MonoBehaviour
{
    [SerializeField] private Robber _robber;
    [SerializeField] private Door _door;
    [SerializeField] private UnityEvent _activated;
    [SerializeField] private float _minVolume;
    [SerializeField] private float _maxVolume;
    [SerializeField] private float _timeBetweenIterations;
    
    private AudioSource _audioSource;

    private void OnValidate()
    {
        if (_minVolume > _maxVolume)
        {
            _minVolume = _maxVolume - 0.1f;
        }

        if (_timeBetweenIterations <= 0)
        {
            _timeBetweenIterations = 0.1f;
        }
    }
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _audioSource.volume = _minVolume;
    }

    private void Update()
    {
        if (_door.Detected)
        {
            _activated?.Invoke();
            StartCoroutine(Activate());

            _door.NotDetect();
        }
    }

    private IEnumerator Activate()
    {
        while (true)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, _robber.Reached ? _maxVolume : _minVolume,
                _maxVolume / _robber.WaitingTime);

            if (_audioSource.volume <= _minVolume) 
                break;
            
            var waitForOneSecond = new WaitForSeconds(_timeBetweenIterations);
            yield return waitForOneSecond;
        }
        
        _audioSource.Stop();
    }
}