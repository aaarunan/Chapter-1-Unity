using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 12;
    Rigidbody myRigidBody;
    Vector3 velocity;
    public int coinCount;
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();



    }


    // Update is called once per frame
    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 direction = input.normalized;
        velocity = direction * speed;
        transform.Translate(direction * Time.deltaTime);
    }

    void FixedUpdate()
    {
        myRigidBody.position += velocity * Time.fixedDeltaTime;
    }

    void OnTriggerEnter(Collider triggerCollider)
    {
        if(triggerCollider.tag == "Coin")
        {
            Destroy(triggerCollider.gameObject);
            coinCount++;
        }
    }
}
