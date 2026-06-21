using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimeLoopManager : MonoBehaviour
{
    public static TimeLoopManager Instance { get; private set; }

    [Header("Quest")]
    [SerializeField] private PlayerQuestManager questManager;

    [Header("Loop Settings")]
    [SerializeField] private float loopDuration = 300f;
    [SerializeField] private bool startLoopOnAwake = true;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject loopBrokenUI;

    [Header("Loop End Panel")]
    [SerializeField] private LoopEndPanel loopEndPanel;
    [SerializeField] private string loopEndMessage = "Zaman doldu!";  // Panelde çıkacak yazı

    [Header("Satellite")]
    [SerializeField] private SatelliteSpawner satelliteSpawner;
    [SerializeField] private float satelliteWarningTime = 30f;
    [SerializeField] private float satelliteSpawnTime   = 5f;

    private static bool s_loopBroken = false;

    private float _timer;
    private bool _loopActive;
    private bool _warningTriggered = false;
    private bool _satelliteSpawned = false;

    public bool LoopBroken     => s_loopBroken;
    public float TimeRemaining => Mathf.Max(0f, loopDuration - _timer);

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        _timer            = 0f;
        _loopActive       = startLoopOnAwake;
        _warningTriggered = false;
        _satelliteSpawned = false;

        questManager?.ResetAll();
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

        // Uyarı: son 30 saniye
        if (!_warningTriggered && TimeRemaining <= satelliteWarningTime)
        {
            _warningTriggered = true;
            OnSatelliteWarning();
        }

        // Spawn: son 5 saniye
        if (!_satelliteSpawned && TimeRemaining <= satelliteSpawnTime)
        {
            _satelliteSpawned = true;
            satelliteSpawner?.SpawnSatellite();
        }

        // Süre doldu
        if (_timer >= loopDuration)
        {
            TriggerReset();
        }
    }

    private void OnSatelliteWarning()
    {
        Debug.Log("[TimeLoop] Satellite warning! 30 seconds left.");
        SoundManager.PlaySound(SoundType.SatelliteCrash);
    }

    public void BreakLoop()
    {
        if (s_loopBroken) return;

        s_loopBroken = true;
        _loopActive  = false;

        Debug.Log($"[TimeLoop] Loop broken! Elapsed time: {_timer:F1}s");

        if (loopBrokenUI != null)
            loopBrokenUI.SetActive(true);

        if (timerText != null)
            timerText.text = "∞";
    }

    public void StartLoop()                    => _loopActive  = true;
    public void StopLoop()                     => _loopActive  = false;
    public void SetLoopDuration(float seconds) => loopDuration = seconds;

    public static void ResetStaticState()
    {
        s_loopBroken = false;
    }

    private void TriggerReset()
    {
        _loopActive = false;
        questManager?.ResetAll();
        Debug.Log("[TimeLoop] Time is up");
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