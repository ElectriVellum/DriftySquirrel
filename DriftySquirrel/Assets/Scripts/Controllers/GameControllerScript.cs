#if UNITY_IOS
using SA.Foundation.Templates;
using SA.iOS.GameKit;
#endif
using UnityEngine;
using UnityEngine.Monetization;

public class GameControllerScript : MonoBehaviour
{
    private static GameControllerScript _instance;

    public static GameControllerScript Instance
    {
        get
        {
            return _instance;
        }
    }

    public const string IOS_ACORNS_LEADERBOARD_ID = "com.electrivellum.driftysquirrel.leaderboard.acorns";
    public const string IOS_SCORE_LEADERBOARD_ID = "com.electrivellum.driftysquirrel.leaderboard.score";
    public const string IOS_TIME_LEADERBOARD_ID = "com.electrivellum.driftysquirrel.leaderboard.time";
    public const string ADS_REWARDED_PLACEMENTID = "rewardedVideo";
    public const string ADS_NONREWARDED_PLACEMENTID = "video";
#if UNITY_IOS
    public const string ADS_GAMEID = "2881619";
#elif UNITY_ANDROID
    public const string ADS_GAMEID = "2881620";
#elif UNITY_EDITOR
    public const string ADS_GAMEID = "1111111";
#endif
    public const bool ADS_TESTMODE = false;
    private const string REVIEW_REQUESTED_VERSION = "Review Requested Version";
    private const string MUSIC_ON = "Music On";
    private const string MUSIC_VOLUME = "Music Volume";
    private const string SOUNDS_ON = "Sounds On";
    private const string SOUNDS_VOLUME = "Sounds Volume";
    private const string ACORNS = "Acorns";
    private const string HIGH_SCORE = "High Score";
    private const string BEST_TIME = "Best Time";
    private const string PLAY_COUNT = "Play Count";

    [SerializeField()]
    private bool _resetPlayerPrefs;

#if UNITY_IOS
    private ISN_GKLocalPlayer _localPlayer;

    public ISN_GKLocalPlayer LocalPlayer
    {
        get
        {
            return _localPlayer;
        }
    }
#endif

    public GameControllerScript()
    {
        _resetPlayerPrefs = false;
#if UNITY_IOS
        _localPlayer = null;
#endif
    }

    private void Awake()
    {
        MakeSingleton();
        InitializePlayerPrefs();
    }

    private void Start()
    {
        if (Monetization.isSupported)
        {
            Monetization.Initialize(ADS_GAMEID, ADS_TESTMODE);
        }
#if UNITY_IOS
        ISN_GKLocalPlayer.Authenticate((SA_Result result) =>
        {
            if (result.IsSucceeded)
            {
                _localPlayer = ISN_GKLocalPlayer.LocalPlayer;
            }
            else
            {
                _localPlayer = null;
            }
        });
#endif
    }

    private void MakeSingleton()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
    }

    public bool CheckFirstRun()
    {
        if (PlayerPrefs.HasKey(REVIEW_REQUESTED_VERSION))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void InitializePlayerPrefs()
    {
        if (CheckFirstRun() || _resetPlayerPrefs)
        {
            ReviewRequestedVersion = string.Empty;
            MusicOn = true;
            MusicVolume = 0.25f;
            SoundsOn = true;
            SoundsVolume = 0.5f;
            HighScore = 0;
            Acorns = 0;
            BestTime = 0f;
            PlayCount = 0;
        }
    }

    public string ReviewRequestedVersion
    {
        get
        {
            return PlayerPrefs.GetString(REVIEW_REQUESTED_VERSION, string.Empty);
        }
        set
        {
            PlayerPrefs.SetString(REVIEW_REQUESTED_VERSION, value);
        }
    }

    public bool MusicOn
    {
        get
        {
            return PlayerPrefs.GetInt(MUSIC_ON) != 0;
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt(MUSIC_ON, 1);
            }
            else
            {
                PlayerPrefs.SetInt(MUSIC_ON, 0);
            }
        }
    }

    public float MusicVolume
    {
        get
        {
            return PlayerPrefs.GetFloat(MUSIC_VOLUME);
        }
        set
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME, value);
        }
    }

    public bool SoundsOn
    {
        get
        {
            return PlayerPrefs.GetInt(SOUNDS_ON) != 0;
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt(SOUNDS_ON, 1);
            }
            else
            {
                PlayerPrefs.SetInt(SOUNDS_ON, 0);
            }
        }
    }

    public float SoundsVolume
    {
        get
        {
            return PlayerPrefs.GetFloat(SOUNDS_VOLUME);
        }
        set
        {
            PlayerPrefs.SetFloat(SOUNDS_VOLUME, value);
        }
    }

    public int Acorns
    {
        get
        {
            return PlayerPrefs.GetInt(ACORNS, 0);
        }
        set
        {
            PlayerPrefs.SetInt(ACORNS, value);
        }
    }

    public int HighScore
    {
        get
        {
            return PlayerPrefs.GetInt(HIGH_SCORE, 0);
        }
        set
        {
            PlayerPrefs.SetInt(HIGH_SCORE, value);
        }
    }

    public float BestTime
    {
        get
        {
            return PlayerPrefs.GetFloat(BEST_TIME, 0f);
        }
        set
        {
            PlayerPrefs.SetFloat(BEST_TIME, value);
        }
    }

    public int PlayCount
    {
        get
        {
            return PlayerPrefs.GetInt(PLAY_COUNT, 0);
        }
        set
        {
            PlayerPrefs.SetInt(PLAY_COUNT, value);
        }
    }

    public void ReportAcorns(int acorns)
    {
#if UNITY_IOS
        ISN_GKScore scoreReporter = new ISN_GKScore(IOS_ACORNS_LEADERBOARD_ID);
        scoreReporter.Value = acorns;
        scoreReporter.Context = 1;

        scoreReporter.Report((result) =>
        {
            if (result.IsSucceeded)
            {
                Debug.Log("Score Report Success");
            }
            else
            {
                Debug.Log("Score Report failed! Code: " + result.Error.Code + " Message: " + result.Error.Message);
            }
        });
#endif
        Acorns = acorns;
    }

    public void ReportScore(int score)
    {
#if UNITY_IOS
        ISN_GKScore scoreReporter = new ISN_GKScore(IOS_SCORE_LEADERBOARD_ID);
        scoreReporter.Value = score;
        scoreReporter.Context = 1;

        scoreReporter.Report((result) =>
        {
            if (result.IsSucceeded)
            {
                Debug.Log("Score Report Success");
            }
            else
            {
                Debug.Log("Score Report failed! Code: " + result.Error.Code + " Message: " + result.Error.Message);
            }
        });
#endif
        if (score > HighScore)
        {
            HighScore = score;
        }
    }

    public void ReportTime(float time)
    {
#if UNITY_IOS
        ISN_GKScore scoreReporter = new ISN_GKScore(IOS_TIME_LEADERBOARD_ID);
        scoreReporter.Value = (long)time * 100;
        scoreReporter.Context = 1;

        scoreReporter.Report((result) =>
        {
            if (result.IsSucceeded)
            {
                Debug.Log("Score Report Success");
            }
            else
            {
                Debug.Log("Score Report failed! Code: " + result.Error.Code + " Message: " + result.Error.Message);
            }
        });
#endif
        if (time > BestTime)
        {
            BestTime = time;
        }
    }
}