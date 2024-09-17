using UnityEngine;

public class Bookcase : MonoBehaviour
{
    public string Contents;
    public int BookcaseID = 0;

    private GameManager GameManager;
    private DialogueManager DialogueManager;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
        DialogueManager = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        // Get the contents;
        Contents = DialogueManager.GetBookcaseContents(BookcaseID);

        // Display the contents;


        // Add the corresponding entry to the journal;
        GameManager.FoundEntry(BookcaseID);
    }
}
