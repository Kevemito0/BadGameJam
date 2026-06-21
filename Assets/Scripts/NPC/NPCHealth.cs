using UnityEngine;

public class NPCHealth : MonoBehaviour
{
    [SerializeField] private float deathRotationSpeed = 10f;

    [Header("Sesler")]
    [SerializeField] private AudioClip deathClip;   // ölüm çığlığı
    [SerializeField] private AudioClip fallClip;    // yere düşme sesi (opsiyonel)
    [SerializeField] private AudioSource audioSource;

    private bool _isDead = false;
    private bool _isFalling = false;
    private bool _fallSoundPlayed = false;

    public bool IsDead => _isDead;

    public void Die()
    {
        if (_isDead) return;
        _isDead = true;

        // Ölüm sesini çal
        if (audioSource != null && deathClip != null)
            audioSource.PlayOneShot(deathClip);

        Collider col = GetComponentInChildren<Collider>();
        if (col != null) col.enabled = false;

        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) agent.enabled = false;

        _isFalling = true;
    }

    private void Update()
    {
        if (!_isFalling) return;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.Euler(90f, transform.eulerAngles.y, transform.eulerAngles.z),
            Time.deltaTime * deathRotationSpeed
        );

        // Yere düşme sesini animasyon bitmek üzereyken çal
        if (!_fallSoundPlayed &&
            Quaternion.Angle(transform.rotation,
                Quaternion.Euler(90f, transform.eulerAngles.y, transform.eulerAngles.z)) < 15f)
        {
            _fallSoundPlayed = true;
            if (audioSource != null && fallClip != null)
                audioSource.PlayOneShot(fallClip);
        }

        if (Quaternion.Angle(transform.rotation,
                Quaternion.Euler(90f, transform.eulerAngles.y, transform.eulerAngles.z)) < 0.5f)
        {
            _isFalling = false;
        }
    }
}