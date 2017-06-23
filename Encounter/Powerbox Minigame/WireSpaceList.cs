using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WireSpaceList : MonoBehaviour
{

	//This will contain the list of Wire Space GameObjects.
	//This gets attached to all Wire Sets.

	public List<GameObject>			wireSpaceList;
	private Text					searchText;

	private AudioSource 			audioPlayer;			//The SoundManager's AudioSource Component.
	private Sounds 					soundEffect;			//Script for the SoundManager's sounds.
	private bool 					winPlaying = false;		//Is the win sound effect playing?
	public string 					selectedSpace;

    private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

    void Awake()
	{
		searchText = GameObject.Find ("Search Description").GetComponent<Text>();

		audioPlayer = GameObject.Find ("SoundManager").GetComponent<AudioSource> ();
		soundEffect = GameObject.Find ("SoundManager").GetComponent<Sounds> ();
	}

    private void Start()
    {
        UserGameData.Instance.StartCoroutine(UserGameData.Instance.UpdatePlayerAttempt("powerbox"));
        stopwatch.Start();
    }

    bool isUpdatingRecords = false;  //Again, I'm sorry for putting this here.

    void Update()
	{
		//Win the game if the wireSpaceList is empty.
		if (wireSpaceList.Count == 0) 
		{
			searchText.text = "You Win!";

            //only perform the following if you are fixing the wires in the second floor and not during the boss fight
            if(GameController.Instance != null && GameController.Instance.finalBattle.Equals(false))
            {
                PixelCrushers.DialogueSystem.DialogueLua.SetQuestField("Fix_Powerbox", "State", "success");
                QuestManager.Instance.QuestDescription = "Success! The lights are on.";
            }
            
//          searchText.fontSize = 500;
//			searchText.alignment = TextAlignment.Center;

			if (winPlaying == false) 
			{
				WinMusic ();
			}

            if (GameController.Instance != null)
            {
                GameController.Instance.encounterWon = true;
                GameController.Instance.powerboxFixed = true;
            }

            if (!isUpdatingRecords)
            {
                stopwatch.Stop();
                System.TimeSpan ts = stopwatch.Elapsed;

                if (UserGameData.Instance != null)
                    UserGameData.Instance.StartCoroutine(UserGameData.Instance.UpdatePlayerRecords("powerbox", ts));
                isUpdatingRecords = true;
            }
            //return to overworld
            StartCoroutine("DelayBeforeLoad");
        }
	}

	void WinMusic()
	{
		winPlaying = true;
		audioPlayer.clip = soundEffect.win;
		audioPlayer.Play ();
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
                SaveAndLoadLevel.Instance.LoadMiniGame("Encounter-Runner");
            }
            else
            {
                //reload scene to try again
                SaveAndLoadLevel.Instance.LoadMiniGame("Encounter-PowerboxGame");
            }
        }
    }

}