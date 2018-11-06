using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class SquirrelScript : MonoBehaviour
{
    public enum DieReason
    {
        Canopy,
        Spikes,
        Water,
        Gorge,
    }

    public delegate void RunStartEventHandler();
    public delegate void AttackEventHandler();
    public delegate void JumpEventHandler();
    public delegate void DriftEventHandler();
    public delegate void DuckEventHandler();
    public delegate void StunEventHandler();
    public delegate void DieEventHandler(DieReason reason);

    public event RunStartEventHandler OnRunStartEvent;
    public event AttackEventHandler OnAttackEvent;
    public event JumpEventHandler OnJumpEvent;
    public event DriftEventHandler OnDriftEvent;
    public event DuckEventHandler OnDuckEvent;
    public event StunEventHandler OnStunEvent;
    public event DieEventHandler OnDieEvent;

    protected virtual void OnRunStart()
    {
        if (OnRunStartEvent != null)
        {
            OnRunStartEvent.Invoke();
        }
    }

    protected virtual void OnAttack()
    {
        if (OnAttackEvent != null)
        {
            OnAttackEvent.Invoke();
        }
    }

    protected virtual void OnJump()
    {
        if (OnJumpEvent != null)
        {
            OnJumpEvent.Invoke();
        }
    }

    protected virtual void OnDrift()
    {
        if (OnDriftEvent != null)
        {
            OnDriftEvent.Invoke();
        }
    }

    protected virtual void OnDuck()
    {
        if (OnDuckEvent != null)
        {
            OnDuckEvent.Invoke();
        }
    }

    protected virtual void OnStun()
    {
        if (OnStunEvent != null)
        {
            OnStunEvent.Invoke();
        }
    }

    protected virtual void OnDie(DieReason reason)
    {
        if (OnDieEvent != null)
        {
            OnDieEvent.Invoke(reason);
        }
    }

    [SerializeField()]
    private bool _cameraFollowX;
    [SerializeField()]
    private bool _cameraFollowY;
    [SerializeField()]
    private LayerMask _groundLayers;
    [SerializeField()]
    private LayerMask _obstacleLayers;
    [SerializeField()]
    private float _speed;
    [SerializeField()]
    private float _speedLevel;
    [SerializeField()]
    private float _flySpeed;
    [SerializeField()]
    private float _jumpForce;
    [SerializeField()]
    private float _flyForce;
    [SerializeField()]
    private bool _canFly;
    [SerializeField()]
    private bool _canFlyFromFall;
    [SerializeField()]
    private GameObject[] _leaves;

    private Animator _animator;
    private CapsuleCollider2D _capsuleCollider2D;
    private Rigidbody2D _rigidbody2D;

    private Camera _camera;
    private float _cameraOffsetX;
    private float _cameraOffsetY;

    private bool _alive;
    private float _direction;
    private bool _grounded;
    private bool _obstacle;
    private bool _jumped;
    private bool _ducked;
    private bool _stunned;
    private bool _attackTrigger;
    private bool _jumpTrigger;

    public SquirrelScript()
    {
        _cameraFollowX = true;
        _cameraFollowY = false;
        _groundLayers = 0;
        _obstacleLayers = 0;
        _speed = 8f;
        _speedLevel = 1f;
        _flySpeed = 4f;
        _jumpForce = 8f;
        _flyForce = 5f;
        _canFly = true;
        _canFlyFromFall = false;
        _leaves = null;

        _animator = null;
        _capsuleCollider2D = null;
        _rigidbody2D = null;

        _camera = null;
        _cameraOffsetX = 0f;
        _cameraOffsetY = 0f;

        _alive = true;
        _direction = 0f;
        _grounded = false;
        _obstacle = false;
        _jumped = false;
        _ducked = false;
        _stunned = false;
        _attackTrigger = false;
        _jumpTrigger = false;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var velocity = _rigidbody2D.velocity;
        if (_alive && !_stunned && !_ducked)
        {
            if (_grounded)
            {
                velocity.x = _speed * _direction * _speedLevel;
                if (_jumpTrigger)
                {
                    velocity.y = _jumpForce;
                    _jumpTrigger = false;
                    _jumped = true;
                    OnJump();
                    SoundsControllerScript.Instance.PlayJumpSound();
                }
            }
            else
            {
                if (_jumpTrigger && ((_canFly && _jumped) || (_canFly && _canFlyFromFall)))
                {
                    if (Mathf.Abs(_direction) > Mathf.Epsilon)
                    {
                        velocity.x = _flySpeed * _direction * _speedLevel;
                    }
                    else
                    {
                        if (velocity.x < 0f)
                        {
                            velocity.x = -_flySpeed * _speedLevel;
                        }
                        else if (velocity.x > 0f)
                        {
                            velocity.x = _flySpeed * _speedLevel;
                        }
                    }
                    velocity.y = _flyForce;
                    _jumpTrigger = false;
                    _animator.SetTrigger("Drift");
                    OnDrift();
                    SoundsControllerScript.Instance.PlayDriftSound();
                }
            }
            _rigidbody2D.velocity = velocity;
        }
    }

    private void LateUpdate()
    {
        if (_camera != null)
        {
            var temp = _camera.transform.position;
            if (_cameraFollowX)
            {
                temp.x = transform.position.x + _cameraOffsetX;
            }
            if (_cameraFollowY)
            {
                temp.y = transform.position.y + _cameraOffsetY;
            }
            _camera.transform.position = temp;
        }
    }

    private void Start()
    {
        CaptureCamera();
        UpdateDirection(1f);
    }

    private void Update()
    {
        if (Mathf.Abs(_direction) > Mathf.Epsilon && Mathf.Abs(_rigidbody2D.velocity.x) > Mathf.Epsilon && _alive)
        {
            PlayControllerScript.Instance.AddTime(Time.deltaTime);
        }
        var obstacle = ObstacleCheck();
        if (obstacle && !_obstacle)
        {
            _obstacle = true;
            UpdateDirection(0f);
            StartCoroutine(Stun());
        }
        else if (!obstacle & _obstacle)
        {
            UpdateDirection(1f);
        }
        _grounded = CheckGrounded();
        var velocity = _rigidbody2D.velocity;
        if ((velocity.x < 0f && transform.localScale.x > 0f) || (velocity.x > 0f && transform.localScale.x < 0f))
        {
            transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
        }
        _animator.SetFloat("Speed", Mathf.Abs(velocity.x));
        var jumping = !_grounded && velocity.y > 0f;
        _animator.SetBool("Jumping", jumping);
        var falling = !_grounded && velocity.y < 0f;
        _animator.SetBool("Falling", falling);
        _animator.SetBool("Ducking", _ducked);
        _animator.SetBool("Stunned", _stunned);
    }

    private IEnumerator Stun()
    {
        StartStunning();
        yield return new WaitForSeconds(0.5f);
        StopStunning();
    }

    private bool CheckGrounded()
    {
        var groundPoint = new Vector2(transform.position.x, transform.position.y) + _capsuleCollider2D.offset + new Vector2(0f, -_capsuleCollider2D.size.y / 2f - 0.15f);
        var ground = Physics2D.OverlapPoint(groundPoint, _groundLayers);
        if (ground != null)
        {
            _jumped = false;
            return true;
        }
        return false;
    }

    private bool ObstacleCheck()
    {
        var obstaclePoint = new Vector2(transform.position.x, transform.position.y) + _capsuleCollider2D.offset + new Vector2(_capsuleCollider2D.size.x / 2f + 0.15f, 0f);
        var obstacle = Physics2D.OverlapPoint(obstaclePoint, _obstacleLayers);
        if (obstacle != null)
        {
            return true;
        }
        return false;
    }

    public void UpdateDirection(float direction)
    {
        var wasRunning = Mathf.Abs(direction) > Mathf.Epsilon;
        _direction = direction;
        if (Mathf.Abs(direction) > Mathf.Epsilon && !wasRunning)
        {
            OnRunStart();
        }
    }

    public void StartDucking()
    {
        if (_alive && _grounded && !_stunned && !_ducked)
        {
            _ducked = true;
            OnDuck();
        }
    }

    public void StopDucking()
    {
        if (_ducked)
        {
            _ducked = false;
        }
    }

    public void StartStunning()
    {
        if (_alive && !_stunned)
        {
            _stunned = true;
            OnStun();
        }
    }

    public void StopStunning()
    {
        if (_stunned)
        {
            _stunned = false;
        }
    }

    public void Jump()
    {
        if (_alive && !_stunned && !_ducked && !_jumpTrigger)
        {
            _jumpTrigger = true;
        }
    }

    public void Attack()
    {
        if (_alive && !_stunned && !_ducked && !_attackTrigger)
        {
            _attackTrigger = true;
        }
    }

    public void Die(DieReason reason)
    {
        if (_alive)
        {
            UpdateDirection(0f);
            _alive = false;
            _rigidbody2D.constraints = RigidbodyConstraints2D.None;
            _animator.SetTrigger("Die");
            OnDie(reason);
            ReleaseCamera();
        }
    }

    public void CaptureCamera()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
            _cameraOffsetX = _camera.transform.position.x - transform.position.x;
            _cameraOffsetY = _camera.transform.position.y - transform.position.y;
        }
    }

    public void ReleaseCamera()
    {
        if (_camera != null)
        {
            _camera = null;
            _cameraOffsetX = 0f;
            _cameraOffsetY = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Canopies")
        {
            var leafCount = Random.Range(3, 6);
            for (int i = 0; i < leafCount; i++)
            {
                var leaf = Instantiate(_leaves[Random.Range(0, _leaves.Length)], collision.transform.position, Quaternion.identity);
            }
            if (_alive)
            {
                Die(DieReason.Canopy);
                SoundsControllerScript.Instance.PlayCanopyDieSound();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_alive && collision.gameObject.tag == "GroundSpikes")
        {
            Die(DieReason.Spikes);
            SoundsControllerScript.Instance.PlaySpikesDieSound();
        }
        if (_alive && collision.gameObject.tag == "GroundWaters")
        {
            Die(DieReason.Water);
            SoundsControllerScript.Instance.PlayWaterDieSound();
        }
        if (_alive && collision.gameObject.tag == "Killers")
        {
            Die(DieReason.Gorge);
            SoundsControllerScript.Instance.PlayGorgeDieSound();
        }
        if (_alive && collision.gameObject.tag == "Collectibles")
        {
            var collectible = collision.GetComponent<CollectibleScript>() as CollectibleScript;
            if (collectible != null)
            {
                SoundsControllerScript.Instance.PlayPingSound();
                PlayControllerScript.Instance.Collect(collectible);
            }
            collision.gameObject.SetActive(false);
        }
    }
}