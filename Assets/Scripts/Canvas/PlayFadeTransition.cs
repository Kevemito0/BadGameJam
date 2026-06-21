using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayFadeTransition : MonoBehaviour
{
    [Header("Canvas Referansları")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private Image fadePanel;

    [Header("Loading Metni")]
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private TextMeshProUGUI tipText;
    [SerializeField] private Image progressBarFill;

    [Header("Press E")]
    [SerializeField] private TextMeshProUGUI pressEText;
    [SerializeField] private string pressEMessage = "press E To Start";

    [SerializeField] private float fadeDuration   = 0.8f;
    [SerializeField] private float minLoadingTime = 2.5f;
    [SerializeField] private string targetScene   = "GameScene";

    [TextArea] [SerializeField] private string[] loadingMessages = new string[] { };
    [TextArea] [SerializeField] private string[] tips            = new string[] { };

    private void Awake()
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }
        gameObject.SetActive(false);
    }

    public void StartTransition()
    {
        gameObject.SetActive(true);
        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        // ── 1. Fade-in ──────────────────────────────────────────
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.blocksRaycasts = true;
            float e = 0f;
            while (e < fadeDuration)
            {
                e += Time.unscaledDeltaTime;
                fadeCanvasGroup.alpha = Mathf.Clamp01(e / fadeDuration);
                yield return null;
            }
            fadeCanvasGroup.alpha = 1f;
        }

        // ── 2. İpucu ────────────────────────────────────────────
        if (tipText != null && tips.Length > 0)
            tipText.text = tips[Random.Range(0, tips.Length)];

        // ── 3. Sahneyi arka planda yükle ────────────────────────
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);
        asyncLoad.allowSceneActivation = false;

        float fakeProgress = 0f;
        float timeSpent    = 0f;
        int   msgIndex     = 0;
        float msgTimer     = 0f;
        float msgInterval  = minLoadingTime / Mathf.Max(1, loadingMessages.Length);

        if (loadingText != null && loadingMessages.Length > 0)
            loadingText.text = loadingMessages[0];

        while (timeSpent < minLoadingTime || asyncLoad.progress < 0.9f)
        {
            timeSpent    += Time.unscaledDeltaTime;
            msgTimer     += Time.unscaledDeltaTime;
            float real    = asyncLoad.progress / 0.9f;
            fakeProgress  = Mathf.MoveTowards(fakeProgress,
                                Mathf.Max(real, timeSpent / minLoadingTime),
                                Time.unscaledDeltaTime * 0.6f);

            if (progressBarFill != null)
                progressBarFill.fillAmount = Mathf.Clamp01(fakeProgress);

            if (msgTimer >= msgInterval && loadingMessages.Length > 0)
            {
                msgTimer = 0f;
                msgIndex = (msgIndex + 1) % loadingMessages.Length;
                if (loadingText != null)
                    loadingText.text = loadingMessages[msgIndex];
            }

            yield return null;
        }

        // ── 4. Loading bitti: bar dolu, loadingText temizle, pressE göster ──
        if (progressBarFill != null) progressBarFill.fillAmount = 1f;
        if (loadingText != null)     loadingText.text = "";

        if (pressEText != null)
            pressEText.text = pressEMessage;

        // ── 5. E tuşunu bekle ───────────────────────────────────
        while (!Input.GetKeyDown(KeyCode.E))
            yield return null;

        if (pressEText != null)
            pressEText.text = "";

        // ── 6. Sahneyi aktifleştir ──────────────────────────────
        asyncLoad.allowSceneActivation = true;
    }
}