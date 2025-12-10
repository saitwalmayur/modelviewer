using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 100, 0);
    public float smoothSpeed = 10f;

    private Vector3 fixedTargetPos;

    void FixedUpdate()
    {
        if (target == null) return;

        // capture the target's position in physics update
        fixedTargetPos = target.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = fixedTargetPos + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
    }
}
