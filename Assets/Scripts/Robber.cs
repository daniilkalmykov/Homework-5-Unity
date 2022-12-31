using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Robber : MonoBehaviour
{
    [SerializeField] private Transform _point;
    [SerializeField] private float _waitingTime;
    [SerializeField] private float _speed;

    private readonly int _speedAnimatorParameter = Animator.StringToHash("Speed");
    
    private Animator _animator;
    private Vector3 _target;
    private Vector3 _startPosition;
    private float _timer;
    private float _startSpeed;

    public float WaitingTime => _waitingTime;
    public bool Reached { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        Reached = false;
    }

    private void Start()
    {
        _startPosition = transform.position;
        _target = _point.position;

        _startSpeed = _speed;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
        
        if (Reached)
        {
            _speed = 0;

            StartCoroutine(WaitTime());
        }
        else
        {
            _speed = _startSpeed;
            
            StopCoroutine(WaitTime());
        }
        
        transform.LookAt(_target);
        
        _animator.SetFloat(_speedAnimatorParameter, _speed);
    }

    public void Reach()
    {
        Reached = true;
    }

    private IEnumerator WaitTime()
    {
        _timer += Time.deltaTime;
        
        if (_timer >= _waitingTime)
        {
            _timer = 0;
            _target = _startPosition;

            Reached = false;
        }
        
        yield return null;
    }
}
