using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonCanvas : MonoBehaviour
{
    public DungeonMovement player;
    public Button navigationButtonPrefab;

    public List<Button> navigationButtons;

    public void MoveToPoint(CellConnection connection)
    {
        player.MoveToPoint(connection);
    }

    internal void SetupCellNavigation(DungeonCell owner)
    {
        if(navigationButtons!= null)
        {
            foreach (Button b in navigationButtons)
            {
                Destroy(b.gameObject);
            }
        }

        navigationButtons = new List<Button>();
        
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
        butt.onClick.AddListener(() => { MoveToPoint(connection.connectedTo); });
    }
}
