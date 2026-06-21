using System.Collections.Generic;
using UnityEngine;

public class BulletHoleManager : MonoBehaviour
{
    public static BulletHoleManager Instance;

    [Header("Bullet Hole")]
    [SerializeField] private GameObject bulletHolePrefab;
    [SerializeField] private int maxHoles = 50;          
    [SerializeField] private float holeLifetime = 10f;   
    [SerializeField] private float offsetFromSurface = 0.01f; 

    private readonly Queue<GameObject> _holePool = new();

    private void Awake() => Instance = this;

    public void SpawnHole(Vector3 position, Vector3 normal)
    {
        if (bulletHolePrefab == null) return;

        if (_holePool.Count >= maxHoles)
        {
            GameObject old = _holePool.Dequeue();
            if (old != null) Destroy(old);
        }

        Quaternion rotation = Quaternion.LookRotation(-normal) 
                              * Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)); 

        Vector3 spawnPos = position + normal * offsetFromSurface;
        GameObject hole = Instantiate(bulletHolePrefab, spawnPos, rotation);
        _holePool.Enqueue(hole);

        if (holeLifetime > 0f)
            Destroy(hole, holeLifetime);
    }
}