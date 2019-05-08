using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DungeonPathGenerator : MonoBehaviour
{
    public DungeonCell[] dungeonCellPrefabs;
    public DungeonPath dungeonPath;
    public DungeonMovement player;
    public int cellCount;
    public int branchCount;

    [ContextMenu("Generate")]
    public void Generate()
    {
        StartCoroutine(GenerateCoroutine());
    }

    private IEnumerator GenerateCoroutine()
    {
        if (dungeonPath != null)
        {
            foreach (DungeonCell c in dungeonPath.dungeonCells)
            {
                if (c != null) Destroy(c.gameObject);
            }
        }

        dungeonPath = new DungeonPath();
        dungeonPath.dungeonCells = new DungeonCell[cellCount];
        StartCell();
        yield return new WaitForSeconds(0.1f);

        for (int i = 1; i < cellCount; i++)
        {
            // AddCell(i);
            yield return AddCell(i);
            if (dungeonPath.dungeonCells[i - 1].blocked)
            {
                Generate();
                yield break;
            }
        }

        for (int i = 0; i < branchCount; i++)
        {
            //find a start point for the branch
            var startCell = dungeonPath.dungeonCells[UnityEngine.Random.Range(2, cellCount - 2)];
        }

        player.InitPlayer(dungeonPath);
    }


    private IEnumerator AddCell(int i)
    {
        var cell = Instantiate(RandomCellPrefab());
        dungeonPath.dungeonCells[i] = cell;

        cell.gameObject.name = "Cell " + i;

        yield return cell.Connect(dungeonPath.dungeonCells[i - 1]);

    }

    private void StartCell()
    {
        var cell = Instantiate(RandomCellPrefab());
        cell.start = true;
        dungeonPath.startPoint = cell.RandomConnection();
        dungeonPath.startPoint.connected = true;
        cell.gameObject.name = "StartCell";
        dungeonPath.dungeonCells[0] = cell;
    }

    private DungeonCell RandomCellPrefab()
    {
        return dungeonCellPrefabs[UnityEngine.Random.Range(0, dungeonCellPrefabs.Length)];
    }
}
