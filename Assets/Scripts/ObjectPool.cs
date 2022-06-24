using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance = null;

    [SerializeField] private List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    private List<GameObject> objects;
    private List<string> tags = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        // else if (Instance != this)
        //     Destroy(gameObject);
        CreatePool();
    }
    private void CreatePool()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        objects = new List<GameObject>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.Size; i++)
            {
                GameObject obj = Instantiate(pool.Prefab, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.Tag, objectPool);
        }
    }
    public GameObject SpawnFromPool(string tag, Vector3 position, Vector3 rotation)
    {
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        poolDictionary[tag].Enqueue(objectToSpawn);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.eulerAngles = rotation;
        objectToSpawn.SetActive(true);
        objects.Add(objectToSpawn);
        tags.Add(tag);
        GameManager.Instance.CubeCount(tag);
        return objectToSpawn;
    }
}

[System.Serializable]
public struct Pool
{
    [SerializeField] string tag;
    [SerializeField] GameObject prefab;
    [SerializeField] int size;

    public string Tag { get { return tag; } }
    public GameObject Prefab { get { return prefab; } }
    public int Size { get { return size; } }
}