using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetPicker : MonoBehaviour {

	//Picks a random set to challenge the player. The scripts for winning the game lead from the
	//currentSet shown in the inspector.

	public List<GameObject>		setList;			//Hold all sets
	public GameObject			currentSet;			//current picked set
	public int 					randomSet;			//randomly chosen set

	// Use this for initialization
	void Awake () 
	{
		randomSet = Random.Range (0, 3);

		currentSet = setList [randomSet];

		currentSet.SetActive (true);
	}
}
