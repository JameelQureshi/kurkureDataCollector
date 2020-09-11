using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class LoginManager : MonoBehaviour
{

    const string login_api = "https://shopanalytica.com/api/login";
    const string logut_api = "https://shopanalytica.com/api/logout";
    public InputField usernameInput;
    public InputField passwordInput; 

    public static string LoginSuccess
    {
        set
        {
            PlayerPrefs.SetString("success",value);
        }
        get
        {
           return PlayerPrefs.GetString("success");
        }
    }

    public static int UserID
    {
        set
        {
            PlayerPrefs.SetInt("UserID", value);
        }
        get
        {
            return PlayerPrefs.GetInt("UserID");
        }
    }

    public static string Token
    {
        set
        {
            PlayerPrefs.SetString("Token", value);
        }
        get
        {
            return PlayerPrefs.GetString("Token");
        }
    }

    public static string Username
    {
        set
        {
            PlayerPrefs.SetString("Username", value);
        }
        get
        {
            return PlayerPrefs.GetString("Username");
        }
    }

    public static int CurrentDay
    {
        set
        {
            PlayerPrefs.SetInt("CurrentDay", value);
        }
        get
        {
            return PlayerPrefs.GetInt("CurrentDay",1);
        }
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)){
            PlayerPrefs.DeleteAll();
        }
    }

    public void LoginUser()
    {
        if (usernameInput.text!="" && passwordInput.text!="")
        {
            StartCoroutine(PostLoginRequest());
        }
    }
    public void LogoutUser()
    {
        StartCoroutine(PostLogoutRequest());
    }

    IEnumerator PostLoginRequest()
    {

        WWWForm form = new WWWForm();
        form.AddField("username", usernameInput.text);
        form.AddField("password", passwordInput.text);

        UnityWebRequest webRequest = UnityWebRequest.Post(login_api, form);

        webRequest.SendWebRequest();

        while (!webRequest.isDone)
        {
            yield return null;

            // Progress is always set to 1 on android
           // Debug.LogFormat("Progress: {0}", webRequest.uploadProgress);
        }


        if (webRequest.isHttpError || webRequest.isNetworkError) {
            Debug.Log(webRequest.error);
            PopupManager.instance.OpenPopup("Login Failed!");
        }
        else {
            Debug.Log("Request Done!:" + webRequest.downloadHandler.text);
            User user = JsonUtility.FromJson<User>(webRequest.downloadHandler.text);
            if (user.success)
            {
                LoginSuccess = "success";
                UserID = user.data.id;
                Token = user.data.token;
                Username = user.data.username;
                FileManager.instance.CreateFolders();
                UIManager.instance.CheackLogin();
            }
            else
            {
                PopupManager.instance.OpenPopup("Login Failed!");
            }

        }

    }

    IEnumerator PostLogoutRequest()
    {

        WWWForm form = new WWWForm();
        form.AddField("user_id", UserID);

        UnityWebRequest webRequest = UnityWebRequest.Post(logut_api, form);

        webRequest.SendWebRequest();

        while (!webRequest.isDone)
        {
            yield return null;

            // Progress is always set to 1 on android
            // Debug.LogFormat("Progress: {0}", webRequest.uploadProgress);
        }


        if (webRequest.isHttpError || webRequest.isNetworkError)
        {
            Debug.Log(webRequest.error);
            PopupManager.instance.OpenPopup("Logout Failed!");
        }
        else
        {
            Debug.Log("Request Done!:" + webRequest.downloadHandler.text);
            LogOutInfo logOutInfo = JsonUtility.FromJson<LogOutInfo>(webRequest.downloadHandler.text);
            if (logOutInfo.success)
            {
                LoginSuccess = "";
                UIManager.instance.ActivateScreen(0); // Login Screen is at 0
            }
            else
            {
                PopupManager.instance.OpenPopup("Logout Failed!");
            }

        }

    }

}
[System.Serializable]
public class User
{
    public bool success;
    public Data data;
}
[System.Serializable]
public class Data
{
    public int id;
    public string token;
    public string username;
}

[System.Serializable]
public class LogOutInfo
{
    public bool success; 
    public string data;
}
