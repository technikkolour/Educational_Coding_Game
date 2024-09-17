using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Player : MonoBehaviour
{
    private Vector2 MovementDirection;
    private Rigidbody2D RBComponent;
    private Collider2D CollidingObject;
    private bool InInteraction = false;

    // Managers;
    private DataManager DataManager;
    private GameManager GameManager;
    private DialogueManager DialogueManager;

    public TMP_Text PromptText;
    public GameObject DialogueUI;
    public PuzzleSpawner InteractingWith;

    // Start is called before the first frame update
    void Start()
    {
        RBComponent = GetComponent<Rigidbody2D>();

        DataManager = FindObjectOfType<DataManager>();
        GameManager = FindObjectOfType<GameManager>();
        DialogueManager = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Includes both WASD and arrow key inputs;
        MovementDirection.x = Input.GetAxisRaw("Horizontal");
        MovementDirection.y = Input.GetAxisRaw("Vertical");

        // The E key is reserved for interacting;
        if (Input.GetKeyDown(KeyCode.E) && !InInteraction && CollidingObject != null) Interact();

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
        List<string> CollidingObjectName = new(CollidingObject.name.Split("_"));
        string CollidingObjectType = CollidingObjectName[^1];

        InInteraction = true;

        if (CollidingObjectType == "Puzzle")
        {
            PuzzleSpawner Spawner = CollidingObject.GetComponent<PuzzleSpawner>();
            
            if (CollidingObject.GetComponent<NPC>() != null)
            {
                // Get the character's name;
                string NPCName = CollidingObject.GetComponent<NPC>().Name;
                int NPCDialoguePhase = CollidingObject.GetComponent<NPC>().DialoguePhase;

                if (!DialogueManager.IsDialogueComplete || !Spawner.IsActive())
                    DialogueManager.StartDialogue(DialogueManager.GetDialogueLines(NPCName, NPCDialoguePhase));

                if (DialogueManager.IsDialogueComplete && Spawner.IsActive())
                    InteractingWith.Spawn();
            }
            else if (Spawner != null && Spawner.IsActive())
                InteractingWith.Spawn();
        }
        else if (CollidingObjectType == "Message")
        {
            if (CollidingObject.GetComponent<NPC>() != null) 
            {
                // Get the character's name;
                string NPCName = CollidingObject.GetComponent<NPC>().Name;
                int NPCDialoguePhase = CollidingObject.GetComponent<NPC>().DialoguePhase;

                // Get the corresponding dialogue lines and begin the dialogue;
                DialogueManager.StartDialogue(DialogueManager.GetDialogueLines(NPCName, NPCDialoguePhase));
            }
            else if (CollidingObject.GetComponent<Bookcase>() != null)
            {
                // Ge the bookcase the player is interacting with;
                Bookcase InteractingBookcase = CollidingObject.GetComponent<Bookcase>();
                // Start the dialogue;
                InteractingBookcase.Interact();
            }
            else InInteraction = false;
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
