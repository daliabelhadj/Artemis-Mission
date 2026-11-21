using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    public TextMeshProUGUI heartsDisplay;
    public TextMeshProUGUI coinsDisplay;
    public TextMeshProUGUI timerDisplay;
    public GameObject finalScoreScreen;
    public TextMeshProUGUI finalScoreText;

    [Header("Stats")]
    public int hearts = 3;
    public int coins = 0;
    private float timer = 0f;
    private bool gameActive = true;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (!gameActive) return;

        timer += Time.deltaTime;
        UpdateTimerDisplay();

        if (Input.GetKeyDown(KeyCode.H)) LoseHeart();
        if (Input.GetKeyDown(KeyCode.W)) WinGame();
        if (Input.GetKeyDown(KeyCode.C)) AddCoins(10);
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        timerDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        coinsDisplay.text = "Coins: " + coins;
    }

    public void LoseHeart()
    {
        hearts--;
        UpdateHeartsDisplay();

        if (hearts <= 0)
        {
            GameOver();
        }
    }

    void UpdateHeartsDisplay()
    {
        string h = "";
        for (int i = 0; i < hearts; i++) h += "♥ ";
        for (int i = hearts; i < 3; i++) h += "♡ ";
        heartsDisplay.text = h;
    }

    public void WinGame()
    {
        gameActive = false;
        Time.timeScale = 0f;

        int timeBonus = Mathf.RoundToInt(timer * 10);
        int heartBonus = hearts * 50;
        int finalScore = coins + timeBonus + heartBonus;

        finalScoreText.text =
            "Coins: " + coins +
            "\nTime Bonus: " + timeBonus +
            "\nHeart Bonus: " + heartBonus +
            "\n─────────────" +
            "\nFinal Score: " + finalScore;

        finalScoreScreen.SetActive(true);
    }

    void GameOver()
    {
        gameActive = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}