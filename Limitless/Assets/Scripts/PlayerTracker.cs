using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerTracker : MonoBehaviour
{
    public GameObject target;
    public float rotateSpeed = 50;
    Vector3 offset;
    public Transform parentObject;
    public float zoomLevel = 10;
    public float sensitivity = 1;
    public float speed = 30;
    public float maxZoom = 20;

    public float clampMax = 70;
    public float clampMin = -4;

    float clamped;
    public float zoomPosition;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        offset = target.transform.position - transform.position;
    }

    void LateUpdate()
    {
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;
        clamped += vertical;
        clamped = Mathf.Clamp(clamped, clampMin, clampMax);
        parentObject.rotation = Quaternion.Euler(clamped, parentObject.rotation.eulerAngles.y, 0);
        target.transform.Rotate(0, horizontal, 0);
        zoomLevel -= Input.mouseScrollDelta.y * sensitivity;
        zoomLevel = Mathf.Clamp(zoomLevel, 0, maxZoom);
        zoomPosition = Mathf.MoveTowards(zoomPosition, zoomLevel, speed * Time.deltaTime);
        transform.position = parentObject.position - (transform.forward * zoomPosition);
    }
}