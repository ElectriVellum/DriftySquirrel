using System;
using System.Collections;
#if UNITY_IOS
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
    private GameObject _readyPanel;
    [SerializeField()]
    private Button _readyButton;
    [SerializeField()]
    private GameObject _readyImage;
    [SerializeField()]
    private GameObject _threeImage;
    [SerializeField()]
    private GameObject _twoImage;
    [SerializeField()]
    private GameObject _oneImage;
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
    private GameObject _hudPanel;
    [SerializeField()]
    private Text _hudAcornsText;
    [SerializeField()]
    private Text _hudScoreText;
    [SerializeField()]
    private Text _hudTimeText;
    [SerializeField()]
    private GameObject _informationPanel;
    [SerializeField()]
    private Text _informationPanelLabel;
    [SerializeField()]
    private GameObject _resumeButton;
    [SerializeField()]
    private GameObject _continueWithAdButton;
    [SerializeField()]
    private GameObject _continueWithFacebookButton;
    [SerializeField()]
    private Text _acornsText;
    [SerializeField()]
    private Text _scoreText;
    [SerializeField()]
    private Text _timeText;
    [SerializeField()]
    private SquirrelScript _squirrel;

    private bool _continueFinished;
    private bool _playedReady;

    public PlayControllerScript()
    {
        _backgroundMusic = null;
        _pauseButton = null;
        _readyPanel = null;
        _readyImage = null;
        _threeImage = null;
        _twoImage = null;
        _oneImage = null;
        _musicOffImage = null;
        _soundsOffImage = null;
        _acorns = 0;
        _score = 0;
        _time = 0f;
        _hudPanel = null;
        _hudAcornsText = null;
        _hudScoreText = null;
        _hudTimeText = null;
        _informationPanel = null;
        _informationPanelLabel = null;
        _resumeButton = null;
        _continueWithAdButton = null;
        _continueWithFacebookButton = null;
        _acornsText = null;
        _scoreText = null;
        _timeText = null;
        _squirrel = null;


        _continueFinished = false;
        _playedReady = false;
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
        AddAcorns(GameControllerScript.Instance.Acorns);
        _squirrel.OnDieEvent += _squirrel_OnDieEvent;
        MusicControllerScript.Instance.FadeIn(_backgroundMusic);
        StartCoroutine(UpdateTime());
        if (_continueScore > 0)
        {
            _continueCount++;
            AddScore(_continueScore);
            AddTime(_continueTime);
            _continueScore = 0;
            _continueTime = 0f;
        }
        GameControllerScript.Instance.PlayCount += 1;
        if (GameControllerScript.Instance.PlayCount > 5)
        {
            if (UnityEngine.Random.Range(0,101) <= 60)
            {
                NonRewardedAd();
            }
        }
        _readyPanel.SetActive(true);
    }

    private void Update()
    {
        if (_continueFinished)
        {
            _continueFinished = false;
            _continueScore = _score;
            _continueTime = _time;
            GameControllerScript.Instance.ReportAcorns(_acorns);
            MusicControllerScript.Instance.FadeOut();
            ScenesControllerScript.Instance.LoadScene("Play");
        }
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
            var timeString = time.Hours.ToString() + ":" + time.Minutes.ToString() + ":" + time.Seconds.ToString() + ":" + time.Milliseconds.ToString().PadLeft(2, '0').Substring(0, 2);
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

    public void ReadyButton()
    {
        if (!_playedReady)
        {
            _playedReady = true;
            StartCoroutine(Ready());
        }
    }

    private IEnumerator Ready()
    {
        _readyImage.SetActive(false);
        _threeImage.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        _threeImage.SetActive(false);
        _twoImage.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        _twoImage.SetActive(false);
        _oneImage.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        _oneImage.SetActive(false);
        _oneImage.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        _oneImage.SetActive(false);
        _readyPanel.SetActive(false);
        _hudPanel.SetActive(true);
        _squirrel.gameObject.SetActive(true);
        SoundsControllerScript.Instance.PlayGuiClickSound();
        Time.timeScale = 1f;
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
        _hudPanel.SetActive(false);
        _informationPanelLabel.text = "PAUSED";
        _resumeButton.SetActive(true);
        _continueWithAdButton.SetActive(false);
        _continueWithFacebookButton.SetActive(false);
        _informationPanel.SetActive(true);
    }

    public void JumpButton()
    {
        _squirrel.Jump();
    }

    public void ResumeButton()
    {
        SoundsControllerScript.Instance.PlayGuiClickSound();
        MusicControllerScript.Instance.FadeIn(_backgroundMusic);
        _informationPanel.SetActive(false);
        _hudPanel.SetActive(true);
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
            _continueFinished = true;
        }
    }

    public void RestartButton()
    {
        _continueCount = 0;
        _continueScore = 0;
        _continueTime = 0f;
        GameControllerScript.Instance.ReportAcorns(_acorns);
        GameControllerScript.Instance.ReportScore(_score);
        GameControllerScript.Instance.ReportTime(_time);
        SoundsControllerScript.Instance.PlayGuiClickSound();
        MusicControllerScript.Instance.FadeOut();
        ScenesControllerScript.Instance.LoadScene("Play");
    }

    public void HomeButton()
    {
        _continueCount = 0;
        _continueScore = 0;
        _continueTime = 0f;
        GameControllerScript.Instance.ReportAcorns(_acorns);
        GameControllerScript.Instance.ReportScore(_score);
        GameControllerScript.Instance.ReportTime(_time);
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

    public void ContinueWithFacebookButton()
    {
        //TODO: Continue with Facebook
    }

    private void AddAcorns(int acorns)
    {
        _acorns += acorns;
        _hudAcornsText.text = _acorns.ToString("N0");
        _acornsText.text = _acorns.ToString("N0");
        SoundsControllerScript.Instance.PlayDingSound();
    }

    private void AddScore(int score)
    {
        _score += score;
        _hudScoreText.text = _score.ToString("N0");
        _scoreText.text = _score.ToString("N0");
        SoundsControllerScript.Instance.PlayDingSound();
    }

    public void AddTime(float seconds)
    {
        _time += seconds;
    }

    private void Die()
    {
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
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
        MusicControllerScript.Instance.FadeOut();
        _hudPanel.SetActive(false);
        _informationPanelLabel.text = "GAME OVER";
        _resumeButton.SetActive(false);
        _continueWithAdButton.SetActive(true);
        _continueWithAdButton.GetComponent<Button>().interactable = _continueCount < 2 && Monetization.IsReady(GameControllerScript.ADS_REWARDED_PLACEMENTID);
        //TODO: Continue with Facebook
        //_continueWithFacebookButton.SetActive(true);
        //_continueWithFacebookButton.GetComponent<Button>().interactable = _continueCount < 2 && Monetization.IsReady(GameControllerScript.ADS_REWARDED_PLACEMENTID);
        _informationPanel.SetActive(true);
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