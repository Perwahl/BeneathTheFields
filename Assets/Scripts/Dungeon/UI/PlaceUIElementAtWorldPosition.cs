using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Place an UI element to a world position
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class PlaceUIElementAtWorldPosition : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 uiOffset;
    private Canvas dungeonCanvas;
    public Camera cam;

    /// <summary>
    /// Initiate
    /// </summary>
    void Init()
    {
        // Get the rect transform
        this.rectTransform = GetComponent<RectTransform>();
        dungeonCanvas = transform.root.GetComponent<Canvas>();

        // Calculate the screen offset
    }

    /// <summary>
    /// Move the UI element to the world position
    /// </summary>
    /// <param name="objectTransformPosition"></param>
    public void MoveToClickPoint(Vector3 objectTransformPosition)
    {
        Init();
        // Get the position on the canvas
        Vector2 ViewportPosition = cam.WorldToViewportPoint(objectTransformPosition);
        Vector2 proportionalPosition = new Vector2(ViewportPosition.x * dungeonCanvas.pixelRect.x, ViewportPosition.y * dungeonCanvas.pixelRect.y);

        // Set the position and remove the screen offset
        this.rectTransform.localPosition = proportionalPosition - uiOffset;
    }
}