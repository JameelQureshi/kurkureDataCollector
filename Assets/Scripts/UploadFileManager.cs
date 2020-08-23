using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UploadFileManager : MonoBehaviour
{
    public static FileUploadStatus fileUploadStatus;
    public GameObject content;
    public GameObject m_prefab;
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
       try
       {
         string path = Application.persistentDataPath + "/Data/UploadStatus.json";
         string contents = File.ReadAllText(path);
         fileUploadStatus = JsonUtility.FromJson<FileUploadStatus>(contents);
        }
       catch (Exception e)
        {
            fileUploadStatus = new FileUploadStatus
            {
                fileNames = new List<string>()
            };
            Debug.Log(e);
        }

    }
    public void CreateItems()
    {
        foreach (Transform child in content.transform)
        {
           Destroy(child.gameObject);
        }

        try {
            foreach (string fname in fileUploadStatus.fileNames)
            {
                GameObject item = Instantiate(m_prefab, content.transform);
                item.GetComponent<UploadZipItem>().Init(fname);
            }

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        float width = canvas.GetComponent<RectTransform>().rect.width;
        Vector2 newSize = new Vector2(width, 300);
        content.GetComponent<GridLayoutGroup>().cellSize = newSize;

    }

    public static void RemoveDoneFileName(string fileName)
    {
        fileUploadStatus.fileNames.Remove(fileName);
        string data = JsonUtility.ToJson(fileUploadStatus);
        File.WriteAllText(Application.persistentDataPath + "/Data/UploadStatus.json", data);
        Debug.Log("FileRemoved");
    }
    public static void AddFileName(string fileName)
    {
        Debug.Log("File Status Added");
        fileUploadStatus.fileNames.Add(fileName);
        string data = JsonUtility.ToJson(fileUploadStatus);
        File.WriteAllText(Application.persistentDataPath + "/Data/UploadStatus.json", data);
    }


}

[Serializable]
public class FileUploadStatus
{
    public List<string> fileNames;
}
