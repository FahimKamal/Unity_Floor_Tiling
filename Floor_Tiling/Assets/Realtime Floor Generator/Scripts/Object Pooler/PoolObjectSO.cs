using UnityEngine;

[CreateAssetMenu(fileName = "Pool Object SO", menuName = "ScriptableObjects/Pool Object SO")]
public class PoolObjectSO : ScriptableObject{
    [SerializeField, SerializeReference]private string objectName;
    [SerializeField, SerializeReference]private GameObject prefab;
    
    public  GameObject Prefab{
        get => prefab;
        private set => prefab = value;
    }

    public string ObjectName{
        get => objectName;
        private set => objectName = value;
    }
}
