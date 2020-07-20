using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        string data1;

        data1 = "jameelqureshi420";
        data1 = data1 + "0721202023012";
        data1 = data1 + "0721202023455";

        for (int i = 0; i < 60; i++)
        {
            data1 = data1 + "234";
            data1 = data1 + "2";
            data1 = data1 + "Ali Store Kraimabad";
            data1 = data1 + "2";
            data1 = data1 + "0721202023dfdfddff0123";
            data1 = data1 + "0721202df02df345dffdf545";
            data1 = data1 + "0721202023df01ddffdf25";
            data1 = data1 + "07212020234dfdf5566f";
            data1 = data1 + "07212020230dffdf12dfdf";
            data1 = data1 + "072120202df3df4dfd55dfdf";
            data1 = data1 + "072120202dffdvdf3012";
            data1 = data1 + "0721202dfvdffv023455";
            data1 = data1 + "07212020vfvfffvv23012";
            data1 = data1 + "07212020234fddfvdfv5fvf5";
            data1 = data1 + "07212020dfvdfvfdv30v12";
            data1 = data1 + "072120202dfdfvdfvdf3455";
            data1 = data1 + "07212020dffvdfv23012";
            data1 = data1 + "07212020dfvdfvdf23455";
        }

        data1 = data1 + "/n YoYo Ending";
        PlayerPrefs.SetString("data1", data1);

        File.WriteAllText(Application.persistentDataPath + "/PotionData.json", PlayerPrefs.GetString("data1"));
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    [System.Serializable]
    public class DayData
    {
        public string M_Id; // Merchandise ID
        public List<Shop> shops;
        public string checkIn;
        public string checkOut;
    }

    [System.Serializable]
    public class Shop
    {
        public string S_Id; /// Shop ID
        public string SS_Id; // Shop Status ID
        public string visit_Id;
        public string shop_Name;
        public string pic_Name_1;
        public string pic_Name_2;
        public string pic_Name_3;
        public string pic_Name_4;
        public string sku1;
        public string sku2;
        public string sku3;
        public string sku4;
        public string sku5;
        public string sku6;
        public string contact_Number;
        public string checkIn;
        public string checkOut;
        public string location;
    }

         
}
