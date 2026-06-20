using UnityEngine;

public class NPCHealth : MonoBehaviour
{
    [SerializeField] private float deathRotationSpeed = 10f;

    private bool _isDead = false;
    private bool _isFalling = false;

    public bool IsDead => _isDead;

    public void Die()
    {
        if (_isDead) return;
        _isDead = true;

        // Collider'ı kapat ki bir daha vurulmasın / interact edilmesin
        Collider col = GetComponentInChildren<Collider>();
        if (col != null) col.enabled = false;

        // Varsa NavMeshAgent durdur
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) agent.enabled = false;

        _isFalling = true;
    }

    private void Update()
    {
        if (!_isFalling) return;

        // Yavaşça yatık konuma getir (90° X ekseni = yere düşmüş gibi)
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.Euler(90f, transform.eulerAngles.y, transform.eulerAngles.z),
            Time.deltaTime * deathRotationSpeed
        );

        // Hedefe ulaşınca durdur
        if (Quaternion.Angle(transform.rotation,
                Quaternion.Euler(90f, transform.eulerAngles.y, transform.eulerAngles.z)) < 0.5f)
        {
            _isFalling = false;
        }
    }
}