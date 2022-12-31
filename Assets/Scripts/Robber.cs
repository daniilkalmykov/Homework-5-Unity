using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Robber : MonoBehaviour
{
    [SerializeField] private Door _door;
    [SerializeField] private Transform _point;
    [SerializeField] private float _delay;
    [SerializeField] private float _speed;

    private readonly int _speedAnimatorParameter = Animator.StringToHash("Speed");
    
    private Animator _animator;
    private Vector3 _target;
    private Vector3 _startPosition;
    private float _timer;
    private float _startSpeed;

    public bool Reached { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _door.Detected += OnReached;
    }

    private void OnDisable()
    {
        _door.Detected -= OnReached;
    }

    private void Start()
    {
        _startPosition = transform.position;
        _target = _point.position;

        _startSpeed = _speed;
        
        transform.LookAt(_target);
        _animator.SetFloat(_speedAnimatorParameter, _speed);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
    }

    private void OnReached()
    {
        Reached = true;
        StartCoroutine(StayInTrigger());
        _speed = 0;
        
        _animator.SetFloat(_speedAnimatorParameter, _speed);
    }

    private IEnumerator StayInTrigger()
    {
        yield return new WaitForSeconds(_delay);

        Reached = false;
        
        _target = _startPosition;
        transform.LookAt(_target);
        
        _speed = _startSpeed;
        _animator.SetFloat(_speedAnimatorParameter, _speed);
    }
}
