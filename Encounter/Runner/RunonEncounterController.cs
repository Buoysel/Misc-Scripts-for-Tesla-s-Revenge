using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
//using UnityEditor;

public class RunonEncounterController : MonoBehaviour {

	public int randCat;							//Randomly select a category.
	public int randSen;							//Randomly select a sentence WITHIN the category.
	public string currentSentence;				//Selected sentence for reseting.
	public int currentPosition = 0;				//Position of the loop
	public int lastPosition;		

	private Text sentence;						//UI Sentence panel
	private Text hint;							//UI Hint Panel
	private RunonSentencesScript sentenceScript; //Script for the sentences.
	private Health healthScript;				 //Script for the player and Runner's health.
	public Animator anim;						 //Runner's animator. Health Script access this, too.
    public Sprite teslaPortrait;                 //for use in the final battle version

	public List<string> usedRunon = new List<string> ();	//Hold any used runons.

	public bool allowInput = true;

	public GameObject helpPanel;						//Help booklet.
										
	void Start () 
	{
		//Get any scripts and components.
		sentence = GameObject.Find ("Sentence").GetComponent<Text>();
		hint = GameObject.Find ("HintText").GetComponent<Text> ();
		sentenceScript = GetComponent<RunonSentencesScript>();
		healthScript = GetComponent<Health> ();

		//Get the Runner's animator.
		anim = GameObject.Find("Runner").GetComponent<Animator>();
        anim.GetComponent<SpriteRenderer>().sprite = teslaPortrait;

		hint.text = "";

		DisplaySentence ();
	}

	void Update () 
	{
		if (allowInput && helpPanel.GetComponent<Help>().isOpen == false)
		{
			if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.LeftArrow)) 
				Retreat ();

