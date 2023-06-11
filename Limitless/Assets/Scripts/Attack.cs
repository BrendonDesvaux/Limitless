using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float damage;
    public GameObject sphere;
    // if L is pressed, instantiate a sphere and launch it forward, destroying it after 1 second
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            //clone the sphere
            GameObject sphereCopy = sphere;
            // instantiate sphere at the position of the player
            sphereCopy = Instantiate(sphere, transform.position, Quaternion.identity) as GameObject;


            sphereCopy.GetComponent<Rigidbody>().AddForce(transform.forward * 2000);
            // add script Weapon with damages equal to 10
            sphereCopy.GetComponent<Weapon>().damage = 10;//damage;
            sphereCopy.GetComponent<Weapon>().timeToLive = 5;//damage;
            sphereCopy.tag = "Weapon";
        }
    }
}
