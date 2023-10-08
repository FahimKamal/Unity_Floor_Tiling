using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour{
    public static FloorManager Instance{ get; private set; }

    private void Awake(){
        Instance = this;
        _floorList = new List<Floor>();
    }

    [SerializeField] private GameObject floorPrefab;
    [SerializeField, Range(20,5000)] private int spawnRadius = 500;

    private List<Floor> _floorList;
    

    private void Start(){
        var floor = Instantiate(floorPrefab, transform);
        var floorScript = floor.GetComponent<Floor>();
        _floorList.Add(floorScript);
        
        var newFloors =PopulateFloor();
        _floorList.AddRange(newFloors);
        newFloors = PopulateFloor();
        _floorList.AddRange(newFloors);
        newFloors = PopulateFloor();
        _floorList.AddRange(newFloors);
        newFloors = PopulateFloor();
        _floorList.AddRange(newFloors);
        // PopulateFloor();
    }

    private List<Floor> PopulateFloor(){
        var newTempList = new List<Floor>();
        var length = _floorList.Count;
        for (int i = 0; i < length; i++){
            var floor = _floorList[i];
            var spawnPosList = floor.SpawnPositions;

            
            foreach (var tra in spawnPosList){
                if (CheckEmptyPosition(tra, _floorList) && CheckEmptyPosition(tra, newTempList)){
                    Debug.Log("Empty spot found");
                    if (Vector3.Distance(transform.position, tra.position) < spawnRadius){
                        var newFloor = Instantiate(floorPrefab, transform);
                        newFloor.transform.position = tra.position;
                        var floorScript = newFloor.GetComponent<Floor>();
                        newTempList.Add(floorScript);
                    }
                }
                Debug.Log("Empty spot not found");
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
}