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

    public ScenesControllerScript()
    {
        _canvas = null;
        _panel = null;
        _inColor = new Color(0f, 0f, 0f, 0f);
        _outColor = new Color(0f, 0f, 0f, 1f);
        _deactivateCanvas = true;
        _fading = false;
        _loading = false;
        _progress = 0f;
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

    private bool _fading;
    private bool _loading;
    private float _progress;

    public float Progress
    {
        get
        {
            return _progress;
        }
    }

    private void Awake()
    {
        MakeSingleton();
        SceneManager.sceneLoaded += SceneManager_SceneLoaded;
    }

    private void SceneManager_SceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _canvas.worldCamera = Camera.main;
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

    public IEnumerator LoadScene(string sceneName)
    {
        _loading = true;
        yield return StartCoroutine(FadeOutScene(0.7f));
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            _progress = asyncLoad.progress;
            yield return null;
        }
        yield return StartCoroutine(FadeInScene(0.7f));
        _loading = false;
        _progress = 0f;
    }

    public IEnumerator LoadScene(string sceneName, LoadSceneMode loadSceneMode)
    {
        while (_loading)
        {
            yield return null;
        }
        _loading = true;
        yield return StartCoroutine(FadeOutScene(0.7f));
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        while (!asyncLoad.isDone)
        {
            _progress = asyncLoad.progress;
            yield return null;
        }
        yield return StartCoroutine(FadeInScene(0.7f));
        _loading = false;
        _progress = 0f;
    }

    public IEnumerator LoadScene(string sceneName, float fadeOutDuration, float fadeInDuration)
    {
        while (_loading)
        {
            yield return null;
        }
        _loading = true;
        yield return StartCoroutine(FadeOutScene(fadeOutDuration));
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            _progress = asyncLoad.progress;
            yield return null;
        }
        yield return StartCoroutine(FadeInScene(fadeInDuration));
        _loading = false;
        _progress = 0f;
    }

    public IEnumerator LoadScene(string sceneName, LoadSceneMode loadSceneMode, float fadeOutDuration, float fadeInDuration)
    {
        while (_loading)
        {
            yield return null;
        }
        _loading = true;
        yield return StartCoroutine(FadeOutScene(fadeOutDuration));
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        while (!asyncLoad.isDone)
        {
            _progress = asyncLoad.progress;
            yield return null;
        }
        yield return StartCoroutine(FadeInScene(fadeInDuration));
        _loading = false;
        _progress = 0f;
    }

    public IEnumerator FadeInScene(float duration)
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
    }

    public IEnumerator FadeOutScene(float duration)
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
    }
}