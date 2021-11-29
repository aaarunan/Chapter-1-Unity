using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 7;
    public float smoothMoveTime = .1f;
    public float turnSpeed = 8;

    float angle;
    float smoothMoveVelocity;
    float smoothInputMagnitude;
    Vector3 velocity;

    Rigidbody rigidbody;

    bool gameOver = false;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        FindObjectOfType<Guard>().OnPlayerFound += OnGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (gameOver)
        {
            input = Vector3.zero;
        }
        
        Vector3 direction = input.normalized;

        float inputMagnitude = direction.magnitude;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);

        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);
        transform.Translate(transform.forward * smoothInputMagnitude * speed * Time.deltaTime, Space.World);
        transform.eulerAngles = Vector3.up *angle;
        velocity = transform.forward * smoothInputMagnitude * speed;


    }

    private void FixedUpdate()
    {
        rigidbody.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);

    }

    void OnGameOver()
    {
        print("Spotted!");
        gameOver = true;
    }

   
}
