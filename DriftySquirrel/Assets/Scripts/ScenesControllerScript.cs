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

    private bool _loading;
    private float _progress;

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
        StartCoroutine(FadeOutScene(0.7f));
        yield return new WaitForSeconds(0.7f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            _progress = asyncLoad.progress;
            yield return null;
        }
        StartCoroutine(FadeInScene(0.7f));
        _loading = false;
        _progress = 0f;
    }

    public IEnumerator LoadScene(string sceneName, LoadSceneMode loadSceneMode)
    {
        _loading = true;
        StartCoroutine(FadeOutScene(0.7f));
        yield return new WaitForSeconds(0.7f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        while (!asyncLoad.isDone)
        {
            _progress = asyncLoad.progress;
            yield return null;
        }
        StartCoroutine(FadeInScene(0.7f));
        _loading = false;
        _progress = 0f;
    }

    public IEnumerator LoadScene(string sceneName, float fadeOutDuration, float fadeInDuration)
    {
        _loading = true;
        StartCoroutine(FadeOutScene(fadeOutDuration));
        yield return new WaitForSeconds(fadeOutDuration);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            _progress = asyncLoad.progress;
            yield return null;
        }
        StartCoroutine(FadeInScene(fadeInDuration));
        _loading = false;
        _progress = 0f;
    }

    public IEnumerator LoadScene(string sceneName, LoadSceneMode loadSceneMode, float fadeOutDuration, float fadeInDuration)
    {
        _loading = true;
        StartCoroutine(FadeOutScene(fadeOutDuration));
        yield return new WaitForSeconds(fadeOutDuration);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        while (!asyncLoad.isDone)
        {
            _progress = asyncLoad.progress;
            yield return null;
        }
        StartCoroutine(FadeInScene(fadeInDuration));
        _loading = false;
        _progress = 0f;
    }

    public IEnumerator FadeInScene(float duration)
    {
        var elapsed = 0f;
        _panel.color = _outColor;
        while (elapsed < duration)
        {
            _panel.color = Color.Lerp(_outColor, _inColor, elapsed);
            yield return null;
            elapsed += Time.deltaTime;
        }
        _panel.color = _inColor;
        if (_deactivateCanvas)
        {
            _canvas.gameObject.SetActive(false);
        }
    }

    public IEnumerator FadeOutScene(float duration)
    {
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
            elapsed += Time.deltaTime;
        }
        _panel.color = _outColor;
    }
}