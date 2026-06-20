using System.Collections;
using UnityEngine;

public class NPCHealth : MonoBehaviour
{
    [SerializeField] private float deathRotationSpeed = 3f;

    [Tooltip("Die animasyonunun süresi (saniye). Animator'daki clip süresine göre ayarla.")]
    [SerializeField] private float deathAnimDuration = 1.5f;

    private bool _isDead = false;
    private bool _isFalling = false;
    private Animator _animator;

    public bool IsDead => _isDead;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void Die()
    {
        if (_isDead) return;
        _isDead = true;

        // Collider kapat
        Collider col = GetComponentInChildren<Collider>();
        if (col != null) col.enabled = false;

        // NavMeshAgent durdur
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) agent.enabled = false;

        if (_animator != null)
        {
            _animator.SetTrigger("Die");
            StartCoroutine(FallAfterAnim());
        }
        else
        {
            _isFalling = true;
        }
    }

    private IEnumerator FallAfterAnim()
    {
        // Die animasyonu oynasın
        yield return new WaitForSeconds(deathAnimDuration);

        // Animator'ı durdur, son frame'de kalsın
        _animator.enabled = false;

        // Şimdi yere yığıl
        _isFalling = true;
    }

    private void Update()
    {
        if (!_isFalling) return;

        Quaternion targetRot = Quaternion.Euler(90f, transform.eulerAngles.y, transform.eulerAngles.z);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            Time.deltaTime * deathRotationSpeed
        );

        if (Quaternion.Angle(transform.rotation, targetRot) < 0.5f)
        {
            transform.rotation = targetRot;
            _isFalling = false;
        }
    }
}