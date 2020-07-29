using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ShopData {
    public class ShopDataCreator : MonoBehaviour
    {

        public GameObject prefab;
        public GameObject canvas;
        public Text dayText;
        public InputField searchBar;
        public static ShopDataCreator instance;
        public static DataManager.DayData dayData;
        public static ShopStatus shopStatus;

        public  List<GameObject> shopList;

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



        public static void CreateShopList()
        {
            if (ShopDataManager.CurrentDayShopInfo == "Loaded")
            {
                string path = Application.persistentDataPath + "/Data/DayInfo.json";
                string contents = File.ReadAllText(path);
                dayData = JsonUtility.FromJson<DataManager.DayData>(contents);

                string path1 = Application.persistentDataPath + "/Data/ShopStatus.json";
                string contents1 = File.ReadAllText(path1);
                shopStatus = JsonUtility.FromJson<ShopStatus>(contents1);

             
            }

        }

        public void OnSearchValueChanged()
        {
            string searchQuery = searchBar.text;
            searchQuery.ToLower();

            if (searchBar.text != "")
            {
                foreach (GameObject item in shopList)
                {
                    string shopNameString = item.GetComponent<ShopItem>().shopNameText.text.ToLower();
                    if (shopNameString.Contains(searchBar.text))
                    {
                        item.gameObject.SetActive(true);
                    }
                    else
                    {
                        item.gameObject.SetActive(false);
                    }
                }

            }
            else
            {
                foreach (GameObject item in shopList)
                {
                   item.gameObject.SetActive(true);
                }

            }
        }

        public void Populate()
        {
            GameObject item; // Create GameObject instance
            dayText.text = "Day " + LoginManager.CurrentDay;
            shopList = new List<GameObject>();

            try
            {
                for (int i = 0; i < dayData.shops.Count; i++)
                {
                    item = Instantiate(prefab, transform);
                    item.GetComponent<ShopItem>().Init(dayData.shops[i].shop_Name, (i + 1).ToString(), dayData.shops[i].id, CheackShopStatus(dayData.shops[i].id));
                    shopList.Add(item);
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

        string CheackShopStatus(int id)
        {
            for (int i = 0; i < shopStatus.id.Count; i++)
            {
                if (id == shopStatus.id[i])
                {
                    return shopStatus.status[i];
                }
            }
            return "Pending";
        }

        public static void SaveImageStatus(int id,int currentImage,string imageName)
        {
            for (int i = 0; i < shopStatus.id.Count; i++)
            {
                if (id == shopStatus.id[i])
                {
                    switch (currentImage)
                    {
                        case 1:
                            shopStatus.image1Status[i] = "Done";
                            break;
                        case 2:
                            shopStatus.image2Status[i] = "Done";
                            break;
                        case 3:
                            shopStatus.image3Status[i] = "Done";
                            break;
                        case 4:
                            shopStatus.image4Status[i] = "Done";
                            break;
                    }
                }
            }

            for (int i = 0; i < dayData.shops.Count; i++)
            {
                if (id == dayData.shops[i].id)
                {
                    switch (currentImage)
                    {
                        case 1:
                            dayData.shops[i].pic_Name_1 = imageName;
                            break;
                        case 2:
                            dayData.shops[i].pic_Name_2 = imageName;
                            break;
                        case 3:
                            dayData.shops[i].pic_Name_3 = imageName;
                            break;
                        case 4:
                            dayData.shops[i].pic_Name_4 = imageName;
                            break;
                    }
                }
            }
            SaveCurrentProgress();
        }

        public static void SaveCurrentProgress()
        {
            string data = JsonUtility.ToJson(dayData);
            File.WriteAllText(Application.persistentDataPath + "/Data/DayInfo.json", data);

            string data1 = JsonUtility.ToJson(shopStatus);
            File.WriteAllText(Application.persistentDataPath + "/Data/ShopStatus.json", data1);
        }


    }
}





