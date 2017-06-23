using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class Health : MonoBehaviour {

	public List<GameObject>	pHealthList;	//Player Health
	public List<GameObject>	eHealthList;	//Enemy Health

	public GameObject			healthOrb;		//current healt orb.

	public Text 				message;		//Status message.
	public int wrong = 1;

    private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

    void Start ()
	{
        UserGameData.Instance.StartCoroutine(UserGameData.Instance.UpdatePlayerAttempt("runner"));
        stopwatch.Start();

		message = GameObject.Find ("MessageText").GetComponent<Text>();
		message.text = "";

		//Get the health orbs
		for (int i = 1; i <= 3; i++) 
		{
			pHealthList.Add(GameObject.Find("pHP" + i));
			eHealthList.Add(GameObject.Find ("eHP" + i));
		}
	}

	public void DamagePlayer()
	{
		healthOrb = pHealthList[wrong - 1];


		message.text = "Ouch!";
		healthOrb.SetActive(false);

		if (wrong == 3)
        { 
			message.text = "Game Over";

            if (GameController.Instance != null)
                GameController.Instance.encounterWon = false;

            UserGameData.Instance.StartCoroutine(UserGameData.Instance.UpdatePlayerRecords("runner"));
            StartCoroutine("DelayBeforeLoad");
        }

		wrong++;
	}

	public void DamageEnemy()
	{
		healthOrb = eHealthList [0];
		eHealthList.Remove (healthOrb);

		message.text = "Correct!";
		Destroy (healthOrb);

		if (eHealthList.Count == 0) 
		{
			RunonEncounterController runoncontrol;
			runoncontrol = GetComponent<RunonEncounterController> ();
			runoncontrol.anim.SetTrigger ("PlayerWins");
			message.text = "Victory!";

            if (GameController.Instance != null)
                GameController.Instance.encounterWon = true;

            stopwatch.Stop();
            System.TimeSpan ts = stopwatch.Elapsed;

            if (UserGameData.Instance != null)
                UserGameData.Instance.StartCoroutine(UserGameData.Instance.UpdatePlayerRecords("runner", ts));
            StartCoroutine("DelayBeforeLoad");
        }
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
                //you won!
                GameController.Instance.beatTesla = true;
                SaveAndLoadLevel.Instance.LoadLevel("MainFloor");
            }
            else
            {
                //reload scene to try again
                SaveAndLoadLevel.Instance.LoadMiniGame("Encounter-Runner");
            }
        }
    }
}
