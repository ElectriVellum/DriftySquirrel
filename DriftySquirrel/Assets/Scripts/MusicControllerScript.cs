using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicControllerScript : MonoBehaviour
{
    private static MusicControllerScript _instance;

    public static MusicControllerScript Instance
    {
        get
        {
            return _instance;
        }
    }

    private AudioSource _audioSource;
    private bool _fading;

    [SerializeField()]
    [Range(0f, 1f)]
    private float _maximumVolume;

    [SerializeField()]
    private float _fadeDuration;

    public MusicControllerScript()
    {
        _audioSource = null;
        _fading = false;
        _maximumVolume = 0.5f;
        _fadeDuration = 0.5f;
    }

    private void Awake()
    {
        MakeSingleton();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        On = GameControllerScript.Instance.MusicOn;
        Volume = GameControllerScript.Instance.MusicVolume;
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
            GameControllerScript.Instance.MusicOn = value;
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
            GameControllerScript.Instance.MusicVolume = value;
            _audioSource.volume = _maximumVolume * Mathf.Clamp01(value);
        }
    }

    public void FadeIn(AudioClip audioClip)
    {
        StartCoroutine(FadeInCoroutine(audioClip, _fadeDuration));
    }

    public void FadeIn(AudioClip audioClip, float duration)
    {
        StartCoroutine(FadeInCoroutine(audioClip, duration));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine(_fadeDuration));
    }

    public void FadeOut(float duration)
    {
        StartCoroutine(FadeOutCoroutine(duration));
    }

    private IEnumerator FadeInCoroutine(AudioClip audioClip, float duration)
    {
        while (_fading)
        {
            yield return null;
        }
        _fading = true;
        var startVolume = 0.2f;
        var fullVolume = _maximumVolume * Mathf.Clamp01(GameControllerScript.Instance.MusicVolume);
        _audioSource.volume = 0f;
        _audioSource.clip = audioClip;
        _audioSource.Play();
        while (_audioSource.volume < fullVolume)
        {
            _audioSource.volume += startVolume * Time.unscaledDeltaTime / duration;
            yield return null;
        }
        if (Mathf.Abs(_audioSource.volume - fullVolume) > Mathf.Epsilon)
        {
            _audioSource.volume = fullVolume;
        }
        _fading = false;
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        while (_fading)
        {
            yield return null;
        }
        _fading = true;
        float startVolume = _audioSource.volume;

        while (_audioSource.volume > 0f)
        {
            _audioSource.volume -= startVolume * Time.unscaledDeltaTime / duration;
            yield return null;
        }
        _audioSource.Stop();
        _audioSource.clip = null;
        _audioSource.volume = startVolume;
        _fading = false;
    }
}