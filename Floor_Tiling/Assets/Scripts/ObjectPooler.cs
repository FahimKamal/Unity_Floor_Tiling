using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance{ get; private set; }
    private void Awake() { 
        if (Instance != null && Instance != this){
            Destroy(gameObject);
        }
        else{
            Instance = this;
        }
    }

    public enum ObjectTag{
        Floor_1,
        Floor_2,
        Floor_3,
        Floor_4,
    }
    [Serializable]
    public class Pool{
        public ObjectTag objectTag;
        public GameObject prefab;
        public int objectSize;
    }
    
    [SerializeField] List<Pool> pools;
    public Dictionary<ObjectTag, Queue<GameObject>> PoolDictionary;
    // Start is called before the first frame update
    void Start()
    {
        PoolDictionary = new Dictionary<ObjectTag, Queue<GameObject>>();

        foreach (var pool in pools){
            var emptyObj = new GameObject(Enum.GetName(typeof(ObjectTag), pool.objectTag));
            emptyObj.transform.SetParent(transform);
            var objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.objectSize; i++){
                var obj = Instantiate(pool.prefab, emptyObj.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            
            PoolDictionary.Add(pool.objectTag, objectPool);
        }
    }

    public GameObject SpawnFromPool(ObjectTag tag, Vector3 position){
        if (!PoolDictionary.ContainsKey(tag)){
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }
        var objToSpawn = PoolDictionary[tag].Dequeue();

        objToSpawn.SetActive(true);
        objToSpawn.transform.position = position;
        // objToSpawn.transform.rotation = rotation;
        
        var pooledObj = objToSpawn.GetComponent<IPooledObject>();
        pooledObj?.OnObjectSpawn();
        
        PoolDictionary[tag].Enqueue(objToSpawn);
        
        return objToSpawn;
    }
}
