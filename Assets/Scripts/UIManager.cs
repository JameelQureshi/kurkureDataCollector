using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject[] screens;

    public static UIManager instance;

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
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject s in screens)
        {
            s.SetActive(false);
        }

#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
        #endif
        CheackLogin();
    }

    public void CheackLogin()
    {
        if (LoginManager.LoginSuccess == "success")
        {
            ActivateScreen(1);  // menu screen is at index 1 
        }
        else
        {
            ActivateScreen(0);
        }
    }


    public void ActivateScreen(int index)
    {
        foreach (GameObject s in screens)
        {
            s.SetActive(false);
        }
        screens[index].SetActive(true);
    }



    public void OpenCamera(int currentImage)
    {
        Screenshot.CurrentImage = currentImage;
        SceneManager.LoadScene(1);
    }

}
