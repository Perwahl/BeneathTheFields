using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonCanvas : MonoBehaviour
{
    public DungeonMovement player;
    public Button navigationButtonPrefab;

    public void MoveToPoint(CellConnection connection)
    {
        player.MoveToPoint(connection);
    }

    internal void SetupCellNavigation(DungeonCell owner)
    {
        foreach(CellConnection connection in owner.connectionPoints)
        {
            if (!connection.isCurrent && connection.connected)
            {
                NavigationPoint(connection);
            }
        }
    }

    private void NavigationPoint(CellConnection connection)
    {
        var butt = Instantiate(navigationButtonPrefab, this.transform);
        butt.onClick.AddListener(() => { MoveToPoint(connection); });
    }
}
