using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button playBtn;
    [SerializeField] private Button settingsBtn;
    [SerializeField] private Button quitBtn;

    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;

    [Header("Scene")]
    [SerializeField] private string gameSceneName = "GameScene";
    
    
    [SerializeField] private PlayerQuestManager questManager;
    private void Start()
    {
        playBtn.onClick.AddListener(OnPlayClicked);
        settingsBtn.onClick.AddListener(OnSettingsClicked);
        quitBtn.onClick.AddListener(OnQuitClicked);

        settingsPanel.SetActive(false);
    }

    private void OnPlayClicked()
    {
        questManager.ResetAll();           
        TimeLoopManager.ResetStaticState();
        SceneManager.LoadScene(gameSceneName);
    }

    private void OnSettingsClicked()
    {
        settingsPanel.SetActive(true);
    }

    private void OnQuitClicked()
    {
        Debug.Log("Quiting Game");
        Application.Quit();
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}