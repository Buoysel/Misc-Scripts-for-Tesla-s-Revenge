using UnityEngine;
using System.Collections;

public class SelectWord : MonoBehaviour {

	//Assigns the spawnlist
	private SpawnableObjectsList spawnList;

	void Start () {
        //Directly assigns spawnList with the Spawn List script from the Game Manager

        spawnList = PowerboxGameManager.instance.gameObject.GetComponent<SpawnableObjectsList>();//GameObject.FindWithTag ("GameController").GetComponent<SpawnableObjectsList>();
	}


	public void SendWord()
	{
		//Sends the name of the word gameobject to the spawn list
		spawnList.wordName = this.gameObject.name;
	}

	public void SendShape()
	{
		//Sends the name of the shape gameobject to the spawn list.
		spawnList.shapeName = this.gameObject.name;
	}
}