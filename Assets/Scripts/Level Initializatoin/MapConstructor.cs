using System.Collections.Generic;
using UnityEngine;

public class MapConstructor : MonoBehaviour
{
    [Header("Map")] 
    [SerializeField] private MapDataSO walls;
    [SerializeField] private MapDataSO players;
    [SerializeField] private MapDataSO boxes;
    [SerializeField] private MapDataSO destinations;
    
    [Space(10)]
    [Header("Camera Settings")]
    [SerializeField] private Transform mainCamera;
    [SerializeField] private float yOffset = 10f;
    
    
    
    private Dictionary<Vector2Int, GridBlock> _coordToGridBlock;

    private int _width; // Max width of the map
    private int _height; // Max height of the map
    
    // For debug purposes
    public GameObject prefab;
    
    private void Awake()
    {
        GameEventsManager.OnLevelDataLoaded.AddListener(OnLevelDataLoaded);
    }

    private void OnDestroy()
    {
        GameEventsManager.OnLevelDataLoaded.RemoveListener(OnLevelDataLoaded);
    }
    
    private void OnLevelDataLoaded(Dictionary<Vector2Int, GridBlock> coordToGridBlock)
    {
        _coordToGridBlock = coordToGridBlock;

        _width = 0;
        _height = 0;

        ConstructMap();
        CenterCamera();
    }

    private void ConstructMap()
    {
        foreach (var gb in _coordToGridBlock)
        {
            var coord = gb.Key;
            var type = gb.Value.type;
            
            
            // GameObject o = Instantiate(prefab);
            // o.name = type.ToString();
            // o.transform.position = new Vector3(coord.x, 0, coord.y);
            // Material mat = o.GetComponent<Renderer>().material;
            // switch (type)
            // {
            //     case BlockType.Wall: mat.color = Color.white; break;
            //     case BlockType.Player: mat.color = Color.blue; break;
            //     case BlockType.Box: mat.color = Color.green; break;
            //     case BlockType.Destination: mat.color = Color.red; break;
            // }
            // o.GetComponent<Renderer>().material = mat;


            switch (type)
            {
                case BlockType.Wall: MakeWall(coord); break;
                case BlockType.Player: MakePlayer(coord); break;
                case BlockType.Box: MakeBox(coord); break;
                case BlockType.Destination: MakeDestination(coord); break;
                default: break;
            }
            
            
            if (coord.x >  _width)
                _width = coord.x;
            if (coord.y < _height)
                _height = coord.y;
        }
    }

    private void MakeWall(Vector2Int coord)
    {
        
    }

    private void MakePlayer(Vector2Int coord)
    {
        
    }

    private void MakeBox(Vector2Int coord)
    {
        
    }

    private void MakeDestination(Vector2Int coord)
    {
        
    }
    
    
    private void CenterCamera()
    {
        var x = (float) _width / 2;
        var z = (float) _height / 2;
        
        mainCamera.transform.position = new Vector3(x, yOffset, z);
    }
}
