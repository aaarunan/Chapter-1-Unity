using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DefaultNamespace;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent((typeof(BoxCollider2D)))]
public class RoadEditor : MonoBehaviour
{

    public GameObject anchorPoint;
    public Path path;
 
    [Range(.05f, 1.5f)]
    public float spacing = 0.1f;
    public float roadWidth = 1;
    public float tiling = 0.1f;

    public RoadEditor()
    {
        path = new Path(transform.position);
    }

    public RoadEditor(Vector2 startPoint, Vector2 endPoint)
    {
        path = new Path(startPoint, endPoint);
    }

    public RoadEditor(Vector2 startPoint)
    {
        path = new Path(startPoint, startPoint + Vector2.up);
    }
    public void Start()
    {
         anchorPoint =  Resources.Load<GameObject>("Prefabs/AnchorPoint");
         path = new Path(transform.position);
         
         Draw();
         UpdateRoad();


    }

    private void OnEnable()
    {
        GetComponent<MeshRenderer>().materials[0] = Resources.Load<Material>("Materials/Road");
        Actions.OnUpdatePath += Draw;
    }
    
    private void OnDisable()
    {
        Actions.OnUpdatePath -= Draw;
    }
    
    public void UpdateRoad()
    {
        Vector2[] points = path.CalculateEvenlySpacedPoints(spacing, transform.position);
        GetComponent<MeshFilter>().mesh = CreateRoadMesh(points, path.IsClosed);

        int textureRepeat = Mathf.RoundToInt(tiling * points.Length * spacing * .05f);
        GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(1, textureRepeat);
    }
    

    private void Draw()
    {
        Remove();
        
        GameObject startAnchorPoint = Instantiate(anchorPoint, transform);
        startAnchorPoint.transform.position = path.GetPointsInSegment(0)[0];
        
        for (int i = 0; i < path.NumSegments; i++)
        {
            Vector2[] pointsInSegment = path.GetPointsInSegment(i);
            GameObject point = Instantiate(anchorPoint, transform);
            
            point.transform.position = pointsInSegment[3];
            
        }
        
        UpdateRoad();
    }

    private void Remove()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void AddSegment(Vector2 anchorPos)
    {   
        path.AddSegment(anchorPos);
        Draw();
    }

    public void ClosePath(bool closePath)
    {
        path.IsClosed = closePath;
        Draw();
    }

    public void EditMode(bool editMode)
    {
        if (editMode)
        {
            Draw();
        }
        else
        {
            Remove();
        }
    }

    public void SplitPath()
    {
        GameObject split0 = new GameObject("Road");
        GameObject split1 = new GameObject("Road");

        split0.AddComponent<RoadEditor>().path = new Path(path[path.NumPoints-1], path[path.NumPoints-1] + Vector2.up * 0.5f);
        split1.AddComponent<RoadEditor>().path = new Path(path[path.NumPoints-1], path[path.NumPoints-1] + Vector2.down * 0.5f);
        split0.
        
        Draw();

    }
    

    
    
    Mesh CreateRoadMesh(Vector2[] points, bool isClosed)
    {
        Vector3[] verts = new Vector3[points.Length * 2];
        Vector2[] uvs = new Vector2[verts.Length];
        int numTris = 2 * (points.Length - 1) + ((isClosed) ? 2 : 0);
        int[] tris = new int[numTris * 3];
        int vertIndex = 0;
        int triIndex = 0;

        for (int i = 0; i < points.Length; i++)
        {
            Vector2 forward = Vector2.zero;
            if (i < points.Length - 1 || isClosed)
            {
                forward += points[(i + 1)%points.Length] - points[i];
            }
            if (i > 0 || isClosed)
            {
                forward += points[i] - points[(i - 1 + points.Length)%points.Length];
            }

            forward.Normalize();
            Vector2 left = new Vector2(-forward.y, forward.x);

            verts[vertIndex] = points[i] + left * roadWidth * .5f;
            verts[vertIndex + 1] = points[i] - left * roadWidth * .5f;

            float completionPercent = i / (float)(points.Length - 1);
            float v = 1 - Mathf.Abs(2 * completionPercent - 1);
            uvs[vertIndex] = new Vector2(0, v);
            uvs[vertIndex + 1] = new Vector2(1, v);

            if (i < points.Length - 1 || isClosed)
            {
                tris[triIndex] = vertIndex;
                tris[triIndex + 1] = (vertIndex + 2) % verts.Length;
                tris[triIndex + 2] = vertIndex + 1;

                tris[triIndex + 3] = vertIndex + 1;
                tris[triIndex + 4] = (vertIndex + 2) % verts.Length;
                tris[triIndex + 5] = (vertIndex + 3)  % verts.Length;
            }

            vertIndex += 2;
            triIndex += 6;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        

        return mesh;
    }
    
    
    
    
    
    
}
