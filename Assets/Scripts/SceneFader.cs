using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeInDuration = 2.5f;
    public float fadeOutDuration = 2.5f;
    public float displayDuration = 4f;
    public float blackHoldDuration =2f;
    public string nextSceneName = "Cutscene_Intro";

    void Start()
    {
        StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        yield return StartCoroutine(Fade(1f, 0f, fadeInDuration)); 
        yield return new WaitForSeconds(displayDuration);
        yield return StartCoroutine(Fade(0f, 1f, fadeOutDuration)); 
        yield return new WaitForSeconds(blackHoldDuration); 
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color c = fadeImage.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            c.a = Mathf.Lerp(startAlpha, endAlpha, t);
            fadeImage.color = c;
            yield return null;
        }

        c.a = endAlpha;
        fadeImage.color = c;
    }

    void Update()
    {
        Debug.Log("Running.");   
    }
}
