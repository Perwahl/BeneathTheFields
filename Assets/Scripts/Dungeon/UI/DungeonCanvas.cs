using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonCanvas : MonoBehaviour
{
    public Canvas canvas;
    public DungeonMovement player;
    public Button navigationButtonPrefab;

    public Camera cam;

    public List<Button> navigationButtons;

    public void MoveToPoint(CellConnection connection)
    {
        player.MoveToPoint(connection);
    }

    internal void SetupCellNavigation(DungeonCell owner)
    {
        AddNavigationPoints(owner);
    }

    internal void AddNavigationPoints(DungeonCell owner)
    {       
        navigationButtons = new List<Button>();

        foreach (CellConnection connection in owner.connectionPoints)
        {
            if (!connection.isCurrent && connection.connected)
            {
                NavigationPoint(connection);
            }
        }
    }

    internal void ClearCellNavigation()
    {
        if (navigationButtons != null)
        {
            foreach (Button b in navigationButtons)
            {
                Destroy(b.gameObject);
            }
        }
    }

    private void NavigationPoint(CellConnection connection)
    {
        var butt = Instantiate(navigationButtonPrefab, this.transform);
        butt.onClick.AddListener(() => { MoveToPoint(connection.connectedTo); });
        MoveToWorldPoint(connection.transform.position, butt.GetComponent<RectTransform>());
        navigationButtons.Add(butt);
    }

    public void MoveToWorldPoint(Vector3 objectTransformPosition, RectTransform button)
    {
        var canvasRect = canvas.GetComponent<RectTransform>();
        var uiOffset = new Vector2((float)canvasRect.rect.height / 2f, (float)canvasRect.rect.width / 2f);

        // Get the position on the canvas
        Vector2 ViewportPosition = cam.WorldToViewportPoint(objectTransformPosition);
        Vector2 proportionalPosition = new Vector2(ViewportPosition.x * canvasRect.rect.height, ViewportPosition.y * canvasRect.rect.width);
        Debug.Log("setting button postion: " + proportionalPosition);
        // Set the position and remove the screen offset
        button.localPosition = proportionalPosition - uiOffset;
       // button.anchoredPosition = ViewportPosition;
    }
}
