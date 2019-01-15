using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoTetrahedron : MonoBehaviour {

    public Shader shader;
    Tetrahedron tetrahedron;

    public void Initialize(Tetrahedron tetrahedron)
    {
        this.tetrahedron = tetrahedron;
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = new Mesh()
        {
            vertices = tetrahedron.Vertices.ToArray(),
            triangles = tetrahedron.Triangles.ToArray()
        };
        List<Color> colors = new List<Color>();
        foreach (Measurement3D measurement in tetrahedron.Measurements)
        {
            colors.Add(measurement.Color);
        }
        meshFilter.mesh.colors = colors.ToArray();
        GetComponent<MeshRenderer>().material = new Material(shader);
    }
    
    public void OnDrawGizmos()
    {
        //for (int i = 0; i < tetrahedron.Triangles.Count; i += 3)
        //{
        //    Vector3 normal = tetrahedron.CalculateNormal(tetrahedron.Triangles[i], tetrahedron.Triangles[i + 1], tetrahedron.Triangles[i + 2]);
        //    Vector3 centroid = (tetrahedron.Vertices[tetrahedron.Triangles[i]] + tetrahedron.Vertices[tetrahedron.Triangles[i + 1]] + tetrahedron.Vertices[tetrahedron.Triangles[i + 2]]) / 3;
        //    Gizmos.DrawLine(centroid, centroid + 3 * normal.normalized);
        //}
    }
}
