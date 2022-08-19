using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour {

	private int mapWidthInTiles, mapDepthInTiles;

	[SerializeField]
	private GameObject tilePrefab;

	[SerializeField]
	TerrainType[] terrainTypes;

	TerrainType tert;

	void Start() {
		mapWidthInTiles = Random.Range (10, 100);
		mapDepthInTiles = Random.Range (10, 100);
		tert = ChooseTerrainType ();
		GenerateMap ();
	}

	void GenerateMap() {

		// Create a random number of Wave
		Wave[] waves = new Wave[Random.Range (2, 5)];
		for (int i = 0; i < waves.Length; i++) {
			waves [i] = new Wave ();
			waves [i].seed = Random.Range (0, 60000);
			waves [i].amplitude = Random.Range (0.25f, 1);
			waves [i].frequency = Random.Range (0.5f, 1);
		}
		// get the tile dimensions from the tile Prefab
		Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer> ().bounds.size;
		int tileWidth = (int)tileSize.x;
		int tileDepth = (int)tileSize.z;

		// for each Tile, instantiate a Tile in the correct position
		for (int xTileIndex = 0; xTileIndex < mapWidthInTiles; xTileIndex++) {
			for (int zTileIndex = 0; zTileIndex < mapDepthInTiles; zTileIndex++) {
				// calculate the tile position based on the X and Z indices
				Vector3 tilePosition = new Vector3(this.gameObject.transform.position.x + xTileIndex * tileWidth, 
					this.gameObject.transform.position.y, 
					this.gameObject.transform.position.z + zTileIndex * tileDepth);
				// instantiate a new Tile
				GameObject tile = Instantiate (tilePrefab, tilePosition, Quaternion.identity) as GameObject;
				tile.GetComponent<TileGeneration> ().terrainType = tert;
				tile.GetComponent<TileGeneration> ().waves = waves;
			}
		}
	}

	TerrainType ChooseTerrainType() {
		// choose randomly the terrain
		return terrainTypes[Random.Range(0, terrainTypes.Length)];
	}

	[System.Serializable]
	public class TerrainType {
		public string name;
		public Texture2D texture;
	}
}
