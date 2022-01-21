using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class RoadHolder : MonoBehaviour
{
    private PathHolder pathHolder;

    void Start()
    {
        pathHolder = new PathHolder();
    }

    private void OnEnable()
    {
        Actions.OnAddBaicRoad += GenerateBasicRoad;
    }

    private void OnDisable()
    {
        Actions.OnAddBaicRoad -= GenerateBasicRoad;
    }


    public void GenerateBasicRoad(Vector2 mousePos)
    {
        GameObject road = new GameObject("Road");
        road.AddComponent<RoadEditor>();
        road.transform.position = mousePos;
        road.transform.parent = gameObject.transform;
        pathHolder.AddPath(road.GetComponent<RoadEditor>().path);

    }

    public void GenerateRoad(Path path, Vector2 mousePos)
    {
        GameObject road = new GameObject("Road");
        
        road.transform.parent = this.gameObject.transform; 
        road.transform.position = mousePos;
        road.AddComponent<RoadEditor>().ChangePath(path);

        pathHolder.AddPath(path);
    }

    public void SplitRoad(Path path)
    {
        Path path0 = new Path(path[path.NumPoints-1], path[path.NumPoints-1] + Vector2.up * 0.5f);
        Path path1 = new Path(path[path.NumPoints-1], path[path.NumPoints-1] + Vector2.down * 0.5f);
        
        pathHolder.AddPath(path0);
        pathHolder.AddPath(path1);
        
        GenerateRoad(path0, path[path.NumPoints-1]);
        GenerateRoad(path1, path[path.NumPoints-1]);
    }
    
}
