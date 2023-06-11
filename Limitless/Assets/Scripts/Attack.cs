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
            sphere = Instantiate(sphere);
            sphere.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);



            sphere.GetComponent<Rigidbody>().AddForce(transform.forward * 2000);
            // add script Weapon with damages equal to 10
            sphere.GetComponent<Weapon>().damage = 10;//damage;
            sphere.tag = "Weapon";
            Destroy(sphere, 5f);
        }
    }
}
