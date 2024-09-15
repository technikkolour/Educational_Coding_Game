using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bookcase : MonoBehaviour
{
    public string Contents;
    public int Index;

    private Player Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        // Display the contents;


        // Add the corresponding entry to the journal;
        Player.FoundEntry(Index);
    }
}
