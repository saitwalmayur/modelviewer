using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Transform))]
public class Axis : MonoBehaviour
{
    [Header("Gizmo settings")]
    public float axisLength = 1.5f;
    public float lineWidth = 0.03f;
    public bool showX = true;
    public bool showY = true;
    public bool showZ = true;
    public bool drawOnStart = true; // create renderers on Start

    // optional children for each axis
    private LineRenderer xLR, yLR, zLR;

    void Start()
    {
        if (drawOnStart) CreateGizmoRenderers();
    }

    // Call this method if you want to (re)create at runtime
    public void CreateGizmoRenderers()
    {
        DestroyGizmoRenderers();

        if (showX) xLR = CreateAxis("Gizmo_X", Vector3.right, axisLength, lineWidth);
        if (showY) yLR = CreateAxis("Gizmo_Y", Vector3.up, axisLength, lineWidth);
        if (showZ) zLR = CreateAxis("Gizmo_Z", Vector3.forward, axisLength, lineWidth);
    }

    LineRenderer CreateAxis(string name, Vector3 dir, float length, float width)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(transform, false);
        var lr = go.AddComponent<LineRenderer>();
        // Basic setup - you can change material / colors in inspector later
        lr.positionCount = 2;
        lr.useWorldSpace = false; // so it follows parent transform
        lr.startWidth = width;
        lr.endWidth = width;
        lr.SetPosition(0, Vector3.zero);
        lr.SetPosition(1, dir.normalized * length);

        // Lightweight default material - if you want to customize, assign a material in Inspector
        Shader shader = Shader.Find("Sprites/Default");
        if (shader != null)
        {
            lr.material = new Material(shader);
        }

        // set color (can be changed from code or inspector)
        if (name.Contains("_X")) lr.startColor = lr.endColor = Color.red;
        else if (name.Contains("_Y")) lr.startColor = lr.endColor = Color.green;
        else if (name.Contains("_Z")) lr.startColor = lr.endColor = Color.blue;

        return lr;
    }

    void DestroyGizmoRenderers()
    {
        if (xLR) Destroy(xLR.gameObject);
        if (yLR) Destroy(yLR.gameObject);
        if (zLR) Destroy(zLR.gameObject);
    }

    void OnValidate()
    {
        // update in editor after changing values
        if (Application.isPlaying)
        {
            CreateGizmoRenderers();
        }
    }

    void OnDestroy()
    {
        DestroyGizmoRenderers();
    }

}