			if (Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.RightArrow)) 
				Advance ();	
		}
	}

	void DisplaySentence()
	{
		//Display a random run-on.
		currentPosition = 0;

		randCat = Random.Range (0, sentenceScript.runonArray.Length);
		randSen = Random.Range (0, sentenceScript.runonArray [randCat].Length);

		//Drop a run-on that has been used.
		if (!usedRunon.Contains (sentenceScript.runonArray [randCat] [randSen])) 
		{
			usedRunon.Add (sentenceScript.runonArray [randCat] [randSen]);

			sentence.text = sentenceScript.runonArray [randCat] [randSen];
			currentSentence = sentenceScript.runonArray [randCat] [randSen];
		} 
		else
			DisplaySentence ();

//		Debug.Log ("runonArray[" + randCat + "][" + randSen + "]");
	}

	void Advance()
	{
		RemoveStyling ();
		lastPosition = currentPosition;
//		Debug.Log ("Last position is " + lastPosition);

		try
		{
			//Find the next blank space.
			for (int i = currentPosition; i <= sentence.text.Length; i++) 
			{
				//Make sure the user actually advances, and not stick to the same
				//blank space.
				if (sentence.text [i] == ' ' && i != currentPosition) 
				{
					currentPosition = i;
					ApplyStyling (currentPosition);
//					Debug.Log ("Current position is " + currentPosition);
					break;
				}
			}
		}
		catch 
		{
			ApplyStyling (lastPosition);
		}
	}

	void Retreat()
	{
		RemoveStyling ();
		lastPosition = currentPosition;
//		Debug.Log ("Last position is " + lastPosition);

		try
		{
			//Find the next blank space.
			for (int i = currentPosition; i <= sentence.text.Length || i >= 0 ; i--) 
			{
				//Make sure the user actually advances, and not stick to the same
				//blank space.
				if (sentence.text [i] == ' ' && i != currentPosition) 
				{
					currentPosition = i;
					ApplyStyling (currentPosition);
//					Debug.Log ("Current position is " + currentPosition);
					break;
				}
			}
		}
		catch 
		{
			ApplyStyling (lastPosition);
		}
	}

	public void Insert(string word)
	{
		//Insert the word button's name at the same position as the empty space.
		word = word.ToLower();

		switch (word) 
		{
			case "period":
				word = ".";
				InsertPunctuation (word);
				Capitalize();
				break;
			case "semicolon":
				word = ";";
				InsertPunctuation (word);
				break;
			case "comma":
				word = ",";
				InsertPunctuation (word);
				break;
			default:
				sentence.text = sentence.text.Insert (currentPosition, " " + word);
				currentPosition = currentPosition + (word.Length + 1);
				break;
		}

		//Offset the '_' position.
		RemoveStyling ();
		ApplyStyling (currentPosition);
	}

	void Capitalize()
	{
		//After Inserting a period, capitalize the next word in the sentence.
		string capitalized = sentence.text [currentPosition + 27].ToString ();
		sentence.text = sentence.text.Remove (currentPosition, 27);
		sentence.text = sentence.text.Insert (currentPosition + 1, capitalized.ToUpper());
		//Debug.Log (capitalized);
	}

	void InsertPunctuation(string punctuation)
	{
		sentence.text = sentence.text.Insert (currentPosition, punctuation);
		currentPosition = currentPosition + punctuation.Length;
	}

	void DeletePunctuation()
	{
		//This is just incase a period is deleted, and the next word needs to be capitalized.

		sentence.text = sentence.text.Remove (currentPosition - 1, 1);
		currentPosition -= 1;

		//Fix any capitalizations made by a period.
		string UNcapitalized = sentence.text [currentPosition + 27].ToString ();
		sentence.text = sentence.text.Remove (currentPosition + 27, 1);
		sentence.text = sentence.text.Insert (currentPosition + 27, UNcapitalized.ToLower ());
	}

	public void Backspace()
	{
		//Checks the previous 3 characters to see if they contain "and" "," or a "."

		int loop = 3;		//Default loop counter
		string sub = "";	//Creates a substring to search for.

		/*
			While the loop counter is greater than 0, add 3 characters backwards from the cursor's current
			position and add them into the sub string.
		*/

		try
		{
			while (loop > 0) 
			{
				sub += sentence.text [currentPosition - loop];

				loop -= 1;
			}
		}
		catch 
		{
			DeletePunctuation ();
		}
			
		if (sub.Contains (".") || sub.Contains (","))
		{
			DeletePunctuation ();
		} 
		else if (sub.Contains ("and")) 
		{
			sentence.text = sentence.text.Remove (currentPosition - 4, 4);
			currentPosition -= 4;
		}
	}
		
	public void Reset()
	{
		//Remove all changes to the sentence.
		sentence.text = currentSentence;
		currentPosition = 0;
	}

	public void Submit()
	{
		//Check to see if the user's response is correct.
		RemoveStyling ();
		string modifiedSentence = sentence.text.ToUpper ();
		bool correct = false;

		print (modifiedSentence);
		print (sentenceScript.correctArray [randCat] [randSen].ToUpper ());

		if (modifiedSentence == sentenceScript.correctArray [randCat] [randSen].ToUpper ()) 
		{
            //play the animations if they are not fighting tesla
//            if(GameController.Instance.finalBattle.Equals(false))
			    anim.SetTrigger ("PlayerIsCorrect");
			healthScript.DamageEnemy ();
			correct = true;

			StartCoroutine (NewRound (correct));
		} 
		else 
		{
//            if(GameController.Instance.finalBattle.Equals(false))
			    anim.SetTrigger ("PlayerIsWrong");
			healthScript.DamagePlayer ();
			StartCoroutine (NewRound (correct));
		}
	}

	IEnumerator NewRound(bool correct)
	{
		allowInput = false;
		yield return new WaitForSeconds (3);
		allowInput = true;

		//Search for the inactive pHP objects. Add one for every object disabled.
		//If 3 pHP objects are disabled, this ends the game.
		int hpCounter = 0;
		for (int i = 0; i < 3; i++) 
		{
			if (healthScript.pHealthList [i].activeInHierarchy == false)
				hpCounter += 1;
		}
		if (healthScript.eHealthList.Count == 0 || hpCounter == 3) 
		{
			//End the game.
		//	EditorApplication.isPlaying = false;
		}


		healthScript.message.text = "";
		hint.text = "";

		if (correct)
		{
			DisplaySentence ();

			//Restore the player HP
			for (int i = 0; i < 2; i++)
			{
				if (healthScript.pHealthList [i].activeInHierarchy == false)
					healthScript.pHealthList [i].SetActive(true);
			}
			healthScript.wrong = 1;
		} 
		else if (hpCounter == 2) 
		{
			hint.text = sentenceScript.hintArray [randCat];
		}
	}

	void ApplyStyling(int position)
	{
		//Create a double underscore and highlight the current cursor's position.
		sentence.text = sentence.text.Remove (position, 1);
		sentence.text = sentence.text.Insert (position, "<color=red>");
		sentence.text = sentence.text.Insert (position + 11, "<b>");
		sentence.text = sentence.text.Insert (position + 14, "</b>");
		sentence.text = sentence.text.Insert (position + 18, "</color>");
		sentence.text = sentence.text.Insert (position + 14, "_");
	}

	void RemoveStyling()
	{
		//Remove the underscore and highlight.
		if (sentence.text.Contains ("_")) 
		{
			sentence.text = sentence.text.Replace ("_", " ");
		}

		if(sentence.text.Contains("<b>"))
		{
			sentence.text = sentence.text.Replace ("<b>", "");
		}

		if(sentence.text.Contains("</b>"))
		{
			sentence.text = sentence.text.Replace ("</b>", "");
		}

		if(sentence.text.Contains("<color=red>"))
		{
			sentence.text = sentence.text.Replace ("<color=red>", "");
		}

		if(sentence.text.Contains("</color>"))
		{
			sentence.text = sentence.text.Replace ("</color>", "");
		}
	}
}