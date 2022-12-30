using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Robber : MonoBehaviour
{
    [SerializeField] private Transform _point;
    [SerializeField] private float _waitingTime;
    [SerializeField] private float _speed;

    private Animator _animator;
    private Vector3 _target;
    private Vector3 _startPosition;
    private float _timer;
    private float _startSpeed;
    private bool _reached;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _reached = false;
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

        if (_reached)
        {
            _timer += Time.deltaTime;
            _speed = 0;

            if (_timer >= _waitingTime)
            {
                _timer = 0;
                _target = _startPosition;

                _reached = false;
            }
        }
        else
        {
            _speed = _startSpeed;
        }
        
        transform.LookAt(_target);
        
        _animator.SetFloat("Speed", _speed);
    }

    public void Reach()
    {
        _reached = true;
    }
}
