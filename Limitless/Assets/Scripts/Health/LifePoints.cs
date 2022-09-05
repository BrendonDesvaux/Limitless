using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifePoints : MonoBehaviour
{
    private GameManager gameManager;
    public int level;
    public float lifePoints;
    private float maxLife;
    private float damage;
    private float physicalDamage;
    private float critChance;
    private float critDamage;
    private float dodgeChance;
    private float exp;
    private float expToNextLevel;
    public Slider healthBar;
    public int ID = 0;
    public float levelHealthMultiplicator;
    public float levelManaMultiplicator;
    public float levelStrengthMultiplicator;
    public float levelDexterityMultiplicator;
    public float levelIntelligenceMultiplicator;
    public float levelArmorMultiplicator;
    public float levelMagicResistanceMultiplicator;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //Get monster from list
        AllMonsters.Monster monster = gameManager.allMonsters.monsters[ID];
        lifePoints = monster.health;
        level = monster.lvl;
        maxLife = Random.Range(monster.maxHealth/1.5f, monster.maxHealth*1.5f);
        damage = Random.Range(monster.strength/2f, monster.strength*2f);
        physicalDamage = Random.Range(monster.physicalDamage/2f, monster.physicalDamage*2f);
        critChance = Random.Range(monster.critChance/2f, monster.critChance*2f);
        critDamage = Random.Range(1, monster.critDamage*2f);
        dodgeChance =  Random.Range(monster.dodgeChance/1.2f, monster.dodgeChance*1.2f);
        exp = 0;
        expToNextLevel = Random.Range(monster.expToNextLevel/1.2f, monster.expToNextLevel*1.2f);
        levelHealthMultiplicator = monster.levelHealthMultiplicator;
        levelManaMultiplicator = monster.levelManaMultiplicator;
        levelStrengthMultiplicator = monster.levelStrengthMultiplicator;
        levelDexterityMultiplicator = monster.levelDexterityMultiplicator;
        levelIntelligenceMultiplicator = monster.levelIntelligenceMultiplicator;
        levelArmorMultiplicator = monster.levelArmorMultiplicator;
        levelMagicResistanceMultiplicator = monster.levelMagicResistanceMultiplicator;
    }

    public void Recalculate(float dmg){
        Debug.Log(dodgeChance);
        if(dodgeChance < Random.Range(0, 100)){
            lifePoints -= dmg;
            if (lifePoints <= 0)
            {
                //get game manager and call death function with second child name as parameter
                gameManager.Death(ID);
                Destroy(gameObject);
            }
            healthBar.value -= 1/maxLife * dmg;
        }
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
            if(collision.gameObject.GetComponent<PlayerLife>().Recalculate(damage)){
                exp += 1;
                if (exp >= expToNextLevel)
                {
                    level += 1;
                    // create list of colors [red, gold, green, blue, purple]
                    List<Color> colors = new List<Color>();
                    colors.Add(Color.red);
                    colors.Add(Color.yellow);
                    colors.Add(Color.green);
                    colors.Add(Color.blue);
                    colors.Add(Color.magenta);
                    //search for a skinnedMeshRenderer in children and change material color
                    foreach (Transform child in transform)
                    {
                        if (child.gameObject.GetComponent<SkinnedMeshRenderer>())
                        {
                            child.gameObject.GetComponent<SkinnedMeshRenderer>().material.color = colors[level-2];
                            break;
                        }
                    }
                    //level up
                    exp = 0;
                    expToNextLevel *= 2;
                    maxLife *= levelHealthMultiplicator;
                    lifePoints = maxLife;
                    damage *= levelStrengthMultiplicator;
                }
            }
        }else if(collision.gameObject.tag == "Weapon"){
            //set animator trigger hit
            GetComponent<Animator>().SetTrigger("Hit");
            // call this script Recalculate function with damage as parameter
            Recalculate(collision.gameObject.GetComponent<Weapon>().damage);
        }
    }
}
