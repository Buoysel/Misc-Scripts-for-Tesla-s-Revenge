using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class ObjectiveFloorChildren : MonoBehaviour {

	//Hold's each floor's list of objectives. A list will be called from the ObjectivePanel script to add
	//each floor's child into a list.

	public List<Text> objectiveChildren = new List<Text>();
}
