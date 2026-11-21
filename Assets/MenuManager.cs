using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip clickSound;

    void Start()
    {
        DontDestroyOnLoad(musicSource.gameObject);
    }

    public void PlayGame()
    {
        musicSource.PlayOneShot(clickSound);
        Invoke("LoadCutscene", 0.5f);
    }

    public void QuitGame()
    {
        musicSource.PlayOneShot(clickSound);
        Invoke("Quit", 0.5f);
    }

    void LoadCutscene()
    {
        SceneManager.LoadScene("Cutscene");
    }

    void Quit()
    {
        Application.Quit();
    }
}