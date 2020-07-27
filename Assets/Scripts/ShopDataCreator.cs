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
        public static ShopDataCreator instance;

        public DataManager.DayData dayData;
        public ShopStatus shopStatus;


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
       
        public void CreateShopList()
        {
            string path = Application.persistentDataPath + "/Data/DayInfo.json";
            string contents = File.ReadAllText(path);
            dayData = JsonUtility.FromJson<DataManager.DayData>(contents);

            string path1 = Application.persistentDataPath + "/Data/ShopStatus.json";
            string contents1 = File.ReadAllText(path1);
            shopStatus = JsonUtility.FromJson<ShopStatus>(contents1);
            Populate();

        }

      

        public void Populate()
        {
            GameObject item; // Create GameObject instance


            try
            {
                for (int i = 0; i < dayData.shops.Count ; i++)
                {
                    item = Instantiate(prefab, transform);
                    item.GetComponent<ShopItem>().Init(dayData.shops[i].shop_Name, (i+1).ToString(), dayData.shops[i].id,CheackShopStatus(dayData.shops[i].id));
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
            for (int i = 0; i<shopStatus.id.Count; i++)
            {
                if (id == shopStatus.id[i])
                {
                   return shopStatus.status[i];
                }
            }
            return "Pending";
        }







    }


}


