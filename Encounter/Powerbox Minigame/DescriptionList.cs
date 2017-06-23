using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DescriptionList: MonoBehaviour {

	//This will hold all the search descriptions.
	//This will also be attached to the GameManager.
	//The Wire spaces should reference this when they change the text in the blue panel.


	[HideInInspector]public GameObject			description;			//References the Search Description GameObject
	[HideInInspector]public Text				descriptionText;						//Text Mesh Component of Search Description
	public List<string>	searchDescription = new List<string> ();		//Holds a list of the search descriptions.

	// Use this for initialization
	void Awake () 
	{
		description = GameObject.Find ("Search Description");

		descriptionText = description.GetComponent<Text> ();

		//Women AND Television
		searchDescription.Add("You are looking for information on " +
		                      "\nwomen in the television industry." +
		                      "\nWhich search will give you the best" +
		                      "\nresults?");

		//Video games AND Children
		searchDescription.Add("While searching for the allure of " +
		                      "\nvideo games for children, which " +
		                      "\nsearch would give you the most " +
		                      "\nprecise results?");

		//Dogs AND Information
		searchDescription.Add ("You are looking for information " +
		                       "\nabout dogs. Which search would be " +
		                       "\nthe best to find the information " +
		                       "\nthat you need?");

		//Persepolis AND Novel NOT Movie
		searchDescription.Add ("You are trying to find information " +
		                       "\non Marjane Satrapi's 'Persepolis,'" +
		                       "\nbut you want to find information on "+
		                       "\nthe graphic novel only, not the " +
		                       "\nmotion picture.");

		//Lightning Strikes AND Arizona AND Survial
		searchDescription.Add ("You are asked to research the " +
		                       "\nsurvival rates of individuals " +
		                       "\nstruck by lightning in Arizona.");

		//Zombies AND Video Games OR Movie
		searchDescription.Add ("You are writing a paper about " +
		                       "\nzombies in popular culture. You " +
		                       "\ndecide to focus on related video " +
		                       "\ngame series, but your analysis also " +
		                       "\nincludes discussions of film " +
		                       "\nadaptations.");

		//John Quincy Adams AND Letters NOT Wife
		searchDescription.Add ("You are conducting research on a " +
		                       "\nproject that focuses on letters " +
		                       "\nwritten only by John Quincy Adams " +
		                       "\nand not his wife.");

		//Samuel T. Coleridge AND Rime of the Ancient Mariner AND Biographical
		searchDescription.Add ("You are writing a paper on Samuel " +
		                       "\nT. Coleridge's the 'Rime of the " +
		                       "\nAncient Mariner.' At the moment, " +
		                       "\nyou only wish to find biographical " +
		                       "\nsources.");

		//Motorcycle NOT Harley
		searchDescription.Add ("Your final project is on all " +
		                       "\nmotorcycles except Harley Davison's. " +
		                       "\nHow would you term your search?");
	}
}