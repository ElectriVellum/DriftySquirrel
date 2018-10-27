using UnityEngine;

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
    }

    [SerializeField()]
    private AudioClip _backgroundMusic;
    [SerializeField()]
    private AudioClip _clickSound;

    private void Awake()
    {
        MakeInstance();
    }

    private void Start()
    {
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
}