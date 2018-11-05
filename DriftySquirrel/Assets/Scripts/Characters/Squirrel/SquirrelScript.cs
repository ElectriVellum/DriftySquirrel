using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class SquirrelScript : MonoBehaviour
{
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

    private Animator _animator;
    private CapsuleCollider2D _capsuleCollider2D;
    private Rigidbody2D _rigidbody2D;

    private Camera _camera;
    private float _cameraOffsetX;
    private float _cameraOffsetY;

    private bool _alive;
    private float _direction;
    private bool _grounded;
    private bool _jumped;
    private bool _ducked;
    private bool _stunned;
    private bool _attackTrigger;
    private bool _jumpTrigger;

    public bool CanFly
    {
        get
        {
            return _canFly;
        }
        set
        {
            _canFly = value;
        }
    }

    public bool CanFlyFromFall
    {
        get
        {
            return _canFlyFromFall;
        }
        set
        {
            _canFlyFromFall = value;
        }
    }

    public bool Alive
    {
        get
        {
            return _alive;
        }
    }

    public float Direction
    {
        get
        {
            return _direction;
        }
    }

    public bool Grounded
    {
        get
        {
            return _grounded;
        }
    }

    public bool Ducked
    {
        get
        {
            return _ducked;
        }
    }

    public bool Stunned
    {
        get
        {
            return _stunned;
        }
    }

    public bool AttackTrigger
    {
        get
        {
            return _attackTrigger;
        }
    }

    public bool JumpTrigger
    {
        get
        {
            return _jumpTrigger;
        }
    }

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

        _animator = null;
        _capsuleCollider2D = null;
        _rigidbody2D = null;

        _camera = null;
        _cameraOffsetX = 0f;
        _cameraOffsetY = 0f;

        _alive = true;
        _direction = 0f;
        _grounded = false;
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
                    _animator.SetTrigger("Fly");
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
        //var horizontal = Input.GetAxis("Horizontal");
        //_direction = horizontal;
        //if (Input.GetButtonDown("Jump"))
        //{
        //    Jump();
        //}
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    Attack();
        //}
        //if (Input.GetKeyDown("c"))
        //{
        //    StartStunning();
        //}
        //if (Input.GetKeyUp("c"))
        //{
        //    StopStunning();
        //}
        //if (Input.GetKeyDown("v"))
        //{
        //    StartDucking();
        //}
        //if (Input.GetKeyUp("v"))
        //{
        //    StopDucking();
        //}
        var obstacle = ObstacleCheck();
        if (obstacle)
        {
            UpdateDirection(0f);
        }
        else
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
        _direction = direction;
    }

    public void StartDucking()
    {
        if (_alive && _grounded && !_stunned && !_ducked)
        {
            _ducked = true;
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

    public void Die()
    {
        if (_alive)
        {
            _alive = false;
            _rigidbody2D.constraints = RigidbodyConstraints2D.None;
            _animator.SetTrigger("Die");
            ReleaseCamera();
            StartCoroutine(DieCoroutine());
        }
    }

    private IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Test");
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GroundWaters" || collision.gameObject.tag == "GroundSpikes" || collision.gameObject.tag == "Killers" || collision.gameObject.tag == "Canopies")
        {
            Die();
        }
        if (collision.gameObject.tag == "Collectibles")
        {
            collision.gameObject.SetActive(false);
        }
    }
}