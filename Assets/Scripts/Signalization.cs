using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Signalization : MonoBehaviour
{
    [SerializeField] private UnityEvent<Coroutine> _reached;
    [SerializeField] private UnityEvent<Coroutine> _exited;
    [SerializeField] private float _minVolume;
    [SerializeField] private float _maxVolume;
    [SerializeField] private float _playingTime;

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

        if (_playingTime <= 0)
        {
            _playingTime = 0.1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Robber robber))
        {
            robber.Reach();
            _reached?.Invoke(StartCoroutine(Reach()));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Robber>())
        {
            //_exited?.Invoke(StartCoroutine(Exit()));
        }
    }

    private IEnumerator Reach()
    {
        _audioSource.Play();
        _audioSource.volume = Mathf.MoveTowards(_minVolume, _maxVolume, _maxVolume / _playingTime);

        var waitForOneSecond = new WaitForSeconds(1);
        yield return waitForOneSecond;
    }

    private IEnumerator Exit()
    {
        _audioSource.volume = Mathf.MoveTowards(_maxVolume, _minVolume, Time.deltaTime);
        
        var waitForOneSecond = new WaitForSeconds(1);
        yield return waitForOneSecond;
    }
}