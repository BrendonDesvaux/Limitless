using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // if L is pressed, instantiate a sphere and launch it forward, destroying it after 1 second
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = transform.position;
            sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            sphere.AddComponent<Rigidbody>();
            sphere.GetComponent<Rigidbody>().AddForce(transform.forward * 1000);
            sphere.tag = "Weapon";
            Destroy(sphere, 5);
        }
    }
}
