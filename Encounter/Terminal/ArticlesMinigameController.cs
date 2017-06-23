using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
//using UnityEditor;

public class ArticlesMinigameController : MonoBehaviour {

	public string currentArticle; 	//Holds an article from the articleList
	public string correctAnswer; 	//Randomly picks the correct answer
	public int answer;				//User picked answer

	public bool allowInput = false; //Prevent input before the timers end.

	public int randArt;				//Random article.
	public int randAns;			 	//Random answer.
	public static List<int> usedQuestion = new List<int>(); //Holds used questions

	//Canvas Parents
	public GameObject article;
	public GameObject helpDesk;

	//Text Gameobjects
	public GameObject articleText;
	public GameObject compareText;
	public GameObject answerText;
	public Text helpText;

	//List to hold information about a Paraphrase, Direct Quote, or Summary
	List<List<string>> PQSInfoList = new List<List<string>>();

	List<string> 		ParaInfo = new List<string>(),
						QuoInfo = new List<string>(),
						SummInfo = new List<string>();

	//List to hold all articles.
	List<List<List<string>>> articleList = new List<List<List<string>>>();

	//Article Number
	List<List<string>>	aOne = new List<List<string>>(),
						aTwo = new List<List<string>>(),
						aThree = new List<List<string>>(),
						aFour = new List<List<string>>()
		/*,
						aFive = new List<List<string>>(),
						aSix = new List<List<string>>(),
						aSeven = new List<List<string>>(),
						aEight = new List<List<string>>()
						*/;

	//Paraphrase, Direct Quote, and Summary
	List<string> 		aOneArticle = new List<string>(),
						aOnePara = new List<string>(),
						aOneQuo = new List<string>(),
						aOneSumm = new List<string>(),

						aTwoArticle = new List<string>(),
						aTwoPara = new List<string>(),
						aTwoQuo = new List<string>(),
						aTwoSumm = new List<string>(),
						
						aThreeArticle = new List<string>(),
						aThreePara = new List<string>(),
						aThreeQuo = new List<string>(),
						aThreeSumm = new List<string>(),

						aFourArticle = new List<string>(),
						aFourPara = new List<string>(),
						aFourQuo = new List<string>(),
						aFourSumm = new List<string>()
		/*,

						aFiveArticle = new List<string>(),
						aFivePara = new List<string>(),
						aFiveQuo = new List<string>(),
						aFiveSumm = new List<string>(),

						aSixArticle = new List<string>(),
						aSixPara = new List<string>(),
						aSixQuo = new List<string>(),
						aSixSumm = new List<string>(),

						aSevenArticle = new List<string>(),
						aSevenPara = new List<string>(),
						aSevenQuo = new List<string>(),
						aSevenSumm = new List<string>(),

						aEightArticle = new List<string>(),
						aEightPara = new List<string>(),
						aEightQuo = new List<string>(),
						aEightSumm = new List<string>()
						*/;

    private static System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

    void Awake()
	{
		article = GameObject.Find ("Article");
		helpDesk = GameObject.Find ("HelpDesk");
		helpDesk.SetActive (false);

		articleText = GameObject.Find ("ArticleText");
		compareText = GameObject.Find ("CompareText");
		answerText = GameObject.Find ("AnswerText");
	}

	void Start()
	{
        UserGameData.Instance.StartCoroutine(UserGameData.Instance.UpdatePlayerAttempt("terminal"));
        stopwatch.Start();

	    //Randomly pick an article
		randArt = Random.Range (0, 4);

		CreateArticles ();
		SetPQSInfo ();
		ShowArticle(randArt);
		StartCoroutine (ShowColumns ());
	}

	void Update()
	{
		//Get the user's answer
		if (allowInput == true)
		{
			if (Input.GetKeyDown (KeyCode.P))
			{
				if (helpDesk.activeInHierarchy)
					StartCoroutine (HelpInfo (0));

				if (article.activeInHierarchy)
					EvaluateAnswer (answer = 1);
			} 
			else if (Input.GetKeyDown (KeyCode.Q))
			{
				if (helpDesk.activeInHierarchy)
					StartCoroutine (HelpInfo (1));

				if (article.activeInHierarchy)
					EvaluateAnswer (answer = 2);
			} 
			else if (Input.GetKeyDown (KeyCode.S))
			{
				if (helpDesk.activeInHierarchy)
					StartCoroutine (HelpInfo (2));

				if (article.activeInHierarchy)
					EvaluateAnswer (answer = 3);
			} 
			else if (Input.GetKeyDown (KeyCode.Return)) 
			{
				if (helpDesk.activeInHierarchy)
				{
                    //Reset the game
                    if (SaveAndLoadLevel.Instance != null)
                        SaveAndLoadLevel.Instance.LoadMiniGame("Encounter-TerminalGame"); 
                    else
                        UnityEngine.SceneManagement.SceneManager.LoadScene("Encounter-TerminalGame");
                }
			}
		}
	}

