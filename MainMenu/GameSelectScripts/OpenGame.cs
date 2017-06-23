using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenGame : MonoBehaviour {

    public string minigameName;

    public void BeginMinigame(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void ContinueMainGame(string name)
    {
        UserGameData.Instance.isLoadingSave = true;
        SceneManager.LoadScene(name);
    }
}
