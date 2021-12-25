using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEditorInternal;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movementSpeed = 8f;
    public float zoomSpeed = 5f;

    public float minOrthograpicSize = 1f;
    public float maxOrthograpicSize = 60f;

    public GameObject anchorPoint;
    private RoadEditor _chosenRoad;

    private static bool editMode;
    private static bool closePath;
    


    void Update()
    {
        //Is using get component to expensive??
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            editMode = !editMode;
            
            Actions.OnEditMode(editMode);
            if (_chosenRoad != null)
            {
                _chosenRoad.EditMode(editMode);
            }
            
            print("Editmode: " + editMode);

        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        
        //TODO: smoothing on camera movement
        
        if (editMode)
        {
            if (hit.collider != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _chosenRoad = hit.collider.gameObject.GetComponent<RoadEditor>();
                    _chosenRoad.EditMode(editMode);
                    Debug.Log("Target Position: " + hit.collider.gameObject.GetComponent<PathCreator>());
                }

                Debug.DrawLine(transform.position, hit.point, Color.red);
            }
            else
            {
                Debug.DrawLine(transform.position, mousePosition, Color.green);
            }
            
            //TODO: put into a another class like "RoadHolder"
            
            if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.LeftShift) )
            {
                GameObject point = Instantiate(anchorPoint, _chosenRoad.transform);
                point.transform.position = mousePosition;

                _chosenRoad.AddSegment(mousePosition);
            }

            if (Input.GetKeyDown(KeyCode.Mouse1) && Input.GetKey(KeyCode.LeftShift))
            {
                GameObject road = new GameObject("Road");
                road.AddComponent<RoadEditor>();
                road.transform.position = mousePosition;
                _chosenRoad = road.GetComponent<RoadEditor>();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                closePath = !closePath;
                _chosenRoad.ClosePath(closePath);
            }
            
            if (Input.GetKeyDown(KeyCode.Z))
            {
                _chosenRoad.SplitPath();
            }
        }
        
        Vector2 dir = Vector2.zero;
        var orthographicSize = Camera.main.orthographicSize;

        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
        }


        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
        }


        if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
        }

        orthographicSize += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        orthographicSize = Mathf.Clamp(orthographicSize, minOrthograpicSize, maxOrthograpicSize);
        Camera.main.orthographicSize = orthographicSize;

        transform.Translate(dir * movementSpeed * Time.deltaTime);
    }


}