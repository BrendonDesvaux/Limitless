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

    public float clampMax = 90;
    public float clampMin = -4;

    float clamped;
    public float zoomPosition;
    float rotation = 0.2f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        offset = target.transform.position - transform.position;
    }

    void LateUpdate()
    {
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;
        target.transform.Rotate(0, horizontal, 0);
        transform.Rotate(vertical, 0, 0);
        clamped += vertical;
        clamped = Mathf.Clamp(clamped, clampMin, clampMax);
        transform.rotation = Quaternion.Euler(clamped, 0, 0);


        float desiredAngle = target.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
        transform.position = target.transform.position - (rotation * offset);


        //transform.LookAt(target.transform);

        zoomLevel += Input.mouseScrollDelta.y * sensitivity;
        zoomLevel = Mathf.Clamp(zoomLevel, 0, maxZoom);
        zoomPosition = Mathf.MoveTowards(zoomPosition, zoomLevel, speed * Time.deltaTime);
        transform.position = parentObject.position - (transform.forward * zoomPosition);

    }
}
