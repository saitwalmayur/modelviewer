using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }
    private static GameManager _instance;
    public GameObject ArcObject;
    public GameManager()
    {
        _instance = this;
    }
    public Material material;
    public bool canSelectPoint;
    public CustomCylinderBetweenTwoPoints line1;
    public CustomCylinderBetweenTwoPoints line2;

    public void ShowLine(Transform one, Transform two, Transform thee, float angle)
    {
        line1.gameObject.SetActive(true);
        line1.SetPos(one, two);

        line2.gameObject.SetActive(true);
        line2.SetPos(two, thee);

    }

    public void HideLine()
    {
        line1.gameObject.SetActive(false);
        line2.gameObject.SetActive(false);
        ArcObject.gameObject.SetActive(false);
    }

    public void CreateAngleMesh(Transform center, Vector3 BA, Vector3 BC, float angle)
    {
        int segments = 40;      // Higher = smoother arc
        float radius = 0.3f;   // Arc size

        ArcObject.transform.SetParent(center);
        ArcObject.transform.localPosition = Vector3.zero;

        MeshFilter mf = ArcObject.GetComponent<MeshFilter>();
        MeshRenderer mr = ArcObject.GetComponent<MeshRenderer>();

        mr.material = material;
        mr.material.color = new Color(1f, 0.3f, 0.1f, 0.6f); // Orange transparent

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero; // Center of fan

        Vector3 axis = Vector3.Cross(BA, BC).normalized;
        Quaternion stepRot = Quaternion.AngleAxis(angle / segments, axis);

        Vector3 dir = BA.normalized;

        for (int i = 0; i <= segments; i++)
        {
            vertices[i + 1] = dir * radius;

            if (i < segments)
            {
                int triIndex = i * 3;
                triangles[triIndex] = 0;
                triangles[triIndex + 1] = i + 1;
                triangles[triIndex + 2] = i + 2;
            }

            dir = stepRot * dir;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        mf.mesh = mesh;
    }

}
