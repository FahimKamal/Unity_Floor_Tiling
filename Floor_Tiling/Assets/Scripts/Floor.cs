
using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Floor : MonoBehaviour{
    [SerializeField] private Transform rightPos;
    [SerializeField] private Transform leftPos;
    [SerializeField] private Transform topPos;
    [SerializeField] private Transform bottomPos;

    private Floor _rightLink = null;
    private Floor _leftLink = null;
    private Floor _topLink = null;
    private Floor _bottomLink = null;

    private void Awake(){
        _rightLink = null;
        _leftLink = null;
        _topLink = null;
        _bottomLink = null;
    }

    public void SetLeftLink(Floor floor){
        _leftLink = floor;
    }
    public void SetRightLink(Floor floor){
        _rightLink = floor;
    }
    public void SetTopLink(Floor floor){
        _topLink = floor;
    }
    public void SetBottomLink(Floor floor){
        _bottomLink = floor;
    }

    public void StartCycle(Transform center, int radius, GameObject prefab, FloorManager floorManager){
        _rightLink = CreateNewFloorAndLinkThem(center, radius, rightPos, _rightLink, prefab);
        _leftLink = CreateNewFloorAndLinkThem(center, radius, leftPos, _leftLink, prefab);
        _topLink = CreateNewFloorAndLinkThem(center, radius, topPos, _topLink, prefab);
        _bottomLink = CreateNewFloorAndLinkThem(center, radius, bottomPos, _bottomLink, prefab);

        if (_rightLink != null){
            _rightLink.SetLeftLink(this);
            _rightLink.StartCycle(center, radius, prefab, floorManager);
        }
        
        if (_leftLink != null){
            _leftLink.SetRightLink(this);
            _leftLink.StartCycle(center, radius, prefab, floorManager);
        }
        
        if (_topLink != null){
            _topLink.SetBottomLink(this);
            _topLink.StartCycle(center, radius, prefab, floorManager);
        }
        
        if (_bottomLink != null){
            _bottomLink.SetTopLink(this);
            _bottomLink.StartCycle(center, radius, prefab, floorManager);
        }
        
        FloorManager.Instance.SubmitFloor(this);
    }

    private Floor CreateNewFloorAndLinkThem(Transform center, int radius, Transform spawnPos, Floor link, GameObject prefab){
        // If the link is already connected them do nothing
        if (link != null){
            return link;
        }
        
        // Check if there is any Floor prefab present at that location or not.
        var linkScript = spawnPos.GetComponent<Link>();
        if (linkScript.TryReturnFloorTile(out var floorTile)){
            return floorTile;
        }
        
        // Check if the spawnPos is inside the radius
        if (Vector3.Distance(center.position, spawnPos.position) < radius){
            // var floor = Instantiate(prefab, spawnPos.position, Quaternion.identity, FloorManager.Instance.transform);
            var floor = Instantiate(prefab, FloorManager.Instance.transform);
            floor.transform.position = spawnPos.position;
            var floorScript = floor.GetComponent<Floor>();
            return floorScript;
        }

        return null;

    }
}
