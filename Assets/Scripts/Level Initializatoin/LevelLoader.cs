using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    // File path strings
    private const string LEVEL_PREFIX = "LevelData/level_";
    private const string FILE_POSTFIX = ".txt";
    
    [Tooltip("Level Number" )]
    [SerializeField] private int levelNumber;
    
    private bool _loadLevelSuccess;

    // Data loading paths
    private string _levelDataToLoad;
    private string _path;
    
    // A list of all grid blocks.
    // Used as data to construct maps.
    private List<GridBlock> _allGridBlocks;
    private Dictionary<Vector2Int, GridBlock> _coordToGridBlock;
    
    // Coordinates. Start with 0
    // x goes positive
    // y goes negative
    private int _x; // this is vector3.x
    private int _y; // this is vector3.z
    
    // For debug purposes
    public GameObject prefab;
    
    private void Awake()
    {
        _loadLevelSuccess = false;
        
        _levelDataToLoad = LEVEL_PREFIX + levelNumber.ToString() + FILE_POSTFIX;
        _path = Path.Combine(Application.streamingAssetsPath, _levelDataToLoad);
        
        _allGridBlocks = new List<GridBlock>();
        _coordToGridBlock = new Dictionary<Vector2Int, GridBlock>();
        _x = 0;
        _y = 0;
        
        ReadLevelData(_path);
        InitializeCoordToGridBlock();
        
        // Test();
    }

    private void Start()
    {
        if (_loadLevelSuccess)  GameEventsManager.OnLevelDataLoaded.Invoke(_coordToGridBlock); // Invoke a global manager level event to prevent from coupling
    }

    private void ReadLevelData(string path) // Read from a .txt file. Parse each line until EOF
    {
        try
        {
            using var sr = new StreamReader(path);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                ParseLine(line);
                _y -= 1;
            }
            _loadLevelSuccess = true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private void ParseLine(string line) // Parse each character from a line.
    {
        try
        {
            using var s = new StringReader(line);
            char c;
            _x = 0;
            while (s.Peek() >= 0)
            {
                c = (char) s.Read();
                var blockType = ParseBlockType(c); // Parse a character into a block type.
                    
                GridBlock gb = new GridBlock(_x, _y, blockType); // Use x and y to construct a new GridBlock
                _allGridBlocks.Add(gb);
                    
                _x += 1;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private BlockType ParseBlockType(char c) // See Assets/StreamingAssets/LevelData/Note.txt for better detail
    {
        BlockType b = BlockType.Void;
        switch (c)
        {
            case '#': b = BlockType.Wall; break;
            case '@': b = BlockType.Player; break;
            case '$': b = BlockType.Box; break;
            case '&': b = BlockType.Destination; break;
        }
        return b;
    }

    private void InitializeCoordToGridBlock()
    {
        _coordToGridBlock ??= new Dictionary<Vector2Int, GridBlock>();

        if (_allGridBlocks.Count == 0)
        {
            Debug.LogError("No grid blocks found");
            return;
        }
        
        foreach (var gb in _allGridBlocks)
        {
            _coordToGridBlock.Add(gb.coord, gb);
        }
    }

#if UNITY_EDITOR
    private void Test(BlockType blockType)
    {
        string s = "";
        switch (blockType)
        {
            case BlockType.Wall: s = "Wall"; break;
            case BlockType.Player: s = "Player"; break;
            case BlockType.Box: s = "Box"; break;
            case BlockType.Destination: s = "Destination"; break;
        }
        s += "\t";
        Debug.Log(s);
    }

    private void Test()
    {
        foreach (var gb in _coordToGridBlock)
        {
            var coord = gb.Key;
            var type = gb.Value.type;
            
            GameObject o = Instantiate(prefab);
            o.transform.position = new Vector3(gb.Value.coord.x, 0,  gb.Value.coord.y);
            Material mat = o.GetComponent<Renderer>().material;
            switch (type)
            {
                case BlockType.Wall: mat.color = Color.white; break;
                case BlockType.Player: mat.color = Color.blue; break;
                case BlockType.Box: mat.color = Color.green; break;
                case BlockType.Destination: mat.color = Color.red; break;
            }
            o.GetComponent<Renderer>().material = mat;
        }
    }
#endif
}
