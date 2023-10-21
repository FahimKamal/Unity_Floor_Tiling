using UnityEngine;

public abstract class IPooledObject : MonoBehaviour{
    public PoolObject objectType;
    public abstract void OnObjectSpawn();
    public abstract void OnObjectDeSpawn();
}