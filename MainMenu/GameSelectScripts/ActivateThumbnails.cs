using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateThumbnails : MonoBehaviour {

    //Set the thumbnails on the GameSelectScreen to interactable when the player is logged in.

	void Start () {
        if (UserGameData.Instance != null)
            UserGameData.Instance.CheckPlayerProgress();
	}
	
}
