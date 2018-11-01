using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundsControllerScript : MonoBehaviour
{
    private static SoundsControllerScript _instance;

    public static SoundsControllerScript Instance
    {
        get
        {
            return _instance;
        }
    }

    private AudioSource _audioSource;

    [SerializeField()]
    [Range(0f, 1f)]
    private float _maximumVolume;

    [SerializeField()]
    private AudioClip _guiClickAudioClip;
    [SerializeField()]
    private AudioClip _flyAudioClip;
    [SerializeField()]
    private AudioClip _pingAudioClip;
    [SerializeField()]
    private AudioClip _dieAudioClip;

    public SoundsControllerScript()
    {
        _audioSource = null;

        _maximumVolume = 0.65f;

        _guiClickAudioClip = null;
        _flyAudioClip = null;
        _pingAudioClip = null;
        _dieAudioClip = null;
    }

    private void Awake()
    {
        MakeSingleton();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        On = GameControllerScript.Instance.SoundsOn;
        Volume = GameControllerScript.Instance.SoundsVolume;
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

    public bool On
    {
        get
        {
            return !_audioSource.mute;
        }
        set
        {
            GameControllerScript.Instance.SoundsOn = value;
            _audioSource.mute = !value;
        }
    }

    public float Volume
    {
        get
        {
            return _audioSource.volume / _maximumVolume;
        }
        set
        {
            GameControllerScript.Instance.SoundsVolume = value;
            _audioSource.volume = _maximumVolume * Mathf.Clamp01(value);
        }
    }

    public void PlaySound(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip);
    }

    public void PlaySound(AudioClip audioClip, float volumeScale)
    {
        _audioSource.PlayOneShot(audioClip, volumeScale);
    }

    public void PlaySound(AudioClip audioClip, float minimumPitch, float maximumPitch)
    {
        _audioSource.pitch = Random.Range(minimumPitch, maximumPitch);
        _audioSource.PlayOneShot(audioClip);
    }

    public void PlaySound(AudioClip audioClip, float volumeScale, float minimumPitch, float maximumPitch)
    {
        _audioSource.pitch = Random.Range(minimumPitch, maximumPitch);
        _audioSource.PlayOneShot(audioClip, volumeScale);
    }

    public void PlayGuiClickSound()
    {
        PlaySound(_guiClickAudioClip);
    }

    public void PlayFlySound()
    {
        PlaySound(_flyAudioClip);
    }

    public void PlayPingSound()
    {
        PlaySound(_pingAudioClip);
    }

    public void PlayDieSound()
    {
        PlaySound(_dieAudioClip);
    }
}