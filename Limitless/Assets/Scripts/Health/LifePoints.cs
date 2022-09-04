using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifePoints : MonoBehaviour
{
    private GameManager gameManager;
    public float lifePoints;
    private float maxLife;
    private float damage;
    private float physicalDamage;
    private float critChance;
    private float critDamage;
    public Slider healthBar;
    public int ID = 0;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //Get monster from list
        AllMonsters.Monster monster = gameManager.allMonsters.monsters[ID];
        lifePoints = monster.health;
        maxLife = Random.Range(monster.maxHealth/1.5f, monster.maxHealth*1.5f);
        damage = Random.Range(monster.strength/2f, monster.strength*2f);
        physicalDamage = Random.Range(monster.physicalDamage/2f, monster.physicalDamage*2f);
        critChance = Random.Range(monster.critChance/2f, monster.critChance*2f);
        critDamage = Random.Range(1, monster.critDamage*2f);
    }

    public void Recalculate(float dmg){
        lifePoints -= dmg;
        if (lifePoints <= 0)
        {
            //get game manager and call death function with second child name as parameter
            gameManager.Death(ID);
            Destroy(gameObject);
        }
        healthBar.value -= 1/maxLife * dmg;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            float totalDamage = damage * physicalDamage;
            if (Random.Range(1f, 101f) < critChance)
            {
                totalDamage *= critDamage;
            }
            // call collision "PlayerLife" Recalculate function with damage as parameter
            collision.gameObject.GetComponent<PlayerLife>().Recalculate(damage);
        }else if(collision.gameObject.tag == "Weapon"){
            //set animator trigger hit
            GetComponent<Animator>().SetTrigger("Hit");
            // call this script Recalculate function with damage as parameter
            Recalculate(1);
        }
    }
}
