using System.Collections.Generic;
using UnityEngine;

public class Bookcase : MonoBehaviour
{
    public string Contents;
    public int BookcaseID;

    private GameManager GameManager;
    private DialogueManager DialogueManager;

    // Start is called before the first frame update;
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
        DialogueManager = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame;
    void Update()
    {
        
    }

    public void Interact()
    {
        // Get the contents;
        Contents = DialogueManager.GetBookcaseContents(BookcaseID);

        // Add the corresponding entry to the journal;
        if (BookcaseID != 999)
            GameManager.FoundEntry(BookcaseID + 1);        
        
        // Display the contents;
        List<string> ContentsList = new()
        {
            Contents
        };

        DialogueManager.StartDialogue(ContentsList);
    }
}
