using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserGameData : MonoBehaviour {

    private static UserGameData instance;
    public static UserGameData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<UserGameData>();
            }
            return instance;
        }
    }

    [SerializeField]
    private string currentUser;  
    public string CurrentUser
    {
        get { return currentUser; }
        set { currentUser = value; }
    }

    //Booleans
    public bool quotronBeaten = false;
    public bool wordsearchBeaten = false;
    public bool pipesBeaten = false;
    public bool powerboxBeaten = false;
    public bool bookcartBeaten = false;
    public bool terminalBeaten = false;
    public bool runnerBeaten = false;
    public bool gameBeaten = false;

    public bool isLoadingSave = false;

    //URLs
    private string userDataURL = "http://127.0.0.1/TRphpFiles/setUserData.php";

    private string taskType; //Controls the operation performed in php.

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    /* For now, redirect any calls to the commands here to the MySQL Connections 
     * script. */

    #region Player Progress

    public IEnumerator GetPlayerProgress()
    {
        MySQLConnections.Instance.GetPlayerProgress(currentUser);
        CheckPlayerProgress();
        yield return StartCoroutine("DownloadSaveFile");
    }

    public void CheckPlayerProgress()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "TitleScreen")
        {
            Button gameSelectButton = GameObject.Find("GameSelect").GetComponent<Button>();
            if (quotronBeaten)
                gameSelectButton.interactable = true;
        }
        else if (scene.name == "GameSelectScreen")
        {
            //Activate the buttons on the Game Select Screen if their games have been beaten.
            if (quotronBeaten)
                GameObject.Find("QuotronThumbnail").GetComponent<Button>().interactable = true;
            if (wordsearchBeaten)
                GameObject.Find("KeywordsearchThumbnail").GetComponent<Button>().interactable = true;
            if (pipesBeaten)
                GameObject.Find("PipeThumbnail").GetComponent<Button>().interactable = true;
            if (powerboxBeaten)
                GameObject.Find("PowerboxThumbnail").GetComponent<Button>().interactable = true;
            if (bookcartBeaten)
                GameObject.Find("BookcartThumbnail").GetComponent<Button>().interactable = true;
            if (terminalBeaten)
                GameObject.Find("TerminalThumbnail").GetComponent<Button>().interactable = true;
            if (runnerBeaten)
                GameObject.Find("RunnerThumbnail").GetComponent<Button>().interactable = true;
        } //End else if
    }//End CheckPlayerProgress()
    #endregion

    #region Update Player Records

    public IEnumerator UpdatePlayerAttempt(string game)
    {
        if (MySQLConnections.Instance.UpdatePlayerAttempt(currentUser, game))
        {
            Debug.Log("Attempt Updated Successfully");
        }
        else
        {
            Debug.Log("Something went wrong updating the attempt.");
        }
        yield return null;
    }


    public IEnumerator UpdatePlayerRecords(string game)
    {
        //Update for if the player loses
        if (MySQLConnections.Instance.UpdatePlayerLoss(currentUser, game))
        {
            Debug.Log("Loss Updated Successfully");
        }
        else
        {
            Debug.Log("Something went wrong trying to update Losses");
        }
        yield return null;
    } //End Update Player Loss

    public IEnumerator UpdatePlayerRecords(string game, TimeSpan time)
    {
        //Update for if the player wins
        if (MySQLConnections.Instance.UpdatePlayerWin(currentUser, game, time))
        {
            Debug.Log("Win updated successfully");
        }
        else
        {
            Debug.Log("Something went wrong updating wins");
        }
        yield return null;
    } //End Update Player Wiin
    #endregion

    #region Save Data

    public IEnumerator UploadSaveFile(string saveData)
    {
        if (MySQLConnections.Instance.UploadSaveFile(currentUser, saveData))
        {
            Debug.Log("Save Succesfully Uploaded.");
        }
        else
        {
            Debug.Log("Something went wrong uploading the save file.");
        }
        yield return null;
    }

    public IEnumerator DownloadSaveFile()
    {
        string saveData = MySQLConnections.Instance.DownloadSaveFile(currentUser);

        yield return saveData;
        //Make the continue button active.
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "TitleScreen")
        {
            Button continueButton = GameObject.Find("Continue").GetComponent<Button>();
            continueButton.interactable = true;
        }
    }

    #endregion
} //END CLASS UserGameData


//This class is supposed to allow me to access data returned from PHP
public class CoroutineWithData 
{
    public Coroutine coroutine { get; private set; }
    public object result;
    private IEnumerator target;

    public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
    {
        this.target = target;
        this.coroutine = owner.StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        while (target.MoveNext())
        {
            result = target.Current;
            yield return result;
        }
    }
}