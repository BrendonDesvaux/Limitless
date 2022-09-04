using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Informations : MonoBehaviour
{
    public string npcName;
    public Quests.Data texts;
    public Transform toFill;

    public void Set(){
        toFill.GetChild(0).GetComponent<Text>().text = texts.name;
        toFill.GetChild(1).GetComponent<Text>().text = texts.locations;
        toFill.GetChild(2).GetComponent<Text>().text = texts.questDescription;
        //get all objects in questReward and add them as string
        string reward = "";
        foreach(string obj in texts.questReward){
            reward += " " + obj;
        }
        toFill.GetChild(3).GetComponent<Text>().text = reward;
        toFill.GetChild(4).GetComponent<Text>().text = "Location :";
        toFill.GetChild(5).GetComponent<Text>().text = "Reward :";
        toFill.GetChild(7).GetComponent<Button>().onClick.AddListener(() => toFill.GetChild(7).GetComponent<ButtonEvent>().Clicked(texts, npcName));
    }
}
