using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects;

public class DungeonMovement : MonoBehaviour
{
    private DungeonPath path;
    private CellConnection currentPoint;
    public SimpleCameraController cam;
    public DungeonCanvas dungeonCanvas;

    public void InitPlayer(DungeonPath path)
    {
        Debug.Log("init player");
        this.path = path;

        transform.position = path.startPoint.cameraPosition.position;
        transform.rotation = path.startPoint.cameraPosition.rotation;
        currentPoint = path.startPoint;
        currentPoint.isCurrent = true;

        dungeonCanvas.SetupCellNavigation(currentPoint.owner);

        // MoveToPoint(path.startPoint);
    }

    public void MoveToPoint(CellConnection point)
    {
        Debug.Log("Move to cell: " + point.owner.name);
        cam.MoveToPoint(point.cameraPosition.transform, currentPoint.owner.transform);
        currentPoint = point;
        currentPoint.isCurrent = true;

        dungeonCanvas.ClearCellNavigation();
        point.owner.SpawnMonsters(cam);


    }

    internal void MoveComplete()
    {
        if(currentPoint.owner.content.monsterBlueprints.Count > 0)
        {
            //currentPoint.owner.SpawnMonsters(cam);
            dungeonCanvas.SetupCellNavigation(currentPoint.owner);

        }
        else
        {
            dungeonCanvas.SetupCellNavigation(currentPoint.owner);
        }
    }
}
