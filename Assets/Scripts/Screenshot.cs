using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class Screenshot : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip imageCaptureAudio;
    public RawImage previewImage;
    public GameObject captureButton;
   
    public static int ImageCount
    {
        set
        {
            PlayerPrefs.SetInt("ImageCount", value);
        }
        get
        {
            return PlayerPrefs.GetInt("ImageCount");
        }
    }

    public void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void TakeScreenShot() 
    {
        captureButton.SetActive(false);
        audioSource.PlayOneShot(imageCaptureAudio);
        StartCoroutine(ScreenshotEncode());
    }

    IEnumerator ScreenshotEncode()
    {

        yield return new WaitForEndOfFrame();
        // create a texture to pass to encoding
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24,false);
        // put buffer into texture
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        // split the process up--ReadPixels() and the GetPixels() call inside of the encoder are both pretty heavy
        yield return 0;

        previewImage.gameObject.SetActive(true);
        previewImage.texture = texture;

        //Save Image to path 
        SaveImageToPath(texture);

    }

    private void SaveImageToPath(Texture2D texture)
    {
        ImageCount++;
        string filePath = Path.Combine(Application.persistentDataPath, "KurKure" + ImageCount + ".jpg");
        File.WriteAllBytes(filePath, texture.EncodeToJPG());
        print(filePath);
    }

    public void TakeAgain()
    {
        previewImage.gameObject.SetActive(false);
        captureButton.SetActive(true);
    }







}

