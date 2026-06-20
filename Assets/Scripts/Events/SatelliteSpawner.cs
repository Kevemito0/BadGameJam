using UnityEngine;

public class SatelliteSpawner : MonoBehaviour
{
    [SerializeField] private GameObject satellitePrefab;
    [SerializeField] private float spawnHeight = 100f;
    private void Start()
    {
        Vector3 spawnPos = new Vector3(gameObject.transform.position.x, spawnHeight, gameObject.transform.position.z);

        Instantiate(satellitePrefab, spawnPos, Quaternion.identity);
    }
    
    
}