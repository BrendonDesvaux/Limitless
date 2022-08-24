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
		float waterLevel = Random.Range(2.1f, 11);

		transform.GetComponent<GrassGenerator>().MinLevel = 0;
		transform.GetComponent<TreeGenerator>().MinLevel = 0;
		if (Random.Range (0, 2) == 0) {
			transform.GetComponent<GrassGenerator>().MinLevel = (waterLevel - 2.4f)*3+1;
			transform.GetComponent<TreeGenerator>().MinLevel = (waterLevel - 2.4f)*3+1;
			if(biom == 1) {
				// change water tag to 'ground'
				water.tag = "ground";
				// change water material to ice
				water.GetComponent<Renderer>().material = ice;
				// add collider to water
				water.AddComponent<MeshCollider>();
			}else{
				// change water tag to 'untagged'
				water.tag = "untagged";
				// change water color to blueWater
				water.GetComponent<Renderer>().material = blueWater;
				// remove collider to water
				Destroy(water.GetComponent<MeshCollider>());
			}
			water.SetActive(true);
			water.transform.position = new Vector3(water.transform.position.x, waterLevel, water.transform.position.z);
		}else{
			water.SetActive(false);
		}
		
		if(biom == 0) {
			transform.GetComponent<GrassGenerator>().Generate();
		}
		transform.GetComponent<TreeGenerator>().Generate();
	}
}