using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scenefader : MonoBehaviour
{
    [SerializeField] private float fadeTime;

    private Image fadeOutImage;

    public enum FadeDirection
    {
        In,
        Out
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        fadeOutImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Fade(FadeDirection _fadeDirection)
    {
        float _alpha = _fadeDirection == FadeDirection.Out ? 1 : 0;
        float _fadeEndValue = _fadeDirection == FadeDirection.Out ? 0 : 1;

        if (_fadeDirection == FadeDirection.Out)
        {
            while (_alpha >= _fadeEndValue)
            {
                SetColorImage(ref _alpha, _fadeDirection);
                yield return null;
            }

            fadeOutImage.enabled = false;
        }
        else
        {
            fadeOutImage.enabled = true;

            while (_alpha <= _fadeEndValue)
            {
                SetColorImage(ref _alpha, _fadeDirection);
                yield return null;
            }
        }
    }

    public IEnumerator FadeAndLoadScene(FadeDirection _fadeDirection, string _sceneToLoad)
    {
        fadeOutImage.enabled = true;

        yield return Fade(_fadeDirection);

        SceneManager.LoadScene(_sceneToLoad);
    }

    void SetColorImage(ref float _alpha, FadeDirection _fadeDirection)
    {
        fadeOutImage.color = new Color(fadeOutImage.color.r, fadeOutImage.color.g, fadeOutImage.color.b, _alpha);

        _alpha += Time.deltaTime * (1 / fadeTime) * (_fadeDirection == FadeDirection.Out ? -1 : 1);
    }
}
