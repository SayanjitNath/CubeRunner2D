using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    [SerializeField] private GameObject orbzPrefab;
    [SerializeField] private GameObject obstaclePrefab;

    [SerializeField] private float minSpawnInterval = 1000; 
    [SerializeField] private float maxSpawnInterval = 2500;
    [SerializeField] private float speed = 5f;


    private float spawnX;
    private float despawnX = -40f;
    private float xOffset = 5f;
    private float speedMultiplier = 1f;
    private float spawnIntervalMultiplier = 1f;
    private float minSpawnMultiplier = 0.5f;
    private bool isSpawning = true;
    private bool lastSpawnedWasObstacle = false; 
    private int orbzSpawnCount = 0;
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private Coroutine spawningCoroutine;


    void Start()
    {
        spawnX = Camera.main.orthographicSize * Camera.main.aspect + xOffset;
        despawnX = -spawnX;
    }

    void Update()
    {
        MoveObjects();
        DespawnObjects();

        // Increase Interactable Move speed with time
        speedMultiplier += Time.deltaTime * 0.01f;
        // Decrease Interactable
        spawnIntervalMultiplier = Mathf.Max(minSpawnMultiplier, spawnIntervalMultiplier - Time.deltaTime * 0.005f);
    }

    private void MoveObjects()
    {
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i] == null)
            {
                spawnedObjects.RemoveAt(i);
                continue;
            }

            spawnedObjects[i].transform.position += Vector3.left * speed * speedMultiplier * Time.deltaTime;
        }
    }

    private void DespawnObjects()
    {
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i] == null)
            {
                spawnedObjects.RemoveAt(i);
                continue;
            }

            if (spawnedObjects[i].transform.position.x <= despawnX)
            {
                ObjectPoolManager.ReturnObjectToPool(spawnedObjects[i]);
                spawnedObjects.RemoveAt(i);
            }
        }
    }

    public void StartSpawning(float updatedSpeed)
    {
        SetSpeed(updatedSpeed, false);
        isSpawning = true;

        if (spawningCoroutine != null)
            StopCoroutine(spawningCoroutine);

        spawningCoroutine = StartCoroutine(SpawnObjects());
    }

    public void StopSpawning(float updatedSpeed)
    {
        SetSpeed(updatedSpeed, false);
        isSpawning = false;

        if (spawningCoroutine != null)
        {
            StopCoroutine(spawningCoroutine);
            spawningCoroutine = null;
        }
    }

    private IEnumerator SpawnObjects()
    {
        while (isSpawning)
        {

            if (!isSpawning) yield break ;

            GameObject toSpawn;

            if (lastSpawnedWasObstacle)
            {
                toSpawn = orbzPrefab;
                orbzSpawnCount++;
            }
            else if (orbzSpawnCount >= 2)
            {
                toSpawn = obstaclePrefab;
                orbzSpawnCount = 0;
            }
            else
            {
                toSpawn = Random.value > 0.5f ? orbzPrefab : obstaclePrefab;
                orbzSpawnCount = (toSpawn == orbzPrefab) ? orbzSpawnCount + 1 : 0;
            }

            lastSpawnedWasObstacle = (toSpawn == obstaclePrefab);

            float minY = -3f;
            float maxY = 0f;
            float randomY = (toSpawn == obstaclePrefab) ? minY : Random.Range(minY, maxY);

            GameObject spawnedObject = ObjectPoolManager.SpawnObject(toSpawn,
                new Vector3(spawnX, randomY, 0), Quaternion.identity, ObjectPoolManager.PoolType.GameObject);

            spawnedObjects.Add(spawnedObject);
            
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval) * spawnIntervalMultiplier);
        }
    }

    public void SetSpeed(float speed, bool resetMultipliers)
    {
        this.speed = speed;
        if (resetMultipliers)
        {
            speedMultiplier = 1f;
            spawnIntervalMultiplier = 1f;
        }
    }
}
