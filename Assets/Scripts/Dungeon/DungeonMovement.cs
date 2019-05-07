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
        MoveToPoint(path.startPoint);
    }

    public void MoveToPoint(CellConnection point)
    {
        cam.MoveToPoint(point.cameraPosition.transform, currentPoint.owner.transform);
        currentPoint = point;
        currentPoint.isCurrent = true;

        dungeonCanvas.SetupCellNavigation(currentPoint.owner);


    }  
}
