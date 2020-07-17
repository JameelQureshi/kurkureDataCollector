using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject loginScreen;
    public GameObject menuScreen;

    // Start is called before the first frame update
    void Start()
    {
        loginScreen.SetActive(true);
        menuScreen.SetActive(false);

    #if UNITY_ANDROID
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam)) {
            Application.RequestUserAuthorization(UserAuthorization.WebCam);
        }
    #endif

    }

    public void Login()
    {
        loginScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    public void StartDay()
    {
        SceneManager.LoadScene(1);
    }

}
