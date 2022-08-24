using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.MapGenerator.Generators;

public class LevelGeneration : MonoBehaviour {
	public GameObject water;
	public Material blueWater;
	public Material ice;
	void Start() {
		GenerateMap ();
	}

	void GenerateMap() {
		int biom = Random.Range (0, 3);
		transform.GetComponent<HeightsGenerator>().Offset = Random.Range (0, 10000000.0f);
		transform.GetComponent<HeightsGenerator>().Generate();
		transform.GetComponent<DungeonTexturesGenerator>().chosenBiom = biom;
		transform.GetComponent<DungeonTexturesGenerator>().Generate();
		if(biom == 0) {
			transform.GetComponent<GrassGenerator>().Generate();
		}
		if (Random.Range (0, 2) == 0) {
			if(biom == 1) {
				// change water material to ice
				water.GetComponent<Renderer>().material = ice;
				// add collider to water
				water.AddComponent<MeshCollider>();
			}else{
				// change water color to blueWater
				water.GetComponent<Renderer>().material = blueWater;
				// remove collider to water
				Destroy(water.GetComponent<MeshCollider>());
			}
			water.SetActive(true);
			water.transform.position = new Vector3(water.transform.position.x, Random.Range(2.1f, 11), water.transform.position.z);
		}else{
			water.SetActive(false);
		}
		transform.GetComponent<TreeGenerator>().Generate();
	}
}