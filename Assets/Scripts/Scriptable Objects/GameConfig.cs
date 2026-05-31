using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Scriptable Objects/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Level")] 
    public int levelNumber = 1;

    [Header("Game Logistics")] 
    public float onWinWaitDelay = 3f;
    public int levelToLoadNumber = 1;
    private static readonly string LevelToLoadPrefix = "Level ";
    
    [Header("Camera")]
    public Vector3 cameraOffset = Vector3.zero;
    public Vector3 cameraRotation = Vector3.zero;
    public float cameraRotationDuration = 1.5f;

    [Header("Map Animation")] 
    public float startDelayMin = 0.3f;
    public float startDelayMax = 1.5f;

    public float riseDuration = 1f;
    
    [Space(10)]
    [Header("Move")]
    public float moveDuration = 0.5f;
    public float turnDuration = 0.1f;
    public Ease easeType = Ease.Linear;

    public string GetLevelToLoad()
    {
        return LevelToLoadPrefix + levelToLoadNumber;
    }
}
