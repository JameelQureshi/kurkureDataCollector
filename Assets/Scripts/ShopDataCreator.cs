using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ShopData {
    public class ShopDataCreator : MonoBehaviour
    {

        public GameObject prefab;
        public GameObject canvas;
        public ShopsInfo shopsInfo;
        public static ShopDataCreator instance;
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
                }

            }

        }

        public void Populate()
        {
            GameObject item; // Create GameObject instance


            try
            {
                for (int i = 0; i < shopsInfo.data.shops.Count ; i++)
                {
                    item = Instantiate(prefab, transform);
                    item.GetComponent<ShopItem>().Init(shopsInfo.data.shops[i].name, (i+1).ToString(), shopsInfo.data.shops[i].id);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            float width = canvas.GetComponent<RectTransform>().rect.width;
            Vector2 newSize = new Vector2(width, 300);
            GetComponent<GridLayoutGroup>().cellSize = newSize;

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
}


