using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectPoolerV2 : MonoBehaviour
{
    public static ObjectPoolerV2 Instance{ get; private set; }
    private void Awake() { 
        if (Instance != null && Instance != this){
            Destroy(gameObject);
        }
        else{
            Instance = this;
        }
        
        PopulatePool();
        
    }
    
    [Serializable]
    public class Pool{
        [FormerlySerializedAs("poolObjectSo")] public PoolObject poolObject;
        public int objectSize;
    }
    
    [SerializeField] private List<Pool> pools;
    private Dictionary<PoolObject, Queue<GameObject>> PoolDictionary;
    private Dictionary<PoolObject, GameObject> _objectTypeParent;

    private void PopulatePool(){
        PoolDictionary = new Dictionary<PoolObject, Queue<GameObject>>();
        _objectTypeParent = new Dictionary<PoolObject, GameObject>();

        foreach (var pool in pools){
            var objParent = new GameObject(pool.poolObject.ObjectName + "Parent");
            objParent.transform.position = Vector3.zero;
            _objectTypeParent[pool.poolObject] = objParent;
            objParent.transform.SetParent(transform);
            var objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.objectSize; i++){
                var obj = Instantiate(pool.poolObject.Prefab, objParent.transform);
                obj.GetComponent<IPooledObject>().objectType = pool.poolObject;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            
            PoolDictionary.Add(pool.poolObject, objectPool);
        }
    }

    public GameObject SpawnFromPool(PoolObject objectType, Vector3 position){
        if (!PoolDictionary.ContainsKey(objectType)){
            Debug.LogWarning("Pool with tag " + objectType.ObjectName + " doesn't exist.");
            return null;
        }

        if (PoolDictionary[objectType].Count == 0){
            Debug.LogWarning("Pool with tag " + objectType + " is empty.");
            var objParent = _objectTypeParent[objectType];
            var obj = Instantiate(objectType.Prefab, objParent.transform);
            obj.GetComponent<IPooledObject>().objectType = objectType;
            obj.SetActive(false);
            PoolDictionary[objectType].Enqueue(obj);
        }
        
        var objToSpawn = PoolDictionary[objectType].Dequeue();

        objToSpawn.SetActive(true);
        objToSpawn.transform.position = position;
        // objToSpawn.transform.rotation = rotation;
        
        var pooledObj = objToSpawn.GetComponent<IPooledObject>();
        pooledObj.OnObjectSpawn();

        return objToSpawn;
    }

    public void ReturnToPool(GameObject obj){
        if(obj.TryGetComponent<IPooledObject>(out var pooledObj)){
            var objType = pooledObj.objectType;
            PoolDictionary[objType].Enqueue(obj);
            pooledObj.OnObjectDeSpawn();
            obj.SetActive(false);
        }
        else{
            Destroy(obj);
            Debug.LogWarning("Object does not have IPooledObject component");
            return;            
        }
    }
}
