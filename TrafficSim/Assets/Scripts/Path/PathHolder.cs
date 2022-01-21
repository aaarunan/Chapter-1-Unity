using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathHolder
{
    private List<Path> pathHolder;
    
    public PathHolder()
    {
        pathHolder = new List<Path>();
    }
    
    public Path this[int i]
    {
        get
        {
            return pathHolder[i];
        }
    }

    public int getLength()
    {
        return pathHolder.Count;
    }

    public void AddPath(Path path)
    {
        pathHolder.Add(path);
    }
    
    



}
