using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    }

    void Start()
    {
        dungeonsSpawnPoints = new List<Vector3>();
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

    void OnLevelWasLoaded()
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
}
