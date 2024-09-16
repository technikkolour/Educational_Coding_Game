using UnityEngine;

public class Item : ScriptableObject
{
    // Properties;
    public string Name;
    public Sprite Sprite;
    public string Description;
    public int ItemID;

    public void Init(string Name, Sprite Sprite, string Description, int id)
    {
        this.Name = Name;
        this.Sprite = Sprite;
        this.Description = Description;
        this.ItemID = id;
    }

    public static Item CreateInstance(string Name, Sprite Sprite, string Description, int id)
    {
        var data = ScriptableObject.CreateInstance<Item>();
        data.Init(Name, Sprite, Description, id);
        return data;
    }
}
