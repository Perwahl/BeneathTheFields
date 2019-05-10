using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonBlueprint", menuName = "Dungeon/DungeonBlueprint", order = 1)]

public class DungeonBlueprint : ScriptableObject
{
    public DungeonFloorBlueprint[] floors;
}
