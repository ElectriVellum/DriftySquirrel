using System;
using System.Collections;
#if UNITY_IOS
using SA.Foundation.Utility;
using SA.iOS.GameKit;
using SA.iOS.Social;
#endif
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

    [SerializeField()]
    private AudioClip _backgroundMusic;
    [SerializeField()]
    private GameObject _musicOffImage;
    [SerializeField()]
    private GameObject _soundsOffImage;
    [SerializeField()]
    private GameObject _scorePanel;
    [SerializeField()]
    private GameObject _helpPanel;
    [SerializeField()]
    private GameObject _instructions1Image;
    [SerializeField()]
    private GameObject _instructions2Image;
    [SerializeField()]
    private GameObject _instructions3Image;
    [SerializeField()]
    private Text _acornsText;
    [SerializeField()]
    private Text _highScoreText;
    [SerializeField()]
    private Text _bestTimeText;
    [SerializeField()]
    private Text _medalText;
    [SerializeField()]
    private GameObject _bronzeMedalImage;
    [SerializeField()]
    private GameObject _silverMedalImage;
    [SerializeField()]
    private GameObject _goldMedalImage;
    [SerializeField()]
    private GameObject _updatePanel;

    public MenuControllerScript()
    {
        _backgroundMusic = null;
        _musicOffImage = null;
        _soundsOffImage = null;
        _scorePanel = null;
        _acornsText = null;
        _highScoreText = null;
        _bestTimeText = null;
        _medalText = null;
        _bronzeMedalImage = null;
        _silverMedalImage = null;
        _goldMedalImage = null;
        _updatePanel = null;
    }

    private void Awake()
    {
        MakeInstance();
    }

    private void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine(AfterStartCoroutine());
        MusicControllerScript.Instance.FadeIn(_backgroundMusic);
        ScenesControllerScript.Instance.FadeInScene();
    }

    private IEnumerator AfterStartCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.1f);
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
#if UNITY_IOS
        SoundsControllerScript.Instance.PlayGuiClickSound();
        ISN_GKGameCenterViewController viewController = new ISN_GKGameCenterViewController();
        viewController.ViewState = ISN_GKGameCenterViewControllerState.Leaderboards;
        viewController.Show();
#endif
    }

    public void PlayButton()
    {
        Time.timeScale = 0f;
        SoundsControllerScript.Instance.PlayGuiClickSound();
        MusicControllerScript.Instance.FadeOut();
        ScenesControllerScript.Instance.LoadScene("Play");
    }

    public void ScoresButton()
    {
        var acorns = GameControllerScript.Instance.Acorns;
        var highScore = GameControllerScript.Instance.HighScore;
        var bestTime = GameControllerScript.Instance.BestTime;
        _acornsText.text = acorns.ToString("N0") + " Acorns";
        _highScoreText.text = highScore.ToString("N0");
        var time = TimeSpan.FromSeconds(bestTime);
        var timeString = time.Hours.ToString() + ":" + time.Minutes.ToString() + ":" + time.Seconds.ToString() + ":" + time.Milliseconds.ToString().PadLeft(2, '0').Substring(0, 2);
        _bestTimeText.text = timeString;
        if (highScore <= 200)
        {
            _medalText.text = "No Medal Awarded";
            _goldMedalImage.SetActive(false);
            _silverMedalImage.SetActive(false);
            _bronzeMedalImage.SetActive(false);
        }
        else if (highScore <= 500)
        {
            _medalText.text = "Bronze Medal";
            _goldMedalImage.SetActive(false);
            _silverMedalImage.SetActive(false);
            _bronzeMedalImage.SetActive(true);
        }
        else if (highScore <= 1000)
        {
            _medalText.text = "Silver Medal";
            _goldMedalImage.SetActive(false);
            _silverMedalImage.SetActive(true);
            _bronzeMedalImage.SetActive(false);
        }
        else
        {
            _medalText.text = "Gold Medal";
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
#if UNITY_IOS
        ISN_GKGameCenterViewController viewController = new ISN_GKGameCenterViewController();
        viewController.ViewState = ISN_GKGameCenterViewControllerState.Leaderboards;
        viewController.LeaderboardIdentifier = GameControllerScript.IOS_SCORE_LEADERBOARD_ID;
        viewController.LeaderboardTimeScope = ISN_GKLeaderboardTimeScope.Today;
        viewController.Show();
#elif UNITY_ANDROID
        Social.ShowLeaderboardUI();
#endif
    }

    public void HelpButton()
    {
        SoundsControllerScript.Instance.PlayGuiClickSound();
        _helpPanel.SetActive(true);
    }

    public void InstructionsButton()
    {
        SoundsControllerScript.Instance.PlayGuiClickSound();
        if (_instructions1Image.activeInHierarchy)
        {
            _instructions1Image.SetActive(false);
            _instructions2Image.SetActive(true);
        }
        else if (_instructions2Image.activeInHierarchy)
        {
            _instructions2Image.SetActive(false);
            _instructions3Image.SetActive(true);
        }
        else
        {
            _instructions3Image.SetActive(false);
            _instructions1Image.SetActive(true);
            _helpPanel.SetActive(false);
        }
    }

    public void FacebookButton()
    {
#if UNITY_IOS
        SA_ScreenUtil.TakeScreenshot((image) =>
        {
            Debug.Log("Image Ready");
            ISN_Facebook.Post("I am passing time on Drifty Squirrel with a high score of " + GameControllerScript.Instance.HighScore.ToString("N0") + " points. Check it out!", image, (result) =>
            {
                Debug.Log("Post result: " + result.IsSucceeded);
            });
        });
#endif
    }

    public void ExitButton()
    {
        SoundsControllerScript.Instance.PlayGuiClickSound();
        MusicControllerScript.Instance.FadeOut(0.2f);
        ScenesControllerScript.Instance.FadeOutScene(0.2f);
        StartCoroutine(ExitCoroutine());
    }

    private IEnumerator ExitCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.2f);
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