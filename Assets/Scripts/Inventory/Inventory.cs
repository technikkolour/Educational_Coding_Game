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
    public List<Sprite> Sprites = new();

    private Player Player;
    private List<Item> Items = new();


    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<Player>();

        Items.Add(Item.CreateInstance("Robot Licence", Sprites[0], "", 0));
        Items.Add(Item.CreateInstance("Rusty Circuit Board", Sprites[1], "", 1));
        Items.Add(Item.CreateInstance("", Sprites[2], "", 2));
        Items.Add(Item.CreateInstance("", Sprites[3], "", 3));
        Items.Add(Item.CreateInstance("", Sprites[4], "", 4));
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
        return Items[Index];
    }
}
