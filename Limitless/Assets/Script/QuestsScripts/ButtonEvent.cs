using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    public void Clicked(Quests.Data json, string npcName){
        GameObject.Find("GameManager").GetComponent<GameManager>().PlayerQuestRequest(json, npcName);
    }
}
