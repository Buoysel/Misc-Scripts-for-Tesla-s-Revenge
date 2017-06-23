using System;
using System.Data;
using System.Security.Cryptography;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class MySQLConnections : MonoBehaviour {

    private static MySQLConnections instance;
    public static MySQLConnections Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<MySQLConnections>();
            }
            return instance;
        }
    }

    //Server information
    public string server, database, user, pass;
    public bool pooling = true;

    //Connection and commands
    public string connectionString;
    private MySqlConnection conn = null;
    private MySqlCommand cmd = null;
    private MySqlDataReader rdr = null;

    private void Awake()
    {
        connectionString = "Server=" + server +
                           ";Database=" + database +
                           ";User=" + user +
                           ";Password=" + pass +
                           ";Pooling=";

        if (pooling)
        {
            connectionString += "true;";
        }
        else
        {
            connectionString += "false;";
        }

        try
        {
            conn = new MySqlConnection(connectionString);
            conn.Open();
            Debug.Log("MySql State: " + conn.State);

            //Start writing or referencing commands for the database.
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }

    #region Login And Registration

    public bool LoginAccount(string username, string password)
    {
        cmd = new MySqlCommand();
        SHA1 sha1Hash = SHA1.Create();

        //Convert password to bytes to be used for SHA1 encryption.
        byte[] data = sha1Hash.ComputeHash(Encoding.ASCII.GetBytes(password));
        StringBuilder sBuilder = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
        string sha1Pass = sBuilder.ToString();

        DateTime lastLogin = new DateTime();
        lastLogin = DateTime.Now;
        string lastLoginFormat = lastLogin.ToString("yyyy-MM-dd HH:mm:ss");

        string sql = @"SELECT user_name
                       FROM logins
                       WHERE user_name = @username
                       AND pass = @password";
        try
        {
            cmd.Connection = conn;
            cmd.CommandText = sql;
            cmd.Prepare();

            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", sha1Pass);

            if (Convert.ToString(cmd.ExecuteScalar()) == username)
            {
                Debug.Log(lastLogin);
                sql = @"UPDATE logins
                        SET last_login = '" + lastLoginFormat + @"'
                        WHERE user_name = @username";
                cmd.CommandText = sql;
                cmd.Prepare();
                cmd.ExecuteScalar();
                return true;
            }
            else
                return false;
        }
        catch (MySqlException ex)
        {
            Debug.Log("Error " + ex.Number + " has occured: " + ex.Message);
            return false;
        }
    }

    public string RegisterAcccount(string username, string firstName, string lastName, string password)
    {
        cmd = new MySqlCommand();

        //Convert password to bytes to be used for SHA1 encryption.
        SHA1 sha1Hash = SHA1.Create();
        byte[] data = sha1Hash.ComputeHash(Encoding.ASCII.GetBytes(password));
        StringBuilder sBuilder = new StringBuilder();
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
        string sha1Pass = sBuilder.ToString();

        string[] games = new string[]
        {
            "game", "quotron", "wordsearch",
            "pipes", "powerbox", "bookcart",
            "terminal", "runner"
        };

        //Determine if the account already exists.
        string sql = @"SELECT user_name
                       FROM logins
                       WHERE user_name = @username";

        try
        {
            cmd.Connection = conn;
            cmd.CommandText = sql;
            cmd.Prepare();

            cmd.Parameters.AddWithValue("@username", username);

            if (Convert.ToString(cmd.ExecuteScalar()) == username)
            {
                /* Return a message that the account already exists */
                return "exists";
            }
            else
            {
                //Create a login.
                sql = @"INSERT INTO logins (user_name, first_name, last_name, pass)
                        VALUES (@username, @firstName, @lastName, @password)";
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@lastName", lastName);
                cmd.Parameters.AddWithValue("@password", sha1Pass);

                if (cmd.ExecuteNonQuery() != 1)
                {
                    /* Something went wrong. Return an error or something. */
                    return "error";
                }
                else
                {
                    foreach (string game in games)
                    {
                        // Create blank game records 
                        sql = @"INSERT INTO " + game + @"_records (user_name, " + game + @"_record)
                                VALUES (@username, '59:59:59')";
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                    // Create blank save data
                    sql = @"INSERT INTO save_data (user_name) 
                            VALUES (@username)";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    return "success";
                }
            }
        }
        catch (MySqlException ex)
        {
            Debug.Log("Error " + ex.Number + " has occured: " + ex.Message);
            return "error";
        }
    }
    #endregion Login and Registration

    #region Player Records

    public void GetPlayerProgress(string username)
    {
        cmd = new MySqlCommand();

        List<bool> flagInts = new List<bool>();

        string[] games = new string[]
        {
            "game", "quotron", "wordsearch",
            "pipes", "powerbox", "bookcart",
            "terminal", "runner"
        };

        cmd.Connection = conn;
        cmd.Prepare();
        cmd.Parameters.AddWithValue("@username", username);

        foreach (string game in games)
        {
            string sql = "SELECT " + game + "_beaten " +
                         "FROM " + game + "_records " +
                         "WHERE user_name = @username";
            try
            {
                
                cmd.CommandText = sql;
                
                flagInts.Add(Convert.ToBoolean(cmd.ExecuteScalar()));
            }
            catch (MySqlException ex)
            {
                Debug.Log("Error " + ex.Number + " has occured: " + ex.Message);
            }

            
        }
        UserGameData.Instance.gameBeaten = flagInts[0];
        UserGameData.Instance.quotronBeaten = flagInts[1];
        UserGameData.Instance.wordsearchBeaten = flagInts[2];
        UserGameData.Instance.pipesBeaten = flagInts[3];
        UserGameData.Instance.powerboxBeaten = flagInts[4];
        UserGameData.Instance.bookcartBeaten = flagInts[5];
        UserGameData.Instance.terminalBeaten = flagInts[6];
        UserGameData.Instance.runnerBeaten = flagInts[7];
    }

    public bool UpdatePlayerAttempt(string username, string game)
    {
        cmd = new MySqlCommand();

        string gameTable = game + "_records";
        string gameAttempt = game + "_attempts";

        string sql = @"UPDATE " + gameTable +
                     " SET " + gameAttempt + " = " + gameAttempt + @" + 1
                       WHERE user_name = @username";

        try
        {
            cmd.Connection = conn;
            cmd.CommandText = sql;
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@username", username);
            cmd.ExecuteNonQuery();
            return true;
        } 
        catch (MySqlException ex)
        {
            Debug.Log("Error " + ex.Number + " has occured: " + ex.Message);
            return false;
        }

    }

    public bool UpdatePlayerLoss(string username, string game)
    {
        cmd = new MySqlCommand();

        string gameTable = game + "_records";
        string gameAttemptLost = game + "_attempts_lost";

        string sql = @"UPDATE " + gameTable + 
                     " SET " + gameAttemptLost + " = " + gameAttemptLost + @" + 1
                       WHERE user_name = @username";

        try
        {
            cmd.Connection = conn;
            cmd.CommandText = sql;
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@username", username);
            cmd.ExecuteNonQuery();
            return true;
        }
        catch (MySqlException ex)
        {
            Debug.Log("Error " + ex.Number + " has occured: " + ex.Message);
            return false;
        }
    }

    public bool UpdatePlayerWin(string username, string game, TimeSpan time)
    {
        cmd = new MySqlCommand();

        string gameBeaten = game + "_beaten";
        string gameTable = game + "_records";
        string gameAttemptWon = game + "_attempts_won";

        //Increase Win Attempts
        string sql = @"UPDATE " + gameTable +
                     " SET " + gameBeaten + " = 1, " + gameAttemptWon + " = " + gameAttemptWon + " + 1 " + @"
                       WHERE user_name = @username";

        try
        {
            cmd.Connection = conn;
            cmd.CommandText = sql;
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@username", username);
            cmd.ExecuteNonQuery();

            //Modify the Time if the player made a new record.
            string timeRecord = game + "_record";
            string temptime = "";

            sql = @"SELECT " + timeRecord + @"
                    FROM " + gameTable + @"
                    WHERE user_name = @username";
            cmd.CommandText = sql;
            cmd.Prepare();
            temptime = Convert.ToString(cmd.ExecuteScalar());

            if (time < TimeSpan.Parse(temptime) || temptime == null)
            {
                sql = @"UPDATE " + gameTable +
                      " SET " + timeRecord + " = '" + time + @"' 
                        WHERE user_name = @username";
                cmd.CommandText = sql;
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            return true;
        }
        catch (MySqlException ex)
        {
            Debug.Log("Error " + ex.Number + " has occured: " + ex.Message);
            return false;
        }
                        
    }

    #endregion Player Records

    #region SaveData

    public string DownloadSaveFile(string username)
    {
        cmd = new MySqlCommand();

        string saveData = "";
        string sql = @"SELECT save_text 
                       FROM save_data 
                       WHERE user_name = @username";
        try
        {
            cmd.Connection = conn;
            cmd.CommandText = sql;
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@username", username);
            saveData = Convert.ToString(cmd.ExecuteScalar());
        }
        catch (MySqlException ex)
        {
            Debug.Log("Error " + ex.Number + " has occured: " + ex.Message);
            return saveData;
        }

        return saveData;
    }

    public bool UploadSaveFile(string username, string saveData)
    {
        cmd = new MySqlCommand();

        string sql = @"UPDATE save_data
                       SET save_text = '" + saveData + @"'
                       WHERE user_name = @username";

        try
        {
            cmd.Connection = conn;
            cmd.CommandText = sql;
            cmd.Prepare();
            cmd.Parameters.AddWithValue("@username", username);
            cmd.ExecuteNonQuery();
            return true;
        }
        catch (MySqlException ex)
        {
            Debug.Log("Error " + ex.Number + " has occured: " + ex.Message);
            return false;
        }
    }
    #endregion SaveData

    private void OnApplicaitonQuit()
    { 
        if (conn != null)
        {
            if (conn.State.ToString() != "Closed")
            {
                conn.Close();
            }
            conn.Dispose();
        }
    } 

}
