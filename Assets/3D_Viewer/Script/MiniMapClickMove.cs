using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMapClickMove : MonoBehaviour, IPointerClickHandler
{
    [Header("Cameras & References")]
    public Camera miniMapCamera; // The orthographic camera rendering the map
    public Camera mainCamera;     // The camera to be moved

    [Header("Boundary & Movement")]
    public BoxCollider modelBounds; // A single BoxCollider encompassing the entire model
    public float moveSpeed = 5.0f; // For smooth transition (Bonus)

    private float initialYHeight;  // Store the main camera's current height
    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        if (mainCamera != null)
        {
            // Store the initial Y height to maintain the camera's angle/distance.
            initialYHeight = mainCamera.transform.position.y;
            targetPosition = mainCamera.transform.position;
        }

        // Ensure the mini-map camera is set up correctly (see Step 1.2).
        SetupMiniMapCamera();
    }
    private void SetupMiniMapCamera()
    {
        if (miniMapCamera == null || modelBounds == null) return;

        // 1. Get World Bounds
        Bounds bounds = modelBounds.bounds;

        // 2. Calculate the size needed to fit the model's horizontal footprint
        float xSize = bounds.size.x;
        float zSize = bounds.size.z;

        // Set Orthographic Size
        // We use the greater of the two dimensions for the size, divided by 2.
        miniMapCamera.orthographicSize = Mathf.Max(xSize, zSize) / 2f;

        // 3. Center the Mini-Map Camera
        // Position it over the center of the bounding box at a fixed height.
        Vector3 center = bounds.center;
        miniMapCamera.transform.position = new Vector3(center.x, 100f, center.z);
        miniMapCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (miniMapCamera == null || modelBounds == null) return;

        // 1. Convert Screen Point (click position) to a Ray from the Mini-Map Camera
        // Note: The UI coordinates (eventData.position) need to be normalized
        // relative to the mini-map camera's viewport if the RawImage doesn't fill the screen.

        // Get the click position normalized to the RawImage bounds (0 to 1)
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localCursor))
            return;

        // Convert local UI position to viewport space (0-1)
        Vector2 normalizedPoint = new Vector2(
            (localCursor.x + rectTransform.pivot.x * rectTransform.rect.width) / rectTransform.rect.width,
            (localCursor.y + rectTransform.pivot.y * rectTransform.rect.height) / rectTransform.rect.height
        );

        // Convert normalized viewport point to a World Ray
        Ray ray = miniMapCamera.ViewportPointToRay(normalizedPoint);

        // 2. Find the Intersection Point (XZ Plane)
        if (Physics.Raycast(ray, out RaycastHit hit, 200f))
        {
            // Found a valid hit point on the ground/model (P_target)
            Vector3 hitPoint = hit.point;

            // 3. Clamp the target position within the model bounds
            Bounds bounds = modelBounds.bounds;
            float clampedX = Mathf.Clamp(hitPoint.x, bounds.min.x, bounds.max.x);
            float clampedZ = Mathf.Clamp(hitPoint.z, bounds.min.z, bounds.max.z);

            // Set the final target position for the main camera
            targetPosition = new Vector3(clampedX, initialYHeight, clampedZ);
            isMoving = true;
        }
    }
    void Update()
    {
        if (isMoving)
        {
            // Smoothly move the camera's horizontal position
            mainCamera.transform.position = Vector3.Lerp(
                mainCamera.transform.position,
                targetPosition,
                Time.deltaTime * moveSpeed
            );

            // Check if we are close enough to stop moving
            if (Vector3.Distance(mainCamera.transform.position, targetPosition) < 0.05f)
            {
                mainCamera.transform.position = targetPosition;
                isMoving = false;
            }
        }
    }
}