	void ShowArticle(int randArt)
	{
		//Set the random Number
		randAns = Random.Range(1,4);

		//If all three questions have been used, clear usedQuesiton
		if (usedQuestion.Count >= 3) 
		{
			usedQuestion.Clear ();
		}

		//Check if a question has been used.
		if (!usedQuestion.Contains (randAns)) 
		{
			//Set the article and answer
			currentArticle = articleList [randArt] [0] [0];
			articleText.GetComponent<Text>().text = (articleText.GetComponent<Text>().text + "\n\n\n\n" + currentArticle);

			correctAnswer = articleList [randArt] [randAns] [0];
			compareText.GetComponent<Text>().text = (compareText.GetComponent<Text>().text + "\n\n\n" + correctAnswer);

			//Add randAns to used Questions
			usedQuestion.Add (randAns);
//			Debug.Log (usedQuestion.Count);
		} 
		else
		{
			ShowArticle (randArt); //If it has been used, run ShowArticle again.
		}
	}

	IEnumerator ShowColumns()
	{
		//Display the text columns with a delay.

		compareText.SetActive (false);
		answerText.SetActive (false);

		yield return new WaitForSeconds (3);
		compareText.SetActive (true);

		yield return new WaitForSeconds (3);
		answerText.SetActive (true);

		allowInput = true;
	}

	void EvaluateAnswer(int answer)
	{
		if (answer == randAns)
		{
			answerText.GetComponent<Text> ().text = (answerText.GetComponent<Text> ().text + "\n\n\nCorrect. You have been" +
			" authenticated as a student of information literacy. Room access granted.");

            //End the game
            //StartCoroutine (QuitGame ());
            if (GameController.Instance != null)
                GameController.Instance.encounterWon = true;
			allowInput = false;

            stopwatch.Stop();
            System.TimeSpan ts = stopwatch.Elapsed;
            if (UserGameData.Instance != null)
                UserGameData.Instance.StartCoroutine(UserGameData.Instance.UpdatePlayerRecords("terminal", ts));

            StartCoroutine("DelayBeforeLoad");
		}
		else 
		{
			answerText.GetComponent<Text>().text = (answerText.GetComponent<Text>().text + "\n\n\nI'm sorry, but that" +
				" answer is incorrect. You will now be connected with the Library HelpDesk. Please Wait...");
            UserGameData.Instance.StartCoroutine(UserGameData.Instance.UpdatePlayerRecords("terminal"));
            StartCoroutine(HelpDesk ());
		}
	}

	IEnumerator HelpDesk()
	{
		allowInput = false;
		yield return new WaitForSeconds (6);
		allowInput = true;

		//Hide the article and activate the help desk.
		article.SetActive (false);
		helpDesk.SetActive (true);
		helpText = GameObject.Find ("HelpText").GetComponent<Text> ();

		//Tell the user the correct answer.
		correctAnswer = "";
		switch (randAns) 
		{
		case 1:
			correctAnswer = " Paraphrase [P] ";
			break;
		case 2:
			correctAnswer = " Direct Quote [Q] ";
			break;
		case 3:
			correctAnswer = " Summary [S] ";
			break;
		}

		helpText.text = "Welcome to the Library HelpDesk. The correct answer was" +
			correctAnswer + ". Please review the following information about a" + correctAnswer + ":";

		helpText.text = helpText.text + "\n\n" + PQSInfoList [randAns - 1] [0];

		helpText.text = helpText.text + "\n\n" + "You may now review additional information or retry the " +
			"room authentication process. Enter 'P' for information about paraphrasing, 'S' for summarizing," +
			"or 'Q' for direct quote. Press Enter to return to the article.";

		helpText.text = helpText.text + "\n\n" + "P, S, Q, or Enter:";

	}

	IEnumerator HelpInfo(int index)
	{
		//Display information about a Paraphrasee, DirectQuote, or Summary
		allowInput = false;

		helpText.text = PQSInfoList [index] [0];

		yield return new WaitForSeconds (3);

		helpText.text = helpText.text + "\n\n" + "Press P, S, Q, or Enter:";
		allowInput = true;
	}

