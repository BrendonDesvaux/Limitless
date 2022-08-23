using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouvements : MonoBehaviour
{
    private Rigidbody rb;
    public bool isGrounded = false;
    public float jumpAmount = 10;
    public float movementSpeed = 10;
    public float runningSpeed = 20;
    public float turningSpeed = 60;
    public float jumpHeight = 1.5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
        if(isGrounded == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector2.up * jumpAmount, ForceMode.Impulse);
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = runningSpeed;
            }
            else
            {
                movementSpeed = 10;
            }
        }
        float horizontal = Input.GetAxis("Horizontal") * turningSpeed * Time.deltaTime;
        transform.Rotate(0, horizontal, 0);

        float vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        transform.Translate(0, 0, vertical);
    }


}
