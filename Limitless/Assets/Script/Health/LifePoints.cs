using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifePoints : MonoBehaviour
{
    public float lifePoints;
    private float maxLife;
    public Slider healthBar;

    void Start()
    {
        maxLife = GameObject.Find("GameManager").GetComponent<GameManager>().playerInfo.maxHealth;
    }

    public void Recalculate(float dmg){
        lifePoints -= dmg;
        if (lifePoints <= 0)
        {
            //get game manager and call death function with second child name as parameter
            GameObject.Find("GameManager").GetComponent<GameManager>().Death(int.Parse(transform.GetChild(1).name));
            Destroy(gameObject);
        }
        healthBar.value -= 1/maxLife * dmg;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // call collision "PlayerLife" Recalculate function with damage as parameter
            collision.gameObject.GetComponent<PlayerLife>().Recalculate(1);
        }
    }
}
