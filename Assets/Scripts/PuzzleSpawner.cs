using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSpawner : MonoBehaviour
{
    public string PuzzleType;
    public int PuzzleID;

    private PuzzleManager PuzzleManager;

    // Start is called before the first frame update
    void Start()
    {
        PuzzleManager = FindObjectOfType<PuzzleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
