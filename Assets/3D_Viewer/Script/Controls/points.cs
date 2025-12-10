using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class points : MonoBehaviour
{
    public float radius = 0.02f;
    public Color color = Color.yellow;
    public List<Vector3> positions = new List<Vector3>();
    public List<GameObject> poso = new List<GameObject>();
    public GameObject m_SP;
    void OnDrawGizmos()
    {
        positions.Clear();
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf == null || mf.sharedMesh == null) return;

        Mesh mesh = mf.sharedMesh;
        Vector3[] verts = mesh.vertices;

        Gizmos.color = color;

        // Draw sphere at each vertex
        if (poso.Count < verts.Length)
        {
            for (int i = 0; i < verts.Length; i++)
            {
                Vector3 worldPos = transform.TransformPoint(verts[i]);
                positions.Add(worldPos);
                var inst = Instantiate(m_SP, transform);
                inst.transform.position = worldPos;
                poso.Add(inst);
                Gizmos.DrawSphere(worldPos, radius);
            }
        }
    }
}
