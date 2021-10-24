using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton

    public static BulletPool Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<Pool> pools;
    [SerializeField]
    GameObject objToSpawn;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    private void OnEnable()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            {
                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    obj.transform.parent = transform;
                    objectPool.Enqueue(obj);
                }
                poolDictionary.Add(pool.tag, objectPool);
            }
        }
    }

    

    public GameObject spawnFromPool(string tag, Transform shootPoint, Vector3 direction) //Vector3 positon, Quaternion rotation, 
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            print("no such key (" + tag + ")");
            return null;
        }

        objToSpawn = poolDictionary[tag].Dequeue();

        objToSpawn.transform.position = shootPoint.position;
        objToSpawn.transform.forward = direction;
        objToSpawn.SetActive(true);

        poolDictionary[tag].Enqueue(objToSpawn);
        return objToSpawn;
    }

    public void destroyPoolBullet(string tag, GameObject bullet)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            print("no such key (" + tag + ")");
            return;
        }

        bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        bullet.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        bullet.SetActive(false);
    }
}
