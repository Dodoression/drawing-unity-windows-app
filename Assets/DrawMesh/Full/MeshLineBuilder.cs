using System.Collections.Generic;
using UnityEngine;

public class MeshLineBuilder
{
    private List<Vector3> vertices = new();
    private List<int> triangles = new();
    private List<Vector2> uvs = new();
    private float thickness;

    public MeshLineBuilder(float thickness)
    {
        this.thickness = thickness;
    }

    public void AddPoint(Vector3 newPoint, Mesh mesh)
    {
        Vector3 lastPoint = vertices.Count >= 2 ? vertices[vertices.Count - 2] : newPoint - Vector3.right * 0.001f;

        Vector3 dir = (newPoint - lastPoint).normalized;
        Vector3 normal = Vector3.Cross(dir, Vector3.forward) * thickness * 0.5f;

        Vector3 v1 = newPoint + normal;
        Vector3 v2 = newPoint - normal;

        int index = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);

        uvs.Add(Vector2.zero);
        uvs.Add(Vector2.one);

        if (vertices.Count >= 4)
        {
            triangles.Add(index - 2);
            triangles.Add(index - 1);
            triangles.Add(index + 0);

            triangles.Add(index - 1);
            triangles.Add(index + 1);
            triangles.Add(index + 0);
        }

        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetUVs(0, uvs);
        mesh.RecalculateBounds();
    }

    public void SetThickness(float newThickness) => thickness = newThickness;
}
