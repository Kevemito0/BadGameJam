using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimeLoopManager : MonoBehaviour
{
    public static TimeLoopManager Instance { get; private set; }
    
    [Header("Quest")]
    [SerializeField] private PlayerQuestManager questManager;
    
    [Header("Loop Ayarları")]
    [SerializeField] private float loopDuration = 300f;
    [SerializeField] private bool startLoopOnAwake = true;

    [Header("UI (Opsiyonel)")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject loopBrokenUI;

    // Sahne resetlenince sıfırlanmayan static flag
    private static bool s_loopBroken = false;

    private float _timer;
    private bool _loopActive;

    public bool LoopBroken => s_loopBroken;
    public float TimeRemaining => Mathf.Max(0f, loopDuration - _timer);

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        _timer = 0f;
        _loopActive = startLoopOnAwake;
    }

    private void Start()
    {
        if (loopBrokenUI != null)
            loopBrokenUI.SetActive(s_loopBroken);
    }

    private void Update()
    {
        if (!_loopActive || s_loopBroken) return;

        _timer += Time.deltaTime;
        UpdateTimerUI();

        if (_timer >= loopDuration)
            TriggerReset();
    }


    public void BreakLoop()
    {
        if (s_loopBroken) return;

        s_loopBroken = true;
        _loopActive = false;

        Debug.Log($"[TimeLoop] Loop kırıldı! Geçen süre: {_timer:F1}s");

        if (loopBrokenUI != null)
            loopBrokenUI.SetActive(true);

        if (timerText != null)
            timerText.text = "∞";
    }

    public void StartLoop() => _loopActive = true;
    public void StopLoop()  => _loopActive = false;

    public void SetLoopDuration(float seconds) => loopDuration = seconds;

    /// Ana menüden yeni oyun başlatılırken çağır
    public static void ResetStaticState()
    {
        s_loopBroken = false;
    }

    // ─── Private ─────────────────────────────────────────────────

    private void TriggerReset()
    {
        _loopActive = false;
        questManager?.ResetAll();
        Debug.Log("[TimeLoop] Süre doldu — sahne resetleniyor...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateTimerUI()
    {
        if (timerText == null || s_loopBroken) return;

        float remaining = TimeRemaining;
        int m = Mathf.FloorToInt(remaining / 60f);
        int s = Mathf.FloorToInt(remaining % 60f);
        timerText.text = $"{m:00}:{s:00}";
    }
}