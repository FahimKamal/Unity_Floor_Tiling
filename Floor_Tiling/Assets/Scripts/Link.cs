using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Link : MonoBehaviour{
    private Floor _collidedFloor;

    private void OnTriggerEnter(Collider other){
        other.TryGetComponent<Floor>(out var floorTile);
        _collidedFloor = floorTile;
    }

    public bool TryReturnFloorTile(out Floor floorTile){
        if (_collidedFloor != null){
            floorTile = _collidedFloor;
            return true;
        }

        floorTile = null;
        return false;
    }
}
