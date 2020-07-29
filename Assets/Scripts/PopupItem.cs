using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PopupItem : MonoBehaviour
{
    public Text message;

    public void Init(string msg)
    {
        message.text = msg;
    }
    public void Close()
    {
        Destroy(gameObject);
    }
}
