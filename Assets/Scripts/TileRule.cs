using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileRule
{
    
    public List<int> affectedTiles = new List<int>();

   
    public string ruleName;

    public TileRule(string name = "CustomRule")
    {
        ruleName = name;
    }

    public void AddTile(int index)
    {
        if (!affectedTiles.Contains(index))
            affectedTiles.Add(index);
    }

    public void Clear()
    {
        affectedTiles.Clear();
    }

    public List<int> GetAffectedTiles()
    {
        return affectedTiles;
    }
}
