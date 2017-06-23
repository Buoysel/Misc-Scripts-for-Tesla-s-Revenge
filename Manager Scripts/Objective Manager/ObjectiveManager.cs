using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour {

	//This script determines what text is shown on the Objective Panel whenever the player
	//views the inventory.

	private static ObjectiveManager instance;

	public static ObjectiveManager Instance
	{
		get 
		{
			if (instance == null) 
			{
				instance = GameObject.FindObjectOfType<ObjectiveManager> ();
			}

			return instance;
		}
	}

	//Save any objectives added.
	private static string[][] objectiveTextArray;
	private bool setupDone = false;

	public void SetText(string details)
	{
        return;

		//Split details
		string[] splitParameters = details.Split('/');
		int floorIndex = Int32.Parse(splitParameters [0]);
		int objectiveIndex = Int32.Parse(splitParameters [1]);
		string content = splitParameters [2];

		//Savs the text to the text array
		objectiveTextArray [floorIndex] [objectiveIndex] = content;

		//Set in-game text to the text array values
		if (objectiveIndex == 0) {
			ObjectivePanel.Instance.floorChildren [floorIndex].GetComponent<Text> ().text = objectiveTextArray [floorIndex] [0];
		} 
		else
		{
			ObjectivePanel.Instance.floorChildren [floorIndex].GetComponent<ObjectiveFloorChildren> ()
				.objectiveChildren [objectiveIndex - 1].text = objectiveTextArray [floorIndex] [objectiveIndex];
		}
	}

	public void CompleteObjective(string details)
	{
        return;

		//Make the objective or floor's text green.
		string[] splitParamaters = details.Split ('/');
		int floorIndex = Int32.Parse (splitParamaters [0]);
		int objectiveIndex = Int32.Parse (splitParamaters [1]);

		objectiveTextArray [floorIndex] [objectiveIndex] = "<color=#00ff00ff>" + objectiveTextArray [floorIndex] [objectiveIndex] + "</color>";

		if (objectiveIndex == 0) {
			ObjectivePanel.Instance.floorChildren [floorIndex].GetComponent<Text> ().text = objectiveTextArray [floorIndex] [0];
		} 
		else
		{
			ObjectivePanel.Instance.floorChildren [floorIndex].GetComponent<ObjectiveFloorChildren> ()
			.objectiveChildren [objectiveIndex - 1].text = objectiveTextArray [floorIndex] [objectiveIndex];
		}
	}

	public void UpdateObjectives()
	{
        return;

		if(!setupDone)
			FirstSetup ();

		//Reset the objective text when leaving an encounter.
		for (int j = 0; j < ObjectivePanel.Instance.floorChildren.Count; j++)
		{
			ObjectivePanel.Instance.floorChildren [j].GetComponent<Text> ().text = objectiveTextArray [j] [0];
			for (int l = 0; l < ObjectivePanel.Instance.floorChildren [j].GetComponent<ObjectiveFloorChildren> ().objectiveChildren.Count; l++) 
			{
				ObjectivePanel.Instance.floorChildren [j].GetComponent<ObjectiveFloorChildren> ().objectiveChildren [l].text = objectiveTextArray [j] [l+1];
			}
		}
	}
		

	void FirstSetup()
	{
        return;

		//Set the first dimension of the objectiveTextArray to the same length as the umber of floor children
		objectiveTextArray = new string[ObjectivePanel.Instance.floorChildren.Count][];

		//Iterate through the floorChildren
		for (int i = 0; i < objectiveTextArray.Length; i++) 
		{
			//Set the second dimension of the objectiveTextArray to the current floorChild's objectiveChildren Length.
			objectiveTextArray [i] = new string[ObjectivePanel.Instance.floorChildren[i].gameObject.GetComponent<ObjectiveFloorChildren> ().objectiveChildren.Count + 1];

			//Iterate through the floorChild's objectiveChildren.
			for (int k = 0; k < objectiveTextArray[i].Length; k++) 
			{
				//Setup the objectiveTextArray. This will finally store the text to be called when the 
				//Objective Panel has to be used again
				objectiveTextArray [i] [k] = "??????????";
			}
		}

		setupDone = true;
	}
}
