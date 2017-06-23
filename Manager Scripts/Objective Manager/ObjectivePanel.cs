using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ObjectivePanel : MonoBehaviour {

	//This is the objective text that shows up in the Inventory.
	//This is managed by the Objective Manager on the Game Controller.

	private static ObjectivePanel instance;

	public static ObjectivePanel Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType<ObjectivePanel> ();
			}

			return instance;
		}
	}

	public List<GameObject> floorChildren;	//Hold each floor in the objective panel.


	void Start()
	{
		//Reinitialize the lists when returning to the main scene.

		foreach (Transform floor in ObjectivePanel.Instance.transform) 
		{
			//Get all the Floors under the Objective Panel and add the ObjectiveFloorChildren Script.
			floorChildren.Add (floor.gameObject);
			floor.gameObject.AddComponent <ObjectiveFloorChildren>();

			//Then, cycle through the current Floor's Objectives (its children), and add each child to the Floor's list
			//of Objecives.
			foreach (Transform objective in floor.transform) 
			{
				floor.gameObject.GetComponent<ObjectiveFloorChildren> ().objectiveChildren.Add (objective.gameObject.GetComponent<Text>());
			}
		}

		//Then update the Objectives Panel
		ObjectiveManager.Instance.UpdateObjectives ();
		//Debug.Log ("Objective Update Complete");
	}
}
