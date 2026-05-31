using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MapConstructor : MonoBehaviour, IInitializable
{
    [Header("Map")] 
    [SerializeField] private MapDataSO walls;
    [SerializeField] private MapDataSO players;
    [SerializeField] private MapDataSO boxes;
    [SerializeField] private MapDataSO destinations;

    [Header("Map Cont.")] 
    [SerializeField] private GridBlockSO platform;

    [SerializeField] private GridBlockSO support;
    [SerializeField] private int supportLayers = 3;
    
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
        StartCoroutine(WaitForAnimation());
        
        IEnumerator WaitForAnimation()
        {
            yield return new WaitForSecondsRealtime(GameDataManager.Instance.GetConfig().startDelayMax + GameDataManager.Instance.GetConfig().riseDuration);
            GameEventsManager.OnMapConstructed.Invoke();
            GameEventsManager.OnMapConstructedTutorial.Invoke(TutorialState.MapConstructed);
        }
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

            Transform t = _mapParent.transform;
            switch (type)
            {
                case BlockType.Wall: t = MakeWall(coord); break;
                case BlockType.Player: MakePlayer(coord); break;
                case BlockType.Box: MakeBox(coord); break;
                case BlockType.Destination: t = MakeDestination(coord); break;
                // Leave BlockType.Void as blank. There is no game object for void.
            }
            
            if (type is not BlockType.Destination) MakePlatform(coord, t);
            
            // Update the scale of the map.
            // Use the width and height to set the camera position.
            if (coord.x >  _width)
                _width = coord.x;
            if (coord.y < _height)
                _height = coord.y;
        }
        
        ConstructSupport(_width, _height, supportLayers);
    }

    private void ConstructSupport(int width, int height, int layers)
    {
        for (int i = 1; i <= layers; i++)
        {
            Transform t;
            t = MakeSupport(new Vector2Int(width, height), i);
            t.GetComponent<RocketParticle>().SetShouldPlay(true);
            
            t = MakeSupport(new Vector2Int(0, height), i);
            t.GetComponent<RocketParticle>().SetShouldPlay(true);
            
            MakeSupport(new Vector2Int(width, 0), i);
            MakeSupport(new Vector2Int(0, 0), i);
        }
    }

    private Transform MakeSupport(Vector2Int coord, int layers)
    {
        var data = support;
        var prefab = data.blockPrefab;
        var pos = new Vector3(coord.x, data.yOffset * layers, coord.y);
        
        var o = Instantiate(prefab, pos, Quaternion.identity);
        o.name = "Support";
        o.transform.rotation = Quaternion.Euler(data.rotation);
        o.transform.localScale *= data.scale;
        StartCoroutine(RiseRoutine(o.transform, pos));
        return o.transform;
    }

    private Transform MakeWall(Vector2Int coord)
    {
        var data = FetchRandomObject(walls);
        var prefab = data.blockPrefab;
        var pos = new Vector3(coord.x, data.yOffset, coord.y);
        
        var o = Instantiate(prefab, pos, Quaternion.identity);
        o.name = "Wall";
        o.transform.rotation = Quaternion.Euler(data.rotation);
        o.transform.localScale *= data.scale;
        StartCoroutine(RiseRoutine(o.transform, pos));
        o.transform.SetParent(_mapParent.transform);
        return o.transform;
    }

    private Transform MakePlayer(Vector2Int coord)
    {
        var data = FetchRandomObject(players);
        var prefab = data.blockPrefab;
        var pos = new Vector3(coord.x, data.yOffset, coord.y);
        
        var o = Instantiate(prefab, pos, Quaternion.identity);
        o.name = "Player";
        o.transform.rotation = Quaternion.Euler(data.rotation);
        o.transform.localScale *= data.scale;
        GameEventsManager.OnPlayerSpawned.Invoke(o.transform);
        StartCoroutine(RiseRoutine(o.transform, pos, true));
        return o.transform;
    }

    private Transform MakeBox(Vector2Int coord)
    {
        var data = FetchRandomObject(boxes);
        var prefab = data.blockPrefab;
        var pos = new Vector3(coord.x, data.yOffset, coord.y);
        
        var o = Instantiate(prefab, pos, Quaternion.identity);
        o.name = "Box";
        o.transform.rotation = Quaternion.Euler(data.rotation);
        o.transform.localScale *= data.scale;
        StartCoroutine(RiseRoutine(o.transform, pos, true));
        GameEventsManager.OnBoxSpawned.Invoke(o.transform);
        return o.transform;
    }

    private Transform MakeDestination(Vector2Int coord)
    {
        var data = FetchRandomObject(destinations);
        var prefab = data.blockPrefab;
        var pos = new Vector3(coord.x, data.yOffset, coord.y);
        
        var o = Instantiate(prefab, pos, Quaternion.identity);
        o.name = "Destination";
        o.transform.rotation = Quaternion.Euler(data.rotation);
        o.transform.localScale *= data.scale;
        StartCoroutine(RiseRoutine(o.transform, pos));
        o.transform.SetParent(_mapParent.transform);
        return o.transform;
    }

    private void MakePlatform(Vector2Int coord, Transform parent)
    {
        var data = platform;
        var prefab = data.blockPrefab;
        var pos = new Vector3(coord.x, data.yOffset, coord.y);
        
        var o = Instantiate(prefab, pos, Quaternion.identity);
        o.name = "Platform";
        o.transform.rotation = Quaternion.Euler(data.rotation);
        o.transform.localScale *= data.scale;
        StartCoroutine(RiseRoutine(o.transform, pos));
    }

    private IEnumerator RiseRoutine(Transform t, Vector3 position, bool fromAbove = false)
    {
        var y = fromAbove ? 50f : -200f;
        t.position = new Vector3(position.x, y, t.position.z);
        
        var startDelay = fromAbove ? 
            GameDataManager.Instance.GetConfig().startDelayMin
            : 
            Random.Range(GameDataManager.Instance.GetConfig().startDelayMin, GameDataManager.Instance.GetConfig().startDelayMax);
        
        yield return new WaitForSecondsRealtime(GameDataManager.Instance.GetConfig().cameraRotationDuration - startDelay);
        t.DOMoveY(position.y, GameDataManager.Instance.GetConfig().riseDuration);
    }

    /// <summary>
    /// Helper function to fetch a random game object prefab from SO list.
    /// </summary>
    /// <param name="data">An array of prefabs of certain type</param>
    /// <returns>GameObject prefab</returns>
    private GridBlockSO FetchRandomObject(MapDataSO data)
    {
        var index = Random.Range(0, data.blockSOs.Length);
        return data.blockSOs[index];
    }
    
    /// <summary>
    /// Set up camera position using the half-width and half-height
    /// </summary>
    private void CenterCamera()
    {
        var x = (float) _width / 2;
        var z = (float) _height / 2;

        var offset = GameDataManager.Instance.GetConfig().cameraOffset;
        var rotation = GameDataManager.Instance.GetConfig().cameraRotation;
        
        mainCamera.transform.rotation = Quaternion.Euler(Vector3.zero);
        
        mainCamera.transform.position = new Vector3(x + offset.x, yOffset, z + offset.z);
        mainCamera.transform.DORotateQuaternion(Quaternion.Euler(rotation), GameDataManager.Instance.GetConfig().cameraRotationDuration);
    }
}
