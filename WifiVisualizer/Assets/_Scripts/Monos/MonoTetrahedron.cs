using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoTetrahedron : MonoBehaviour {

    Tetrahedron tetrahedron;

    public void Initialize(Tetrahedron tetrahedron)
    {
        if (tetrahedron.IsArtificial)
        {
            Destroy(gameObject);
        }
        this.tetrahedron = tetrahedron;
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = new Mesh()
        {
            vertices = tetrahedron.Vectors.ToArray(),
            triangles = tetrahedron.Indices.ToArray()
        };
        meshFilter.mesh.RecalculateBounds();
        meshFilter.mesh.RecalculateNormals();
        meshFilter.mesh.RecalculateTangents();
        List<Color> colors = new List<Color>();
        foreach (Measurement3D measurement in tetrahedron.Measurements)
        {
            colors.Add(measurement.Color);
        }
        meshFilter.mesh.colors = colors.ToArray();
        gameObject.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Custom/Tetrahedron"));
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
    
    public override int GetHashCode()
    {
        var hashCode = 570353101;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<Tetrahedron>.Default.GetHashCode(tetrahedron);
        return hashCode;
    }
}
