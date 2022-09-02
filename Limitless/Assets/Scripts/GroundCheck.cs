using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public mouvements mouvements;
    //if player collider is collinding with groung
    private void OnTriggerEnter(Collider collision)
    {
        //if collider is ground
        if (collision.gameObject.tag == "ground")
        {
            mouvements.isGrounded = true;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            mouvements.isGrounded = false;
        }
    }
}
