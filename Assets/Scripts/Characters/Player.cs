using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using TMPro;
using System;
using System.Runtime.CompilerServices;
using System.Linq;

public class Player : MonoBehaviour
{
    // Progress Data;
    private List<int> Medals = new List<int>() { 0, 0, 0 };
    private List<bool> CompltetedPuzzles = new List<bool>( Enumerable.Repeat(false, 10) );
    private List<bool> JournalEntriesFound = new List<bool>( Enumerable.Repeat(false, 15) );
    private List<Item> Inventory = new List<Item>(5);

    private UnityEngine.Vector2 MovementDirection;
    private Rigidbody2D RBComponent;
    private Collider2D CollidingObject;
    private bool inInteraction = false;
    private string prompt;

    public TMP_Text PromptText;
    public GameObject PromptObject;

    // Start is called before the first frame update
    void Start()
    {
        RBComponent = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Includes both WASD and arrow key inputs;
        MovementDirection.x = Input.GetAxisRaw("Horizontal");
        MovementDirection.y = Input.GetAxisRaw("Vertical");

        // The E key is reserved for interacting;
        if (Input.GetKeyDown(KeyCode.E) && CollidingObject != null) Interact();
    }

    // Called a set number or times, not depenent on framerate;
    void FixedUpdate()
    {
        float speed = 7;
        RBComponent.MovePosition(RBComponent.position + MovementDirection * speed * Time.fixedDeltaTime);
    }

    // Defines the behaviour of the player character when interacting with various objects and NPCs;
    public void Interact() 
    {
        switch (inInteraction)
        {
            case false:
                inInteraction = true;
                PromptText.text = prompt;
                PromptObject.SetActive(true);
                Time.timeScale = 0.0f;
                break;
            case true:
                inInteraction = false;
                PromptObject.SetActive(false);
                Time.timeScale = 1.0f;
                break;
        }
        
    }

    // The expected behaviour of the player character when colliding with a trigger, such as an NPC's collision box;
    void OnTriggerStay2D(Collider2D col)
    {
        CollidingObject = col;
        string CollidingObjectType = CollidingObject.name.Split("_")[1];


        prompt = CollidingObjectType switch
        {
            "Message" => "Hi! You must be new here!",
            _ => "Hmm... Nothing to see here!",
        };
    }

    // Reset the values once the player is no longer colliding with a trigger;
    void OnTriggerExit2D(Collider2D collision)
    {
        CollidingObject = null;
        prompt = "";
    }    

    // Inventory interaction functions;
    public void PickUpItem(Item Item)
    {
        Inventory[Item.ItemID-1] = Item;
    }
    public void UseItem(int ItemID)
    {
        Inventory.RemoveAt(ItemID);
    }

    // Journal update function;
    public void FoundEntry(int EntryID)
    {
        JournalEntriesFound[EntryID - 1] = true;
    }
}
