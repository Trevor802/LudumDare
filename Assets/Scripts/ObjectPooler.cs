using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    #region Singleton
    public static ObjectPooler Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region PoolStructure
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    #endregion

    public List<Pool> pools;

    protected Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Start()
    {
        // Pools initialization
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            GameObject rootObject = new GameObject(pool.tag + "_pool");
            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, rootObject.transform);
                objectPool.Enqueue(obj);
                obj.SetActive(false);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
        // Initialization ends
    }

    // Use this function to spawn a "new" prefab
    public GameObject SpawnFromPool(string tag, Vector3 position,
        Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pools don't contain the pool named: " + tag);
            return null;
        }
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);
        if (objectToSpawn.GetComponent<IPooledObject>() != null)
            objectToSpawn.GetComponent<IPooledObject>().OnObjectSpawn();
        // poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn; 
    }
}
