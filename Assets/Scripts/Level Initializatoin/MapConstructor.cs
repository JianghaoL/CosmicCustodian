using System.Collections.Generic;
using UnityEngine;

public class MapConstructor : MonoBehaviour, IInitializable
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

    private GameObject _mapParent;
    

    public void InitializeOnAwake()
    {
        GameEventsManager.OnLevelDataLoaded.AddListener(OnLevelDataLoaded);
    }


    private void OnDestroy()
    {
        GameEventsManager.OnLevelDataLoaded.RemoveListener(OnLevelDataLoaded);
    }
    
    /// <summary>
    /// Construct game map using data loaded from the .txt files.
    /// </summary>
    /// <param name="coordToGridBlock">The data read from the files</param>
    private void OnLevelDataLoaded(Dictionary<Vector2Int, GridBlock> coordToGridBlock)
    {
        _coordToGridBlock = coordToGridBlock;

        _width = 0;
        _height = 0;

        ConstructMap();
        CenterCamera();
        
        // When the system finishes setting up the map, announce this message.
        GameEventsManager.OnMapConstructed.Invoke();
    }

    
    /// <summary>
    /// Helper function to construct a map.
    /// Creates a parent object "map" to hold all "Wall"s and "Destination".
    /// </summary>
    private void ConstructMap()
    {
        _mapParent = new GameObject("Map");
        foreach (var gb in _coordToGridBlock)
        {
            var coord = gb.Key;
            var type = gb.Value.type;
            
            switch (type)
            {
                case BlockType.Wall: MakeWall(coord); break;
                case BlockType.Player: MakePlayer(coord); break;
                case BlockType.Box: MakeBox(coord); break;
                case BlockType.Destination: MakeDestination(coord); break;
                // Leave BlockType.Void as blank. There is no game object for void.
            }
            
            // Update the scale of the map.
            // Use the width and height to set the camera position.
            if (coord.x >  _width)
                _width = coord.x;
            if (coord.y < _height)
                _height = coord.y;
        }
    }

    private void MakeWall(Vector2Int coord)
    {
        var prefab = FetchRandomObject(walls);
        var pos = new Vector3(coord.x, 0f, coord.y);
        
        var o = Instantiate(prefab, pos, Quaternion.identity);
        o.name = "Wall";
        o.transform.SetParent(_mapParent.transform);
    }

    private void MakePlayer(Vector2Int coord)
    {
        var prefab = FetchRandomObject(players);
        var pos = new Vector3(coord.x, 0.5f, coord.y);
        
        var o = Instantiate(prefab, pos, Quaternion.identity);
        o.name = "Player";
        GameEventsManager.OnPlayerSpawned.Invoke(o.transform);
    }

    private void MakeBox(Vector2Int coord)
    {
        var prefab = FetchRandomObject(boxes);
        var pos = new Vector3(coord.x, 0f, coord.y);
        
        var o = Instantiate(prefab, pos, Quaternion.identity);
        o.name = "Box";
        GameEventsManager.OnBoxSpawned.Invoke(o.transform);
    }

    private void MakeDestination(Vector2Int coord)
    {
        var prefab = FetchRandomObject(destinations);
        var pos = new Vector3(coord.x, 0f, coord.y);
        
        var o = Instantiate(prefab, pos, Quaternion.identity);
        o.name = "Destination";
        o.transform.SetParent(_mapParent.transform);
    }

    /// <summary>
    /// Helper function to fetch a random game object prefab from SO list.
    /// </summary>
    /// <param name="data">An array of prefabs of certain type</param>
    /// <returns>GameObject prefab</returns>
    private GameObject FetchRandomObject(MapDataSO data)
    {
        var index = Random.Range(0, data.blockSOs.Length);
        return data.blockSOs[index].blockPrefab;
    }
    
    /// <summary>
    /// Set up camera position using the half-width and half-height
    /// </summary>
    private void CenterCamera()
    {
        var x = (float) _width / 2;
        var z = (float) _height / 2;
        
        mainCamera.transform.position = new Vector3(x, yOffset, z);
    }
}
