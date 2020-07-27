using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopItem : MonoBehaviour
{
    public Text shopNumberText;
    public Text shopNameText;
    public int shopID;
    

    // Start is called before the first frame update
    public void Init(string shopName , string shopNumber , int id )
    {
        shopNumberText.text = shopNumber;
        shopNameText.text = shopName;
        shopID = id;
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Debug.Log(shopID);
        UIManager.instance.ActivateScreen(3); // Shop   inputScreen is at 3
    }
}