	IEnumerator QuitGame()
	{
		//End the game, though this should send the player back to their spot in the main game.
		allowInput = false;
		yield return new WaitForSeconds (6);
//		EditorApplication.isPlaying = false;
	}

	void SetPQSInfo()
	{
		//Initialize PQS information.
		ParaInfo.Add ("Paraphrasing involves borrowing ideas from the author and representing them " +
			         "in your own words without losing any of the main points. When paraphrasing, it " +
			         "is best to keep the number of words taken from the original text to a minimum. To " +
			         "avoid being mistaken as plagarism, paraphrases should always be followed by a citation " +
			         "to the original source.");
		QuoInfo.Add ("A direct quote is a sentence or paragraph taken verbatim " +
					"(word-for-word) from another source. Direct quotes are indicated by " +
					"quotation marks, followed by the page number the quote came from. ");
		SummInfo.Add ("A summary is a shortened version of the author's text that covers all " +
			         "the major points. This can be done in a single sentence or a paragraph. " +
			         "A summarization should include a citation with all pages used.");

		PQSInfoList.InsertRange(PQSInfoList.Count, new List<string>[]{
			ParaInfo, QuoInfo, SummInfo
		});

		/*http://www.plagiarism.org/citing-sources/how-to-paraphrase/*/
		/*http://isites.harvard.edu/icb/icb.do?keyword=k70847&pageid=icb.page350378*/
	}

