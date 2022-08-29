using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorAdd : MonoBehaviour
{
    public Material mat;

    // Update is called once per frame
    void Update()
    {
        //for each child of the parent object, set the mat to the first material element
        foreach (Transform child in transform)
        {
            child.GetComponent<Renderer>().material = mat;
        }
    }
}
