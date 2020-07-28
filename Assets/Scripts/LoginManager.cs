using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class LoginManager : MonoBehaviour
{

    const string login_api = "http://shopanalytica.com/public/api/login";
    const string logut_api = "http://shopanalytica.com/public/api/logout";
    public InputField username;
    public InputField password; 

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

    public void LoginUser()
    {
        if (username.text!="" && password.text!="")
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
        form.AddField("username", username.text);
        form.AddField("password", password.text);

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
        }
        else {
            Debug.Log("Request Done!:" + webRequest.downloadHandler.text);
            User user = JsonUtility.FromJson<User>(webRequest.downloadHandler.text);
            if (user.success)
            {
                LoginSuccess = "success";
                UserID = user.data.id;
                Token = user.data.token;
                FileManager.instance.CreateFolders();
                UIManager.instance.CheackLogin();
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
}

[System.Serializable]
public class LogOutInfo
{
    public bool success; 
    public string data;
}
