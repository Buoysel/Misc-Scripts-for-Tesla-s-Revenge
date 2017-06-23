using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {

	//Script for button functionality

	private RunonEncounterController mainScript; //Reference the main script.

	void Start()
	{
		mainScript = GameObject.Find ("MinigameController").GetComponent<RunonEncounterController> ();
	}

	public void InsertWord()
	{
		if (mainScript.allowInput && mainScript.currentPosition > 0)
			mainScript.Insert (this.gameObject.name);
	}

	public void Backspace()
	{
		if (mainScript.allowInput && mainScript.currentPosition > 0)
			mainScript.Backspace ();
	}

	public void ResetSentence()
	{
		if (mainScript.allowInput)
			mainScript.Reset ();
	}

	public void SubmitAnswer()
	{
		if (mainScript.allowInput)
			mainScript.Submit ();
	}
}
