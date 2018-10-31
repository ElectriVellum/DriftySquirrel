using System.Collections;
using SA.iOS.GameKit;
using UnityEngine;
using UnityEngine.UI;

public class MenuControllerScript : MonoBehaviour
{
    private static MenuControllerScript _instance;

    public static MenuControllerScript Instance
    {
        get
        {
            return _instance;
        }
    }

    public MenuControllerScript()
    {
        _backgroundMusic = null;
        _musicOffImage = null;
        _soundsOffImage = null;
        _scorePanel = null;
        _highScoreText = null;
        _medalText = null;
        _bronzeMedalImage = null;
        _silverMedalImage = null;
        _goldMedalImage = null;
        _player = null;
    }

    [SerializeField()]
    private AudioClip _backgroundMusic;
    [SerializeField()]
    private GameObject _musicOffImage;
    [SerializeField()]
    private GameObject _soundsOffImage;
    [SerializeField()]
    private GameObject _scorePanel;
    [SerializeField()]
    private Text _highScoreText;
    [SerializeField()]
    private Text _medalText;
    [SerializeField()]
    private GameObject _bronzeMedalImage;
    [SerializeField()]
    private GameObject _silverMedalImage;
    [SerializeField()]
    private GameObject _goldMedalImage;
    [SerializeField()]
    private GameObject _player;

    private void Awake()
    {
        MakeInstance();
    }

    private void Start()
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
        StartCoroutine(ShowPlayer());
        StartCoroutine(MusicControllerScript.Instance.FadeIn(_backgroundMusic, 0.5f));
        StartCoroutine(ScenesControllerScript.Instance.FadeInScene(0.7f));
    }

    private IEnumerator ShowPlayer()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        _player.SetActive(true);
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

    public void GameCenterButton()
    {
        SoundsControllerScript.Instance.PlayGuiClickSound();
        ISN_GKGameCenterViewController viewController = new ISN_GKGameCenterViewController();
        viewController.ViewState = ISN_GKGameCenterViewControllerState.Leaderboards;
        viewController.Show();
    }

    public void PlayButton()
    {
        Time.timeScale = 0f;
        SoundsControllerScript.Instance.PlayGuiClickSound();
        StartCoroutine(MusicControllerScript.Instance.FadeOut(0.5f));
        StartCoroutine(ScenesControllerScript.Instance.LoadScene("Play"));
    }

    public void ScoresButton()
    {
        var highScore = GameControllerScript.Instance.HighScore;
        _highScoreText.text = highScore.ToString("N0");
        if (highScore <= 10)
        {
            _medalText.text = "None";
            _goldMedalImage.SetActive(false);
            _silverMedalImage.SetActive(false);
            _bronzeMedalImage.SetActive(false);
        }
        else if (highScore <= 20)
        {
            _medalText.text = "Bronze";
            _goldMedalImage.SetActive(false);
            _silverMedalImage.SetActive(false);
            _bronzeMedalImage.SetActive(true);
        }
        else if (highScore <= 40)
        {
            _medalText.text = "Silver";
            _goldMedalImage.SetActive(false);
            _silverMedalImage.SetActive(true);
            _bronzeMedalImage.SetActive(false);
        }
        else
        {
            _medalText.text = "Gold";
            _goldMedalImage.SetActive(true);
            _silverMedalImage.SetActive(false);
            _bronzeMedalImage.SetActive(false);
        }
        SoundsControllerScript.Instance.PlayGuiClickSound();
        _scorePanel.SetActive(true);
    }

    public void LeaderboardButton()
    {
        SoundsControllerScript.Instance.PlayGuiClickSound();
        ISN_GKGameCenterViewController viewController = new ISN_GKGameCenterViewController();
        viewController.ViewState = ISN_GKGameCenterViewControllerState.Leaderboards;
        viewController.LeaderboardIdentifier = GameControllerScript.LEADERBOARD_ID;
        viewController.LeaderboardTimeScope = ISN_GKLeaderboardTimeScope.Today;
        viewController.Show();
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

    public void ScorePanelHomeButton()
    {
        SoundsControllerScript.Instance.PlayGuiClickSound();
        _scorePanel.SetActive(false);
    }
}