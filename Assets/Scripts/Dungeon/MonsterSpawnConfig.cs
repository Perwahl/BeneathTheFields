using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterSpawnConfig", menuName = "Dungeon/MonsterSpawnConfig", order = 1)]
public class MonsterSpawnConfig : ScriptableObject
{   
    public MonsterBlueprint[] type1;
    public int takeType1;

    public MonsterBlueprint[] type2;
    public int takeType2;

    public MonsterBlueprint[] type3;
    public int takeType3;

    public List<MonsterBlueprint> GetMonsters()
    {
        var monsterList = new List<MonsterBlueprint>();  

        for(int i=0; i< takeType1; i++)
        {
            monsterList.Add(type1[UnityEngine.Random.Range(0, type1.Length)]);
        }

        for (int i = 0; i < takeType2; i++)
        {
            monsterList.Add(type2[UnityEngine.Random.Range(0, type2.Length)]);
        }

        for (int i = 0; i < takeType3; i++)
        {
            monsterList.Add(type3[UnityEngine.Random.Range(0, type3.Length)]);
        }
        return monsterList;
    }
}
