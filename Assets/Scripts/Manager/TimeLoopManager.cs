using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class TimeLoopManager : MonoBehaviour
{
    public static TimeLoopManager Instance { get; private set; }

    [Header("Loop Ayarları")]
    [SerializeField] private float loopDuration = 300f; 
    [SerializeField] private bool startLoopOnAwake = true;

    [Header("UI (Opsiyonel)")]
    [SerializeField] private TextMeshProUGUI timerText;   
    [SerializeField] private GameObject loopBrokenUI;     

   
    private static bool s_loopBroken = false;
    private static float s_elapsedBeforeReset = 0f; 

    private float timer;
    private bool loopActive;

    public bool LoopBroken => s_loopBroken;
    public float TimeRemaining => Mathf.Max(0f, loopDuration - timer);
    public float LoopDuration => loopDuration;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;


        ResetTimer();

        if (startLoopOnAwake)
            loopActive = true;
    }

    private void Start()
    {
        if (loopBrokenUI != null)
            loopBrokenUI.SetActive(s_loopBroken);
    }

    private void Update()
    {
        if (!loopActive || s_loopBroken)
            return;

        timer += Time.deltaTime;

        UpdateTimerUI();

        if (timer >= loopDuration)
        {
            TriggerReset();
        }
    }

    
    public void BreakLoop()
    {
        if (s_loopBroken) return;

        s_loopBroken = true;
        s_elapsedBeforeReset = timer;
        loopActive = false;

        Debug.Log($"[TimeLoop] Loop kırıldı! Geçen süre: {s_elapsedBeforeReset:F1}s");

        if (loopBrokenUI != null)
            loopBrokenUI.SetActive(true);
    }

   
    public void StartLoop()
    {
        loopActive = true;
    }

    
    public void SetLoopDuration(float seconds)
    {
        loopDuration = seconds;
    }

   
    public static void ResetStaticState()
    {
        s_loopBroken = false;
        s_elapsedBeforeReset = 0f;
    }


    private void ResetTimer()
    {
        timer = 0f;
    }

    private void TriggerReset()
    {
        loopActive = false;
        Debug.Log("[TimeLoop] Süre doldu — sahne resetleniyor...");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;

        if (s_loopBroken)
        {
            timerText.text = "∞";
            return;
        }

        float remaining = TimeRemaining;
        int minutes = Mathf.FloorToInt(remaining / 60f);
        int seconds = Mathf.FloorToInt(remaining % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}