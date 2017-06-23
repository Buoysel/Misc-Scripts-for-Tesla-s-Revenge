using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WireConnectorList : MonoBehaviour
{
	//This script will contain a list of the wires and connectors that will go into either silhouette.
	//Every silhouette will have this as a component.

	public List<GameObject>			wcList;
	public List<Transform>			transformList; 	//Holds a list of positions for each object

	// Use this for initialization
	/*void Awake()
	{
		wcList = new List<GameObject>();

		transformList = new List<Transform> ();
	}
	*/
}