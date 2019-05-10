using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonFloorBlueprint", menuName = "Dungeon/DungeonFloorBlueprint", order = 1)]
public class DungeonFloorBlueprint : ScriptableObject
{
    public int cellCount;
    public MonsterSpawnConfig[] monsterConfigs;

    [Tooltip("Feature available to spawn in this cell")]
    public DungeonFeatureBlueprint[] featureOptions;

    internal DungeonCellContent GetCellContent(DungeonCell.CellType type)
    {
        var c = new DungeonCellContent()
        {
            monsters = new List<MonsterBlueprint>()
        };
        if (type == DungeonCell.CellType.standard)
        {
            c.monsters = monsterConfigs[UnityEngine.Random.Range(0, monsterConfigs.Length)].GetMonsters();
        }
        return c;
    }
}
