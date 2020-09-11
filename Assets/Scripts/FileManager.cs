using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FileManager : MonoBehaviour
{


    public Image progressFill;
    public Text  progressText;
    public GameObject loadingObject;
    private string ZipFileName;

    public static FileManager instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static int ZipIndex
    {
        set
        {
            PlayerPrefs.SetInt("ZipIndex", value);
        }
        get
        {
            return PlayerPrefs.GetInt("ZipIndex");
        }
    }

    private void Start()
    {
        loadingObject.SetActive(false);

       

        if (ShopData.ShopDataManager.CurrentDayShopInfo == "Loaded")
        {
            ShopData.ShopDataCreator.CreateShopList();
        }

    }
    public void StartUploadingProcess()
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
                CompressFolder();
            }
        }

    }


    public void CompressFolder()
    {
        string[] file = Directory.GetFiles(Application.persistentDataPath + "/Completed/");

        if (file.Length <= 4) {
            PopupManager.instance.OpenPopup("No Data to upload!");
            Debug.Log("No Data to upload!");
            return;
         }


        string source = Application.persistentDataPath+ "/Completed";
        ZipIndex++;
        ZipFileName = LoginManager.UserID + "_" + LoginManager.CurrentDay+ "_" + ZipIndex;
        string destination = Application.persistentDataPath+"/Zips/"+ ZipFileName+ ".zip";
        ZipFile.CreateFromDirectory(source,destination);
        DeleteAllFiles();
    }

    public void CreateFolders()
    {
        var zipFolder = Directory.CreateDirectory(Application.persistentDataPath + "/Zips/"); // returns a DirectoryInfo object
        var dataFolder = Directory.CreateDirectory(Application.persistentDataPath + "/Data/"); // returns a DirectoryInfo object
        var completedFolder = Directory.CreateDirectory(Application.persistentDataPath + "/Completed/"); // returns a DirectoryInfo object
       
    }

    void DeleteAllFiles()
    {

        foreach (var file in Directory.GetFiles(Application.persistentDataPath + "/Completed/"))
        {
            FileInfo file_info = new FileInfo(file);
            Debug.Log(file_info);
            file_info.Delete();
        }
        StartCoroutine(UploadUserData());
        loadingObject.SetActive(true);
    }



    IEnumerator UploadUserData()
    {
        PopupManager.instance.SetLoading(true);

        UploadFileManager.AddFileName(ZipFileName);
        WWWForm form = new WWWForm();
        string path = Application.persistentDataPath + "/Zips/"+ZipFileName+".zip";
        byte[] bytes = File.ReadAllBytes(path);
        form.AddField("user_id", LoginManager.UserID);
        form.AddBinaryData("file", bytes, ZipFileName+".zip"); ;
        UnityWebRequest webRequest = UnityWebRequest.Post("http://shopanalytica.com/public/api/save-zip-file", form);

        webRequest.SendWebRequest();

        while (!webRequest.isDone)
        {
            yield return null;

            // Progress is always set to 1 on android
            //progressText.text =  webRequest.uploadProgress*100+"%";
            //progressFill.fillAmount = webRequest.uploadProgress;

            progressText.text =  "Uploading...";
            progressFill.fillAmount = 0;
        }


        if (webRequest.isHttpError || webRequest.isNetworkError) {
            Debug.Log(webRequest.error);
            PopupManager.instance.OpenPopup("Upload Failed!");
            PopupManager.instance.SetLoading(false);
        }
        else {
            Debug.Log("Request Done!:" + webRequest.downloadHandler.text);
            loadingObject.SetActive(false);
            UploadFileManager.RemoveDoneFileName(ZipFileName);
            PopupManager.instance.OpenPopup("Upload Done!");
            PopupManager.instance.SetLoading(false);
            // ShopData.ShopDataManager.CurrentDayShopInfo = "UnLoaded";
        }



    }

    public void MoveData()
    {
        string tempPath = Application.persistentDataPath + "/Data/";
        foreach (string file in Directory.GetFiles(tempPath, "*.jpg"))
        {

            string des = file.Replace(Application.productName + "/Data", Application.productName + "/Completed");
            Debug.Log(file);
            Debug.Log(des);
            File.Move(file,des);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            //DeleteAllFiles();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            //SaveIntoJson();
            Debug.Log("Function Called");
            // CreateFolders();
            //CompressFolder();
            //MoveData();
            //StartCoroutine(UploadUserData());
        }
    }
}

[Serializable]
public class ServerResponse
{
    public bool success;
    public string data;
}