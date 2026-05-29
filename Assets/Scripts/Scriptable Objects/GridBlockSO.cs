using UnityEngine;

[CreateAssetMenu(fileName = "GridBlockSO", menuName = "Scriptable Objects/Level Initialization/GridBlockSO")]
public class GridBlockSO : ScriptableObject
{
    public GameObject blockPrefab;
    public float yOffset = 0f;
    public Vector3 rotation = Vector3.zero;
    public float scale = 1f;
}
