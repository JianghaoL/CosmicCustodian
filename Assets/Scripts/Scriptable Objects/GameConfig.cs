using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Scriptable Objects/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Level")] 
    public int levelNumber = 1;
    
    [Space(10)]
    [Header("Move")]
    public float moveDuration = 0.5f;
}
