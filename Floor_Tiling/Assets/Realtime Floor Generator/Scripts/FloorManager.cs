using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FloorManager : MonoBehaviour{
    public static FloorManager Instance{ get; private set; }

    private void Awake(){
        Instance = this;
        _floorList = new List<Floor>();
    }

    [SerializeField] private List<PoolObjectSO> floorPrefabs;
    [SerializeField] private int spawnRadius = 50;

    private List<Floor> _floorList;
    

    private  PoolObjectSO FloorPrefab => floorPrefabs[Random.Range(0, floorPrefabs.Count)];

    private void Start(){
        // var floor = Instantiate(FloorPrefab, transform);
        var floor = ObjectPoolerV2.Instance.SpawnFromPool(FloorPrefab, transform.position);
        var floorScript = floor.GetComponent<Floor>();
        _floorList.Add(floorScript);
        
        while (true){
            var newFloors =PopulateFloor(transform ,out var continueLoop);
            _floorList.AddRange(newFloors);
            if (continueLoop == false){
                break;
            }
        }
    }

    private List<Floor> PopulateFloor(Transform center,out bool continueLoop){
        var newTempList = new List<Floor>();
        var length = _floorList.Count;
        continueLoop = false;
        for (int i = 0; i < length; i++){
            var floor = _floorList[i];
            var spawnPosList = floor.SpawnPositions;
            
            foreach (var tra in spawnPosList){
                if (CheckEmptyPosition(tra, _floorList) && CheckEmptyPosition(tra, newTempList)){
                    // Debug.Log("Empty spot found");
                    if (Vector3.Distance(center.position, tra.position) < spawnRadius){
                        Debug.Log("Distance is: " + Vector3.Distance(center.position, tra.position));
                        // var newFloor = Instantiate(FloorPrefab, transform);
                        var newFloor = ObjectPoolerV2.Instance.SpawnFromPool(FloorPrefab, center.position);
                        newFloor.transform.position = tra.position;
                        var floorScript = newFloor.GetComponent<Floor>();
                        newTempList.Add(floorScript);
                        continueLoop = true;
                    }
                }
                // Debug.Log("Empty spot not found");
            }
        }

        return newTempList;
    }

    private bool CheckEmptyPosition(Transform trans, List<Floor> checkList){
        foreach (var floor in checkList){
            if (floor.transform.position == trans.position){
                return false;
            }
        }

        return true;
    }

    private List<Floor> GetOutOfRadiusFloors(Transform center){
        var tempList = new List<Floor>();
        foreach (var floor in _floorList){
            if (Vector3.Distance(center.position, floor.transform.position) > spawnRadius){
                tempList.Add(floor);
            }
        }
        return tempList;
    }
    
    private void DeleteFloors(List<Floor> floors){
        
        foreach (var floor in floors){
            _floorList.Remove(floor);
            // Destroy(floor.gameObject);
            // floor.gameObject.SetActive(false);
            ObjectPoolerV2.Instance.ReturnToPool(floor.gameObject);
        }
    }

    public void UpdateFloors(Transform center){
        var floorsToDelete = GetOutOfRadiusFloors(center);
        DeleteFloors(floorsToDelete);
        
        while (true){
            var newFloors =PopulateFloor(center ,out var continueLoop);
            _floorList.AddRange(newFloors);
            if (continueLoop == false){
                break;
            }
        }
    }
}