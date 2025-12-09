using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInOut : MonoBehaviour
{
    public float zoomSpeed = 10f;
    public float minFOV = 20f;
    public float maxFOV = 80f;

    float targetFOV;
    Camera cam;

    void Start()
    {
        cam = Camera.main;
        targetFOV = cam.fieldOfView;
    }

    void Update()
    {
        float scroll = Input.mouseScrollDelta.y;
        targetFOV -= scroll * zoomSpeed;
        targetFOV = Mathf.Clamp(targetFOV, minFOV, maxFOV);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * 10f);
    }
}
