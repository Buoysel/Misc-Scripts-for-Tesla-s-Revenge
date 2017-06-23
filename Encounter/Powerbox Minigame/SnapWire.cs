using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnapWire : MonoBehaviour {

	private WireConnectorList wiresList;		//Script for the list of wires/connectors on each space
	private SetPicker setPicker;				//Script for the set picke
	private WireSpaceList spaceList;			//Script for the list of spaces
	private Draggable dragScript;				//Script that enables objects to be dragged
	private PowerboxPlayerHealth playerHealth;			//Script for the player health.
	private Collider2D otherCollider;			//The other gameobjet's collider	
	private SpawnableObjectsList spawnList;     //The list of Spawnable objects

	private bool correctWord = false;			//Check to see if the player has the correct name.
	public bool canSnap = false;
	public GameObject otherWire;

	private AudioSource audioPlayer;			//The SoundManager's AudioSource Component.
	private Sounds soundEffect;					//Script for the SoundManager's sounds.


	void Start()
	{
		wiresList = this.gameObject.GetComponent<WireConnectorList> ();
        setPicker = PowerboxGameManager.instance.GetComponent<SetPicker>();
		spaceList = setPicker.currentSet.GetComponent<WireSpaceList>();
		playerHealth = PowerboxGameManager.instance.GetComponent<PowerboxPlayerHealth>(); //GameObject.FindWithTag ("GameController").GetComponent<PowerboxPlayerHealth> ();

		audioPlayer = GameObject.Find ("SoundManager").GetComponent<AudioSource> ();
		soundEffect = GameObject.Find ("SoundManager").GetComponent<Sounds> ();

		/*SpawnList is called here to enable the creation of more wires.*/
		spawnList = PowerboxGameManager.instance.GetComponent<SpawnableObjectsList>();
	}

	void Update()
	{
		//When all the wires have been matched into a space, the wire space deletes itself.
		if (wiresList.wcList.Count == 0) 
		{
			for (int i = 0; i < spaceList.wireSpaceList.Count; i++) 
			{
				if (spaceList.wireSpaceList [i].name == this.gameObject.name) 
				{
					spaceList.wireSpaceList.Remove (spaceList.wireSpaceList [i]);
				}
			}

			Destroy (this.gameObject);
		}
	}

	void FixedUpdate()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			if (canSnap)
			{
				string combinedName = otherWire.name;		//Get the name of the other object
				string[] splitName = combinedName.Split('_');		//Split the shape and keyword based on the _ character
				switch (splitName[0])								//Apend _ to the shape
				{
				case "L":
					splitName[0] = splitName[0] + "_";
					break;
				case "Loop":
					splitName[0] = splitName[0] + "_";
					break;
				case "Stair":
					splitName[0] = splitName[0] + "_";
					break;
				case "W":
					splitName[0] = splitName[0] + "_";
					break;
				}						
				
				//search through the list
				for (int i = 0; i < wiresList.wcList.Count; i++) 
				{
					if (otherWire.tag == "Connector")
					{
						//Matching the connectors
						if (splitName[0] == wiresList.wcList[i].name)
						{
							correctWord = true;
							
							Snap (i, otherWire.GetComponent<Collider2D>());
							
							DeleteEntry (i);
							break;				//So it doesn't delete any additional connectors with the same name
						}
					}
					else
					{
						//Matching the wires
						//Check the word
						if (wiresList.wcList [i].name.Contains(splitName[1])) 
						{
							correctWord = true;
							
							//Check the shapes
							if (wiresList.wcList [i].name.StartsWith (splitName[0])) 
							{
								
								Snap (i, otherWire.GetComponent<Collider2D>());
								
								//Delete any other words that start with the same shape.
								for (int j = 0; j < wiresList.wcList.Count; j++) 
								{
									if (wiresList.wcList [j].name.StartsWith (splitName[0])) 
									{
										DeleteEntry (j);
										j = 0;
									}
								}
								
								//Delete any similar words with different shapes.
								for (int k = 0; k < wiresList.wcList.Count; k++) 
								{
									if (wiresList.wcList [k].name.EndsWith (splitName[1])) 
									{
										DeleteEntry (k);
										k = 0;
									}
								}
								
							}
						}
					}
				}
				
				if (correctWord == false) 
				{
					playerHealth.TakeDamage ();
					Destroy (otherWire.gameObject);
					spawnList.canSpawn = true;
				}
				else
				{
					correctWord = false;
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		canSnap = true;

		//Get the other object draggable script
		dragScript = other.gameObject.GetComponent<Draggable> ();
		otherCollider = other.gameObject.GetComponent<Collider2D> ();

		otherWire = other.gameObject;
	}

	void OnTriggerExit2D(Collider2D other)
	{
		canSnap = false;
	}

	/*
	void OnTriggerStay2D(Collider2D other)
	{
		//Only activate if the player clicks with the mouse.
		if (Input.GetButtonDown ("Fire1")) 
		{
			string combinedName = other.gameObject.name;		//Get the name of the other object
			string[] splitName = combinedName.Split('_');		//Split the shape and keyword based on the _ character
			switch (splitName[0])								//Apend _ to the shape
			{
				case "L":
					splitName[0] = splitName[0] + "_";
					break;
				case "Loop":
					splitName[0] = splitName[0] + "_";
					break;
				case "Stair":
					splitName[0] = splitName[0] + "_";
					break;
				case "W":
					splitName[0] = splitName[0] + "_";
					break;
			}						

			//search through the list
			for (int i = 0; i < wiresList.wcList.Count; i++) 
			{
				if (other.gameObject.tag == "Connector")
				{
					//Matching the connectors
					if (splitName[0] == wiresList.wcList[i].name)
					{
						correctWord = true;
						
						Snap (i, other);
						
						DeleteEntry (i);
						break;				//So it doesn't delete any additional connectors with the same name
					}
				}
				else
				{
					//Matching the wires
					//Check the word
					if (wiresList.wcList [i].name.Contains(splitName[1])) 
					{
						correctWord = true;
						
						//Check the shapes
						if (wiresList.wcList [i].name.StartsWith (splitName[0])) 
						{
							
							Snap (i, other);
							
							//Delete any other words that start with the same shape.
							for (int j = 0; j < wiresList.wcList.Count; j++) 
							{
								if (wiresList.wcList [j].name.StartsWith (splitName[0])) 
								{
									DeleteEntry (j);
									j = 0;
								}
							}
							
							//Delete any similar words with different shapes.
							for (int k = 0; k < wiresList.wcList.Count; k++) 
							{
								if (wiresList.wcList [k].name.EndsWith (splitName[1])) 
								{
									DeleteEntry (k);
									k = 0;
								}
							}
							
						}
					}
				}
			}

			if (correctWord == false) 
			{
				playerHealth.TakeDamage ();
				Destroy (other.gameObject);
				spawnList.canSpawn = true;
			}
			else
			{
				correctWord = false;
			}
		}
	}
*/

	void DeleteEntry(int i)
	{
		//Removes the involved word from the list.

		wiresList.wcList.Remove (wiresList.wcList [i]);
		wiresList.transformList.Remove (wiresList.transformList [i]);
	}

	void Snap(int i, Collider2D other)
	{
		//Snap wires to their correct positions.

		//Remove the collider from the other gameobject
		otherCollider.enabled = false;
		//snap the other game object to the correct position
		other.gameObject.transform.position = wiresList.transformList [i].transform.position;
		//remove the drag script from the other object.
		dragScript.enabled = false;

		audioPlayer.clip = soundEffect.snapClick;
		audioPlayer.Play ();

		spawnList.canSpawn = true;
		canSnap = false;
	}
}