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

        public GameObject prefab;
        public GameObject canvas;
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
            if (Input.GetKeyDown(KeyCode.G))
            {
                StartCoroutine(GetShopData());
            }
        }

        public void StartYourDay()
        {
            if (CurrentDayShopInfo == "Loaded")
            {
                UIManager.instance.ActivateScreen(2);
                ShopDataCreator.instance.CreateShopList();
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

            for (int i =0 ; i<shopsInfo.data.shops.Count;i++ )
            {

                shopStatus.id.Add(shopsInfo.data.shops[i].id);
                shopStatus.status.Add("Pending");

            }

            string data = JsonUtility.ToJson(shopStatus);

            File.WriteAllText(Application.persistentDataPath + "/Data/ShopStatus.json", data);
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
    }
}


