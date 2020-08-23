using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UploadZipItem : MonoBehaviour
{
    public GameObject m_statusMessage;
    public GameObject uploadButton;
    public Text fileNameText;
    string fileName;

    public void Init(string fileName)
    {
        this.fileName = fileName;
        fileNameText.text = fileName;
        uploadButton.GetComponent<Button>().onClick.AddListener(Upload);
    }


    private void Upload()
    {
        StartCoroutine(CheackServer());
    }

    IEnumerator CheackServer()
    {

        WWWForm form = new WWWForm();

        UnityWebRequest webRequest = UnityWebRequest.Get("https://shopanalytica.com/api/check-server");

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
            PopupManager.instance.OpenPopup("Uploading Failed!");
        }
        else
        {
            Debug.Log("Request Done!:" + webRequest.downloadHandler.text);
            ServerResponse serverResponse = JsonUtility.FromJson<ServerResponse>(webRequest.downloadHandler.text);
            if (serverResponse.data == "Success")
            {
                StartCoroutine(UploadUserData());
            }
        }

    }


    IEnumerator UploadUserData()
    {
        uploadButton.SetActive(false);
        m_statusMessage.SetActive(true);

        WWWForm form = new WWWForm();
        string path = Application.persistentDataPath + "/Zips/" + fileName + ".zip";
        byte[] bytes = File.ReadAllBytes(path);
        form.AddField("user_id", LoginManager.UserID);
        form.AddBinaryData("file", bytes, fileName + ".zip"); ;
        UnityWebRequest webRequest = UnityWebRequest.Post("http://shopanalytica.com/public/api/save-zip-file", form);

        webRequest.SendWebRequest();

        while (!webRequest.isDone)
        {
            yield return null;

            // Progress is always set to 1 on android
            m_statusMessage.GetComponent<Text>().text = webRequest.uploadProgress * 100 + "%";

        }


        if (webRequest.isHttpError || webRequest.isNetworkError)
        {
            Debug.Log(webRequest.error);
        }
        else
        {
            Debug.Log("Request Done!:" + webRequest.downloadHandler.text);
            UploadFileManager.RemoveDoneFileName(fileName);
        }



    }
}
