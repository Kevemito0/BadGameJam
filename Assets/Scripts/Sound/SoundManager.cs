using UnityEngine;

public enum SoundType
{
    Footstep,
    SatelliteCrash,
    SatelliteSound,
    NPCTalk,
    Jump,
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
        int randomIndex = Random.Range(0, instance.footSteps.Length);
        
        if(sound == SoundType.Footstep)
            instance.audioSource.PlayOneShot(instance.footSteps[randomIndex], volume);
        else
        {
            instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
        }
    }

}
