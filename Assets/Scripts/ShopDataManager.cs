using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ShopData {
    public class ShopDataManager : MonoBehaviour
    {


        public ShopsInfo shopsInfo;
        public static ShopDataManager instance;
        const string getShopApi = "http://shopanalytica.com/public/api/getShopsByUser";

        public static string CurrentDayShopInfo
        {
            set
            {
                PlayerPrefs.SetString("CurrentDayShopInfo", value);
            }
            get
            {
                return PlayerPrefs.GetString("CurrentDayShopInfo", "Empty");
            }
        }

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
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                //CreateShopStatusFile();
                //LoginManager.LoginSuccess = "success";
                //LoginManager.UserID = 1;
                CurrentDayShopInfo = "UnLoaded";
            }
        }

        public void StartYourDay()
        {
            if (CurrentDayShopInfo == "Loaded")
            {
                UIManager.instance.ActivateScreen(2);

                if (ShopDataCreator.instance.transform.childCount==0)
                {
                    ShopDataCreator.instance.Populate();
                }

            }
            else
            {
                StartCoroutine(GetShopData());
            }
        }


        IEnumerator GetShopData()
        {

            WWWForm form = new WWWForm();

            UnityWebRequest webRequest = UnityWebRequest.Get(getShopApi+"/"+LoginManager.UserID);

            webRequest.SendWebRequest();

            while (!webRequest.isDone)
            {
                yield return null;

                // Progress is always set to 1 on android
                // Debug.LogFormat("Progress: {0}", webRequest.uploadProgress);
            }


            if (webRequest.isHttpError || webRequest.isNetworkError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                Debug.Log("Request Done!:" + webRequest.downloadHandler.text);  
                shopsInfo = JsonUtility.FromJson<ShopsInfo>(webRequest.downloadHandler.text);
                if (shopsInfo.success)
                {
                    DataManager.instance.CreateCurrentDayShopInfo(shopsInfo);
                    CreateShopStatusFile();
                }

            }

        }

        void CreateShopStatusFile()
        {   
            ShopStatus shopStatus = new ShopStatus();
            shopStatus.id = new List<int>();
            shopStatus.status = new List<string>();
            shopStatus.image1Status = new List<string>();
            shopStatus.image2Status = new List<string>();
            shopStatus.image3Status = new List<string>();
            shopStatus.image4Status = new List<string>();
            shopStatus.address = new List<string>();
            shopStatus.sku = new List<string>();
            shopStatus.uploadStatus = new List<string>();

            for (int i =0 ; i<shopsInfo.data.shops.Count;i++ )
            {

                shopStatus.id.Add(shopsInfo.data.shops[i].id);
                shopStatus.status.Add("Pending");
                shopStatus.image1Status.Add("Pending");
                shopStatus.image2Status.Add("Pending");
                shopStatus.image3Status.Add("Pending");
                shopStatus.image4Status.Add("Pending");
                shopStatus.address.Add(shopsInfo.data.shops[i].address);
                shopStatus.uploadStatus.Add("Pending");
            }
            for (int i = 0; i < shopsInfo.data.sku.Count; i++)
            {
                shopStatus.sku.Add(shopsInfo.data.sku[i].name);
            }

            string data = JsonUtility.ToJson(shopStatus);
            File.WriteAllText(Application.persistentDataPath + "/Data/ShopStatus.json", data);

            /// Load Local Json Data 
            ShopDataCreator.CreateShopList();
            UIManager.instance.ActivateScreen(2);
            ShopDataCreator.instance.Populate();

        }




    }

    [System.Serializable]
    public class Sku
    {
        public int id;
        public string name;
    }
    [System.Serializable]
    public class Shop
    {
        public int id;
        public string name;
        public string address;
        public int user_day;

    }
    [System.Serializable]
    public class Data
    {
        public List<Sku> sku;
        public List<Shop> shops;

    }
    [System.Serializable]
    public class ShopsInfo
    {
        public bool success;
        public Data data;
    }

    [System.Serializable]
    public class ShopStatus
    {
        public List<int> id;
        public List<string> status;
        public List<string> image1Status;
        public List<string> image2Status;
        public List<string> image3Status;
        public List<string> image4Status;
        public List<string> address;
        public List<string> sku;
        public List<string> uploadStatus;

    }
}


