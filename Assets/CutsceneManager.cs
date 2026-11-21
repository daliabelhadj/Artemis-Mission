using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public GameObject[] frames;
    public Image fadePanel;
    public float[] displayTimes = { 4f, 4f, 4f, 5f };
    public float fadeSpeed = 1.5f;
    public string nextScene = "SampleScene";

    void Start()
    {
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        if (fadePanel == null) yield break;

        Color c = fadePanel.color;
        c.a = 1f;
        fadePanel.color = c;

        for (int i = 0; i < frames.Length; i++)
        {
            for (int j = 0; j < frames.Length; j++)
                frames[j].SetActive(j == i);

            yield return StartCoroutine(Fade(1f, 0f));

            yield return new WaitForSeconds(displayTimes[i]);

            yield return StartCoroutine(Fade(0f, 1f));
        }

        SceneManager.LoadScene(nextScene);
    }

    IEnumerator Fade(float fromAlpha, float toAlpha)
    {
        if (fadePanel == null) yield break;

        float elapsed = 0f;
        Color c = fadePanel.color;

        while (elapsed < fadeSpeed)
        {
            if (fadePanel == null) yield break;

            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(fromAlpha, toAlpha, elapsed / fadeSpeed);
            fadePanel.color = c;
            yield return null;
        }

        if (fadePanel == null) yield break;

        c.a = toAlpha;
        fadePanel.color = c;
    }
}