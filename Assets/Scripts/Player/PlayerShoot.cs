// Assets/Scripts/Player/PlayerShoot.cs
using UnityEngine;
using TMPro;

public class PlayerShoot : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private PlayerQuestManager questManager;

    [Header("Animator")]
    [SerializeField] private Animator playerAnimator;

    [Header("VFX")]
    [SerializeField] private ParticleSystem muzzleFlash;

    [Header("Raycast")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float shootRange = 50f;
    [SerializeField] private LayerMask shootMask;

    [Header("Fire Rate")]
    [SerializeField] private float fireRate = 0.2f;
    private float _nextFireTime = 0f;

    [Header("Crosshair")]
    [SerializeField] private TextMeshProUGUI hitMarkerText;
    [SerializeField] private float hitMarkerDuration = 0.1f;
    private float _hitMarkerTimer = 0f;

    private void Update()
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsOpen) return;
        if (!questManager.hasWeapon) return;

        if (hitMarkerText != null && _hitMarkerTimer > 0f)
        {
            _hitMarkerTimer -= Time.deltaTime;
            if (_hitMarkerTimer <= 0f)
                hitMarkerText.text = "+";
        }

        if (Input.GetMouseButton(0) && Time.time >= _nextFireTime)
        {
            _nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (playerAnimator != null)
            playerAnimator.SetTrigger("Shoot");

        if (muzzleFlash != null)
            muzzleFlash.Play();

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
            else if (npc == null)
            {
                BulletHoleManager.Instance?.SpawnHole(hitInfo.point, hitInfo.normal);
            }
        }
    }

    private void ShowHitMarker()
    {
        if (hitMarkerText == null) return;
        hitMarkerText.text = "X";
        _hitMarkerTimer = hitMarkerDuration;
    }
}