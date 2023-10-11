using System;
using System.Collections.Generic;
using UnityEngine;

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
        public PoolObjectSO poolObjectSo;
        public int objectSize;
    }
    
    [SerializeField] List<Pool> pools;
    private Dictionary<PoolObjectSO, Queue<GameObject>> PoolDictionary;
    private List<GameObject> _objectTypeParent;

    private void PopulatePool(){
        PoolDictionary = new Dictionary<PoolObjectSO, Queue<GameObject>>();
        _objectTypeParent = new List<GameObject>();

        foreach (var pool in pools){
            var objParent = new GameObject(pool.poolObjectSo.ObjectName + "Parent");
            objParent.transform.position = Vector3.zero;
            _objectTypeParent.Add(objParent);
            objParent.transform.SetParent(transform);
            var objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.objectSize; i++){
                var obj = Instantiate(pool.poolObjectSo.Prefab, objParent.transform);
                obj.GetComponent<IPooledObject>().objectType = pool.poolObjectSo;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            
            PoolDictionary.Add(pool.poolObjectSo, objectPool);
        }
    }

    public GameObject SpawnFromPool(PoolObjectSO objectType, Vector3 position){
        if (!PoolDictionary.ContainsKey(objectType)){
            Debug.LogWarning("Pool with tag " + objectType.ObjectName + " doesn't exist.");
            return null;
        }

        if (PoolDictionary[objectType].Count == 0){
            Debug.LogWarning("Pool with tag " + objectType + " is empty.");
            var objParent = _objectTypeParent.Find(x => x.name == objectType.ObjectName + "Parent");
            // var poolObjType = pools.Find(x => x.poolObjectSo.ObjectName == objectType).poolObjectSo;
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
            obj.SetActive(false);
        }
        else{
            Destroy(obj);
            Debug.LogWarning("Object does not have IPooledObject component");
            return;            
        }
    }
}
