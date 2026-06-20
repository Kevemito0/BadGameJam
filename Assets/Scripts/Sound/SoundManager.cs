using UnityEngine;

public enum SoundType
{
    GunShot,
    Footstep,
    SatelliteCrash,
    SatelliteSound,
    NPCTalk,
    Jump
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    [SerializeField] private AudioClip[] footSteps;
    private static SoundManager instance;
    private AudioSource audioSource;

    void Awake()
    {
        instance = this;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

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

}
