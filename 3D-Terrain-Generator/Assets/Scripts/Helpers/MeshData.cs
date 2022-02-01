using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    [SerializeField] private Vector3[] vertices;
    [SerializeField] private int[] triangles;
    [SerializeField] private Vector2[] uvs;

    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[((meshWidth - 1) * (meshHeight - 1)) * 6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals(); // For correct lighting
        return mesh;
    }

    public Vector3[] GetVertices()
    {
        return vertices;
    }

    public int[] GetTriangles()
    {
        return triangles;
    }

    public Vector2[] GetUVS()
    {
        return uvs;
    }
}
