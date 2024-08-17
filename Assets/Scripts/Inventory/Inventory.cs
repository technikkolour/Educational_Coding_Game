using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public TMP_Text Gold, Silver, Bronze;

    private Player Player;
    private DataManager DataManager;

    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<Player>();
        DataManager = FindObjectOfType<DataManager>();
    }

    // Update is called once per frame
    void Update()
    {   
        List<Item> CurrentInventory = Player.GetInventory();
        List<int> Medals = Player.GetMedals();

        Gold.text = Medals[0].ToString();
        Silver.text = Medals[1].ToString();
        Bronze.text = Medals[2].ToString();

        for(int i = 0; i< CurrentInventory.Count; i++)
        {
            GameObject.Find("InventoryEntry_0" + (i + 1)).GetComponentInChildren<TMP_Text>().text = CurrentInventory[i].Name;
            GameObject.Find("InventoryEntry_0" + (i + 1)).GetComponentInChildren<Image>().sprite = CurrentInventory[i].Sprite;
        }
            
    }

    public Item ReturnItemForIndex(int Index)
    {
        return DataManager.Items[Index];
    }
}
