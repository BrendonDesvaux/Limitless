using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDungeon : MonoBehaviour
{
    public int biom;

    void Start()
    {
        biom = Random.Range(0, 3);
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player"){
            GameObject.Find("GameManager").GetComponent<GameManager>().generatedDungeonType = biom;
            GameObject.Find("GameManager").GetComponent<GameManager>().LoadingScene("Dungeon");
        }
    }
}
