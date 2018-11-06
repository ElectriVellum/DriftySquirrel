using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesControllerScript : MonoBehaviour
{
    private static ScenesControllerScript _instance;

    public static ScenesControllerScript Instance
    {
        get
        {
            return _instance;
        }
    }

    public delegate void FadeCompleteEventHandler();

    public delegate void LoadingStartedEventHandler();

    public delegate void LoadingCompleteEventHandler();

    public delegate void LoadingProgressChangedEventHandler();

    public FadeCompleteEventHandler FadeInComplete;

    public FadeCompleteEventHandler FadeOutComplete;

    public LoadingStartedEventHandler LoadingStarted;

    public LoadingCompleteEventHandler LoadingComplete;

    public LoadingProgressChangedEventHandler LoadingProgressChanged;

    protected void OnFadeInComplete()
    {
        if (FadeInComplete != null)
        {
            FadeInComplete.Invoke();
        }
    }

    protected void OnFadeOutComplete()
    {
        if (FadeOutComplete != null)
        {
            FadeOutComplete.Invoke();
        }
    }

    protected void OnLoadingStarted()
    {
        if (LoadingStarted != null)
        {
            LoadingStarted.Invoke();
        }
    }

    protected void OnLoadingComplete()
    {
        if (LoadingComplete != null)
        {
            LoadingComplete.Invoke();
        }
    }

    protected void OnLoadingProgressChanged()
    {
        if (LoadingProgressChanged != null)
        {
            LoadingProgressChanged.Invoke();
        }
    }

    [SerializeField()]
    private Canvas _canvas;
    [SerializeField()]
    private Image _panel;

    [SerializeField()]
    private Color _inColor;
    [SerializeField()]
    private Color _outColor;

    [SerializeField()]
    private bool _deactivateCanvas;

    [SerializeField()]
    private float _fadeInDuration;
    [SerializeField()]
    private float _fadeOutDuration;

    private bool _fading;
    private bool _loading;
    private float _progress;

    public bool Fading
    {
        get
        {
            return _fading;
        }
    }

    public bool Loading
    {
        get
        {
            return _loading;
        }
    }

    public float Progress
    {
        get
        {
            return _progress;
        }
    }

    public ScenesControllerScript()
    {
        _canvas = null;
        _panel = null;

        _inColor = new Color(0f, 0f, 0f, 0f);
        _outColor = new Color(0f, 0f, 0f, 1f);

        _deactivateCanvas = true;

        _fadeInDuration = 0.7f;
        _fadeOutDuration = 0.7f;

        _fading = false;
        _loading = false;
        _progress = 0f;
    }

    private void Awake()
    {
        MakeSingleton();
        SceneManager.sceneLoaded += SceneManager_SceneLoaded;
    }

    private void SceneManager_SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (loadSceneMode == LoadSceneMode.Single && _canvas != null)
        {
            _canvas.worldCamera = Camera.main;
        }
        if (_loading)
        {
            _loading = false;
        }
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

    public void LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName, loadSceneMode));
    }

    public void FadeInScene()
    {
        StartCoroutine(FadeInSceneCoroutine(_fadeInDuration));
    }

    public void FadeInScene(float duration)
    {
        StartCoroutine(FadeInSceneCoroutine(duration));
    }

    public void FadeOutScene()
    {
        StartCoroutine(FadeOutSceneCoroutine(_fadeOutDuration));
    }

    public void FadeOutScene(float duration)
    {
        StartCoroutine(FadeOutSceneCoroutine(duration));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        while (_loading)
        {
            yield return null;
        }
        OnLoadingStarted();
        yield return StartCoroutine(FadeOutSceneCoroutine(_fadeOutDuration));
        _loading = true;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        while (_loading)
        {
            if (Mathf.Abs(_progress - asyncLoad.progress) > Mathf.Epsilon)
            {
                _progress = asyncLoad.progress;
                OnLoadingProgressChanged();
            }
            yield return null;
        }
        if (Mathf.Abs(_progress) > Mathf.Epsilon)
        {
            _progress = 0f;
            OnLoadingProgressChanged();
        }
        yield return StartCoroutine(FadeInSceneCoroutine(_fadeInDuration));
        while (_fading)
        {
            yield return null;
        }
        OnLoadingComplete();
    }

    private IEnumerator FadeInSceneCoroutine(float duration)
    {
        while (_fading)
        {
            yield return null;
        }
        _fading = true;
        var elapsed = 0f;
        _panel.color = _outColor;
        while (elapsed < duration)
        {
            _panel.color = Color.Lerp(_outColor, _inColor, elapsed);
            yield return null;
            elapsed += Time.unscaledDeltaTime;
        }
        _panel.color = _inColor;
        if (_deactivateCanvas)
        {
            _canvas.gameObject.SetActive(false);
        }
        _fading = false;
        OnFadeInComplete();
    }

    private IEnumerator FadeOutSceneCoroutine(float duration)
    {
        while (_fading)
        {
            yield return null;
        }
        _fading = true;
        var elapsed = 0f;
        _panel.color = _inColor;
        if (_deactivateCanvas)
        {
            _canvas.gameObject.SetActive(true);
        }
        while (elapsed < duration)
        {
            _panel.color = Color.Lerp(_inColor, _outColor, elapsed);
            yield return null;
            elapsed += Time.unscaledDeltaTime;
        }
        _panel.color = _outColor;
        _fading = false;
        OnFadeOutComplete();
    }
}