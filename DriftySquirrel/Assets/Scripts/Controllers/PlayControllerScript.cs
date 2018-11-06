using System;
using System.Collections;
#if UNITY_IOS
using SA.Foundation.Utility;
using SA.iOS.Social;
using SA.iOS.StoreKit;
#endif
using UnityEngine;
using UnityEngine.Monetization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayControllerScript : MonoBehaviour
{
    private static int _continueCount;
    private static int _continueScore;
    private static float _continueTime;
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
    private int _acorns;
    [SerializeField()]
    private int _score;
    [SerializeField()]
    private float _time;
    [SerializeField()]
    private GameObject _hudAcornsLabel;
    [SerializeField()]
    private Text _hudAcornsText;
    [SerializeField()]
    private GameObject _hudScoreLabel;
    [SerializeField()]
    private Text _hudScoreText;
    [SerializeField()]
    private GameObject _hudTimeLabel;
    [SerializeField()]
    private Text _hudTimeText;
    [SerializeField()]
    private GameObject _uiPanel;
    [SerializeField()]
    private GameObject _pausedLabel;
    [SerializeField()]
    private GameObject _gameOverLabel;
    [SerializeField()]
    private GameObject _resumeButton;
    [SerializeField()]
    private GameObject _continueButton;
    [SerializeField()]
    private Text _acornsText;
    [SerializeField()]
    private Text _scoreText;
    [SerializeField()]
    private Text _timeText;
    [SerializeField()]
    private SquirrelScript _squirrel;

    private bool _adFinished;

    public PlayControllerScript()
    {
        _backgroundMusic = null;
        _pauseButton = null;
        _instructionPanel = null;
        _musicOffImage = null;
        _soundsOffImage = null;
        _acorns = 0;
        _score = 0;
        _time = 0f;
        _hudAcornsLabel = null;
        _hudAcornsText = null;
        _hudScoreLabel = null;
        _hudScoreText = null;
        _hudTimeLabel = null;
        _hudTimeText = null;
        _uiPanel = null;
        _pausedLabel = null;
        _gameOverLabel = null;
        _resumeButton = null;
        _continueButton = null;
        _acornsText = null;
        _scoreText = null;
        _timeText = null;
        _squirrel = null;

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

    private void Start()
    {
        _squirrel.OnRunStartEvent += _squirrel_OnRunStartEvent;
        _squirrel.OnAttackEvent += _squirrel_OnAttackEvent;
        _squirrel.OnJumpEvent += _squirrel_OnJumpEvent;
        _squirrel.OnDriftEvent += _squirrel_OnDriftEvent;
        _squirrel.OnDuckEvent += _squirrel_OnDuckEvent;
        _squirrel.OnStunEvent += _squirrel_OnStunEvent;
        _squirrel.OnDieEvent += _squirrel_OnDieEvent;

        StartCoroutine(UpdateTime());
        if (_continueScore > 0)
        {
            _continueCount++;
            AddScore(_continueScore);
            AddTime(_continueTime);
            _continueScore = 0;
            _continueTime = 0f;
        }
    }

    private void _squirrel_OnRunStartEvent()
    {
    }

    private void _squirrel_OnAttackEvent()
    {
    }

    private void _squirrel_OnJumpEvent()
    {
    }

    private void _squirrel_OnDriftEvent()
    {
    }

    private void _squirrel_OnDuckEvent()
    {
    }

    private void _squirrel_OnStunEvent()
    {
    }

    private void _squirrel_OnDieEvent(SquirrelScript.DieReason reason)
    {
        Die();
    }

    private IEnumerator UpdateTime()
    {
        while (true)
        {
            var time = TimeSpan.FromSeconds(_time);
            var timeString = time.Hours.ToString() + ":" + time.Minutes.ToString() + ":" + time.Seconds.ToString() + ":" + time.Milliseconds.ToString().PadLeft(2, ' ').Substring(0, 2);
            _hudTimeText.text = timeString;
            _timeText.text = timeString;
            yield return new WaitForSeconds(0.01f);
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
        _hudAcornsLabel.gameObject.SetActive(true);
        _hudAcornsText.gameObject.SetActive(true);
        _hudScoreLabel.gameObject.SetActive(true);
        _hudScoreText.gameObject.SetActive(true);
        _hudTimeLabel.gameObject.SetActive(true);
        _hudTimeText.gameObject.SetActive(true);
        _squirrel.gameObject.SetActive(true);
        Time.timeScale = 1f;
        _squirrel.Jump();
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
        _pauseButton.SetActive(false);
        _hudAcornsLabel.gameObject.SetActive(false);
        _hudAcornsText.gameObject.SetActive(false);
        _hudScoreLabel.gameObject.SetActive(false);
        _hudScoreText.gameObject.SetActive(false);
        _hudTimeLabel.gameObject.SetActive(false);
        _hudTimeText.gameObject.SetActive(false);
        _pausedLabel.SetActive(true);
        _gameOverLabel.SetActive(false);
        _resumeButton.SetActive(true);
        _continueButton.SetActive(false);
        _uiPanel.SetActive(true);
    }

    public void JumpButton()
    {
        _squirrel.Jump();
    }

    public void ResumeButton()
    {
        SoundsControllerScript.Instance.PlayGuiClickSound();
        MusicControllerScript.Instance.FadeIn(_backgroundMusic);
        _uiPanel.SetActive(false);
        _pauseButton.SetActive(true);
        _hudAcornsLabel.gameObject.SetActive(true);
        _hudAcornsText.gameObject.SetActive(true);
        _hudScoreLabel.gameObject.SetActive(true);
        _hudScoreText.gameObject.SetActive(true);
        _hudTimeLabel.gameObject.SetActive(true);
        _hudTimeText.gameObject.SetActive(true);
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
        _continueTime = 0f;
        GameControllerScript.Instance.ReportScore(_score);
        SoundsControllerScript.Instance.PlayGuiClickSound();
        MusicControllerScript.Instance.FadeOut();
        ScenesControllerScript.Instance.LoadScene("Play");
    }

    public void HomeButton()
    {
        _continueCount = 0;
        _continueScore = 0;
        _continueTime = 0f;
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

    public void FacebookButton()
    {
#if UNITY_IOS
        SA_ScreenUtil.TakeScreenshot((image) =>
        {
            Debug.Log("Image Ready");
            ISN_Facebook.Post("I am passing time on Drifty Squirrel with " + _score.ToString("N0") + " points. Check it out!", image, (result) =>
            {
                Debug.Log("Post result: " + result.IsSucceeded);
            });
        });
#endif
    }

    public void AddAcorns(int acorns)
    {
        _acorns += acorns;
        _hudAcornsText.text = _score.ToString("N0");
        _acornsText.text = _score.ToString("N0");
        SoundsControllerScript.Instance.PlayPingSound();
    }

    public void AddScore(int score)
    {
        _score += score;
        _hudScoreText.text = _score.ToString("N0");
        _scoreText.text = _score.ToString("N0");
        SoundsControllerScript.Instance.PlayPingSound();
    }

    public void AddTime(float seconds)
    {
        _time += seconds;
    }

    public void Die()
    {
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(0.8f);
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
        _pauseButton.SetActive(false);
        _hudAcornsLabel.gameObject.SetActive(false);
        _hudAcornsText.gameObject.SetActive(false);
        _hudScoreLabel.gameObject.SetActive(false);
        _hudScoreText.gameObject.SetActive(false);
        _hudTimeLabel.gameObject.SetActive(false);
        _hudTimeText.gameObject.SetActive(false);
        _pausedLabel.SetActive(false);
        _gameOverLabel.SetActive(true);
        _resumeButton.SetActive(false);
        _continueButton.SetActive(true);
        _continueButton.GetComponent<Button>().interactable = _continueCount < 2 && Monetization.IsReady(GameControllerScript.ADS_REWARDED_PLACEMENTID);
        _uiPanel.SetActive(true);
#if UNITY_IOS
        yield return new WaitForSeconds(2F);
        if (GameControllerScript.Instance.ReviewRequestedVersion != Application.version.ToString())
        {
            ISN_SKStoreReviewController.RequestReview();
            GameControllerScript.Instance.ReviewRequestedVersion = Application.version.ToString();
        }
#endif
    }

    public void Collect(CollectibleScript collectible)
    {
        if (collectible != null)
        {
            AddAcorns(collectible.AcornsCount);
            AddScore(collectible.ScoreCount);
        }
    }
}