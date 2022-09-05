using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{ 
    private float lifePoints;
    private float maxLife;
    public Slider healthBar;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        lifePoints = gameManager.playerInfo.health;
        maxLife =  gameManager.playerInfo.maxHealth;
        healthBar.value -= 1/maxLife * (maxLife - lifePoints);
    }

    public bool Recalculate(float dmg){
        lifePoints -= dmg;
        healthBar.value -= 1/maxLife * dmg;

        if (lifePoints <= 0)
        {   
            lifePoints = 0;
            Application.Quit();
            //stop application in Unity Debugger
            lifePoints = 10;
            healthBar.value = maxLife;
            //call UpdatePlayerInfo with array of string and array of objects
            gameManager.UpdatePlayerInfo(new string[]{"health"}, new object[]{lifePoints});
            Debug.Break();
            return true;
        }
        //call UpdatePlayerInfo with array of string and array of objects
        gameManager.UpdatePlayerInfo(new string[]{"health"}, new object[]{lifePoints});
        return false;
    }
}
