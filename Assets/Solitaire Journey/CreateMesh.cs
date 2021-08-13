using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CreateMesh : MonoBehaviour {

    public int textureWidth;
    public int textureHeight;
    public float sampleRate = 10f;
    public float yRatio = 1f;
    public float xRatio = 1f;

    private MeshFilter filter;
    private Mesh mesh;
    int xVerticeCount = 10;
    int yVerticeCount = 10;
    Vector3[] vertices;
    Vector2[] uv;

    private void Awake() {
        // 获取GameObject的Filter组件
        filter = GetComponent<MeshFilter>();
        // 并新建一个mesh给它
        mesh = new Mesh();
        filter.mesh = mesh;

        // 初始化网格
        InitMesh();
    }

    void Update() {
        if (vertices == null)
            return;

        for (int i = 0, y = 0; y <= yVerticeCount; y++) {
            for (int x = 0; x <= xVerticeCount; x++, i++) {
                float xValue = x;
                float yValue = y + yRatio * ((float)x / xVerticeCount) * yVerticeCount * Mathf.Sin(Mathf.PI * (xRatio * x / xVerticeCount + Time.time));
                xValue *= sampleRate;
                yValue *= sampleRate;
                vertices[i] = new Vector3(xValue, yValue);
            }
        }
        mesh.vertices = vertices;

    }

    void InitMesh() {
        mesh.name = "MyMesh";

        xVerticeCount = (int)(textureWidth / sampleRate);
        yVerticeCount = (int)(textureHeight / sampleRate);
        vertices = new Vector3[(xVerticeCount + 1) * (yVerticeCount + 1)];
        uv = new Vector2[vertices.Length];
        for (int i = 0, y = 0; y <= yVerticeCount; y++) {
            for (int x = 0; x <= xVerticeCount; x++, i++) {
                float xValue = x;
                float yValue = y;
                xValue *= sampleRate;
                yValue *= sampleRate;
                vertices[i] = new Vector3(xValue, yValue);
                uv[i] = new Vector2((float)x / xVerticeCount, (float)y / yVerticeCount);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.RecalculateNormals();

        // 通过顶点为网格创建三角形
        int[] triangles = new int[xVerticeCount * yVerticeCount * 6];
        for (int ti = 0, vi = 0, y = 0; y < yVerticeCount; y++, vi++) {
            for (int x = 0; x < xVerticeCount; x++, ti += 6, vi++) {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xVerticeCount + 1;
                triangles[ti + 5] = vi + xVerticeCount + 2;
                mesh.triangles = triangles;
            }
        }
    }
}