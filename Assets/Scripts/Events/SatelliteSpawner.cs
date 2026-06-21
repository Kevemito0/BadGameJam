using UnityEngine;

public class SatelliteSpawner : MonoBehaviour
{
    [SerializeField] private GameObject satellitePrefab;
    [SerializeField] private float spawnHeight = 100f;

    public void SpawnSatellite()
    {
        if (satellitePrefab == null) return;

        Vector3 spawnPos = new Vector3(
            transform.position.x,
            spawnHeight,
            transform.position.z);

        Instantiate(satellitePrefab, spawnPos, Quaternion.identity);
        // Ses SatelliteCrash.Awake() içinde otomatik başlıyor
    }
}