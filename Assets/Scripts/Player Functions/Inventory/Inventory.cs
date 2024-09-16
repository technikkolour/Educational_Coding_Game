using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

        for (int i = 0; i < CurrentInventory.Count; i++)
        {
            GameObject.Find("InventoryEntry_0" + (i + 1)).GetComponentInChildren<TMP_Text>().text = CurrentInventory[i].Name;
            GameObject.Find("InventoryEntry_0" + (i + 1)).GetComponentInChildren<Image>().sprite = CurrentInventory[i].Sprite;
        } 
    }

    // Display the description of the selected item;
    private Item Item;

    public void GetItem(int Index)
    {
        if (Index <= Player.GetInventory().Count - 1)
            Item = Player.GetInventory()[Index];
    }
    public void DisplayDescription()
    {
        // Get the text field for the item description;
        TMP_Text DescriptionField = GameObject.Find("ItemDescription").GetComponent<TMP_Text>();

        // If there is an item in the selected slot, display description, if not the description is empty;
        if (Item != null) DescriptionField.text = Item.Description;
        else DescriptionField.text = "";

        // Reset the item so that emoty inventory slots do not display the descriptions of the previous, filled slot;
        Item = null;
    }
}
