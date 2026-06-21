using UnityEngine;

public enum SoundType
{
    GunShot,
    GunPickup,
    CardDeclined,
    SatelliteCrash,
    SatelliteSound,
    Jump,
    Footstep,
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    [SerializeField] private AudioClip[] footSteps;

    [Header("Music")]
    [SerializeField] private AudioSource musicSource;   
    [SerializeField] [Range(0f, 1f)] private float defaultMusicVolume = 0.5f;

    private static SoundManager instance;
    private AudioSource audioSource;

    private const string MusicVolKey = "MusicVolume";

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Kaydedilmiş ses seviyesini yükle, yoksa default kullan
        float saved = PlayerPrefs.GetFloat(MusicVolKey, defaultMusicVolume);
        SetMusicVolume(saved);
    }

    // ── Statik API ──────────────────────────────────────────────

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        if (sound == SoundType.Footstep)
        {
            if (instance.footSteps == null || instance.footSteps.Length == 0) return;
            int randomIndex = Random.Range(0, instance.footSteps.Length);
            instance.audioSource.PlayOneShot(instance.footSteps[randomIndex], volume);
            return;
        }

        int index = (int)sound;
        if (instance.soundList == null || index >= instance.soundList.Length)
        {
            Debug.LogWarning($"[SoundManager] No clip for {sound} (index {index})");
            return;
        }

        instance.audioSource.PlayOneShot(instance.soundList[index], volume);
    }


    public static void SetMusicVolume(float value)
    {
        if (instance == null) return;
        value = Mathf.Clamp01(value);

        if (instance.musicSource != null)
            instance.musicSource.volume = value;

        PlayerPrefs.SetFloat(MusicVolKey, value);
        PlayerPrefs.Save();
    }

    public static float GetMusicVolume()
    {
        return instance != null && instance.musicSource != null
            ? instance.musicSource.volume
            : PlayerPrefs.GetFloat(MusicVolKey, 0.5f);
    }
}