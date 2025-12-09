using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshLineBetweenTwoPoints : MonoBehaviour
{
    [Header("References")]
    public Transform pointA;
    public Transform pointB;

    [Header("Line Settings")]
    [Tooltip("Thickness of the line in world units")]
    public float lineWidth = 0.1f;

    [Tooltip("If true the mesh will be rebuilt every frame (useful for moving points). Turn off for static lines.")]
    public bool updateEveryFrame = true;

    private Mesh mesh;


    public void SetPos(Transform one,Transform two)
    {
        pointA = one;
        pointB = two;

        MeshFilter mf = GetComponent<MeshFilter>();
        mesh = new Mesh();
        mesh.name = "MeshLine";
        mf.mesh = mesh;

        BuildMesh();
    }

    private void Update()
    {
        BuildMesh();
    }
    /// <summary>
    /// Builds a simple quad mesh between pointA and pointB with a given thickness.
    /// This is a world-space mesh (not camera-facing). If you need camera-facing/billboarded lines,
    /// see the comment inside for a small change.
    /// </summary>
    void BuildMesh()
    {
        if (pointA == null || pointB == null)
            return;

        Vector3 a = pointA.position;
        Vector3 b = pointB.position;

        Vector3 dir = b - a;
        if (dir.sqrMagnitude < Mathf.Epsilon)
        {
            // points are essentially the same - make a tiny degenerate quad
            mesh.Clear();
            return;
        }

        // Find a perpendicular direction for the quad's width.
        // We try to use world up as a fallback; if dir is parallel to up, use forward.
        Vector3 perp = Vector3.Cross(dir.normalized, Vector3.up);
        if (perp.sqrMagnitude < 0.0001f)
            perp = Vector3.Cross(dir.normalized, Vector3.forward);
        perp.Normalize();
        perp *= (lineWidth * 0.5f);

        // Four vertices for the quad (A+: top at A, A-: bottom at A, B+: top at B, B-: bottom at B)
        Vector3 v0 = a + perp; // 0
        Vector3 v1 = a - perp; // 1
        Vector3 v2 = b + perp; // 2
        Vector3 v3 = b - perp; // 3

        Vector3[] vertices = new Vector3[4] { v0, v1, v2, v3 };

        // Simple UV mapping along the line (u is 0 at A and 1 at B)
        float length = dir.magnitude;
        Vector2[] uvs = new Vector2[4]
        {
            new Vector2(0, 1),
            new Vector2(0, 0),
            new Vector2(1, 1),
            new Vector2(1, 0)
        };

        // Two triangles: 0,2,1 and 2,3,1
        int[] tris = new int[6] { 0, 2, 1, 2, 3, 1 };

        // Normals (optional but useful for lighting)
        Vector3 normal = Vector3.Cross(dir, perp).normalized;
        Vector3[] normals = new Vector3[4] { normal, normal, normal, normal };

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = tris;
        mesh.normals = normals;

        // Optionally: recalculate bounds so Unity culls correctly
        mesh.RecalculateBounds();
    }

    // Editor helper to build the mesh in edit mode when properties change
#if UNITY_EDITOR
    void OnValidate()
    {
        if (!Application.isPlaying)
        {
            if (GetComponent<MeshFilter>() != null)
            {
                if (mesh == null)
                {
                    MeshFilter mf = GetComponent<MeshFilter>();
                    mesh = new Mesh();
                    mesh.name = "MeshLine";
                    mf.mesh = mesh;
                }
                BuildMesh();
            }
        }
    }
#endif
}
