using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class PaintEdges : MonoBehaviour {

    public struct Edge
    {
        public Vector3 v1;
        public Vector3 v2;

        public Edge(Vector3 v1, Vector3 v2)
        {
            if (v1.x < v2.x || (v1.x == v2.x && (v1.y < v2.y || (v1.y == v2.y && v1.z <= v2.z))))
            {
                this.v1 = v1;
                this.v2 = v2;
            }
            else
            {
                this.v1 = v2;
                this.v2 = v1;
            }
        }
    }

    private void Awake()
    {
        // copy this object's materials
        var materials = GetComponent<MeshRenderer>().sharedMaterials;
        var mesh = GetComponent<MeshFilter>().sharedMesh;
        if (materials.Length != mesh.subMeshCount) throw new UnityException("Material and Submesh count are different");

        var edges = GetMeshEdges(mesh);
    }

    private Edge[][] GetMeshEdges(Mesh mesh)
    {
        Edge[][] submeshEdges = new Edge[mesh.subMeshCount][];
        var verts = mesh.vertices;
        for (int si = 0; si < mesh.subMeshCount; si++)
        {
            HashSet<Edge> edges = new HashSet<Edge>();
            var triangles = mesh.GetTriangles(si);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                var v1 = verts[triangles[i]];
                var v2 = verts[triangles[i + 1]];
                var v3 = verts[triangles[i + 2]];
                edges.Add(new Edge(v1, v2));
                edges.Add(new Edge(v1, v3));
                edges.Add(new Edge(v2, v3));
            }
            submeshEdges[si] = edges.ToArray();
        }
        return submeshEdges;
    }
}
