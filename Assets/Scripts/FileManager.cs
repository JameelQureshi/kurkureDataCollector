using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FileManager : MonoBehaviour
{

    [SerializeField] private PotionData _PotionData = new PotionData();

    public void SaveIntoJson()
    {
        string potion = JsonUtility.ToJson(_PotionData);
        File.WriteAllText(Application.persistentDataPath + "/PotionData.json", potion);
    }

    public void CompressFolder()
    {
        lzip.compressDir( Application.persistentDataPath, 3, Application.persistentDataPath+"/userdata.zip");
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


    IEnumerator PrepareFile()
    {


        // Read the zip file.
        WWW loadTheZip = new WWW(Application.persistentDataPath + "/userdata.zip");

        yield return loadTheZip;

        PrepareStepTwo(loadTheZip);
    }

    void PrepareStepTwo(WWW post)
    {
        StartCoroutine(UpLoadUserData(post));
    }

    IEnumerator UpLoadUserData(WWW post)
    {

        WWWForm form = new WWWForm();
        form.AddBinaryData("myTestFile.zip",post.bytes,"myFile.zip","application / zip");
        UnityWebRequest www = UnityWebRequest.Post("https://euphoriaxr.com/Files/ZipUpload.php", form);
        yield return www.Send();
        if (www.isNetworkError)
            Debug.Log(www.error);
        else
            Debug.Log("Uploaded");
        Debug.Log(www.downloadHandler.text);
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
            StartCoroutine(PrepareFile());
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
