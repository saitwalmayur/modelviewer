using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CustomCylinderBetweenTwoPoints : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    [Range(3, 64)]
    public int segments = 16;
    public float radius = 0.1f;
    public bool realTimeUpdate = true;

    private Mesh mesh;

    void Update()
    {
        if (isLinePresent)
        {
            if (realTimeUpdate)
                GenerateCylinder();
        }
    }
    public bool isLinePresent;
    public void SetPos(Transform one,Transform two)
    {
        pointA = one;
        pointB = two;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GenerateCylinder();
        isLinePresent = true;
    }

    void GenerateCylinder()
    {
        if (!pointA || !pointB) return;

        mesh.Clear();

        Vector3 localA = transform.InverseTransformPoint(pointA.position);
        Vector3 localB = transform.InverseTransformPoint(pointB.position);
        Vector3 direction = (localB - localA).normalized;
        float height = Vector3.Distance(localA, localB);

        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, direction);

        Vector3[] vertices = new Vector3[(segments + 1) * 2];
        int[] triangles = new int[segments * 6];

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            Vector3 circle = new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);

            vertices[i] = rotation * circle + localA;
            vertices[i + segments + 1] = rotation * (circle + Vector3.up * height) + localA;
        }

        int index = 0;
        for (int i = 0; i < segments; i++)
        {
            int a = i;
            int b = i + 1;
            int c = i + segments + 1;
            int d = i + segments + 2;

            triangles[index++] = a;
            triangles[index++] = c;
            triangles[index++] = b;

            triangles[index++] = b;
            triangles[index++] = c;
            triangles[index++] = d;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
