//using SA.iOS.GameKit;
using UnityEngine;

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
    private AudioClip _clickSound;
    [SerializeField()]
    private GameObject _musicOffImage;
    [SerializeField()]
    private GameObject _soundsOffImage;

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
        StartCoroutine(MusicControllerScript.Instance.FadeIn(_backgroundMusic, 0.25f));
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

    public void GameCenterButton()
    {
        SoundsControllerScript.Instance.PlaySound(_clickSound);
        //ISN_GKGameCenterViewController viewController = new ISN_GKGameCenterViewController();
        //viewController.ViewState = ISN_GKGameCenterViewControllerState.Leaderboards;
        //viewController.Show();
    }

    public void PlayButton()
    {
        SoundsControllerScript.Instance.PlaySound(_clickSound);
        StartCoroutine(MusicControllerScript.Instance.FadeOut(0.25f));
        StartCoroutine(ScenesControllerScript.Instance.LoadScene("Play"));
    }

    public void ScoresButton()
    {
        SoundsControllerScript.Instance.PlaySound(_clickSound);
    }

    public void LeaderboardButton()
    {
        SoundsControllerScript.Instance.PlaySound(_clickSound);
        //ISN_GKGameCenterViewController viewController = new ISN_GKGameCenterViewController();
        //viewController.ViewState = ISN_GKGameCenterViewControllerState.Leaderboards;
        //viewController.LeaderboardIdentifier = GameControllerScript.LEADERBOARD_ID;
        //viewController.LeaderboardTimeScope = ISN_GKLeaderboardTimeScope.Today;
        //viewController.Show();
    }

    public void ExitButton()
    {
        SoundsControllerScript.Instance.PlaySound(_clickSound);
        StartCoroutine(MusicControllerScript.Instance.FadeOut(0.25f));
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