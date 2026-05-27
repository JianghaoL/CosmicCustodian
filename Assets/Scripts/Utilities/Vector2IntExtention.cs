using UnityEngine;

public static class Vector2IntExtention
{
    public class AllDirections
    {
        public static Vector2Int[] Directions => new []{Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};
    }
    public static Vector2Int GetNextMoveFromDirection(Vector2Int dir, Transform t)
    {
        var currentCoord = new Vector2Int(Mathf.RoundToInt(t.position.x), Mathf.RoundToInt(t.position.z));
        var toGo = currentCoord + dir;
        return toGo;
    }

    public static Vector2Int GetNextMoveFromDirection(Vector2Int dir, Vector2Int currentCoord)
    {
        var toGo = currentCoord + dir;
        return toGo;
    }
    
    public static Vector2Int Vector2ToCoord(Vector2 v)
    {
        if (Mathf.Abs(v.x) > Mathf.Abs(v.y))
            return v.x > 0 ? Vector2Int.right : Vector2Int.left;

        if (Mathf.Abs(v.y) > 0.1f)
            return v.y > 0 ? Vector2Int.up : Vector2Int.down;

        return Vector2Int.zero;
    }
}
