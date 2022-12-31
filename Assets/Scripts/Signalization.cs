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
    private bool _isCoroutineStarted;
    
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
        _isCoroutineStarted = false;        
        
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = _minVolume;
    }

    private void OnEnable()
    {
        _door.Detected += OnDetected;
    }

    private void OnDisable()
    {
        _door.Detected -= OnDetected; 
    }

    private IEnumerator Activate()
    {
        _isCoroutineStarted = true;
        
        var waitForOneSecond = new WaitForSeconds(_timeBetweenIterations);
        
        _audioSource.Play();
        
        while (_audioSource.volume >= _minVolume)
        {
            if (_robber.Reached)
            {
                _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, _maxVolume, _volumeSpeed);
            }
            else
            {
                _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, _minVolume, _volumeSpeed);
                
                if(_audioSource.volume <= _minVolume)
                    break;
            }
            
            yield return waitForOneSecond;
        }
        
        _audioSource.Stop();
        _isCoroutineStarted = false;
    }
    
    private void OnDetected()
    {
        if(_isCoroutineStarted)
            StartCoroutine(Activate());
    }
}