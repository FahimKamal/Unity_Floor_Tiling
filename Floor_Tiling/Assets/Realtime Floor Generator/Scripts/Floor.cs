using System.Collections.Generic;
using UnityEngine;

public class Floor : IPooledObject
{
    [SerializeField] private List<Transform> spawnPositions;

    public List<Transform> SpawnPositions => spawnPositions;

    private void OnTriggerEnter(Collider other){
        // if (other.CompareTag("Player")){
            FloorManager.Instance.UpdateFloors(transform);
        // }

        // Debug.Log("found something");
    }

    public override void OnObjectSpawn(){
        
    }

    public override void OnObjectDeSpawn(){
        
    }
}