using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayControllerScript : MonoBehaviour
{
    private static PlayControllerScript _instance;

    public static PlayControllerScript Instance
    {
        get
        {
            return _instance;
        }
    }

    public PlayControllerScript()
    {
        _backgroundMusic = null;
        _musicOffImage = null;
        _soundsOffImage = null;
        _score = 0;
        _pausePanel = null;
        _scoreText = null;
        _brownSquirrel = null;
        _redSquirrel = null;
        _whiteSquirrel = null;
        _squirrel = null;
        _rigidbody2D = null;
        _animator = null;

        _camera = null;
        _cameraOffsetX = 0f;

        _forwardSpeed = 3f;
        _bounceSpeed = 5f;
        _didFly = false;
        _alive = true;
    }

    [SerializeField()]
    private AudioClip _backgroundMusic;
    [SerializeField()]
    private GameObject _musicOffImage;
    [SerializeField()]
    private GameObject _soundsOffImage;
    [SerializeField()]
    private int _score;
    [SerializeField()]
    private GameObject _pausePanel;
    [SerializeField()]
    private Text _scoreText;
    [SerializeField()]
    private GameObject _brownSquirrel;
    [SerializeField()]
    private GameObject _redSquirrel;
    [SerializeField()]
    private GameObject _whiteSquirrel;
    private GameObject _squirrel;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private Camera _camera;
    private float _cameraOffsetX;

    private float _forwardSpeed;
    private float _bounceSpeed;
    private bool _didFly;
    private bool _alive;

    private void Awake()
    {
        if (GameControllerScript.Instance == null)
        {
            SceneManager.LoadScene("Menu");
        }
        MakeInstance();
    }

    private void FixedUpdate()
    {
        if (_alive)
        {
            var temp = _squirrel.transform.position;
            temp.x += _forwardSpeed * Time.deltaTime;
            _squirrel.transform.position = temp;
            if (_didFly)
            {
                _didFly = false;
                _rigidbody2D.velocity = new Vector2(0f, _bounceSpeed);
                _animator.SetTrigger("Fly");
                SoundsControllerScript.Instance.PlayFlySound();
            }
            if (_rigidbody2D.velocity.y >= 0)
            {
                _squirrel.transform.rotation = Quaternion.identity;
            }
            else
            {
                var angle = 0f;
                angle = Mathf.Lerp(0f, -90f, -_rigidbody2D.velocity.y / 20f);
                _squirrel.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }
    }

    private void Start()
    {
        switch (GameControllerScript.Instance.SelectedSquirrel)
        {
            case GameControllerScript.Squirrels.Brown:
                _squirrel = _brownSquirrel;
                break;
            case GameControllerScript.Squirrels.Red:
                _squirrel = _redSquirrel;
                break;
            case GameControllerScript.Squirrels.White:
            default:
                _squirrel = _whiteSquirrel;
                break;
        }
        _squirrel.SetActive(true);
        _rigidbody2D = _squirrel.GetComponent<Rigidbody2D>();
        _animator = _squirrel.GetComponent<Animator>();
        _camera = Camera.main;
        _cameraOffsetX = (_camera.transform.position.x - _squirrel.transform.position.x) - 1f;
        StartCoroutine(MusicControllerScript.Instance.FadeIn(_backgroundMusic, 0.5f));
        StartCoroutine(ScenesControllerScript.Instance.FadeInScene(0.7f));
    }

    private void Update()
    {
        if (_alive)
        {
            var temp = _camera.transform.position;
            temp.x = _squirrel.transform.position.x + _cameraOffsetX;
            _camera.transform.position = temp;
        }
    }

    private void MakeInstance()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PauseButton()
    {
        if (MusicControllerScript.Instance.On)
        {
            _musicOffImage.SetActive(false);
        }
        else
        {
            _musicOffImage.SetActive(true);
        }
        if (SoundsControllerScript.Instance.On)
        {
            _soundsOffImage.SetActive(false);
        }
        else
        {
            _soundsOffImage.SetActive(true);
        }
        _scoreText.text = _score.ToString("N0");
        SoundsControllerScript.Instance.PlayGuiClickSound();
        StartCoroutine(MusicControllerScript.Instance.FadeOut(0.5f));
        _pausePanel.SetActive(true);
    }

    public void FlyButton()
    {
        _didFly = true;
    }

    public void ResumeButton()
    {
        SoundsControllerScript.Instance.PlayGuiClickSound();
        StartCoroutine(MusicControllerScript.Instance.FadeIn(_backgroundMusic, 0.5f));
        _pausePanel.SetActive(false);
    }

    public void RestartButton()
    {
        SoundsControllerScript.Instance.PlayGuiClickSound();
        StartCoroutine(MusicControllerScript.Instance.FadeOut(0.5f));
        StartCoroutine(ScenesControllerScript.Instance.LoadScene("Play"));
    }

    public void ExitButton()
    {
        SoundsControllerScript.Instance.PlayGuiClickSound();
        StartCoroutine(MusicControllerScript.Instance.FadeOut(0.2f));
        StartCoroutine(ScenesControllerScript.Instance.FadeOutScene(0.2f));
        Application.Quit();
    }

    public void MusicButton()
    {
        SoundsControllerScript.Instance.PlayGuiClickSound();
        MusicControllerScript.Instance.On = !MusicControllerScript.Instance.On;
        if (MusicControllerScript.Instance.On)
        {
            _musicOffImage.SetActive(false);
        }
        else
        {
            _musicOffImage.SetActive(true);
        }
    }

    public void SoundsButton()
    {
        SoundsControllerScript.Instance.PlayGuiClickSound();
        SoundsControllerScript.Instance.On = !SoundsControllerScript.Instance.On;
        if (SoundsControllerScript.Instance.On)
        {
            _soundsOffImage.SetActive(false);
        }
        else
        {
            _soundsOffImage.SetActive(true);
        }
    }
}