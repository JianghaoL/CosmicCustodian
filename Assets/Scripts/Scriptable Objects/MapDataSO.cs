using UnityEngine;

[CreateAssetMenu(fileName = "MapDataSO", menuName = "Scriptable Objects/Level Initialization/MapDataSO")]
public class MapDataSO : ScriptableObject
{
    public GridBlockSO[] blockSOs;
}
