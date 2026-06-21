using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    
    [SerializeField] GameObject menuPanel;
    [SerializeField] Button resumeBtn;
    [SerializeField] Button quitBtn;
    
    bool isMenuOpen = false;
    
    void Start()
    {
        menuPanel.SetActive(false);
        resumeBtn.onClick.AddListener(OnResumeClicked);
        quitBtn.onClick.AddListener(onQuitClicked);
        isMenuOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        menuPanel.SetActive(isMenuOpen);
        
        Time.timeScale = isMenuOpen ? 0 : 1;
        
        Cursor.visible = isMenuOpen;
        Cursor.lockState = isMenuOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }

    void OnResumeClicked()
    {
        ToggleMenu();
    }

    void onQuitClicked()
    {
        SceneManager.LoadScene(0);
    }
}
