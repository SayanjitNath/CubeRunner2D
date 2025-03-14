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

    [SerializeField] private bool isSpawning = true;   

    private float spawnX;
    private float despawnX = -40f;
    private float xOffset = 5f;
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private CancellationTokenSource cancellationTokenSource;


    void Start()
    {
        spawnX = Camera.main.orthographicSize * Camera.main.aspect + xOffset;
        despawnX = -spawnX;
    }

    void Update()
    {
        MoveObjects();
        DespawnObjects();
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

            spawnedObjects[i].transform.position += Vector3.left * speed * Time.deltaTime;
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
                //Destroy(spawnedObjects[i]);
                ObjectPoolManager.ReturnObjectToPool(spawnedObjects[i]);
                spawnedObjects.RemoveAt(i);
            }
        }
    }

    public void StartSpawning(float updatedSpeed)
    {
        SetSpeed(updatedSpeed);
        isSpawning = true;
        cancellationTokenSource = new CancellationTokenSource();
        SpawnObjects(cancellationTokenSource.Token);
    }

    public void StopSpawning(float updatedSpeed)
    {
        SetSpeed(updatedSpeed);
        isSpawning = false;

        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();
        cancellationTokenSource = null;
    }

    async void SpawnObjects(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                if (!isSpawning)
                {
                    await Task.Delay(500); // Wait a bit before checking again
                    continue;
                }

                await Task.Delay(Random.Range((int)minSpawnInterval, (int)maxSpawnInterval), token);

                if (token.IsCancellationRequested || !isSpawning) return;


                GameObject toSpawn = Random.value > 0.5f ? orbzPrefab : obstaclePrefab;

                float minY = -3f;
                float maxY = 0f;
                float randomY = Random.Range(minY, maxY);

                if (toSpawn == obstaclePrefab) randomY = minY;

                //GameObject spawnedObject = Instantiate(toSpawn, new Vector3(spawnX, randomY, 0), Quaternion.identity);
                GameObject spawnedObject = ObjectPoolManager.SpawnObject(toSpawn,
                    new Vector3(spawnX, randomY, 0), Quaternion.identity, ObjectPoolManager.PoolType.GameObject);

                spawnedObjects.Add(spawnedObject);
            }
            catch (TaskCanceledException)
            {
                return;
            }
        }
    }

    public void SetSpeed(float speed) => this.speed = speed;



    void OnDestroy()
    {
        if (cancellationTokenSource != null)
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;
        }
    }
}
