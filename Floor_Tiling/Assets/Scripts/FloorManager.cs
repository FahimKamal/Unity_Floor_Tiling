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
        floorScript.StartCycle(transform, spawnRadius, floorPrefab, this);
    }

    public void SubmitFloor(Floor floorTile){
        _floorList.Add(floorTile);
    }
}
