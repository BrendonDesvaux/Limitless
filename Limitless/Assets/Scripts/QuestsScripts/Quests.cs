using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Reflection;

public class Quests {

    [Serializable]
    public class JSONData {
        public List<JSData> npcs;
    }

    [Serializable]
    public class JSData {
        public string npcName;
        public List<Data> quests;
    }

    [Serializable]
    public class Data {
        public string name;
        public int type;
        public string locations;
        public int questCompletion;
        public int questCompletionNumber;
        public string questDescription;
        public int[] questObjectsIDs;
        public string[] questReward;
        public bool complete;
    }

    public JSONData Get(TextAsset json) {
        return JsonUtility.FromJson<JSONData>(json.text);
    }
}

[Serializable]
public class PlayerInfos {
    [Serializable]
    public class PlayerInfo {
        //all basic player Infos with getter and setter
        public object name;
        public int level;
        public float exp;
        public float expToNextLevel;
        public float health;
        public float maxHealth;
        public float mana;
        public float maxMana;
        public float strength;
        public float dexterity;
        public float intelligence;
        public float luck;
        public float armor;
        public float magicResistance;
        public float movementSpeed;
        public float attackSpeed;
        public float physicalDamage;
        public float magicalDamage;
        public float critChance;
        public float critDamage;
        public float dodgeChance;
    }

    public PlayerInfo Get(TextAsset json) {
        return JsonUtility.FromJson<PlayerInfo>(json.text);
    }
}

[Serializable]
public class AllMonsters {
    //list of Monster
    [Serializable]
    public class Monsters {
        public List < Monster > monsters;
    }

    //class containing am monster informations"
    [Serializable]
    public class Monster {
        public int id;
        public string name;
        public string description;
        public float exp;
        public float expToNextLevel;
        public float health;
        public float maxHealth;
        public float mana;
        public float maxMana;
        public float strength;
        public float armor;
        public float magicResistance;
        public float movementSpeed;
        public float attackSpeed;
        public float physicalDamage;
        public float magicalDamage;
        public float critChance;
        public float critDamage;
        public float dodgeChance;
        public int rewardExp;
        public int gold;
        public int lvl;
        public int[] possibleDrops;
    }

    public Monsters Get(TextAsset json) {
        return JsonUtility.FromJson<Monsters>(json.text);
    }
}