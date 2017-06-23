using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnableObjectsList : MonoBehaviour {

	//Holds a list of the spawnable objects
	//This should be attcahed to the GameManager.

	public List<GameObject>		lShapes;
	public List<GameObject>		loopShapes;
	public List<GameObject>		stairShapes;
	public List<GameObject>		wShapes;
	public List<GameObject>		connectors;

	public Vector3 mousePos3D;
	public GameObject wordToInstantiate;

	public string wordName = "";
	public string shapeName = "";
	public string objectName = "";		//Take the name of the current gameobject.

	public bool canSpawn;

	/*void Awake()
	{
		lShapes = new List<GameObject> ();
		loopShapes = new List<GameObject> ();
		stairShapes = new List<GameObject> ();
		wShapes = new List<GameObject> ();
		connectors = new List<GameObject> ();
	}
	*/

	void Start()
	{
		canSpawn = true;
	}

	void Update()
	{
		//get 2D mouse position
		Vector3 mousePos2D = Input.mousePosition;

		//convert mouse position to 3D world coords
		mousePos2D.z = -Camera.main.transform.position.z;
		mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
	}

	public void SpawnWord()
	{
		//Spawn a wire or connector where the player has their mouse.
		if (canSpawn) {
			//Search the L list
			for (int i = 0; i < lShapes.Count; i++)
			{
				if (lShapes [i].name == (shapeName + wordName)) 
				{
					wordToInstantiate = lShapes [i];
					canSpawn = false;
					break;
				}
			}

			//Search the Loop list
			for (int i = 0; i < loopShapes.Count; i++) 
			{
				if (loopShapes [i].name == (shapeName + wordName)) 
				{
					wordToInstantiate = loopShapes [i];
					canSpawn = false;
					break;
				}
			}

			//Search the Stair list
			for (int i = 0; i < stairShapes.Count; i++) 
			{
				if (stairShapes [i].name == (shapeName + wordName))
				{
					wordToInstantiate = stairShapes [i];
					canSpawn = false;
					break;
				}
			}

			//Search the W list

			for (int i = 0; i < wShapes.Count; i++) 
			{
				if (wShapes [i].name == (shapeName + wordName))
				{
					wordToInstantiate = wShapes [i];
					canSpawn = false;
					break;
				}
			}

			//Search the connector list
			for (int i = 0; i < connectors.Count; i++) 
			{
				if (connectors [i].name == wordName) 
				{
					wordToInstantiate = connectors [i];
					canSpawn = false;
					break;
				}
			}

			if (wordToInstantiate != null)
			{
				//Spawn the object when found.
				GameObject go = Instantiate (wordToInstantiate, mousePos3D, transform.rotation) as GameObject;
				go.name = wordToInstantiate.name;
			}

			wordName = "";
			shapeName = "";
			wordToInstantiate = null;
		}
	}
}