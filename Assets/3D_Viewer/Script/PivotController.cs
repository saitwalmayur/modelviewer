using UnityEngine;
public class PivotController : MonoBehaviour
{
    [Header("References")]
    public Transform modelRoot; 
    [Header("Pivot Marker")]
    public GameObject pivotMarkerPrefab; 
    public float markerScale = 0.02f;
    Transform currentPivot;   
    GameObject currentMarker;

    public GameObject[] m_VertexPoints;
    public Point[] m_Point;
    void Start()
    {
        if (modelRoot == null)
        {
            Debug.LogError("PivotController: modelRoot not assigned.");
            enabled = false;
            return;
        }

        SetPivotAtPoint(transform.position, Vector3.zero);
    }
    
    void Update()
    {
        HandleInput();
    }
    void HandleInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonUp(0))
        {
            OnPointerDown(Input.mousePosition);
        }
#endif
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Ended)
                OnPointerDown(t.position);
        }
    }

    void OnPointerDown(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (IsChildOfModel(hit.transform))
            {
                Debug.Log(hit.transform);
                SetPivotAtPoint(hit.point, hit.normal);
            }
        }
    }
    bool IsChildOfModel(Transform t)
    {
        return t == modelRoot || t.IsChildOf(modelRoot);
    }
    public Transform m_CurrentPivot;
    public void SetPivotAtPoint(Vector3 worldPoint, Vector3 normal)
    {
        // 1. Unparent from previous pivot first
        if (currentPivot != null)
        {
            // Move model back to original parent BEFORE setting new pivot
            modelRoot.SetParent(currentPivot.parent, true);
            Destroy(currentPivot.gameObject);
        }

        // 2. Create new pivot
        GameObject pivotGO = new GameObject("Pivot");
        pivotGO.AddComponent<MoveRotateControls>();
        Transform pivotTF = pivotGO.transform;
        pivotTF.position = worldPoint;

        // 3. Place pivot under the model's parent
        Transform originalParent = modelRoot.parent;
        pivotTF.SetParent(originalParent, true);

        // 4. Reparent model to the pivot
        modelRoot.SetParent(pivotTF, true);

        // 5. Create pivot marker
        if (currentMarker != null) Destroy(currentMarker);

        if (pivotMarkerPrefab != null)
        {
            currentMarker = Instantiate(pivotMarkerPrefab, pivotTF.position, Quaternion.identity);
            currentMarker.transform.localScale = Vector3.one * markerScale;
            currentMarker.transform.SetParent(pivotTF, true);

        }
        else
        {
            currentMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            currentMarker.transform.position = pivotTF.position;
            currentMarker.transform.localScale = Vector3.one * markerScale;
            Destroy(currentMarker.GetComponent<Collider>());
            currentMarker.transform.SetParent(pivotTF, true);
        }

        // 6. Store current pivot
        currentPivot = pivotTF;
        m_CurrentPivot = pivotTF;
        m_CurrentPivot.gameObject.AddComponent<Pivot>();
    }

    public void ClearPivot(Transform originalParent = null)
    {
        if (currentPivot != null)
        {
            modelRoot.SetParent(originalParent, true); // worldPositionStays = true
            if (currentMarker != null) Destroy(currentMarker);
            Destroy(currentPivot.gameObject);
            currentPivot = null;
            currentMarker = null;
        }
    }
    public Bounds GetModelBounds(GameObject modelRoot)
    {
        Renderer[] renderers = modelRoot.GetComponentsInChildren<Renderer>();
        Bounds b = renderers[0].bounds;

        foreach (var r in renderers)
            b.Encapsulate(r.bounds);

        return b;
    }


}
