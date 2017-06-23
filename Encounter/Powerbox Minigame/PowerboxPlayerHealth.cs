using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PowerboxPlayerHealth : MonoBehaviour {

	//Gives the player health, and ends the game when the player runs out of health.

	public List<GameObject>		healthList;		//A list of health orbs
	private GameObject 			healthOrb;		//The current health orbs
	private Text				descriptionText;				//The text mesh.

	private GameObject wireSpaces;

	private AudioSource audioPlayer;			//The SoundManager's AudioSource Component.
	private Sounds soundEffect;					//Script for the SoundManager's sounds.

	void Start()
	{
		descriptionText = GameObject.Find ("Search Description").GetComponent<Text> ();

		wireSpaces = GetComponent<SetPicker> ().currentSet;

		audioPlayer = GameObject.Find ("SoundManager").GetComponent<AudioSource> ();
		soundEffect = GameObject.Find ("SoundManager").GetComponent<Sounds> ();
	}

	public void TakeDamage()
	{
		Debug.Log ("The Player has lost health");

		//The current health orb is the first element
		healthOrb = healthList [0].gameObject;

		Debug.Log (healthOrb.name);

		//remove the first element
		healthList.Remove (healthList [0]);

		//destroy the current health orb
		Destroy (healthOrb);

		audioPlayer.clip = soundEffect.shock;
		audioPlayer.Play ();

		//If the list is empty, GameOver.
		if (healthList.Count == 0)
		{
			wireSpaces.SetActive(false);

            descriptionText.text = "Game Over";
//			descriptionText.fontSize = 500;
//			descriptionText.alignment = TextAlignment.Center;

			audioPlayer.clip = soundEffect.lose;
			audioPlayer.Play ();

            if (GameController.Instance != null)
                GameController.Instance.encounterWon = false;

    }
	}
}
