using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private float zPos;

    void Start()
    {
        // Save object's z-depth on screen
        zPos = Camera.main.WorldToScreenPoint(transform.position).z;
    }

    void Update()
    {
        // Start drag on mouse down
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;

            offset = transform.position - GetMouseWorldPos();
        }

        // Move object while dragging
        if (Input.GetMouseButton(0) && isDragging)
        {
            transform.position = GetMouseWorldPos() + offset;
        }

        // Release drag
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zPos;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
