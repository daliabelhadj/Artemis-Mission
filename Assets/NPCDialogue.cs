using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    public GameObject darkOverlay;
    public RectTransform npcPanel;
    public TextMeshProUGUI npcText;
    public TextMeshProUGUI npcName;
    public GameObject continueButton;
    public string npcNameText = "Alien Guide";
    public string[] dialogueLines;

    private int currentLine = 0;
    private bool isTyping = false;
    private bool isShowing = false;
    private float typeSpeed = 0.05f;
    private Vector2 hiddenPos = new Vector2(0, -120);
    private Vector2 shownPos = new Vector2(0, 0);
    private Coroutine typingCoroutine;

    void Start()
    {
        npcPanel.anchoredPosition = hiddenPos;
        darkOverlay.SetActive(false);
        continueButton.SetActive(false);
        Invoke("TestShow", 1f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Enter pressed! isShowing = " + isShowing);
        }

        if (isShowing && Input.GetKeyDown(KeyCode.Return))
        {
            OnContinue();
        }

        if (isShowing && Input.GetKeyDown(KeyCode.Space))
        {
            OnContinue();
        }
    }

    void TestShow()
    {
        ShowNPC(new string[] {
            "You must be the one who fell from the sky...",
            "This planet is dangerous. Watch your step.",
            "Find the portal at the end. Go now."
        });
    }

    public void ShowNPC(string[] lines)
    {
        if (isShowing) return;
        isShowing = true;
        dialogueLines = lines;
        currentLine = 0;
        npcText.text = "";
        darkOverlay.SetActive(true);
        continueButton.SetActive(false);
        npcName.text = npcNameText;
        Time.timeScale = 0f;
        StopAllCoroutines();
        StartCoroutine(SlideUp());
    }

    IEnumerator SlideUp()
    {
        float elapsed = 0f;
        float duration = 0.4f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            npcPanel.anchoredPosition = Vector2.Lerp(
                hiddenPos, shownPos, elapsed / duration);
            yield return null;
        }
        npcPanel.anchoredPosition = shownPos;
        typingCoroutine = StartCoroutine(
            TypeLine(dialogueLines[currentLine]));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        npcText.text = "";
        continueButton.SetActive(false);
        foreach (char letter in line)
        {
            npcText.text += letter;
            yield return new WaitForSecondsRealtime(typeSpeed);
        }
        isTyping = false;
        continueButton.SetActive(true);
    }

    public void OnContinue()
    {
        Debug.Log("OnContinue called! currentLine = " + currentLine);

        if (isTyping)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            isTyping = false;
            npcText.text = dialogueLines[currentLine];
            continueButton.SetActive(true);
            return;
        }

        currentLine++;

        if (currentLine < dialogueLines.Length)
        {
            typingCoroutine = StartCoroutine(
                TypeLine(dialogueLines[currentLine]));
        }
        else
        {
            StartCoroutine(SlideDown());
        }
    }

    IEnumerator SlideDown()
    {
        float elapsed = 0f;
        float duration = 0.4f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            npcPanel.anchoredPosition = Vector2.Lerp(
                shownPos, hiddenPos, elapsed / duration);
            yield return null;
        }
        npcPanel.anchoredPosition = hiddenPos;
        darkOverlay.SetActive(false);
        Time.timeScale = 1f;
        isShowing = false;
        gameObject.SetActive(false);
    }
}