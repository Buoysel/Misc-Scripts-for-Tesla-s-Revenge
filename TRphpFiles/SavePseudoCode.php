<!--  IT SQL Server:  168.16.222.104 Port: 3306
      Login: game_user
      Pass: itec_mysql_game
      Database: game_user

      Redesign Database to take player save data as text
      Fix the save system

      save_data:
          user_name varchar(255), Foreign Key where user_name = logins user_name
          save_text text

      The save function should access the database instead of create a save file on the user's
      computer

      SaveGame()
          UploadSaveFile(PersistentDataManager.GetSaveData());

      UploadSaveFile(string saveData) {
          taskType = "SAVE_DATA";

          WWWForm saveForm = new WWWForm();
          saveForm.AddField("Task", taskType);
          saveForm.AddField("SaveData", saveData);
          WWW saveWWW = new WWW(userDataURL, saveForm);
          yield return saveWWW;

          if (saveWWW.error != null) {
            Debug.Log("Could not connect to setUserData");
          } else {
            Debug.Log ("Save Successfully uploaded");
          }
      }

      The load function should retrieve data from the database

      LoadGame()
          saveData = DownloadSaveFile();
          levelManager.LoadGame(saveData);

      string DownloadSaveFile() {
          taskType = "DOWNLOAD_SAVE";

          WWForm downloadForm = new WWWForm();
          downloadForm.AddField("Username", currentUser);

          WWW downloadWWW = new WWW(userDataURL, downloadForm);
          yield return downloadWWW;

          if (downloadWWW.error != null) {
              Debug.Log("Could not connect to setUserData");
              return null;
          } else {
              return downloadWWW.text;
          }
      }



-->
