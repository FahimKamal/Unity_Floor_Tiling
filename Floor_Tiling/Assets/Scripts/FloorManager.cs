using System;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour{
    public static FloorManager Instance{ get; private set; }

    private void Awake(){
        Instance = this;
        _floorList = new List<Floor>();
    }

    [SerializeField] private GameObject floorPrefab;
    [SerializeField, Range(20,500)] private int spawnRadius = 500;

    private List<Floor> _floorList;
    

    private void Start(){
        var floor = Instantiate(floorPrefab, transform);
        var floorScript = floor.GetComponent<Floor>();
        _floorList.Add(floorScript);
        
        PopulateFloor();
        PopulateFloor();
        PopulateFloor();
    }

    private void PopulateFloor(){
        var continueLoop = true;
        // while (continueLoop){
            var length = _floorList.Count;
            for (int i = 0; i < length; i++){
                var floor = _floorList[i];
                var spawnPosList = floor.SpawnPositions;
                continueLoop = false;
                
                // var newTempList = new List<Floor>();
                foreach (var tra in spawnPosList){
                    // create a box raycast and find if any floor object is present there or not
                    var halfext = new Vector3(0.5f, 0.5f, 0.5f);
                    var boxCast = Physics.BoxCast(tra.position, halfext, Vector3.up, out var hitInfo);
                    if (boxCast && hitInfo.collider.CompareTag("Floor")){
                        Debug.Log("Floor is present");
                        continue;
                    }

                    if (Vector3.Distance(transform.position, tra.position) < spawnRadius){
                        var newFloor = Instantiate(floorPrefab, transform);
                        newFloor.transform.position = tra.position;
                        var floorScript = newFloor.GetComponent<Floor>();
                        _floorList.Add(floorScript);
                        // continueLoop = true;
                    }
                    
                    // _floorList.AddRange(newTempList);
                }
            }
        // }
    }
}
