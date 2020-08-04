using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        for (int i = 0; i < ShopDataCreator.shopStatus.id.Count; i++)
        {
            if (id == ShopDataCreator.shopStatus.id[i])
            {
                if (ShopDataCreator.shopStatus.image1Status[i] == "Done")
                {
                    image1.interactable = false;
                }
                else
                {
                    image1.interactable = true;
                }

                if (ShopDataCreator.shopStatus.image2Status[i] == "Done")
                {
                    image2.interactable = false;
                }
                else
                {
                    image2.interactable = true;
                }
                if (ShopDataCreator.shopStatus.image3Status[i] == "Done")
                {
                    image3.interactable = false;
                }
                else
                {
                    image3.interactable = true;
                }
                if (ShopDataCreator.shopStatus.image4Status[i] == "Done")
                {
                    image4.interactable = false;
                }
                else
                {
                    image4.interactable = true;
                }

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

    public void OpenVerify(){

        for (int i = 0; i < ShopDataCreator.shopStatus.id.Count; i++)
        {
            if (CurrentID == ShopDataCreator.shopStatus.id[i])
            {
                if (ShopDataCreator.shopStatus.image1Status[i] == "Done" &&
                    ShopDataCreator.shopStatus.image2Status[i] == "Done" &&
                    ShopDataCreator.shopStatus.image3Status[i] == "Done" &&
                    ShopDataCreator.shopStatus.image4Status[i] == "Done")
                {

                }
                else
                {
                    PopupManager.instance.OpenPopup("Please Capture All the Images");
                    return;
                }

            }
        }


        if (shopName.text != "" && sku1.text != "" && sku2.text != "" && sku3.text != ""
            && sku4.text != "" && sku5.text != "" && sku6.text != "")
            {
                UIManager.instance.ActivateScreen(4);
                VerifyPage.instance.OpenVerify(skuText, shopName.text, new string[] {sku1.text,sku2.text,sku3.text,sku4.text,sku5.text,sku6.text});
            }
            else
            {
               
                PopupManager.instance.OpenPopup("Please Fill All the Inputs");
                return;
            }
    }

    public void SubmitData()
    {

        for (int i = 0; i < ShopDataCreator.dayData.shops.Count; i++)
        {
            if (CurrentID == ShopDataCreator.dayData.shops[i].id)
            {
                ShopDataCreator.dayData.shops[i].SS_Id = dropdownShopStatus.value;
                ShopDataCreator.dayData.checkOut = DateTime.UtcNow.ToString();
                if (shopName.text!= "" && sku1.text!= "" && sku2.text!="" && sku3.text!= ""
                && sku4.text!="" && sku5.text!="" && sku6.text!= "" )
                {
                    ShopDataCreator.dayData.shops[i].shop_Name = shopName.text;
                    ShopDataCreator.dayData.shops[i].checkOut = DateTime.UtcNow.ToString();
                    ShopDataCreator.dayData.shops[i].sku[0].count = int.Parse(sku1.text);
                    ShopDataCreator.dayData.shops[i].sku[1].count = int.Parse(sku2.text);
                    ShopDataCreator.dayData.shops[i].sku[2].count = int.Parse(sku3.text);
                    ShopDataCreator.dayData.shops[i].sku[3].count = int.Parse(sku4.text);
                    ShopDataCreator.dayData.shops[i].sku[4].count = int.Parse(sku5.text);
                    ShopDataCreator.dayData.shops[i].sku[5].count = int.Parse(sku6.text);
                }
                else
                {
                    PopupManager.instance.OpenPopup("Please Fill All the Inputs");
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

                    MoveCompletedData();
                }
                else
                {
                    PopupManager.instance.OpenPopup("Please Capture All the Images");
                    return;
                }

            }
        }
    }

    public void MoveCompletedData()
    {
        DataManager.DayData dayData;


        string[] file = Directory.GetFiles(Application.persistentDataPath + "/Completed/");

        Debug.Log(file.Length);
        if (file.Length <= 4)
        {
            Debug.Log("No Previous Data");
            dayData = new DataManager.DayData
            {
                shops = new List<DataManager.Shop>()
            };
        }
        else
        {
            string path = Application.persistentDataPath + "/Completed/DayInfo.json";
            string contents = File.ReadAllText(path);
            dayData = JsonUtility.FromJson<DataManager.DayData>(contents);
        }

        for (int i = 0; i < ShopDataCreator.dayData.shops.Count; i++)
        {
            if (CurrentID == ShopDataCreator.dayData.shops[i].id)
            {
                for (int j = 0; j < ShopDataCreator.shopStatus.id.Count; j++)
                {
                    if (CurrentID == ShopDataCreator.shopStatus.id[j])
                    {
                        if (ShopDataCreator.shopStatus.uploadStatus[j]=="Pending")
                        {
                            dayData.M_Id = ShopDataCreator.dayData.M_Id;
                            dayData.day = ShopDataCreator.dayData.day;
                            dayData.checkIn = ShopDataCreator.dayData.checkIn;
                            dayData.checkOut = ShopDataCreator.dayData.checkOut;
                            dayData.shops.Add(ShopDataCreator.dayData.shops[i]);

                            string tempPath = Application.persistentDataPath + "/Data/";
                            foreach (string filepath in Directory.GetFiles(tempPath, "*.jpg"))
                            {
                               
                                string path = Application.persistentDataPath + "/Data/"+ShopDataCreator.dayData.shops[i].pic_Name_1+".jpg";
                                byte[] contents = File.ReadAllBytes(path);
                                File.WriteAllBytes(Application.persistentDataPath + "/Completed/" + ShopDataCreator.dayData.shops[i].pic_Name_1 + ".jpg", contents);
                                 
                                 path = Application.persistentDataPath + "/Data/" + ShopDataCreator.dayData.shops[i].pic_Name_2 + ".jpg";
                                 contents = File.ReadAllBytes(path);
                                File.WriteAllBytes(Application.persistentDataPath + "/Completed/" + ShopDataCreator.dayData.shops[i].pic_Name_2 + ".jpg", contents);

                                path = Application.persistentDataPath + "/Data/" + ShopDataCreator.dayData.shops[i].pic_Name_3 + ".jpg";
                                 contents = File.ReadAllBytes(path);
                                File.WriteAllBytes(Application.persistentDataPath + "/Completed/" + ShopDataCreator.dayData.shops[i].pic_Name_3 + ".jpg", contents);
                                 
                                 path = Application.persistentDataPath + "/Data/" + ShopDataCreator.dayData.shops[i].pic_Name_4 + ".jpg";
                                 contents = File.ReadAllBytes(path);
                                File.WriteAllBytes(Application.persistentDataPath + "/Completed/" + ShopDataCreator.dayData.shops[i].pic_Name_4 + ".jpg", contents);

                            }

                            string dataToSave = JsonUtility.ToJson(dayData);
                            File.WriteAllText(Application.persistentDataPath + "/Completed/DayInfo.json", dataToSave);
                            ShopDataCreator.shopStatus.uploadStatus[j] = "Ready";
                            UIManager.isShopSelected = false;
                            SceneManager.LoadScene(0);

                        }
                    }
                }

            }

        }


    }

}
