using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.MapGenerator.Generators;

public class LevelGeneration : MonoBehaviour {
	void Start() {
		GenerateMap ();
	}

	void GenerateMap() {
		int Biom = Random.Range (0, 0);
		transform.GetComponent<HeightsGenerator>().Offset = Random.Range (0, 10000000.0f);
		transform.GetComponent<HeightsGenerator>().Generate();
		transform.GetComponent<DungeonTexturesGenerator>().chosenBiom = Biom;
		transform.GetComponent<DungeonTexturesGenerator>().Generate();
	}
}