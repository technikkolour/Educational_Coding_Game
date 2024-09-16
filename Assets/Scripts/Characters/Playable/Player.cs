using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    private bool InInteraction = false;
    private string prompt;

    // Managers;
    private DataManager DataManager;
    private GameManager GameManager;

    public TMP_Text PromptText;
    public GameObject DialogueUI;
    public PuzzleSpawner InteractingWith;

    // Start is called before the first frame update
    void Start()
    {
        RBComponent = GetComponent<Rigidbody2D>();

        DataManager = FindObjectOfType<DataManager>();
        GameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Includes both WASD and arrow key inputs;
        MovementDirection.x = Input.GetAxisRaw("Horizontal");
        MovementDirection.y = Input.GetAxisRaw("Vertical");

        // The E key is reserved for interacting;
        if (Input.GetKeyDown(KeyCode.E) && CollidingObject != null) Interact();

        // For testing only;
        //if (Input.GetKeyDown(KeyCode.K)) PickUpItem(0);
    }

    // Called a set number or times, not depenent on framerate;
    void FixedUpdate()
    {
        float speed = 7;
        RBComponent.MovePosition(RBComponent.position + speed * Time.fixedDeltaTime * MovementDirection);
    }

    // Defines the behaviour of the player character when interacting with various objects and NPCs;
    public void Interact() 
    {
        switch (InInteraction)
        {
            case false:
                InInteraction = true;
                PromptText.text = prompt;

                List<string> CollidingObjectName = new(CollidingObject.name.Split("_"));
                string CollidingObjectType = CollidingObjectName[^1];
                if (CollidingObjectType == "Puzzle")
                    InteractingWith.Spawn();
                else if (CollidingObjectType == "Message") DialogueUI.SetActive(true);
                break;
            case true:
                InInteraction = false;
                DialogueUI.SetActive(false);
                break;
        }
    }

    // The expected behaviour of the player character when colliding with a trigger, such as an NPC's collision box;
    void OnTriggerEnter2D(Collider2D col)
    {
        CollidingObject = col;
        List<string> CollidingObjectName = new(CollidingObject.name.Split("_"));
        string CollidingObjectType = CollidingObjectName[^1];

        switch (CollidingObjectType)
        {
            case "Message":
                prompt = "Hi! You must be new here!";
                break;
            case "AcademyEntry":
                GameManager.EnterAcademy();
                break;
            case "WarehouseEntry":
                GameManager.EnterWarehouse();
                break;
            case "LevelExit":
                GameManager.ReturnToCity();
                break;
            case "BossBattleTrigger":
                GameManager.EnterBattle();
                break;
            case "Puzzle":
                InteractingWith = CollidingObject.gameObject.GetComponent<PuzzleSpawner>();
                break;
        }
    }

    // Reset the values once the player is no longer colliding with a trigger;
    void OnTriggerExit2D(Collider2D collision)
    {
        CollidingObject = null;
        InteractingWith = null;
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


    // Medals functions;
    public List<int> GetMedals()
    {
        return Medals;
    }


    // CurrentInventory interaction functions;
    public void PickUpItem(int ItemID)
    {
        Item Item = DataManager.ReturnItemForIndex(ItemID);
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

        if (attempts <= 3) Medals[attempts - 1] += 1;
    }
}
