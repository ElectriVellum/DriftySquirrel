using SA.Foundation.Templates;
using SA.iOS.GameKit;
using UnityEngine;

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

    public const string LEADERBOARD_ID = "com.electrivellum.driftysquirrel.leaderboard";
    private const string MUSIC_ON = "Music On";
    private const string MUSIC_VOLUME = "Music Volume";
    private const string SOUNDS_ON = "Sounds On";
    private const string SOUNDS_VOLUME = "Sounds Volume";
    private const string HIGH_SCORE = "High Score";
    private const string SELECTED_SQUIRREL = "Selected Squirrel";
    private const string RED_SQUIRREL_UNLOCKED = "Red Squirrel";
    private const string WHITE_SQUIRREL_UNLOCKED = "White Squirrel";
    private const string REWARD_ACTIVE = "Reward Active";

    public enum Squirrels
    {
        Brown,
        Red,
        White,
    }

    public GameControllerScript()
    {
        _resetPlayerPrefs = false;
    }

    [SerializeField()]
    private bool _resetPlayerPrefs;

    private void Awake()
    {
        MakeSingleton();
        InitializePlayerPrefs();
    }

    private void Start()
    {
        ISN_GKLocalPlayer.Authenticate((SA_Result result) => {
            if (result.IsSucceeded)
            {
                Debug.Log("Authenticate is succeeded!");
                ISN_GKLocalPlayer player = ISN_GKLocalPlayer.LocalPlayer;
                Debug.Log(player.PlayerID);
                Debug.Log(player.Alias);
                Debug.Log(player.DisplayName);
                Debug.Log(player.Authenticated);
                Debug.Log(player.Underage);
            }
            else
            {
                Debug.Log("Authenticate is failed! Error with code: " + result.Error.Code + " and description: " + result.Error.Message);
            }
        });
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
        if (!PlayerPrefs.HasKey(MUSIC_ON))
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
            MusicOn = true;
            MusicVolume = 0.5f;
            SoundsOn = true;
            SoundsVolume = 0.75f;
            HighScore = 0;
            SelectedSquirrel = Squirrels.Brown;
            RedSquirrelUnlocked = false;
            WhiteSquirrelUnlocked = false;
            RewardActive = false;
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

    public Squirrels SelectedSquirrel
    {
        get
        {
            return (Squirrels)System.Enum.Parse(typeof(Squirrels), PlayerPrefs.GetString(SELECTED_SQUIRREL, "Brown"));
        }
        set
        {
            PlayerPrefs.SetString(SELECTED_SQUIRREL, value.ToString());
        }
    }

    public bool RedSquirrelUnlocked
    {
        get
        {
            return bool.Parse(PlayerPrefs.GetString(RED_SQUIRREL_UNLOCKED, "false"));
        }
        set
        {
            PlayerPrefs.SetString(RED_SQUIRREL_UNLOCKED, value.ToString());
        }
    }

    public bool WhiteSquirrelUnlocked
    {
        get
        {
            return bool.Parse(PlayerPrefs.GetString(WHITE_SQUIRREL_UNLOCKED, "false"));
        }
        set
        {
            PlayerPrefs.SetString(WHITE_SQUIRREL_UNLOCKED, value.ToString());
        }
    }

    public bool RewardActive
    {
        get
        {
            return bool.Parse(PlayerPrefs.GetString(REWARD_ACTIVE, "false"));
        }
        set
        {
            PlayerPrefs.SetString(REWARD_ACTIVE, value.ToString());
        }
    }

    public void ReportScore(int score)
    {
        ISN_GKScore scoreReporter = new ISN_GKScore(LEADERBOARD_ID);
        scoreReporter.Value = score;
        scoreReporter.Context = 1;

        scoreReporter.Report((result) => {
            if (result.IsSucceeded)
            {
                Debug.Log("Score Report Success");
            }
            else
            {
                Debug.Log("Score Report failed! Code: " + result.Error.Code + " Message: " + result.Error.Message);
            }
        });
        if (score > HighScore)
        {
            HighScore = score;
        }
    }
}