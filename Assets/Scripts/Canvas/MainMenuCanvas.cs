// Assets/Scripts/Canvas/MainMenuCanvas.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button playBtn;
    [SerializeField] private Button quitBtn;


    [SerializeField] private PlayFadeTransition fadeTransition;   

    [SerializeField] private PlayerQuestManager questManager;

    private void Start()
    {
        playBtn.onClick.AddListener(OnPlayClicked);
        quitBtn.onClick.AddListener(OnQuitClicked);

    }

    private void OnPlayClicked()
    {
        questManager.ResetAll();
        TimeLoopManager.ResetStaticState();

        if (fadeTransition != null)
            fadeTransition.StartTransition();
        else
            SceneManager.LoadScene("GameScene");   // fallback
    }


    private void OnQuitClicked()
    {
        Debug.Log("Quiting Game");
        Application.Quit();
    }
}