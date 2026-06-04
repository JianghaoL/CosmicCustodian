using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] private Sprite highLightSprite;

    private GameObject _effect;
    private Image[] _highlightSlots;

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
    
    public bool HasHighLight()
    {
        return highLightSprite != null;
    }

    public void StartHighLight(params Image[] highlightSlot)
    {
        foreach (var slot in highlightSlot)
        {
            slot.enabled = true;
        }
        _highlightSlots = highlightSlot;
    }

    public void EndHighLight()
    {
        if (_highlightSlots == null) return;
        foreach (var slot in _highlightSlots)
        {
            slot.enabled = false;
        }
        _highlightSlots = null;
    }
}
