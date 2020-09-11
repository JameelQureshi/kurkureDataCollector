using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{

    public GameObject prefab;
    public GameObject loading;
    private static GameObject m_loading;
    public static PopupManager instance;

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
    public void OpenPopup(string message)
    {
        GameObject popup = Instantiate(prefab);
        popup.GetComponent<PopupItem>().Init(message);
    }

    public void SetLoading(bool status)
    {
        if (status)
        {
            m_loading = Instantiate(loading);
        }
        else
        {
            if (m_loading!=null)
            {
                Destroy(m_loading);
            }
        }
        
    }

}
