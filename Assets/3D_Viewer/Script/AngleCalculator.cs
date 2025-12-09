using UnityEngine;
using System.Collections.Generic;

public class AngleCalculator : MonoBehaviour
{
    public Camera cam;

    public List<Transform> selectedPoints = new List<Transform>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectPoint();
        }
    }

    void SelectPoint()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Point"))
            {
                Transform p = hit.collider.transform;

                if (!selectedPoints.Contains(p))
                {
                    selectedPoints.Add(p);
                    Debug.Log("Selected: " + p.name);
                }

                if (selectedPoints.Count == 3)
                {
                    CalculateAngle();
                    selectedPoints.Clear();
                }
            }
        }
    }

    void CalculateAngle()
    {
        Transform A = selectedPoints[0];
        Transform B = selectedPoints[1];
        Transform C = selectedPoints[2];

        // Vectors
        Vector3 BA = A.position - B.position;
        Vector3 BC = C.position - B.position;

        // Angle
        float angle = Vector3.Angle(BA, BC);

        Debug.Log($"Angle at B ({B.name}) = {angle}°");
    }
}
