using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    Vector3 destination;

    //create void oncolisionenter
    void OnCollisionEnter(Collision col)
    {
        if(this.name =="portailDesert")
        {
            destination = GameObject.Find("portailVille").transform.position;
        }else{
            destination = GameObject.Find("portailDesert").transform.position;
        }

        col.transform.position = destination + Vector3.forward * 5;
        col.transform.Rotate(Vector3.up * 90);
    }
}
