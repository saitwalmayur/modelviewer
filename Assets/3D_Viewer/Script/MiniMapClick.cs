using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMapClick : MonoBehaviour, IPointerClickHandler
{
    public Camera miniMapCamera;
    public Camera mainCamera;
    public float cameraHeight = 10f;

    public Bounds modelBounds;
    public float smoothSpeed = 5f;

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        Rect rect = (transform as RectTransform).rect;

        // Convert local UI point → normalized (0-1)
        float u = (localPoint.x - rect.x) / rect.width;
        float v = (localPoint.y - rect.y) / rect.height;

        // Convert to MiniMapCamera position
        Vector3 worldPos = MiniMapToWorldPosition(u, v);

        // Clamp inside model bounds
        worldPos.x = Mathf.Clamp(worldPos.x, modelBounds.min.x, modelBounds.max.x);
        worldPos.z = Mathf.Clamp(worldPos.z, modelBounds.min.z, modelBounds.max.z);

        // Keep camera height
        worldPos.y = cameraHeight;

        // Start smooth movement
        StopAllCoroutines();
        StartCoroutine(MoveCameraSmooth(worldPos));
    }

    Vector3 MiniMapToWorldPosition(float u, float v)
    {
        Ray ray = miniMapCamera.ViewportPointToRay(new Vector3(u, v, 0));
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        float dist;
        if (plane.Raycast(ray, out dist))
            return ray.GetPoint(dist);

        return Vector3.zero;
    }

    System.Collections.IEnumerator MoveCameraSmooth(Vector3 target)
    {
        while (Vector3.Distance(mainCamera.transform.position, target) > 0.1f)
        {
            mainCamera.transform.position = Vector3.Lerp(
                mainCamera.transform.position,
                target,
                Time.deltaTime * smoothSpeed
            );
            yield return null;
        }
    }
}
