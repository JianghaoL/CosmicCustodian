using UnityEngine;

[CreateAssetMenu(fileName = "TutorialSO", menuName = "Scriptable Objects/TutorialSO")]
public class TutorialSO : ScriptableObject
{
    [Header("Basics")]
    public int order;
    [TextArea(1, 5)] public string mobileTutorialText;
    [TextArea(1, 5)] public string desktopTutorialText;

    [Header("Special Effect")]
    [SerializeField] private GameObject specialEffectPrefab;
    [SerializeField] private Vector2Int specialEffectCoord;
    [SerializeField] private float yOffset;

    private GameObject _effect;

    public bool HasSpecialEffect()
    {
        return specialEffectPrefab != null;
    }
    
    public void StartSpecialEffect()
    {
        var pos = Vector2IntExtention.CoordToVector3(specialEffectCoord);
        pos.y = yOffset;
        
        _effect = Instantiate(specialEffectPrefab, pos, Quaternion.identity);
    }

    public void EndSpecialEffect()
    {
        Destroy(_effect);
    }
}
