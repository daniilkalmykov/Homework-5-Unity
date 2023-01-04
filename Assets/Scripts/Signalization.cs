using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Signalization : MonoBehaviour
{
    [SerializeField] private Robber _robber;
    [SerializeField] private Door _door;
    [SerializeField] private float _minVolume;
    [SerializeField] private float _maxVolume;
    [SerializeField] private float _timeBetweenIterations;
    [SerializeField, Range(0, 1)] private float _volumeSpeed;

    private const float MinDifference = 0.1f;
    private const float MinTimeBetweenIterations = 0.1f;

    private AudioSource _audioSource;
    private Coroutine _coroutine;
    
    private void OnValidate()
    {
        if (_minVolume > _maxVolume)
        {
            _minVolume = _maxVolume - MinDifference;
        }

        if (_timeBetweenIterations < MinTimeBetweenIterations)
        {
            _timeBetweenIterations = MinTimeBetweenIterations;
        }
    }
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = _minVolume;
    }

    private void OnEnable()
    {
        _door.Detected += OnDetected;
        _door.UnDetected += OnUnDetected;
    }

    private void OnDisable()
    {
        _door.Detected -= OnDetected; 
        _door.UnDetected -= OnUnDetected;
    }

    private IEnumerator Activate(float target)
    {
        var waitForOneSecond = new WaitForSeconds(_timeBetweenIterations);
        _audioSource.Play();
        
        while (_audioSource.volume != target)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, target, _volumeSpeed);
            
            yield return waitForOneSecond;
        }
        
        _audioSource.Stop();
    }

    private void OnDetected()
    {
        if(_coroutine != null)
            StopCoroutine(_coroutine);
        
        _coroutine = StartCoroutine(Activate(_maxVolume));
    }

    private void OnUnDetected()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        
        _coroutine = StartCoroutine(Activate(_minVolume));
    }
}