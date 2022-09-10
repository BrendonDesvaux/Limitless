using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnPortal : MonoBehaviour
{
    public GameObject[] mobs;
    public GameObject Spawnpoint;
    public Animator animWalk;
    public Animator animRun;
    public float Timer = 1f;


    // Start is called before the first frame update
    void Start()
    {
        //Vector3 randomizePosition = new Vector3(Random.Range(0, 100), 0, Random.Range(0, 100));
        //instantiate mobs in portal 
        for (int i = 0; i < mobs.Length; i++)
        {
            Instantiate(mobs[i], Spawnpoint.transform.position, Quaternion.LookRotation(Vector3.back));
        }
        //for each mob in portal, set the animator to walk
        foreach (GameObject mob in mobs)
        {
            mob.GetComponent<Animator>().SetBool("Walk", true);
            mob.GetComponent<Rigidbody>().AddForce(mob.transform.forward * 10, ForceMode.Impulse);

        }

    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 randomizePosition = new Vector3(Random.Range(0, 100), 0, Random.Range(0, 100));
   
        Timer -= Time.deltaTime;
        if (Timer <= 0f)
        {
            for (int i = 0; i < mobs.Length; i++)
            {
                Instantiate(mobs[i], Spawnpoint.transform.position, Quaternion.LookRotation(Vector3.back));
            }
            foreach (GameObject mob in mobs)
            {
                mob.GetComponent<Animator>().SetBool("Walk", true);
                mob.GetComponent<Rigidbody>().AddForce(mob.transform.forward * 10, ForceMode.Impulse);

            }
            Timer = 1f;
        }
    }
}
