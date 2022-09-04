using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouvements : MonoBehaviour
{
    private Rigidbody rb;
    public bool isGrounded = false;
    public float jumpAmount = 10;
    public float movementSpeed = 30;
    public float turningSpeed = 60;
    private string npc;
    public bool locked;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!locked){
        if(isGrounded == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector2.up * jumpAmount, ForceMode.Impulse);
            }
        }
        //if click right mouse button and npc is not null then call gameManager.Interact(npc)
        if (Input.GetKeyDown(KeyCode.T) && npc != null)
        {
            if (npc != null){
                // gameObject.GetComponent<PlayerInput>().DeactivateInput();
                //get mouse to be movable
                locked = true;
                Cursor.lockState = CursorLockMode.None;
                GameObject.Find("GameManager").GetComponent<GameManager>().Interact(npc);
            }
        }
        float horizontal = Input.GetAxis("Horizontal") * turningSpeed * Time.deltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, horizontal, 0));

        float vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + transform.forward * vertical);
    }
    }

    public void Reactivate(){
        locked = false;
        //get mouse locked
        Cursor.lockState = CursorLockMode.Locked;
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
