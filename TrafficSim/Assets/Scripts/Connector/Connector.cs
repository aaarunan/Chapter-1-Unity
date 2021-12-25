using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class Connector
{
    private List<Vector2> points;
    
    public Connector(Vector2 startPoint, Vector2 endPoint)
    {

    }
    
    public Vector2 this[int i]
    {
        get
        {
            return points[i];
        }
    }
    



}
