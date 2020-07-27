using System.Collections;
using System.Collections.Generic;
using ShopData;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopFormManager : MonoBehaviour
{
    public static int CurrentID
    {
        set
        {
            PlayerPrefs.SetInt("CurrentID", value);
        }
        get
        {
            return PlayerPrefs.GetInt("CurrentID");
        }
    }

    public static ShopFormManager instance;

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

    public Button image1;
    public Button image2;
    public Button image3;
    public Button image4;
    public Text address;
    public InputField shopName;
    public Dropdown dropdownShopStatus;

    public InputField sku1;
    public InputField sku2;
    public InputField sku3;
    public InputField sku4;
    public InputField sku5;
    public InputField sku6;

    public Text[] skuText;



    // Update is called once per frame
    public void UpdateForm()
    {
        CheackCaptureImageStatus(CurrentID);
        UpdateAddress(CurrentID);
    }

    public void CheackCaptureImageStatus(int id)
    {
        for (int i=0;i<ShopDataCreator.shopStatus.id.Count;i++)
        {
            if (id== ShopDataCreator.shopStatus.id[i])
            {
                image1.interactable &= ShopDataCreator.shopStatus.image1Status[i] != "Done";
                image2.interactable &= ShopDataCreator.shopStatus.image2Status[i] != "Done";
                image3.interactable &= ShopDataCreator.shopStatus.image3Status[i] != "Done";
                image4.interactable &= ShopDataCreator.shopStatus.image4Status[i] != "Done";
                address.text = ShopDataCreator.shopStatus.address[i];
            }
        }
        for (int i = 0; i < ShopDataCreator.shopStatus.sku.Count; i++)
        {
            skuText[i].text = ShopDataCreator.shopStatus.sku[i];
        }
    }

    public void UpdateAddress(int id)
    {
        for (int i = 0; i < ShopDataCreator.dayData.shops.Count; i++)
        {
            if (id == ShopDataCreator.dayData.shops[i].id)
            {
                shopName.text = ShopDataCreator.dayData.shops[i].shop_Name;
            }
        }

    }

    public void SubmitData()
    {



        for (int i = 0; i < ShopDataCreator.dayData.shops.Count; i++)
        {
            if (CurrentID == ShopDataCreator.dayData.shops[i].id)
            {
                ShopDataCreator.dayData.shops[i].SS_Id = dropdownShopStatus.value;
                if (shopName.text!= "" && sku1.text!= "" && sku2.text!="" && sku3.text!= ""
                && sku4.text!="" && sku5.text!="" && sku6.text!= "" )
                {
                    ShopDataCreator.dayData.shops[i].shop_Name = shopName.text;
                    ShopDataCreator.dayData.shops[i].sku[0].count = int.Parse(sku1.text);
                    ShopDataCreator.dayData.shops[i].sku[1].count = int.Parse(sku2.text);
                    ShopDataCreator.dayData.shops[i].sku[2].count = int.Parse(sku3.text);
                    ShopDataCreator.dayData.shops[i].sku[3].count = int.Parse(sku4.text);
                    ShopDataCreator.dayData.shops[i].sku[4].count = int.Parse(sku5.text);
                    ShopDataCreator.dayData.shops[i].sku[5].count = int.Parse(sku6.text);
                }
                else
                {
                    Debug.Log("Fill All the inputs");
                    return;
                }
            }

        }
        for (int i = 0; i < ShopDataCreator.shopStatus.id.Count; i++)
        {
            if (CurrentID == ShopDataCreator.shopStatus.id[i])
            {
                if (ShopDataCreator.shopStatus.image1Status[i] == "Done" &&
                    ShopDataCreator.shopStatus.image2Status[i] == "Done" &&
                    ShopDataCreator.shopStatus.image3Status[i] == "Done" &&
                    ShopDataCreator.shopStatus.image4Status[i] == "Done")
                {
                    Debug.Log("All Imgages Done");
                    ShopDataCreator.shopStatus.status[i] = "Done";
                    ShopDataCreator.SaveCurrentProgress();

                    SceneManager.LoadScene(0);
                }
                else
                {
                    Debug.Log("Imgages Need to be Done");
                    return;
                }

            }
        }
    }

}
