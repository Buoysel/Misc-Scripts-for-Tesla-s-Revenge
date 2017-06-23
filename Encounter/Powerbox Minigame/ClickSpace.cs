using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickSpace : MonoBehaviour {

	//Attach to the wireSpaces, and set as the "OnClick()" function.

	private GameObject 				wireSpace; 	//Holds the name of the wire space it's attached to.
	private WireSpaceList			spaceList;	//List of wire spaces
	private SetPicker				setList;
	private DescriptionList			searchList;	//Calls the SearchDescription script.
	private Sprite					defaultSprite; //The default sprite image.
	private Sprite					pressedSprite; //The pressed sprite image.
	private bool					isActive = false; //This is the wirespace the player is currently working on.

	private AudioSource audioPlayer;			//The SoundManager's AudioSource Component.
	private Sounds soundEffect;					//Script for the SoundManager's sounds.

	// Use this for initialization
	void Start () 
	{
        //Gets the GameManager
		GameObject gameManager = GameObject.Find("PowerboxGameManager");
		//Get the SearchDescription script WITHIN the GameManager.
		searchList = gameManager.GetComponent<DescriptionList> ();
		//Get the Set Picker Script
		setList = gameManager.GetComponent<SetPicker>();
		//Get the WireSpaceList Script
		spaceList = setList.currentSet.GetComponent<WireSpaceList> ();

		//Sets the name of the wirespace.
		wireSpace = this.gameObject;
		defaultSprite = wireSpace.GetComponent<Image> ().sprite;
		pressedSprite = wireSpace.GetComponent<Button> ().spriteState.pressedSprite;

		//Sets the sounds
		audioPlayer = GameObject.Find ("SoundManager").GetComponent<AudioSource> ();
		soundEffect = GameObject.Find ("SoundManager").GetComponent<Sounds> ();
	}

	void FixedUpdate()
	{

		if (spaceList.selectedSpace == wireSpace.name)
		{
			GetComponent<Image> ().sprite = pressedSprite;
		} 
		else 
		{
			GetComponent<Image> ().sprite = defaultSprite;
			isActive = false;
			wireSpace.GetComponent<Button> ().enabled = true;
		}


	}

	//This function is called on OnClick()
	public void ChangeDescription()
	{
		//This switch statement will check the name of the WireSpace the ClickSpace script is currently attached to.
		//The text in the search description will change to reflect the wire space it is attached to.
		spaceList.selectedSpace = wireSpace.name;

		switch (wireSpace.name) 
		{
		case("Women AND Television Space"):
			searchList.descriptionText.text = searchList.searchDescription [0];
			break;
		case("Video Games AND Children Space"):
			searchList.descriptionText.text = searchList.searchDescription [1];
			break;
		case("Dogs AND Information"):
			searchList.descriptionText.text = searchList.searchDescription [2];
			break;
		case("Persepolis AND Novel NOT Movie Space"):
			searchList.descriptionText.text = searchList.searchDescription [3];
			break;
		case("Lightning Strikes AND Surival AND Arizona Space"):
			searchList.descriptionText.text = searchList.searchDescription [4];
			break;
		case("Zombies AND Video Games OR Movies"):
			searchList.descriptionText.text = searchList.searchDescription [5];
			break;
		case("JQA AND Letters NOT Wife"):
			searchList.descriptionText.text = searchList.searchDescription [6];
			break;
		case("STC AND Rime AND Biographical"):
			searchList.descriptionText.text = searchList.searchDescription [7];
			break;
		case("Motorcycle NOT Harley"):
			searchList.descriptionText.text = searchList.searchDescription [8];
			break;
		}
			
		if (isActive == false)
		{
			audioPlayer.clip = soundEffect.beep;
			audioPlayer.Play ();
			isActive = true;
			wireSpace.GetComponent<Button> ().enabled = false;
		}

	}
}