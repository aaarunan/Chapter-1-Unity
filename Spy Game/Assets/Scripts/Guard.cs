using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public Transform pathHolder;

    public float speed = 5f;
    public float rotateRate = 90;
    public float delay = .3f;

    public Light spotLight;
    public float viewDistance;
    float viewAngle;

    public LayerMask mask;

    public event Action OnPlayerFound;

    private void Start()
    {
        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        viewAngle = spotLight.spotAngle/2;

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i].y += transform.position.y;
        }

        StartCoroutine(followPath(waypoints));
        
    }

    private void Update()

    {
        GameObject player = GameObject.Find("Player");
        Transform playerTransform = player.transform;
        Vector3 playerPosition = playerTransform.position;
        Vector3 direction = (playerPosition - transform.position);
        Ray triggerRay = new Ray(transform.position, direction);
        RaycastHit hitInfo;
        float angle = Mathf.Abs(Vector3.Angle(transform.forward, direction));

        if (direction.magnitude <= viewDistance && angle < viewAngle && angle > 0)
        {
            if (Physics.Raycast(triggerRay, out hitInfo, direction.magnitude, mask))
            {
                Debug.DrawLine(triggerRay.origin, hitInfo.point, Color.green);
            }
            else 
            {
                Debug.DrawLine(triggerRay.origin, triggerRay.origin + triggerRay.direction * direction.magnitude, Color.red);
                
                if (OnPlayerFound != null)
                {
                    OnPlayerFound();
                }

            }
        }

    }



    IEnumerator followPath(Vector3[] waypoints)
    {
        transform.position = waypoints[0];
        
        int nextPositionIndex = 1;
        Vector3 nextPosition = waypoints[nextPositionIndex];
        transform.LookAt(nextPosition);
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);
            if (transform.position == nextPosition)
            {
                nextPositionIndex = (nextPositionIndex + 1) % waypoints.Length;
                nextPosition = waypoints[nextPositionIndex];
                yield return new WaitForSeconds(delay);
                yield return StartCoroutine(Turn(nextPosition));
            }
            yield return null;
        }

    }

    IEnumerator Turn(Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float AngleToTarget = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, AngleToTarget)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, AngleToTarget, rotateRate * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider triggerCollider)
    {
       
    }



    //Draws the waypoints as gizmos and draws lines between them for visual purposes
    private void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);

    }
}
