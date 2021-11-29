using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movementSpeed = 8f;
    public float zoomSpeed = 5f;

    public float minOrthograpicSize = 1f;
    public float maxOrthograpicSize = 60f;

    public GameObject anchorPoint;
    private GameObject _chosenRoad;

    private static bool editMode;
    


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            editMode = !editMode;
            
            Actions.OnEditMode();
            print("Edit mode " + editMode);
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        
        //TODO: smoothing on camera movement

        if (hit.collider != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _chosenRoad = hit.collider.gameObject;

                Debug.Log("Target Position: " + hit.collider.gameObject.GetComponent<PathCreator>());
            }

            Debug.DrawLine(transform.position, hit.point, Color.red);
        }
        else
        {
            Debug.DrawLine(transform.position, mousePosition, Color.green);
        }

        if (editMode)
        {
            //Add segment to a road
            if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.LeftShift) && _chosenRoad != null)
            {
                GameObject point = Instantiate(anchorPoint, _chosenRoad.transform);
                point.transform.position = mousePosition;
                point.transform.position = mousePosition;

                Actions.OnAddPath(point.transform.position);
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