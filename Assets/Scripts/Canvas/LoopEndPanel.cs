// Assets/Scripts/Canvas/LoopEndPanel.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoopEndPanel : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panel;

    [Header("Butonlar")]
    [SerializeField] private Button menuBtn;
    [SerializeField] private Button retryBtn;          // İstersen retry de ekleyebilirsin

    [Header("Opsiyonel")]
    [SerializeField] private TextMeshProUGUI messageText;   // Paneldeki yazı
    [SerializeField] private string mainMenuScene = "MainMenuScene";

    private void Start()
    {
        panel.SetActive(false);

        if (menuBtn  != null) menuBtn.onClick.AddListener(GoToMenu);
        if (retryBtn != null) retryBtn.onClick.AddListener(Retry);
    }

  
    public void ShowPanel(string message = "Loop is ended")
    {
        if (messageText != null && !string.IsNullOrEmpty(message))
            messageText.text = message;

        panel.SetActive(true);

        Time.timeScale = 0f;
        Cursor.visible    = true;
        Cursor.lockState  = CursorLockMode.None;
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