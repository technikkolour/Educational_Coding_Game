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
    private List<int> Medals = new() { 0, 0, 0 };
    private List<bool> CompletedPuzzles = new( Enumerable.Repeat(false, 10) );
    private List<bool> JournalEntriesFound = new( Enumerable.Repeat(false, 11) );
    private List<Item> CurrentInventory = new();

    private UnityEngine.Vector2 MovementDirection;
    private Rigidbody2D RBComponent;
    private Collider2D CollidingObject;
    private bool inInteraction = false;
    private string prompt;

    public TMP_Text PromptText;
    public GameObject DialogueUI;

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

        if (Input.GetKeyDown(KeyCode.K)) PickUpItem(0);
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
                DialogueUI.SetActive(true);
                Time.timeScale = 0.0f;
                break;
            case true:
                inInteraction = false;
                DialogueUI.SetActive(false);
                Time.timeScale = 1.0f;
                break;
        }
        
    }

    // The expected behaviour of the player character when colliding with a trigger, such as an NPC's collision box;
    void OnTriggerStay2D(Collider2D col)
    {
        CollidingObject = col;
        List<string> CollidingObjectName = new(CollidingObject.name.Split("_"));
        string CollidingObjectType = CollidingObjectName[^1];

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


    // Journal functions;
    public List<bool> GetJournalEntriesFound()
    {
        return JournalEntriesFound;
    }
    public void FoundEntry(int EntryID)
    {
        JournalEntriesFound[EntryID - 1] = true;
    }


    public List<int> GetMedals()
    {
        return Medals;
    }


    // CurrentInventory interaction functions;
    public void PickUpItem(int ItemID)
    {
        Inventory Inventory = FindObjectOfType<Inventory>();
        Item Item = Inventory.ReturnItemForIndex(ItemID);
        if (!CurrentInventory.Contains(Item)) CurrentInventory.Add(Item);
    }
    public void UseItem(int ItemID)
    {
        CurrentInventory.RemoveAt(ItemID);
    }
    public List<Item> GetInventory()
    {
        return CurrentInventory;
    }


    // Puzzle completion update function;
    public void CompletedPuzzle(int PuzzleID, int attempts)
    {
        CompletedPuzzles[PuzzleID - 1] = true;

        if (attempts <= 3) Medals[PuzzleID - 1] += 1;
    }
}