	void CreateArticles()
	{
		//Initialize the articles and their answers.

		aOneArticle.Add ("The art of rolling down a hill takes years to perfect. " +
						"It is nearly impossible to roll straight down a hill without curving. " +
						"The arm is extremely important to train when preparing for steep competitions. " +
						"Using an arm to steady and steer trajectory helps keep an individual " +
						"on a straighter course. The arm can also be used to launch an individual a " +
						"few feet into the air. This helps an individual dodge unwelcome rocks, sticks, " +
						"or trash by bouncing directly over such objects.");
		aOnePara.Add("Perfecting rolling down a hill can take a very long time. " +
					"A person can use his or her arm to help guide his or her descent. " +
					"When rolling down hill, the arm is also useful for avoiding objects " +
					"(Brooks 37).");
		aOneQuo.Add("\"Using an arm to steady and steer trajectory helps " +
					"keep an individual on a straighter course\"(Brooks 37).");
		aOneSumm.Add("According to Brooks’ article, in order to roll straight " +
					"down a hill and avoid objects, an individual will need to " +
					"spend hours in practice and focus on strengthening his or her arm.");



		aTwoArticle.Add ("To ensure an increased level of safety for children, " +
						" all playgrounds use either sand or mulch as padding " +
						"for potential falls. Though useful for increasing safety, " +
						"mulch and sand still come with a few drawbacks. For example, " +
						"if sand comes in contact with eyes, it may irritate or damage " +
						"the eyes. Such instances where sand may get into a child's " +
						"eyes include when a child falls from objects such as swings, " +
						"slides, merry-go-rounds, and seesaws or when sand is airborne " +
						"as a thrown object or wind traveler.");
		aTwoPara.Add("In child play areas, safety is the primary focus. Sand or mulch is " +
					"used to cushion falls. Despite being a useful padding, sand can still " +
					"pose a threat to children. Children can get sand in their eyes from " +
					"activities like throwing sand or falling off of slides, swings, merry-go-" +
					"rounds, or see-saws (Walker 73).");
		aTwoQuo.Add("\"To ensure an increased level of safety for children, " +
					"all playgrounds use either sand or mulch as padding for " +
					"potential falls\" (Walker 73).");
		aTwoSumm.Add("Walker writes that despite the productions implemented for a " +
					"child's safety on a playground, a child may still be susceptible to " +
					"injury from eye contact with sand.");



		aThreeArticle.Add ("Tomorrow, January 7, 3044 will mark the release of the " +
						  "new animal-friendly hand gliders. Animal lovers have already " +
						  "begun gathering at the testing site: Mount Camjeds. The animal " +
						  "friendly hand gliders will include custom sized chairs with " +
						  "safety belts. Each animal will be fitted with a parachute that " +
						  "automatically opens if the animal becomes separated from the glider.");
		aThreePara.Add("Sullivan writes that hang gliders for animals will be available tomorrow " +
					"for testing at Mount Camjeds. The new product comes with a chair, a safety " +
					"belt, and a parachute.");
		aThreeQuo.Add("\"The animal-friendly hand gliders will include custom sized " +
					 "chairs with safety belts\" (Sullivan 33).");
		aThreeSumm.Add("Sullivan reports that the release of animal " +
					"friendly hand gliders will offer animal lovers such " +
					"safety features as safety belts and parachutes.");



		aFourArticle.Add ("The phobias that develop during childhood from various " +
						 "and unusual situations may linger into adulthood and be " +
						 "transferred into the conscience of future children. Consider a " +
						 "parent with the terrified fear of frogs. Upon seeing a frog, " +
						 "the parent screeches, jumps about frantically, and repeatedly " +
						 "declares that there is a frog in an abnormal volume. Having " +
						 "witnessed the parent’s response to frogs, the child begins " +
						 "mimicking the parent’s action whenever a frog is seen. In the child, " +
						 "the fear expands to encompass toy frogs as objects worthy to " +
						 "jump around in terror.");
		aFourPara.Add("Messer suggests that a child’s fear towards something " +
					"may stay with them even as an adult and be passed to future " +
					"generations. For example, a parent’s fear of frogs may be " +
					"adopted as a child's fear through the observation and " +
					"imitation of the parent's reactions.");
		aFourQuo.Add("\"Having witnessed the parent’s response to frogs, " +
					"the child begins mimicking the parent’s action whenever " +
					"a frog is seen\" (Messer 777).");
		aFourSumm.Add("According to Messer, parent to child phobias are " +
					"transferred by the child's observation of a parent's " +
					"response to a certain situation.");


		/*
		aFiveArticle.Add ("This is Article Five.");
		aFivePara.Add("FiveParaphrase");
		aFiveQuo.Add("FiveQuote");
		aFiveSumm.Add("FiveSummary");

		aSixArticle.Add ("This is Article Six.");
		aSixPara.Add("SixParaphrase");
		aSixQuo.Add("SixQuote");
		aSixSumm.Add("SixSummary");

		aSevenArticle.Add ("This is Article Seven.");
		aSevenPara.Add("SevenParaphrase");
		aSevenQuo.Add("SevenQuote");
		aSevenSumm.Add("SevenSummary");

		aEightArticle.Add ("This is Article Eight.");
		aEightPara.Add("EightParaphrase");
		aEightQuo.Add("EightQuote");
		aEightSumm.Add("EightSummary");
		*/


		//Add the PQS Lists to their respective articles.
		aOne.InsertRange (aOne.Count, new List<string>[] {
			aOneArticle, aOnePara, aOneQuo, aOneSumm
		});

		aTwo.InsertRange (aTwo.Count, new List<string>[] {
			aTwoArticle, aTwoPara, aTwoQuo, aTwoSumm
		});

		aThree.InsertRange (aThree.Count, new List<string>[]{
			aThreeArticle, aThreePara, aThreeQuo, aThreeSumm
		});

		aFour.InsertRange (aFour.Count, new List<string>[] {
			aFourArticle, aFourPara, aFourQuo, aFourSumm
		});

		/*
		aFive.InsertRange (aFive.Count, new List<string>[] {
			aFiveArticle, aFivePara, aFiveQuo, aFiveSumm
		});

		aSix.InsertRange (aSix.Count, new List<string>[] {
			aSixArticle, aSixPara, aSixQuo, aSixSumm
		});

		aSeven.InsertRange (aSeven.Count, new List<string>[] {
			aSevenArticle, aSevenPara, aSevenQuo, aSevenSumm
		});

		aEight.InsertRange (aEight.Count, new List<string>[] {
			aEightArticle, aEightPara, aEightQuo, aEightSumm
		});
		*/

		//Add the articles to the main list.
		articleList.InsertRange(articleList.Count, new List<List<string>>[]{
			aOne, aTwo, aThree, aFour/*, aFive, aSix, aSeven, aEight*/
		});
	}

    //----------------------------------COROUTINE DELAY LEVEL LOAD----------------------------------

    private IEnumerator DelayBeforeLoad()
    {
        yield return new WaitForSeconds(1.5f);

        if (SaveAndLoadLevel.Instance == null)
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameSelectScreen");

        //load next battle phase if it's the final battle
        if (GameController.Instance.finalBattle.Equals(false))
        {
            SaveAndLoadLevel.Instance.LoadLevel("MainFloor");
        }
        else
        {
            if (GameController.Instance.encounterWon.Equals(true))
            {
                GameController.Instance.encounterWon = false;
                SaveAndLoadLevel.Instance.LoadMiniGame("Encounter-PowerboxGame");
            }
            else
            {
                //reload scene to try again
                SaveAndLoadLevel.Instance.LoadMiniGame("Encounter-TerminalGame");
            }
        }
    }
}