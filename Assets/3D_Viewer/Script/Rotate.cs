using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotationSpeed = 200f;

    private Vector3 lastMousePos;
    private bool isDragging = false;

    void Update()
    {
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
    }
}
