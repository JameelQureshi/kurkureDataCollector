using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using ShopData;
using UnityEngine.SceneManagement;

public class Screenshot : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip imageCaptureAudio;
    public RawImage previewImage;
    public GameObject captureButton;
    public GameObject closeButton;
    private Texture2D texture;
   
    public static int CurrentImage
    {
        set
        {
            PlayerPrefs.SetInt("CurrentImage", value);
        }
        get
        {
            return PlayerPrefs.GetInt("CurrentImage");
        }
    }

    public void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void TakeScreenShot() 
    {
        captureButton.SetActive(false);
        closeButton.SetActive(false);
        audioSource.PlayOneShot(imageCaptureAudio);
        StartCoroutine(ScreenshotEncode());
    }

    IEnumerator ScreenshotEncode()
    {

        yield return new WaitForEndOfFrame();
        // create a texture to pass to encoding
        texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24,false);
        // put buffer into texture
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        // split the process up--ReadPixels() and the GetPixels() call inside of the encoder are both pretty heavy
        yield return 0;

        previewImage.gameObject.SetActive(true);
        previewImage.texture = texture;

        //Save Image to path 
        //SaveImageToPath(texture);

    }

    public void SaveImageToPath()
    {
        string fileName = "Image_" +LoginManager.UserID+ "_"+LoginManager.CurrentDay + "_" + ShopFormManager.CurrentID + "_" + CurrentImage ;
        string filePath = Path.Combine(Application.persistentDataPath+ "/Data", fileName+ ".jpg");
        File.WriteAllBytes(filePath, texture.EncodeToJPG());

        ShopDataCreator.SaveImageStatus(ShopFormManager.CurrentID, CurrentImage,fileName);
        SceneManager.LoadScene(0);
        print(filePath);
    }

    public void TakeAgain()
    {
        previewImage.gameObject.SetActive(false);
        captureButton.SetActive(true);
        closeButton.SetActive(true);
    }

    public void CloseCamera()
    {
        SceneManager.LoadScene(0);
    }





}

