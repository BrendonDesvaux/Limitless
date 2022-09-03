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

    public void Recalculate(float dmg){
        lifePoints -= dmg;
        if (lifePoints <= 0)
        {   lifePoints = 0;

            Application.Quit();
            //stop application in Unity Debugger
            Debug.Break();
            lifePoints = 10;
            healthBar.value = maxLife;
        }
        healthBar.value -= 1/maxLife * dmg;
        gameManager.UpdatePlayerInfo("health", lifePoints);
    }
}
