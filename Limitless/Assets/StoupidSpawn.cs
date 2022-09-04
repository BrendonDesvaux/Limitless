using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoupidSpawn : MonoBehaviour
{
    private bool stoupidWasCalled;
    public Transform fountain;
    //oncollisionenter if name = Plane invoke spawnslime of game manager once every 30 sec, if name = Plane1 spawn 10 slime from game manager above gameobject fountain
    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.name == "Plane" && !stoupidWasCalled)
        {
            //call function "StoupidInvoke()" of gamemanager
            GameObject.Find("GameManager").GetComponent<GameManager>().StoupidInvoke();
            stoupidWasCalled = true;
        }
        if (gameObject.name == "Plane (1)")
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject slime = Instantiate(GameObject.Find("GameManager").GetComponent<GameManager>().slime, new Vector3(fountain.position.x, fountain.position.y + 10, fountain.position.z), Quaternion.identity) as GameObject;
                slime.name = "Slime";
                slime.transform.parent = GameObject.Find("Monsters").transform;
            }
        }
    }
    
}
