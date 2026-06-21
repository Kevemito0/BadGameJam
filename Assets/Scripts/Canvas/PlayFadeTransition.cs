// Assets/Scripts/Canvas/PlayFadeTransition.cs
// Bağımlılıklar: TMPro, UnityEngine.UI
// Sahneye bir Canvas objesi ekle, bu scripti bağla, ardından MainMenuCanvas içinde
// OnPlayClicked() metodunu bu scriptin StartTransition() metoduna yönlendir.

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayFadeTransition : MonoBehaviour
{
    [Header("Canvas Referansları")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;   // siyah panelin CanvasGroup'u
    [SerializeField] private Image fadePanel;               // tam ekran siyah Image

    [Header("Loading Metni")]
    [SerializeField] private TextMeshProUGUI loadingText;   // "Yükleniyor..." yazısı
    [SerializeField] private TextMeshProUGUI tipText;       // opsiyonel ipucu yazısı

    [Header("Progress Bar (opsiyonel)")]
    [SerializeField] private Image progressBarFill;         // fillAmount kullanılacak Image

    [Header("Süre Ayarları")]
    [SerializeField] private float fadeDuration    = 0.8f;  // siyaha geçiş süresi
    [SerializeField] private float minLoadingTime  = 2.5f;  // sahne hazır olsa bile minimum bekleme
    [SerializeField] private string targetScene    = "GameScene";

    [Header("Fake Loading Mesajları")]
    [TextArea]
    [SerializeField] private string[] loadingMessages = new string[]
    {
        "Dünya yükleniyor...",
        "NPC'ler hazırlanıyor...",
        "Zaman döngüsü başlatılıyor...",
        "İstanbul kartı aranıyor...",
        "Uyduları yörüngeye oturtuyoruz...",
        "Geçmiş silinemez, yükleniyor..."
    };

    [Header("İpuçları (Tip)")]
    [TextArea]
    [SerializeField] private string[] tips = new string[]
    {
        "İPUCU: Önce kartı al, sonra silahı.",
        "İPUCU: Kuyruğu temizlemeden terminale gidemezsin.",
        "İPUCU: Uydu düşmeden önce kaç.",
        "İPUCU: NPC ile konuşmak döngüyü etkiler.",
    };

    private void Awake()
    {
        // Başlangıçta tamamen şeffaf ve deaktif
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }
        gameObject.SetActive(false);
    }

    /// <summary>
    /// MainMenuCanvas.OnPlayClicked() içinden çağır.
    /// </summary>
    public void StartTransition()
    {
        gameObject.SetActive(true);
        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        // ── 1. Fade-in (şeffaftan siyaha) ──────────────────────────────────
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.blocksRaycasts = true;
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                fadeCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
                yield return null;
            }
            fadeCanvasGroup.alpha = 1f;
        }

        // ── 2. İpucunu seç ─────────────────────────────────────────────────
        if (tipText != null && tips.Length > 0)
            tipText.text = tips[Random.Range(0, tips.Length)];

        // ── 3. Sahneyi arka planda yükle ───────────────────────────────────
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);
        asyncLoad.allowSceneActivation = false;   // hazır olsa bile bekle

        float fakeProgress = 0f;
        float realProgress = 0f;
        float timeSpent    = 0f;
        int   msgIndex     = 0;
        float msgTimer     = 0f;
        float msgInterval  = minLoadingTime / Mathf.Max(1, loadingMessages.Length);

        // İlk mesajı göster
        if (loadingText != null && loadingMessages.Length > 0)
            loadingText.text = loadingMessages[0];

        while (timeSpent < minLoadingTime || asyncLoad.progress < 0.9f)
        {
            timeSpent    += Time.unscaledDeltaTime;
            msgTimer     += Time.unscaledDeltaTime;
            realProgress  = asyncLoad.progress / 0.9f;          // Unity 0..0.9 döndürür
            fakeProgress  = Mathf.MoveTowards(fakeProgress,
                                Mathf.Max(realProgress, timeSpent / minLoadingTime),
                                Time.unscaledDeltaTime * 0.6f);

            // Progress bar güncelle
            if (progressBarFill != null)
                progressBarFill.fillAmount = Mathf.Clamp01(fakeProgress);

            // Mesaj döngüsü
            if (msgTimer >= msgInterval && loadingMessages.Length > 0)
            {
                msgTimer = 0f;
                msgIndex = (msgIndex + 1) % loadingMessages.Length;
                if (loadingText != null)
                    loadingText.text = loadingMessages[msgIndex];
            }

            yield return null;
        }

        // Son mesaj & bar
        if (progressBarFill != null) progressBarFill.fillAmount = 1f;
        if (loadingText != null)     loadingText.text = "Hazır!";

        yield return new WaitForSecondsRealtime(0.4f);

        // ── 4. Sahneyi aktifleştir ─────────────────────────────────────────
        asyncLoad.allowSceneActivation = true;
    }
}