using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonCanvas : MonoBehaviour
{
    public RectTransform navRect;
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
        MoveToWorldPoint(connection.heightOffset.transform.position, butt.GetComponent<RectTransform>());
        navigationButtons.Add(butt);
    }
       
    public void MoveToWorldPoint(Vector3 objectTransformPosition, RectTransform button)
    {
        var pos = cam.WorldToViewportPoint(objectTransformPosition);

        var x = Mathf.Clamp(pos.x, 0.0f, 1.0f);
        var y = Mathf.Clamp(pos.y, 0.0f, 1.0f);

        button.anchoredPosition = new Vector2(navRect.rect.width * x, navRect.rect.height * y);
    }
}
