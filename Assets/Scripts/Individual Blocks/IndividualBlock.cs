using UnityEngine;

public class IndividualBlock : MonoBehaviour
{
    private BlockType _blockType;
    
    public BlockType GetBlockType() => _blockType;
    
    public void SetBlockType(BlockType blockType)
    {
        if (_blockType is BlockType.Wall or BlockType.Destination) return;
        _blockType = blockType;
    }
    
    
}
