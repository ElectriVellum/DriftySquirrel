using UnityEngine;
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
        _clickSound = null;
        _musicOffImage = null;
        _soundsOffImage = null;
        _score = 0;
        _pausePanel = null;
        _scoreText = null;

    }

    [SerializeField()]
    private AudioClip _backgroundMusic;
    [SerializeField()]
    private AudioClip _clickSound;
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

    private void Awake()
    {
        MakeInstance();
    }

    private void Start()
    {
        StartCoroutine(MusicControllerScript.Instance.FadeIn(_backgroundMusic, 0.5f));
        StartCoroutine(ScenesControllerScript.Instance.FadeInScene(0.7f));
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
        SoundsControllerScript.Instance.PlaySound(_clickSound);
        StartCoroutine(MusicControllerScript.Instance.FadeOut(0.5f));
        _pausePanel.SetActive(true);
    }

    public void ResumeButton()
    {
        SoundsControllerScript.Instance.PlaySound(_clickSound);
        StartCoroutine(MusicControllerScript.Instance.FadeIn(_backgroundMusic, 0.5f));
        _pausePanel.SetActive(false);
    }

    public void RestartButton()
    {
        SoundsControllerScript.Instance.PlaySound(_clickSound);
        StartCoroutine(MusicControllerScript.Instance.FadeOut(0.5f));
        StartCoroutine(ScenesControllerScript.Instance.LoadScene("Play"));
    }

    public void ExitButton()
    {
        SoundsControllerScript.Instance.PlaySound(_clickSound);
        StartCoroutine(MusicControllerScript.Instance.FadeOut(0.2f));
        StartCoroutine(ScenesControllerScript.Instance.FadeOutScene(0.2f));
        Application.Quit();
    }

    public void MusicButton()
    {
        SoundsControllerScript.Instance.PlaySound(_clickSound);
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
        SoundsControllerScript.Instance.PlaySound(_clickSound);
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