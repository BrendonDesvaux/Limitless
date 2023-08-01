
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMOVETEST : MonoBehaviour
{
    public bool master = true;
    public float angle = 70f;
    private GameObject focused;
    private Vector3 tempStep;
    private List<GameObject> bckpDetectedObjects = new List<GameObject>();
    private List<GameObject> detectedObjects = new List<GameObject>();
    private Rigidbody rb;
    private float bodyWidth;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Detection", 1f, 0.3f);
        rb = GetComponent<Rigidbody>();
        bodyWidth = GetComponent<CapsuleCollider>().radius;
    }

    // Update is called once per frame
    void Update()
    {
        if (focused != null)
        {
            var fwd = focused.transform.position - transform.position;
            fwd.Normalize();
            var dist = Vector3.Distance(transform.position, focused.transform.position);
            if (tempStep == Vector3.zero)
            {
                var raycast = Physics.Raycast(transform.position, fwd, out RaycastHit hit, dist + 1f, 1 << LayerMask.NameToLayer("Default"), QueryTriggerInteraction.Ignore);
                if (raycast && hit.collider.gameObject != focused)
                {
                    MeshFilter yourMeshFilter = hit.collider.GetComponent<MeshFilter>();
                    float bufferNumb = 0.2f;
                    // Now you have the MeshFilter component and can perform your calculations
                    Vector3 hitPoint = hit.point;
                    Vector3 meshCenter = yourMeshFilter.transform.TransformPoint(yourMeshFilter.mesh.bounds.center);
                    Vector3 directionToHitPoint = hitPoint - meshCenter;
                    Vector3 meshExtents = yourMeshFilter.mesh.bounds.extents;

                    float dotProduct = Vector3.Dot(directionToHitPoint, yourMeshFilter.transform.right);

                    float distanceToRight;
                    float distanceToLeft;

                    distanceToRight = -dotProduct + meshExtents.x;
                    distanceToLeft = dotProduct + meshExtents.x;
                    Debug.Log(distanceToRight + " " + distanceToLeft + " those are the distances");
                    Debug.Log(meshCenter + " " + meshExtents.x + " those are the mesh&extents");
                    // meshExtents to world space
                    meshExtents = yourMeshFilter.transform.TransformVector(meshExtents);
                    if (distanceToRight > distanceToLeft)
                    {
                        tempStep = meshCenter - yourMeshFilter.transform.right * (distanceToRight + bodyWidth + bufferNumb);
                    }
                    else
                    {
                        tempStep = meshCenter + yourMeshFilter.transform.right * (distanceToLeft + bodyWidth + bufferNumb);
                    }


                    tempStep.y = transform.position.y;

                    // Now use tempStep as the destination for the NPC's movement logic

                    Debug.Log(tempStep);
                    //spaw a round red sphere at the hit point
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.position = tempStep;
                    sphere.transform.localScale = new Vector3(2f, 2f, 2f);
                    sphere.GetComponent<Renderer>().material.color = Color.red;
                }
            }

            if (Vector3.Distance(transform.position, tempStep) < 0.1f)
            {
                tempStep = Vector3.zero;
                transform.rotation = Quaternion.LookRotation(fwd);
                if (Search(fwd, dist))
                {
                    focused = null;
                    return;
                }
            }

            // if tempStep is not zero, move towards tempStep
            if (tempStep != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(tempStep);
                rb.MovePosition(Vector3.MoveTowards(transform.position, tempStep, 0.07f));
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(fwd);
                if (dist > 2f)
                {
                    rb.MovePosition(Vector3.MoveTowards(transform.position, focused.transform.position, 0.07f));
                }
            }
        }
    }

    bool Search(Vector3 fwd, float dist)
    {
        var raycast = Physics.Raycast(transform.position, fwd, out RaycastHit hit, dist, 1 << LayerMask.NameToLayer("Default"), QueryTriggerInteraction.Ignore);
        if (raycast && hit.collider.gameObject == focused)
        {
            return true;
        }
        return false;
    }

    void Detection()
    {
        detectedObjects = new List<GameObject>();
        var colliders = Physics.OverlapSphere(transform.position, 35f);
        Vector3 fwd = transform.position;
        foreach (var collider in colliders)
        {
            Vector3 toOther = collider.gameObject.transform.position - transform.forward;
            if (!(collider.gameObject.layer == LayerMask.NameToLayer("Default")) || collider.isTrigger || !(Vector3.Dot(toOther.normalized, fwd) / 1000 > Math.Cos(angle * 0.5 * Mathf.Deg2Rad)))
            {
                continue;
            }
            if (focused == null && collider.tag == "Player")
            {
                focused = collider.gameObject;
            }
            detectedObjects.Add(collider.gameObject);
        }
    }
}
