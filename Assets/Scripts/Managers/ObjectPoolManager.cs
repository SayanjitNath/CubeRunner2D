using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

    private GameObject objectsPoolEmptyHolder;

    private static GameObject particleSystemsEmpty;
    private static GameObject gameObjectsEmpty;


    public enum PoolType
    {
        ParticleSystem,
        GameObject,
        None
    }

    public static PoolType PoolingType;

    private void Awake()
    {
        Application.targetFrameRate = 90;
        SetUpEmpties();

        DontDestroyOnLoad(this.gameObject);
    }


    private void SetUpEmpties()
    {
        objectsPoolEmptyHolder = new GameObject("Pooled Objects");

        particleSystemsEmpty = new GameObject("Particle Effects");
        particleSystemsEmpty.transform.SetParent(objectsPoolEmptyHolder.transform);

        gameObjectsEmpty = new GameObject("Game Objects");
        gameObjectsEmpty.transform.SetParent(objectsPoolEmptyHolder.transform);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, PoolType poolType = PoolType.None)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookUpString == objectToSpawn.name);


        if (pool == null)
        {
            pool = new PooledObjectInfo { LookUpString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObject = null;
        spawnableObject = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObject == null)
        {
            GameObject parentObject = SetParentObject(poolType);
            spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

            if (parentObject != null)
            {
                spawnableObject.transform.SetParent(parentObject.transform);
            }
        }
        else
        {
            spawnableObject.transform.position = spawnPosition;
            spawnableObject.transform.rotation = spawnRotation;
            pool.InactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }

        return spawnableObject;
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Transform parentTransform)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.LookUpString == objectToSpawn.name);


        if (pool == null)
        {
            pool = new PooledObjectInfo { LookUpString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObject = null;
        spawnableObject = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObject == null)
        {
            spawnableObject = Instantiate(objectToSpawn, parentTransform);
        }
        else
        {
            pool.InactiveObjects.Remove(spawnableObject);
            spawnableObject.SetActive(true);
        }

        return spawnableObject;
    }


    public static void ReturnObjectToPool(GameObject returnedObj)
    {
        string poolName = returnedObj.name.Substring(0, returnedObj.name.Length - 7);

        PooledObjectInfo pool = ObjectPools.Find(p => p.LookUpString == poolName);

        if (pool == null)
        {
            Debug.LogWarning("Pool has not been created, still trying return the game object: " + returnedObj.name);
        }
        else
        {
            returnedObj.SetActive(false);
            pool.InactiveObjects.Add(returnedObj);
        }
    }


    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.None:
                return null;
            case PoolType.ParticleSystem:
                return particleSystemsEmpty;
            case PoolType.GameObject:
                return gameObjectsEmpty;
            default:
                return null;
        }
    }

}


public class PooledObjectInfo
{
    public string LookUpString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}