using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesMaker : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;
    [SerializeField] private float _timeBetweenIterations;
    
    private const float MinTimeBetweenIterations = 0.1f;

    private List<Transform> _points;
    private bool _isCoroutineStarted = false;

    private void OnValidate()
    {
        if (_timeBetweenIterations < MinTimeBetweenIterations)
        {
            _timeBetweenIterations = MinTimeBetweenIterations;
        }
    }

    private void Start()
    {
        _points = GetComponentsInChildren<Transform>().ToList();

        if (_isCoroutineStarted == false)
        {
            StartCoroutine(CreateEnemies());
            _isCoroutineStarted = true;
        }
    }

    private IEnumerator CreateEnemies()
    {
        var waitForTwoSeconds = new WaitForSeconds(_timeBetweenIterations);
        
        while (_points.Count > 0)
        {
            int randomNumber = Random.Range(0, _points.Count);
            Instantiate(_enemy, _points[randomNumber], false);
            
            _points.RemoveAt(randomNumber);
            yield return waitForTwoSeconds;
        }

        _isCoroutineStarted = false;
    }
}
