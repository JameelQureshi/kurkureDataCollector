using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

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

    public void CreateCurrentDayShopInfo(ShopData.ShopsInfo shopsInfo)
    {
        DayData dayData = new DayData();
        dayData.M_Id = LoginManager.UserID;
        dayData.day = shopsInfo.data.shops[0].user_day;
        dayData.checkIn = DateTime.UtcNow.ToString();
        dayData.checkOut = DateTime.UtcNow.ToString();
        dayData.shops = new List<Shop>();
        for (int i = 0; i < shopsInfo.data.shops.Count; i++)
        {
            Shop shop = new Shop();
            shop.id = shopsInfo.data.shops[i].id;
            shop.SS_Id = 2; // still pending from server
            shop.shop_Name = shopsInfo.data.shops[i].name;
            shop.pic_Name_1 = "";
            shop.pic_Name_2 = "";
            shop.pic_Name_3 = "";
            shop.pic_Name_4 = "";
            shop.contact_Number = "";
            shop.location = "";
            shop.checkIn = DateTime.UtcNow.ToString();
            shop.checkOut = DateTime.UtcNow.ToString();
            shop.sku = new List<SkuData>();
            for (int j = 0; j < shopsInfo.data.sku.Count; j++)
            {
                SkuData sku = new SkuData
                {
                    id = shopsInfo.data.sku[j].id,
                    count = 0
                };

                shop.sku.Add(sku);
            }
            dayData.shops.Add(shop);
        }




        string data = JsonUtility.ToJson(dayData);
        File.WriteAllText(Application.persistentDataPath + "/Data/DayInfo.json", data);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    [System.Serializable]
    public class DayData
    {
        public int M_Id; // Merchandise ID
        public List<Shop> shops;
        public int day;
        public string checkIn;
        public string checkOut;
    }

    [System.Serializable]
    public class Shop
    {
        public int id; /// Shop ID
        public int SS_Id; // Shop Status ID
        public string shop_Name;
        public string pic_Name_1;
        public string pic_Name_2;
        public string pic_Name_3;
        public string pic_Name_4;
        public List<SkuData> sku;
        public string contact_Number;
        public string checkIn;
        public string checkOut;
        public string location;
    }
    [System.Serializable]
    public class SkuData
    {
        public int id;
        public int count;
    }

}
