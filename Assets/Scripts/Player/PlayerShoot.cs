using UnityEngine;
using TMPro;

public class PlayerShoot : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private PlayerQuestManager questManager;
    
    [Header("VFX")]
    [SerializeField] private ParticleSystem muzzleFlash;
    
    [Header("Raycast")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float shootRange = 50f;
    [SerializeField] private LayerMask shootMask; // Opsiyonel: sadece belirli layer'ları vursun

    [Header("Fire Rate")]
    [SerializeField] private float fireRate = 0.2f; // saniyede kaç kez ateş edebilir
    private float _nextFireTime = 0f;

    [Header("Crosshair")]
    [SerializeField] private TextMeshProUGUI hitMarkerText; // "+" gibi bir UI text
    [SerializeField] private float hitMarkerDuration = 0.1f;
    private float _hitMarkerTimer = 0f;

    private void Update()
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsOpen) return;
        
        if (!questManager.hasWeapon) return;
        
        // Hit marker söndür
        if (hitMarkerText != null && _hitMarkerTimer > 0f)
        {
            _hitMarkerTimer -= Time.deltaTime;
            if (_hitMarkerTimer <= 0f)
                hitMarkerText.text = "+"; // normal crosshair
        }

        if (Input.GetMouseButton(0) && Time.time >= _nextFireTime)
        {
            _nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        Debug.Log("Shoot");

        if (muzzleFlash != null)
        {
            Debug.Log("Play Muzzle");
            muzzleFlash.Play();
        }
        
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        bool hit = shootMask != 0
            ? Physics.Raycast(ray, out RaycastHit hitInfo, shootRange, shootMask)
            : Physics.Raycast(ray, out hitInfo, shootRange);

        SoundManager.PlaySound(SoundType.GunShot);
        
        if (hit)
        {
            NPCHealth npc = hitInfo.collider.GetComponentInParent<NPCHealth>();
            if (npc != null && !npc.IsDead)
            {
                npc.Die();
                ShowHitMarker();
            }
            else if (npc == null) // NPC değilse delik koy
            {
                BulletHoleManager.Instance?.SpawnHole(hitInfo.point, hitInfo.normal);
            }
        }
    }

    private void ShowHitMarker()
    {
        if (hitMarkerText == null) return;
        hitMarkerText.text = "X"; // isabet göstergesi
        _hitMarkerTimer = hitMarkerDuration;
    }
}