using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    private Vector2 MovementDirection;
    private Rigidbody2D RBComponent;
    private Collider2D CollidingObject;
    private bool InInteraction = false;

    // Managers;
    private GameManager GameManager;
    private DialogueManager DialogueManager;

    public TMP_Text PromptText;
    public PuzzleSpawner InteractingWith;

    // Start is called before the first frame update
    void Start()
    {
        RBComponent = GetComponent<Rigidbody2D>();

        GameManager = FindObjectOfType<GameManager>();
        DialogueManager = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Includes both WASD and arrow key inputs;
        if (!InInteraction)
        {
            MovementDirection.x = Input.GetAxisRaw("Horizontal");
            MovementDirection.y = Input.GetAxisRaw("Vertical");
        }

        // The E key is reserved for interacting;
        if (Input.GetKeyDown(KeyCode.E) && !InInteraction && CollidingObject != null) Interact();

        // For testing only;
        if (Input.GetKeyDown(KeyCode.K)) GameManager.PickUpItem(0);
    }

    // Called a set number or times, not depenent on framerate;
    void FixedUpdate()
    {
        float speed = 7;

        if(!InInteraction)
            RBComponent.MovePosition(RBComponent.position + speed * Time.fixedDeltaTime * MovementDirection);
    }

    private string LastNPC = "";

    // Defines the behaviour of the player character when interacting with various objects and NPCs;
    public void Interact() 
    {
        List<string> CollidingObjectName = new(CollidingObject.name.Split("_"));
        string CollidingObjectType = CollidingObjectName[^1];

        InInteraction = true;

        if (CollidingObjectType == "Puzzle")
        {
            PuzzleSpawner Spawner = CollidingObject.GetComponent<PuzzleSpawner>();

            if (CollidingObject.GetComponent<NPC>() != null)
            {
                // Get the character's name and the stage of the dialogue;
                string NPCName = CollidingObject.GetComponent<NPC>().Name;
                int NPCDialoguePhase = CollidingObject.GetComponent<NPC>().DialoguePhase;
                int EntryID = CollidingObject.GetComponent<NPC>().JournalEntryID;

                // If the dialogue has not completed or the spawner no longer works, display the dialogue;
                if (!DialogueManager.IsDialogueComplete || !Spawner.IsActive() || LastNPC != NPCName)
                {
                    DialogueManager.StartDialogue(DialogueManager.GetDialogueLines(NPCName, NPCDialoguePhase));
                    LastNPC = NPCName;
                    if (EntryID != -1)
                        GameManager.FoundEntry(EntryID + 1);
                }

                // If the dialogue has concluded, spawn the puzzle;
                if (DialogueManager.IsDialogueComplete && Spawner.IsActive() && LastNPC == NPCName)
                    InteractingWith.Spawn();
            }
            else if (Spawner != null && Spawner.IsActive())
                InteractingWith.Spawn();
            else
            {
                DialogueManager.IsDialogueComplete = false;
                InInteraction = false;
            }
        }
        else if (CollidingObjectType == "Message")
        {
            if (CollidingObject.GetComponent<NPC>() != null)
            {
                // Get the character's name and the stage of the dialogue;
                string NPCName = CollidingObject.GetComponent<NPC>().Name;
                int NPCDialoguePhase = CollidingObject.GetComponent<NPC>().DialoguePhase;
                int EntryID = CollidingObject.GetComponent<NPC>().JournalEntryID;

                if (DialogueManager.IsDialogueComplete && NPCName == "William")
                    GameManager.EndGame();
                else
                {
                    // Get the corresponding dialogue lines and begin the dialogue;
                    DialogueManager.StartDialogue(DialogueManager.GetDialogueLines(NPCName, NPCDialoguePhase));
                    LastNPC = NPCName;
                    if (EntryID != -1)
                        GameManager.FoundEntry(EntryID + 1);
                }

            }
            else if (CollidingObject.GetComponent<Bookcase>() != null)
            {
                // Ge the bookcase the player is interacting with;
                Bookcase InteractingBookcase = CollidingObject.GetComponent<Bookcase>();
                // Start the dialogue;
                InteractingBookcase.Interact();
            }
            else
            {
                DialogueManager.IsDialogueComplete = false;
                InInteraction = false;
            }
        }
        else
        {
            DialogueManager.IsDialogueComplete = false;
            InInteraction = false;
        }
    }

    public void FinishedInteraction()
    {
        InInteraction = false;
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
                if (!GameManager.IsDefeated(int.Parse(CollidingObjectName[^2]) - 1))
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
    }    
}
