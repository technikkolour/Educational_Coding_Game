using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSpawner : MonoBehaviour
{
    public string PuzzleType;
    public int PuzzleID;
    public int Attempts;

    private PuzzleManager PuzzleManager;

    // Start is called before the first frame update
    void Start()
    {
        PuzzleManager = FindObjectOfType<PuzzleManager>();
        Attempts = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        PuzzleManager.SpawnPuzzle(PuzzleType, PuzzleID);

        Puzzle Puzzle = GameManager.FindObjectOfType<Puzzle>();

        if (Puzzle != null)
            Puzzle.SetAttempts(Attempts);
    }
}
