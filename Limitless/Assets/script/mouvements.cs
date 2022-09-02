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
    private string npc;

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
        //if click right mouse button and npc is not null then call gameManager.Interact(npc)
        if (Input.GetKeyDown(KeyCode.T) && npc != null)
        {
            // gameObject.GetComponent<PlayerInput>().DeactivateInput();
            if (npc != null)
                GameObject.Find("GameManager").GetComponent<GameManager>().Interact(npc);
        }
        float horizontal = Input.GetAxis("Horizontal") * turningSpeed * Time.deltaTime;
        transform.Rotate(0, horizontal, 0);

        float vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        transform.Translate(0, 0, vertical);
    }


    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            npc = other.gameObject.name;
        }
    }

    /// <summary>
    /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            npc = null;
        }
    }
}
