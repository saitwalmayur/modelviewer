using System;
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
    public Transform m_Model;

    public LayerMask m_ModelLayer;
    public Transform m_CurrentPivot;

    // Store original parent & position for Reset
    Transform originalModelParent;
    Vector3 originalModelPosition;
    Quaternion originalModelRotation;

    private void OnEnable()
    {
        foreach (var item in m_Point)
        {
            item.gameObject.SetActive(false);
        }
        GameEvents.OnResetTool += GameEvents_OnResetTool;
    }
    private void OnDisable()
    {
        GameEvents.OnResetTool -= GameEvents_OnResetTool;
    }

    private void GameEvents_OnResetTool(object sender, SelectedTool e)
    {
        ResetPivot();
    }

    void Start()
    {
        if (modelRoot == null)
        {
            Debug.LogError("PivotController: modelRoot not assigned.");
            enabled = false;
            return;
        }

        // Save original state
        originalModelParent = modelRoot.parent;
        originalModelPosition = modelRoot.position;
        originalModelRotation = modelRoot.rotation;

        // Create initial default pivot
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
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, m_ModelLayer))
        {
            if (IsChildOfModel(hit.transform))
            {
                SetPivotAtPoint(hit.point, hit.normal);
            }
        }
    }

    bool IsChildOfModel(Transform t)
    {
        return t == modelRoot || t.IsChildOf(modelRoot);
    }

    public void SetPivotAtPoint(Vector3 worldPoint, Vector3 normal)
    {
        // Unparent from previous pivot
        if (currentPivot != null)
        {
            modelRoot.SetParent(currentPivot.parent, true);
            Destroy(currentPivot.gameObject);
        }

        // Create new pivot
        GameObject pivotGO = new GameObject("Pivot");
        pivotGO.AddComponent<MoveRotateControls>();

        Transform pivotTF = pivotGO.transform;
        pivotTF.position = worldPoint;

        // Place pivot under original parent
        Transform parent = modelRoot.parent != null ? modelRoot.parent : originalModelParent;
        pivotTF.SetParent(parent, true);

        // Reparent model to pivot
        modelRoot.SetParent(pivotTF, true);

        // Create pivot marker
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

        // Store pivot
        currentPivot = pivotTF;
        m_CurrentPivot = pivotTF;
        m_CurrentPivot.gameObject.AddComponent<Pivot>();
    }

    public void ClearPivot(Transform originalParent = null)
    {
        if (currentPivot != null)
        {
            modelRoot.SetParent(originalParent, true);

            if (currentMarker != null)
                Destroy(currentMarker);

            Destroy(currentPivot.gameObject);

            currentPivot = null;
            currentMarker = null;
        }
    }

    // -----------------------------------------
    // 🔥 RESET PIVOT TO INITIAL STATE (NEW)
    // -----------------------------------------
    public void ResetPivot()
    {
        if (modelRoot == null)
            return;

        // 1. Remove any current pivot
        if (currentPivot != null)
        {
            Transform parent = currentPivot.parent;
            modelRoot.SetParent(parent, true);

            if (currentMarker != null) Destroy(currentMarker);
            Destroy(currentPivot.gameObject);

            currentPivot = null;
            currentMarker = null;
        }

        // 2. Restore model to its original transform
        modelRoot.SetParent(originalModelParent, true);
        modelRoot.position = originalModelPosition;
        modelRoot.rotation = originalModelRotation;

        // 3. Create fresh default pivot
        SetPivotAtPoint(originalModelPosition, Vector3.zero);

        Debug.Log("Pivot reset to original start position.");
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
