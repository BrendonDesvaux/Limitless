using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Reflection;

public class GameManager : MonoBehaviour
{

    private float min;
    private float max;
    public  GameObject dungeonPortal;
    public  GameObject plane;
    public List<Vector3> dungeonsSpawnPoints;
    public int generatedDungeonType;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        instQuest = new Quests();
        instAllMonsters = new AllMonsters();
        instPlayerInfos = new PlayerInfos();
        playerInfo = GetJSONPlayerInfos();
        player = GetJSONPlayerQuest();
    }

    void Start()
    {
        dungeonsSpawnPoints = new List<Vector3>();
        RecTr = questPanel.GetComponent<RectTransform>();
        height = RecTr.rect.height;
        //call function once every 4 hours
        InvokeRepeating("GenerateDungeon", 0, 14400);
    }

    // Update is called once per frame
    public void LoadingScene(string sceneName)
    { 
        StartCoroutine(LoadDungeonScene(sceneName));
    }

    IEnumerator LoadDungeonScene(string sceneName)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    void GenerateDungeon()
    {
        // Generate new random position
        Vector3 pos = RandomPosition();

        // Loop while new random position is closer than 10 units to existing dungeon portal
        while (Vector3.Distance(pos, dungeonPortal.transform.position) < 10)
        {
            // Generate new random position
            pos = RandomPosition();
        }
        //check if scene is "sample scene
        if (SceneManager.GetActiveScene().name == "SampleScene"){
            //Instantiate Dungeon portal in position
            Instantiate(dungeonPortal, pos, Quaternion.identity);
        }
        dungeonsSpawnPoints.Add(pos);
    }

    Vector3 RandomPosition()
    {
        // Get plane size and height in a random position
        min = plane.GetComponent<Renderer>().bounds.min.x;
        max = plane.transform.position.x + plane.GetComponent<Renderer>().bounds.max.x;
        float randX = UnityEngine.Random.Range(min, min + max);
        min = plane.GetComponent<Renderer>().bounds.min.z;
        max = plane.transform.position.z + plane.GetComponent<Renderer>().bounds.max.z;
        float randZ = UnityEngine.Random.Range(min, min + max);
        RaycastHit hit;
        float yVal = 0;
        Ray ray = new Ray(new Vector3(randX,3000,randZ), Vector3.down);
        if (plane.GetComponent<MeshCollider>().Raycast(ray, out hit, 2000)) {
            yVal = hit.point.y + 10;
        }
        return new Vector3(randX, yVal, randZ);
    }

    void OnSceneLoaded()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            //loop through all dungeons
            foreach (Vector3 pos in dungeonsSpawnPoints)
            {
                //Instantiate Dungeon portal in position
                Instantiate(dungeonPortal, pos, Quaternion.identity);
            }
        }
    }

    private Quests.JSONData player;
    private Quests.JSONData quests;
    public PlayerInfos.PlayerInfo playerInfo;
    private AllMonsters.Monsters allMonsters;
    public TextAsset jsonPlayerQuests;
    public TextAsset jsonNPC;
    public TextAsset jsonPlayer;
    public TextAsset jsonAllMonsters;
    private Quests instQuest;
    private AllMonsters instAllMonsters;
    private PlayerInfos instPlayerInfos;

    public GameObject panel;
    public Transform questPanel;
    public Transform questDescrPanel;
    private float height;
    private RectTransform RecTr;
    public GameObject questB;
    private int yPos = -55;

    public Quests.JSONData GetJSONPlayerQuest(){
        return instQuest.Get(jsonPlayerQuests);
    } 

    public Quests.JSONData GetJSONNPC(){
        return instQuest.Get(jsonNPC);
    }
    public PlayerInfos.PlayerInfo GetJSONPlayerInfos(){
        return instPlayerInfos.Get(jsonPlayer);
    } 

    public AllMonsters.Monsters GetJSONMonsters(){
        return instAllMonsters.Get(jsonAllMonsters);
    }


    public void Interact(string name) {
            player = GetJSONPlayerQuest();
            panel.SetActive(true);
            panel.transform.GetChild(1).GetComponent <Text>().text = name;
            Quests.JSONData npcData = GetJSONNPC();
            foreach(Quests.JSData npc in npcData.npcs) {
                if (npc.npcName == name) {
                    foreach(Quests.Data quest in npc.quests) {
                        GameObject q = Instantiate(questB, new Vector3(0, yPos, 0), Quaternion.identity) as GameObject;
                        q.transform.SetParent(questPanel, false);
                        q.transform.GetChild(0).GetComponent<Text>().text = quest.name;

                        q.transform.GetComponent<Informations>().npcName = npc.npcName;
                        q.transform.GetComponent<Informations>().texts = quest;
                        q.transform.GetComponent<Informations>().toFill = questDescrPanel;

                        if (Math.Abs(yPos) > height)
                            RecTr.sizeDelta = new Vector2(RecTr.sizeDelta.x,yPos * -1 + 20);
                        yPos -= 20;
                    }
                }
            }
            yPos = -55;

            // JSON< result = JsonConvert.SerializeObject(emp);
    }

    public void PlayerQuestRequest(Quests.Data json, string npcName){
        bool npcExist = false;
        bool done = false;
        bool exist = false;
        foreach(Quests.JSData npc in player.npcs) {
            if (npc.npcName == npcName) {
                npcExist = true;
                foreach(Quests.Data quest in npc.quests){
                    if(quest.name == json.name)
                        exist = true;

                }
                if (!exist){
                    npc.quests.Add(json);
                    done = true;
                }
            }
        }
        if(!exist){
            if (done && npcExist){
                string str = JsonUtility.ToJson(player);
                System.IO.File.WriteAllText("./Assets/Scripts/JSON/PlayerQuests.json", str);
            }else{
                Quests.JSData newNPC = new Quests.JSData();
                newNPC.npcName = npcName;
                newNPC.quests = new List<Quests.Data>();
                newNPC.quests.Add(json);
                player.npcs.Add(newNPC);
                string str = JsonUtility.ToJson(player);
                System.IO.File.WriteAllText("./Assets/Scripts/JSON/PlayerQuests.json", str);
            }
        }
    }

    // Death function, when npc dies search in player quests if the ID is in the list of questObjectsIDs and if it is, increment questCompletion
    public void Death(int npcID){
        foreach(Quests.JSData npc in player.npcs) {
            foreach(Quests.Data quest in npc.quests){
                foreach(int id in quest.questObjectsIDs){
                    if (id == npcID){
                        quest.questCompletion++;
                        if (quest.questCompletion >= quest.questCompletionNumber){
                            quest.complete = true;
                        }
                    }
                }
            }
        }
        string str = JsonUtility.ToJson(player);
        System.IO.File.WriteAllText("./Assets/Scripts/JSON/PlayerQuests.json", str);
    }

    //use playerInfo and search for the string given in parameters and update it with the new float value
    public void UpdatePlayerInfo(string info, object newValue){
        Type typeClass = typeof(PlayerInfos.PlayerInfo);
        FieldInfo[] properties = typeClass.GetFields();
        foreach(FieldInfo property in properties){
            if (property.Name == info){
                //get type of property
                string type = property.FieldType.Name;
                if (type == "Int32"){
                    property.SetValue(playerInfo, (int)newValue);
                }else if (type == "Single"){
                    property.SetValue(playerInfo, (float)newValue);
                }else if (type == "String"){
                    property.SetValue(playerInfo, newValue.ToString());
                }
            }
        }

        string str = JsonUtility.ToJson(playerInfo);
        System.IO.File.WriteAllText("./Assets/Scripts/JSON/PlayerInfos.json", str);
    }
}
