using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerifyPage : MonoBehaviour
{
    public static VerifyPage instance;

    public Text[] skus;
    public Text[] skuValues;
    public Toggle[] cheack;
    public Text address;

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

    public void OpenVerify(Text[] sku, string ad,string[] sku_values)
    {
        for (int i = 0; i<sku.Length;i++)
        {
            skus[i].text = sku[i].text;
            skuValues[i].text = sku_values[i];
        }
        address.text = ad;
    }

    public void Submit()
    {
        foreach (Toggle toggle in cheack)
        {
            if (!toggle.isOn)
            {
                PopupManager.instance.OpenPopup("Please Mark All Options");
                return;
            }
        }
        ShopFormManager.instance.SubmitData();
    }


}
