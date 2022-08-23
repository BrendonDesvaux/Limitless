using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.MapGenerator.Generators;

public class LevelGeneration : MonoBehaviour {
	void Start() {
		GenerateMap ();
	}

	void GenerateMap() {
		int biom = Random.Range (0, 3);
		transform.GetComponent<HeightsGenerator>().Offset = Random.Range (0, 10000000.0f);
		transform.GetComponent<HeightsGenerator>().Generate();
		transform.GetComponent<DungeonTexturesGenerator>().chosenBiom = biom;
		transform.GetComponent<DungeonTexturesGenerator>().Generate();
		if(biom != 1) {
			transform.GetComponent<GrassGenerator>().Generate();
		}
		transform.GetComponent<TreeGenerator>().Generate();
	}
}