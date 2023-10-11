using UnityEngine;

public abstract class IPooledObject : MonoBehaviour{
    public PoolObjectSO objectType;
    public abstract void OnObjectSpawn();
    public abstract void OnObjectDeSpawn();
}