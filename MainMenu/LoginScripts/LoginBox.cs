using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginBox : MonoBehaviour {

    private bool allowInput = true; //So the user can press Enter to login.

    private DateTime lastLogin;

    //Login fields
    private string username = "";
    private string password = "";

    //Register Fields
    /*Username*/
    /*Password*/
    private string cPassword = "";   //Password Confirmation
    private string firstName = "";
    private string lastName = "";

    //Text Fields
    private GameObject userField;
    private GameObject passField;
    private GameObject pass2Field;
    private GameObject fNameField;
    private GameObject lNameField;

    //Urls
    private string RegisterAccountURL = "http://127.0.0.1/TRphpFiles/register.php";
    private string LoginURL = "http://127.0.0.1/TRphpFiles/login.php";
    MySQLConnections mySQL = MySQLConnections.Instance;

    //Main Menu Forms
    [Header ("Forms")]
    public GameObject loginForm;
    public GameObject registerForm;
    public GameObject menu;
    public Text errorText;
    public GameObject guestButton;

    private void Awake()
    {
        //make sure there is only one LoginController (could be multiple when loading the title scene again)
        GameObject[] tmp = GameObject.FindGameObjectsWithTag("loginController");
        if (tmp.Length > 1)
        {
            if (tmp[0].GetInstanceID() > tmp[1].GetInstanceID())
            {
                Destroy(tmp[0].gameObject);
            }
            else
            {
                Destroy(tmp[1].gameObject);
            }
        }

        if (UserGameData.Instance.CurrentUser != "")
            ShowTitleMenu();
    }

    private void FixedUpdate()
    {
        if (allowInput && Input.GetKeyDown(KeyCode.Return))
        {
            if (loginForm.activeInHierarchy)
            {
                Login();
            }
            else if (registerForm.activeInHierarchy)
            {
                Register();
            }
        }    
        
    }

    public void Exit()
    {
        Application.Quit();
    }

 #region GUI Functionality
    public void OpenRegisterForm()
    {
        //This will be called from the Login() Method. Clear the LoginForm's fields.
        GameObject.Find("UserField").GetComponent<InputField>().text = "";
        GameObject.Find("PassField").GetComponent<InputField>().text = "";

        //Swap Active Screens
        registerForm.SetActive(true);
        loginForm.SetActive(false);

        
    }

    public void BackToLogin()
    {
        allowInput = true;
        //This will be called from the Register() Method. Clear the RegisterForm's fields,
        GameObject.Find("UserField").GetComponent<InputField>().text = "";
        GameObject.Find("FnameField").GetComponent<InputField>().text = "";
        GameObject.Find("LnameField").GetComponent<InputField>().text = "";
        GameObject.Find("Pass1Field").GetComponent<InputField>().text = "";
        GameObject.Find("Pass2Field").GetComponent<InputField>().text = "";

        //Swap Active Screens
        loginForm.SetActive(true);
        registerForm.SetActive(false);

        
    }

    public void ShowTitleMenu()
    {
        allowInput = true;
        //After logging in successfully, show the main menu so the player can start the game.
        errorText.text = "";
        menu.SetActive(true);
        loginForm.SetActive(false);
        guestButton.SetActive(false);

        //Enable the Game Select Button
        if (UserGameData.Instance.CurrentUser == "")
            UserGameData.Instance.CurrentUser = username.ToLower();
        UserGameData.Instance.StartCoroutine("GetPlayerProgress");

       // UserGameData.Instance.CheckPlayerProgress();
       /*CheckPlayerProgres ensures that the 'GameSelect' button is enabled when the player
        loads the game. But for some reason, it remained uninteractable, even after the
        quotronBeaten flag was true. So I've made the GetPlayerProgress CoRoutine
        run this instead.*/
    }
#endregion

    public void Register()
    {
        allowInput = false;

        //Then find RegisterForm's fields
        userField = GameObject.Find("UserField");
        fNameField = GameObject.Find("FnameField");
        lNameField = GameObject.Find("LnameField");
        passField = GameObject.Find("Pass1Field");
        pass2Field = GameObject.Find("Pass2Field");

        username = userField.GetComponent<InputField>().text;
        firstName = fNameField.GetComponent<InputField>().text;
        lastName = lNameField.GetComponent<InputField>().text;
        password = passField.GetComponent<InputField>().text;
        cPassword = pass2Field.GetComponent<InputField>().text;

        bool valid = true;

        if (username == "")
        {
            Debug.Log("Username is Empty");
            errorText.text = "Username is Empty";
            valid = false;
        }
        if (firstName == "" || lastName == "")
        {
            Debug.Log("Please fill both name fields");
            errorText.text = "Please fill both name fields";
            valid = false;
        }
        if (password == "" || cPassword == "")
        {
            Debug.Log("Enter a password");
            errorText.text = "Enter a password";
            valid = false;
        }
        else if (!password.Equals(cPassword))
        {
            Debug.Log("Passwords must match");
            errorText.text = "Passwords must match";
            valid = false;
        }

        if (valid)
            StartCoroutine("RegisterAccount");
        else
            allowInput = true;
    } // End Register()

    public void Login()
    {
        allowInput = false;

        userField = GameObject.Find("UserField");
        passField = GameObject.Find("PassField");

        username = userField.GetComponent<InputField>().text;
        password = passField.GetComponent<InputField>().text;

        bool valid = true;

        if (username == "" || password == "")
        {
            Debug.Log("Please enter a username and password");
            errorText.text = "Please enter a username and password";
            valid = false;
            allowInput = true;
        }

        if (valid)
            LoginAccount();
    } // End Login()

    void RegisterAccount()
    {
        string result = MySQLConnections.Instance.RegisterAcccount(username.ToLower(), firstName, lastName, password);

        //Debug.Log(result);
        if (result == "success")
        {
            Debug.Log("Account Created Successfully");
            errorText.text = "Account Created Successfully";
            BackToLogin();
        }
        else if (result == "exists")
        {
            Debug.Log("Account already exists");
            errorText.text = "Account already exists";
            allowInput = true;
        }
        else if (result == "error")
        {
            Debug.Log("Somethign went wrong. Either in the way the query was formatted, or when inserting values");
            allowInput = true;
        }

    }

    void LoginAccount()
    {
        bool found = MySQLConnections.Instance.LoginAccount(username.ToLower(), password);

        if (found)
        {
            //Debug.Log("Logged in Successfully");
            ShowTitleMenu();
        }
        else if (!found)
        {
            // Debug.Log("Incorrect Username / Password combination");
            errorText.text = "Incorrect Username / Password combination";
            allowInput = true;
        }
    }
}
