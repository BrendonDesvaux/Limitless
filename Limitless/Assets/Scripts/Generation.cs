using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Generation : MonoBehaviour
{
    private int previousChunkX = 0;
    private int previousChunkY = 0;
    public GameObject Player;
    // how many chunks are shown around the player
    public int shownChunks = 2;
    // size of each chunk
    public int chunkSize = 20;
    // frequency of ups and downs
    public float frequency = 0.1f;
    // height of ups and downs
    public float peak = 10f;
    // LOD of the center chunk
    public int basicLOD = 1;
    // basic LOD range of the chunks around the center chunk, 0 means only the center chunk is basicLOD
    public int basicLODRange = 1;

    private List<Vector2> chunks = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        float playerX = Player.transform.position.x;
        float playerY = Player.transform.position.z;

        int actualChunkX = Mathf.CeilToInt(playerX / chunkSize);
        int actualChunkY = Mathf.CeilToInt(playerY / chunkSize);
        UpdateMesh(actualChunkX, actualChunkY);
    }

    // Update is called once per frame
    void Update()
    {
        float playerX = Player.transform.position.x;
        float playerY = Player.transform.position.z;

        int actualChunkX = Mathf.FloorToInt(playerX / chunkSize);
        int actualChunkY = Mathf.FloorToInt(playerY / chunkSize);
        if (previousChunkX != actualChunkX || previousChunkY != actualChunkY)
        {
            previousChunkX = actualChunkX;
            previousChunkY = actualChunkY;
            UpdateMesh(actualChunkX, actualChunkY);
        }
    }

    void UpdateMesh(int actualChunkX, int actualChunkY)
    {
        float playerX = Player.transform.position.x;
        float playerY = Player.transform.position.z;

        // Create a new Mesh object
        Mesh mesh = new Mesh();


        List<Vector3> verticesList = new List<Vector3>();
        List<int> trianglesList = new List<int>();
        List<Vector3> normalsList = new List<Vector3>();
        List<Vector2> uvList = new List<Vector2>();

        int LOD = 1;
        int vI = 0;
        for (int i = actualChunkX - shownChunks; i <= shownChunks + actualChunkX; i++)
        {
            for (int j = actualChunkY - shownChunks; j <= shownChunks + actualChunkY; j++)
            {
                // bool exists = false;
                // for (int n = 0; n < chunks.Count; n++)
                // {
                //     if (chunks[n].x == i && chunks[n].y == i)
                //     {
                //         exists = true;
                //         break;
                //     }
                // }
                // if (exists)
                //     continue;
                //lod = the smallest distance to player's chunk
                if (i == actualChunkX && j == actualChunkY)
                    LOD = basicLOD;
                else
                    LOD = Mathf.Max(Mathf.Abs(actualChunkX - i), Mathf.Abs(actualChunkY - j)) - basicLODRange + basicLOD;
                if (LOD < basicLOD)
                    LOD = basicLOD;
                for (int x = i * chunkSize; x < (i + 1) * chunkSize; x += LOD)
                {
                    for (int y = j * chunkSize; y < (j + 1) * chunkSize; y += LOD)
                    {
                        float scaledX = (float)(x * frequency);
                        float scaledY = (float)(y * frequency);
                        float depth1 = Mathf.PerlinNoise(scaledX, scaledY);

                        scaledX = (float)((x + LOD) * frequency);
                        float depth2 = Mathf.PerlinNoise(scaledX, scaledY);

                        scaledY = (float)((y + LOD) * frequency);
                        float depth3 = Mathf.PerlinNoise(scaledX, scaledY);

                        scaledX = (float)(x * frequency);
                        float depth4 = Mathf.PerlinNoise(scaledX, scaledY);

                        depth1 *= peak;
                        depth2 *= peak;
                        depth3 *= peak;
                        depth4 *= peak;

                        Vector3[] vertices = new Vector3[4]
                        {
                            new Vector3(x, depth1, y),
                            new Vector3(x+LOD, depth2, y),
                            new Vector3(x+LOD, depth3, y+LOD),
                            new Vector3(x, depth4, y+LOD)
                        };

                        int[] triangles = new int[6]
                        {
                            vI, vI+2, vI+1,
                            vI, vI+3, vI+2
                        };
                        Vector3[] normals = new Vector3[4]
                        {
                            -Vector3.forward,
                            -Vector3.forward,
                            -Vector3.forward,
                            -Vector3.forward
                        };
                        Vector2[] uv = new Vector2[4]
                        {
                            new Vector2(0, 0),
                            new Vector2(1, 0),
                            new Vector2(1, 1),
                            new Vector2(0, 1)
                        };
                        verticesList.AddRange(vertices);
                        trianglesList.AddRange(triangles);
                        normalsList.AddRange(normals);
                        uvList.AddRange(uv);
                        vI += 4;
                    }
                }
            }
        }

        mesh.vertices = verticesList.ToArray();
        mesh.triangles = trianglesList.ToArray();
        mesh.normals = normalsList.ToArray();
        mesh.uv = uvList.ToArray();
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;


    }


}


