// Assets/Scripts/Canvas/LoopEndPanel.cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoopEndPanel : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panel;
    [SerializeField] private CanvasGroup canvasGroup; // Panel'e CanvasGroup ekle

    [Header("Fade")]
    [SerializeField] private float fadeDuration = 1.5f;

    [Header("Butonlar")]
    [SerializeField] private Button menuBtn;
    [SerializeField] private Button retryBtn;

    [Header("Opsiyonel")]
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private string mainMenuScene = "MainMenuScene";

    private void Start()
    {
        panel.SetActive(false);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        if (menuBtn  != null) menuBtn.onClick.AddListener(GoToMenu);
        if (retryBtn != null) retryBtn.onClick.AddListener(Retry);
    }

    public void ShowPanel(string message = "Johnnie Sattelite took a bus but it crashed and exploded, there were no survivors.")
    {
        if (messageText != null && !string.IsNullOrEmpty(message))
            messageText.text = message;

        Cursor.visible   = true;
        Cursor.lockState = CursorLockMode.None;

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        panel.SetActive(true);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime; // timeScale 0 olsa bile çalışır
            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        Time.timeScale = 0f; // Fade bittikten sonra dondur
    }

    private void GoToMenu()
    {
        Time.timeScale = 1f;
        TimeLoopManager.ResetStaticState();
        SceneManager.LoadScene(mainMenuScene);
    }

    private void Retry()
    {
        Time.timeScale = 1f;
        TimeLoopManager.ResetStaticState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}