using System.Collections.Generic;
using UnityEngine;

public class BulletHoleManager : MonoBehaviour
{
    public static BulletHoleManager Instance;

    [Header("Bullet Hole")]
    [SerializeField] private GameObject bulletHolePrefab;
    [SerializeField] private int maxHoles = 50;          // Maksimum aynı anda kaç delik
    [SerializeField] private float holeLifetime = 10f;   // Saniye sonra yok olur (0 = sonsuz)
    [SerializeField] private float offsetFromSurface = 0.01f; // Z-fighting önlemek için

    private readonly Queue<GameObject> _holePool = new();

    private void Awake() => Instance = this;

    public void SpawnHole(Vector3 position, Vector3 normal)
    {
        if (bulletHolePrefab == null) return;

        // Havuz doluysa en eskiyi sil
        if (_holePool.Count >= maxHoles)
        {
            GameObject old = _holePool.Dequeue();
            if (old != null) Destroy(old);
        }

        // Yüzeyin normaline göre döndür
        Quaternion rotation = Quaternion.LookRotation(-normal) 
                              * Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)); // rastgele z dönüşü

        Vector3 spawnPos = position + normal * offsetFromSurface;
        GameObject hole = Instantiate(bulletHolePrefab, spawnPos, rotation);
        _holePool.Enqueue(hole);

        if (holeLifetime > 0f)
            Destroy(hole, holeLifetime);
    }
}