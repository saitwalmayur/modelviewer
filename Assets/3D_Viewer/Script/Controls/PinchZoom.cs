using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    public Camera cam;
    public float zoomSpeed = 0.1f;
    public float minZoom = 5f;
    public float maxZoom = 40f;

    void Start()
    {
        if (cam == null) cam = Camera.main;
    }

    void Update()
    {
        // ----- TOUCH PINCH ZOOM -----
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            Vector2 t0Prev = t0.position - t0.deltaPosition;
            Vector2 t1Prev = t1.position - t1.deltaPosition;

            float prevDist = (t0Prev - t1Prev).magnitude;
            float currDist = (t0.position - t1.position).magnitude;

            float diff = currDist - prevDist;

            Zoom(diff * zoomSpeed);
        }

        // ----- MOUSE SCROLL (EDITOR TEST) -----
        if (Input.mouseScrollDelta.y != 0)
        {
            Zoom(Input.mouseScrollDelta.y);
        }
    }

    void Zoom(float increment)
    {
        float fov = cam.orthographic == false ? cam.fieldOfView : cam.orthographicSize;

        fov -= increment;
        fov = Mathf.Clamp(fov, minZoom, maxZoom);

        if (cam.orthographic == false)
            cam.fieldOfView = fov;
        else
            cam.orthographicSize = fov;
    }
}
