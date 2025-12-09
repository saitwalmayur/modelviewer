using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRotateControls : MonoBehaviour
{
    public float rotationSpeed = 50f;

    private Vector3 lastMousePos;
    private bool isDragging = false;

    public bool isMoveControl;
    public ControlType controlType;

    private void OnEnable()
    {
        controlType = GameEvents.controlType;
        GameEvents.OnSetControl += GameEvents_OnSetControl;
    }
    private void GameEvents_OnSetControl(object sender, ControlType e)
    {
        controlType = e;
    }
    private void OnDisable()
    {
        GameEvents.OnSetControl -= GameEvents_OnSetControl;
    }
    private void OnDestroy()
    {
        GameEvents.OnSetControl -= GameEvents_OnSetControl;
    }
 
    void Update()
    {
        switch (controlType)
        {
            case ControlType.Move:
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
                break;
            case ControlType.Rotate:
                // When mouse is first pressed → store initial mouse pos
                if (Input.GetMouseButtonDown(0))
                {
                    lastMousePos = Input.mousePosition;
                    isDragging = true;
                }

                // When mouse is released → stop dragging
                if (Input.GetMouseButtonUp(0))
                {
                    isDragging = false;
                }

                // Rotate only while dragging
                if (isDragging)
                {
                    Vector3 delta = Input.mousePosition - lastMousePos;

                    // vertical drag rotates X, horizontal rotates Y
                    float rotX = delta.y * rotationSpeed * Time.deltaTime;
                    float rotY = -delta.x * rotationSpeed * Time.deltaTime;

                    transform.Rotate(rotX, rotY, 0, Space.World);

                    // update the last position AFTER using it
                    lastMousePos = Input.mousePosition;
                }
                break;
        }
    }

    private Vector3 offset;
    private float zPos;

    void Start()
    {
        // Save object's z-depth on screen
        zPos = Camera.main.WorldToScreenPoint(transform.position).z;
    }

  

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zPos;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
