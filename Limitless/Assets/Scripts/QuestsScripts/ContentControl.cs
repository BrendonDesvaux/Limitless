using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentControl : MonoBehaviour
{
    // Start is called before the first frame update
    public void RemoveContent()
    {
            transform.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x,390);
            foreach (Transform child in transform) {
                GameObject.Destroy(child.gameObject);
            }
    }
}
