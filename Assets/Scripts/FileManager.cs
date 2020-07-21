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

    [SerializeField] private PotionData _PotionData = new PotionData();

    public Text progress;

    public void SaveIntoJson()
    {
        string potion = JsonUtility.ToJson(_PotionData);
        File.WriteAllText(Application.persistentDataPath + "/PotionData.json", potion);
    }
    int index = 0;
    public void CompressFolder()
    {
        //var folder = Directory.CreateDirectory(Application.persistentDataPath + "/Zips/"); // returns a DirectoryInfo object

        string source = Application.persistentDataPath+"/Data";
        string destination = Application.persistentDataPath+"/Zips/userdata"+index+".zip";
        index++;
        ZipFile.CreateFromDirectory(source,destination);

    }

    void DeleteAllFiles()
    {
        foreach (var directory in Directory.GetDirectories(Application.persistentDataPath))
        {
            DirectoryInfo data_dir = new DirectoryInfo(directory);
            data_dir.Delete(true);
        }

        foreach (var file in Directory.GetFiles(Application.persistentDataPath))
        {
            FileInfo file_info = new FileInfo(file);
            Debug.Log(file_info);
            file_info.Delete();
        }
    }

    public void UploadZip()
    {
        StartCoroutine(UploadUserData());
    }


    IEnumerator UploadUserData()
    {

        WWWForm form = new WWWForm();
        string path = Application.persistentDataPath + "/Zips/userdata0.zip";
        byte[] bytes = File.ReadAllBytes(path);
        form.AddField("user_id", 1);
        form.AddBinaryData("file", bytes, "userdata0.zip"); ;
        UnityWebRequest webRequest = UnityWebRequest.Post("http://shopanalytica.com/public/api/save-zip-file", form);

        webRequest.SendWebRequest();

        while (!webRequest.isDone)
        {
            yield return null;

            // Progress is always set to 1 on android
            progress.text = "" + webRequest.uploadProgress;
            Debug.LogFormat("Progress: {0}", webRequest.uploadProgress);
        }


        if (webRequest.isHttpError || webRequest.isNetworkError)
                Debug.Log(webRequest.error);
            else
                Debug.Log("Request Done!:" + webRequest.downloadHandler.text);


    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            DeleteAllFiles();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            //SaveIntoJson();
            Debug.Log("Function Called");
            //CompressFolder();
            StartCoroutine(UploadUserData());
        }
    }
}

[System.Serializable]
public class PotionData
{
    public string potion_name;
    public int value;
    public List<Effect> effect = new List<Effect>();
}

[System.Serializable]
public class Effect
{
    public string name;
    public string desc;
}
