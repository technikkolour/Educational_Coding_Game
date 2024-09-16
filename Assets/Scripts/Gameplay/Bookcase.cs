using UnityEngine;

public class Bookcase : MonoBehaviour
{
    public string Contents;
    public int Index;

    private GameManager GameManager;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        // Display the contents;


        // Add the corresponding entry to the journal;
        GameManager.FoundEntry(Index);
    }
}
