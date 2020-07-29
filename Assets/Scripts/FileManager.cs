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





    public void CompressFolder()
    {
        string[] file = Directory.GetFiles(Application.persistentDataPath + "/Completed/");

        if (file.Length <= 4) {
            Debug.Log("No Data to upload!");
            return;
         }


        string source = Application.persistentDataPath+ "/Completed";
        ZipIndex++;
        ZipFileName = LoginManager.UserID+"_"+ LoginManager.CurrentDay+"_"+ZipIndex;
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
            progressText.text =  webRequest.uploadProgress*100+"%";
            progressFill.fillAmount = webRequest.uploadProgress;
        }


        if (webRequest.isHttpError || webRequest.isNetworkError) {
            Debug.Log(webRequest.error);
        }
        else {
            Debug.Log("Request Done!:" + webRequest.downloadHandler.text);
            loadingObject.SetActive(false);
            //ShopData.ShopDataManager.CurrentDayShopInfo = "UnLoaded";
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

