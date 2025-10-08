using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignalGridManager : MonoBehaviour
{
    public static SignalGridManager Instance;

    [Header("Grid Settings")]
    public int gridSize = 5;
    public GameObject tilePrefab;
    public Transform gridParent;
    public float tileSpacing = 60f;

    [Header("UI")]
    public TextMeshProUGUI feedbackText;
    public UIManager uiManager;

    public List<Tile> tiles = new List<Tile>();
    private List<TileRule> tileRules = new List<TileRule>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        GenerateGrid();
        GenerateRandomRules();
        GenerateSolvableGrid();
    }

    // ---------------- GRID CREATION ----------------
    void GenerateGrid()
    {
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                GameObject newTile = Instantiate(tilePrefab, gridParent);
                RectTransform rect = newTile.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(x * tileSpacing, -y * tileSpacing);

                int index = y * gridSize + x;
                Tile tile = newTile.GetComponent<Tile>();
                tile.Init(index, this);
                tiles.Add(tile);
            }
        }
    }

    // ---------------- RULE GENERATION ----------------
    void GenerateRandomRules()
    {
        tileRules.Clear();
        for (int i = 0; i < tiles.Count; i++)
        {
            TileRule rule = new TileRule($"Tile_{i}");
            int pattern = Random.Range(0, 6);
            Vector2Int pos = IndexToGrid(i);

            switch (pattern)
            {
                case 0: // Cross
                    AddIfValid(rule, pos.x, pos.y);
                    AddIfValid(rule, pos.x + 1, pos.y);
                    AddIfValid(rule, pos.x - 1, pos.y);
                    AddIfValid(rule, pos.x, pos.y + 1);
                    AddIfValid(rule, pos.x, pos.y - 1);
                    break;
                case 1: // Diagonals
                    AddIfValid(rule, pos.x, pos.y);
                    AddIfValid(rule, pos.x + 1, pos.y + 1);
                    AddIfValid(rule, pos.x - 1, pos.y - 1);
                    AddIfValid(rule, pos.x + 1, pos.y - 1);
                    AddIfValid(rule, pos.x - 1, pos.y + 1);
                    break;
                case 2: // Knight moves
                    AddIfValid(rule, pos.x, pos.y);
                    AddIfValid(rule, pos.x + 2, pos.y + 1);
                    AddIfValid(rule, pos.x + 2, pos.y - 1);
                    AddIfValid(rule, pos.x - 2, pos.y + 1);
                    AddIfValid(rule, pos.x - 2, pos.y - 1);
                    AddIfValid(rule, pos.x + 1, pos.y + 2);
                    AddIfValid(rule, pos.x - 1, pos.y + 2);
                    AddIfValid(rule, pos.x + 1, pos.y - 2);
                    AddIfValid(rule, pos.x - 1, pos.y - 2);
                    break;
                case 3: // Row
                    for (int x = 0; x < gridSize; x++)
                        AddIfValid(rule, x, pos.y);
                    break;
                case 4: // Column
                    for (int y = 0; y < gridSize; y++)
                        AddIfValid(rule, pos.x, y);
                    break;
                case 5: // Self-only
                    AddIfValid(rule, pos.x, pos.y);
                    break;
            }

            tileRules.Add(rule);
        }
    }

    void AddIfValid(TileRule rule, int x, int y)
    {
        if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
            rule.AddTile(GridToIndex(x, y));
    }

    int GridToIndex(int x, int y) => y * gridSize + x;
    Vector2Int IndexToGrid(int index) => new Vector2Int(index % gridSize, index / gridSize);

    // ---------------- TOGGLE FUNCTION ----------------
    public void ToggleGroup(int tileIndex)
    {
        if (tileIndex < 0 || tileIndex >= tileRules.Count) return;

        List<int> affected = tileRules[tileIndex].GetAffectedTiles();
        int changed = 0;

        foreach (int i in affected)
        {
            tiles[i].Toggle();
            changed++;
        }

        if (feedbackText != null)
            feedbackText.text = $"Tiles changed: {changed}";

        if (uiManager != null)
            uiManager.RegisterMove(changed);

        if (AreAllTilesOff())
        {
            if (feedbackText != null)
                feedbackText.text = " All Lights Off! Puzzle Solved!";

            if (uiManager != null)
                uiManager.ShowWinMessage();
        }
    }

    public bool AreAllTilesOff()
    {
        foreach (Tile tile in tiles)
            if (tile.IsOn) return false;
        return true;
    }

    public void GenerateSolvableGrid()
    {
       
        foreach (Tile tile in tiles)
            tile.SetState(false);

        
        int moves = Random.Range(3, 6);
        for (int i = 0; i < moves; i++)
        {
            int randomTile = Random.Range(0, tiles.Count);
            ToggleGroup(randomTile); 
        }
    }

}
