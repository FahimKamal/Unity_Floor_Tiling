using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPositions;

    public List<Transform> SpawnPositions => spawnPositions;
}