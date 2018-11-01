using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Monetization;

public class PlayControllerScript : MonoBehaviour
{
    private static int _continueCount;
    private static int _continueScore;
    private static PlayControllerScript _instance;

    public static PlayControllerScript Instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField()]
    private AudioClip _backgroundMusic;
    [SerializeField()]
    private GameObject _pauseButton;
    [SerializeField()]
    private GameObject _instructionPanel;
    [SerializeField()]
    private GameObject _musicOffImage;
    [SerializeField()]
    private GameObject _soundsOffImage;
    [SerializeField()]
    private int _score;
    [SerializeField()]
    private Text _hudScoreText;
    [SerializeField()]
    private GameObject _endPanel;
    [SerializeField()]
    private GameObject _pausedLabel;
    [SerializeField()]
    private GameObject _gameOverLabel;
    [SerializeField()]
    private GameObject _resumeButton;
    [SerializeField()]
    private GameObject _continueButton;
    [SerializeField()]
    private Text _scoreText;
    [SerializeField()]
    private GameObject _brownSquirrel;
    [SerializeField()]
    private GameObject _redSquirrel;
    [SerializeField()]
    private GameObject _whiteSquirrel;
    [SerializeField()]
    private Transform _treeSpawner;
    [SerializeField()]
    private GameObject _treePrefab;

    private GameObject _squirrel;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private Camera _camera;
    private float _cameraOffsetX;

    private float _forwardSpeed;
    private float _bounceSpeed;
    private bool _didFly;
    private bool _alive;

    private bool _adFinished;

    public PlayControllerScript()
    {
        _backgroundMusic = null;
        _pauseButton = null;
        _instructionPanel = null;
        _musicOffImage = null;
        _soundsOffImage = null;
        _score = 0;
        _hudScoreText = null;
        _endPanel = null;
        _pausedLabel = null;
        _gameOverLabel = null;
        _resumeButton = null;
        _continueButton = null;
        _scoreText = null;
        _brownSquirrel = null;
        _redSquirrel = null;
        _whiteSquirrel = null;
        _treeSpawner = null;
        _treePrefab = null;

        _squirrel = null;
        _rigidbody2D = null;
        _animator = null;

        _camera = null;
        _cameraOffsetX = 0f;

        _forwardSpeed = 3f;
        _bounceSpeed = 5f;
        _didFly = false;
        _alive = true;
        _adFinished = false;
    }

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
                angle = Mathf.Lerp(0f, -90f, -_rigidbody2D.velocity.y / 16f);
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
        _rigidbody2D = _squirrel.GetComponent<Rigidbody2D>();
        _animator = _squirrel.GetComponent<Animator>();
        _camera = Camera.main;
        _cameraOffsetX = (_camera.transform.position.x - _squirrel.transform.position.x);
        MusicControllerScript.Instance.FadeIn(_backgroundMusic);
        if (Random.Range(0, 100) <= 25 && _continueScore == 0)
        {
            NonRewardedAd();
        }
    }

    private void Update()
    {
        if (_alive)
        {
            var temp = _camera.transform.position;
            temp.x = _squirrel.transform.position.x + _cameraOffsetX;
            _camera.transform.position = temp;
        }
        if (_adFinished)
        {
            _adFinished = false;
            _continueScore = _score;
            MusicControllerScript.Instance.FadeOut();
            ScenesControllerScript.Instance.LoadScene("Play");
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

    public void InstructionButton()
    {
        _instructionPanel.SetActive(false);
        _pauseButton.SetActive(true);
        _hudScoreText.gameObject.SetActive(true);
        _squirrel.SetActive(true);
        Time.timeScale = 1f;
        _didFly = true;
        if (_continueScore > 0)
        {
            _continueCount++;
            Score(_continueScore);
            _continueScore = 0;
        }
        StartCoroutine(GenerateTrees());
    }

    public void PauseButton()
    {
        Time.timeScale = 0f;
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
        SoundsControllerScript.Instance.PlayGuiClickSound();
        MusicControllerScript.Instance.FadeOut();
        _hudScoreText.gameObject.SetActive(false);
        _pausedLabel.SetActive(true);
        _gameOverLabel.SetActive(false);
        _resumeButton.SetActive(true);
        _continueButton.SetActive(false);
        _endPanel.SetActive(true);
    }

    public void FlyButton()
    {
        _didFly = true;
    }

    public void ResumeButton()
    {
        SoundsControllerScript.Instance.PlayGuiClickSound();
        MusicControllerScript.Instance.FadeIn(_backgroundMusic);
        _endPanel.SetActive(false);
        _hudScoreText.gameObject.SetActive(true);
        Time.timeScale = 1f;
    }

    public void ContinueButton()
    {
        SoundsControllerScript.Instance.PlayGuiClickSound();
        RewardedAd();
    }

    public void NonRewardedAd()
    {
        if (Monetization.isInitialized)
        {
            if (Monetization.IsReady(GameControllerScript.ADS_NONREWARDED_PLACEMENTID))
            {
                var ad = Monetization.GetPlacementContent(GameControllerScript.ADS_NONREWARDED_PLACEMENTID) as ShowAdPlacementContent;
                if (ad != null)
                {
                    ad.Show(NonRewardedAdFinished);
                }
            }
        }
    }

    private void NonRewardedAdFinished(ShowResult result)
    {
    }

    private void RewardedAd()
    {
        if (Monetization.isInitialized)
        {
            if (Monetization.IsReady(GameControllerScript.ADS_REWARDED_PLACEMENTID))
            {
                var ad = Monetization.GetPlacementContent(GameControllerScript.ADS_REWARDED_PLACEMENTID) as ShowAdPlacementContent;
                if (ad != null)
                {
                    ad.Show(RewardedAdFinished);
                }
            }
        }
    }

    private void RewardedAdFinished(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            _adFinished = true;
        }
    }

    public void RestartButton()
    {
        _continueCount = 0;
        _continueScore = 0;
        GameControllerScript.Instance.ReportScore(_score);
        SoundsControllerScript.Instance.PlayGuiClickSound();
        MusicControllerScript.Instance.FadeOut();
        ScenesControllerScript.Instance.LoadScene("Play");
    }

    public void HomeButton()
    {
        _continueCount = 0;
        _continueScore = 0;
        GameControllerScript.Instance.ReportScore(_score);
        SoundsControllerScript.Instance.PlayGuiClickSound();
        MusicControllerScript.Instance.FadeOut();
        ScenesControllerScript.Instance.LoadScene("Menu");
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

    public void Score(int points)
    {
        _score += points;
        _hudScoreText.text = _score.ToString("N0");
        _scoreText.text = _score.ToString("N0");
        SoundsControllerScript.Instance.PlayPingSound();
    }

    public void Die()
    {
        Time.timeScale = 0f;
        _alive = false;
        SoundsControllerScript.Instance.PlayDieSound();
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
        SoundsControllerScript.Instance.PlayGuiClickSound();
        MusicControllerScript.Instance.FadeOut();
        _hudScoreText.gameObject.SetActive(false);
        _pausedLabel.SetActive(false);
        _gameOverLabel.SetActive(true);
        _resumeButton.SetActive(false);
        _continueButton.SetActive(true);
        _continueButton.GetComponent<Button>().interactable = _continueCount < 2 && Monetization.IsReady(GameControllerScript.ADS_REWARDED_PLACEMENTID);
        _endPanel.SetActive(true);
    }

    private IEnumerator GenerateTrees()
    {
        while (_alive)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            var treeScript = Instantiate(_treePrefab, _treeSpawner.position, Quaternion.identity, transform).GetComponent<TreeScript>();
            treeScript.Generate();
        }
    }
}