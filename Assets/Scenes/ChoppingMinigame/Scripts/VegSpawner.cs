using UnityEngine;

public class VegetableSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] vegetablePrefabs;
    [SerializeField] private int vegetableCount = 5;
    
    [Header("Spawn Area")]
    [SerializeField] private float minX = -5f;
    [SerializeField] private float maxX = 5f;
    [SerializeField] private float spawnY = -6f;
    
    [Header("Spawn Timing")]
    [SerializeField] private float delayBetweenSpawns = 0.3f;

    void Start()
    {
        SpawnAllVegetables();
    }

    void SpawnAllVegetables()
    {
        for (int i = 0; i < vegetableCount; i++)
        {
            Invoke(nameof(SpawnVegetable), i * delayBetweenSpawns);
        }
    }

    void SpawnVegetable()
    {
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPos = new Vector3(randomX, spawnY, 0);
        
        GameObject veggiePrefab = vegetablePrefabs[Random.Range(0, vegetablePrefabs.Length)];
        GameObject veggie = Instantiate(veggiePrefab, spawnPos, Quaternion.identity);
        
    
    }
}