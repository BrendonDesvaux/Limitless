using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    private Vector3 sideTarget;
    private Vector3 target;
    private int targetID;
    private  List<(Transform, int)> detectedPlayers;

    void Start(){
        detectedPlayers = new List<(Transform, int)>();
        target = new Vector3(0,0,0);
        sideTarget = new Vector3(0,0,0);
        //Invoke Pathfinding once every quarter second
        InvokeRepeating("PathFinding", 0.0f, 0.10f);
    }

     /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (detectedPlayers.Count > 0)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * 10);
        }
    }

    /// <summary>
    /// LateUpdate is called every frame, if the Behaviour is enabled.
    /// It is called after all Update functions have been called.
    /// </summary>
    private void PathFinding()
    {
        //check if still on ground with raycast
        if (detectedPlayers.Count > 0){
            // target = detectedPlayers[0].Item1.position;
            targetID = detectedPlayers[0].Item2;
            //Look at target without y axis
            transform.LookAt(new Vector3(detectedPlayers[0].Item1.position.x, transform.position.y, detectedPlayers[0].Item1.position.z));


            // Get distance between transform and detectedPlayers[0].Item1.position
            Vector3 offset = transform.position - detectedPlayers[0].Item1.position;
            float stepDistance = offset.sqrMagnitude;
            float distance = Mathf.Sqrt(stepDistance);
            //use raycast to check if there is no obstacle in the way
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distance))
            {
                    if (hit.collider.gameObject.GetInstanceID() == detectedPlayers[0].Item2)
                    {
                        transform.LookAt(detectedPlayers[0].Item1.position);
                        sideTarget = new Vector3(0, 0, 0);
                    }else if(sideTarget != new Vector3(0,0,0))
                    {
                        transform.LookAt(sideTarget);
                        //when side target is reached with an error margin of 0.1, reset it to 0,0,0
                        if ((transform.position- sideTarget).sqrMagnitude < 0.2f)
                        {
                            sideTarget = new Vector3(0, 0, 0);
                        }
                    }else{
                        List<(Vector3, int)> notFoundPath = new List<(Vector3, int)>();
                        bool found = false;
                        for (int i = 0; i < 20; i++)
                        {
                            Vector3 direction = Quaternion.Euler(0, 5*i, 0) * transform.forward;

                            if (Physics.Raycast(transform.position, direction, out RaycastHit hitRight, distance));  //Vector3.Distance(transform.position, hit.point)))
                            {
                                if (hitRight.collider == null)
                                    {
                                        direction =transform.position+direction*distance;
                                        //raycast from hitright end to player and check if collider exists
                                        if (Physics.Linecast(direction, detectedPlayers[0].Item1.position, out RaycastHit hitCheck))
                                        {
                                            //check if hit object
                                            if (hitCheck.collider.gameObject.tag != null){
                                            //check if hit player with ID and move forward if not
                                            if (hitCheck.collider.gameObject.GetInstanceID() == detectedPlayers[0].Item2)
                                            {
                                                sideTarget = direction;
                                                transform.LookAt(sideTarget);
                                                found = true;
                                                break;
                                            }else if(notFoundPath.Count == 0){
                                                notFoundPath.Add((direction, i));
                                            }
                                            }else if(notFoundPath.Count == 0){
                                                notFoundPath.Add((direction, i));
                                            }
                                        }

                                }
                            }       
                        }
                        if (!found){
                            for (int i = 0; i < 20; i++)
                            {
                                Vector3 direction = Quaternion.Euler(0, -5*i, 0) * transform.forward;

                                if (Physics.Raycast(transform.position, direction, out RaycastHit hitLeft, distance));  //Vector3.Distance(transform.position, hit.point)))
                                {
                                    if (hitLeft.collider == null)
                                    {
                                        direction =transform.position+direction*distance;

                                            //raycast from hitLeft end to player and check if collider exists
                                        if (Physics.Linecast(direction, detectedPlayers[0].Item1.position, out RaycastHit hitCheck))
                                        {
                                            //check if hit object
                                            if (hitCheck.collider.gameObject.tag != null){
                                                //check if hit player with ID and move forward if not
                                                if (hitCheck.collider.gameObject.GetInstanceID() == detectedPlayers[0].Item2)
                                                {
                                                    sideTarget = direction;
                                                    transform.LookAt(sideTarget);
                                                    transform.Translate(Vector3.forward * Time.deltaTime * 10);
                                                    found = true;
                                                    break;
                                                }else if(notFoundPath.Count == 0){
                                                notFoundPath.Add((direction, i));
                                            }else if (notFoundPath.Count == 1 && notFoundPath[0].Item2 <= i){
                                                if (notFoundPath[0].Item2 == i){
                                                    notFoundPath.Add((direction, i));
                                                }else{
                                                    notFoundPath.RemoveAt(0);
                                                    notFoundPath.Add((direction, i));
                                                }
                                            }
                                            }else if (notFoundPath.Count == 1 && notFoundPath[0].Item2 <= i){
                                                if (notFoundPath[0].Item2 == i){
                                                    notFoundPath.Add((direction, i));
                                                }else{
                                                    notFoundPath.RemoveAt(0);
                                                    notFoundPath.Add((direction, i));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (!found){
                            int random = Random.Range(0, notFoundPath.Count);
                            sideTarget = notFoundPath[random].Item1;
                        }
                    }
                }
        }
    }

    // OnTriggerEnter when object enters trigger and save its position in detectedObjects list
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            detectedPlayers.Add((other.gameObject.transform, other.gameObject.GetInstanceID()));
        }
    }

    // OnTriggerExit when object exits trigger and remove its position from detectedObjects list
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            for (int i = 0; i < detectedPlayers.Count; i++)
            {
                if (detectedPlayers[i].Item2 == other.gameObject.GetInstanceID())
                {
                    detectedPlayers.Remove((detectedPlayers[i].Item1, detectedPlayers[i].Item2));
                }

            };
        }
    }
}
