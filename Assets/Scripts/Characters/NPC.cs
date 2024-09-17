using UnityEngine;

public class NPC : MonoBehaviour
{
    public string Name;

    // Functionality properties;
    public bool HasPuzzle;
    public bool AddsJournalEntry;
    public int DialoguePhase;

    // Start is called before the first frame update
    void Start()
    {
        DialoguePhase = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
