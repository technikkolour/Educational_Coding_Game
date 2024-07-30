using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private Player Player;
    private Puzzle CurrentPuzzle;

    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        CurrentPuzzle = FindObjectOfType<Puzzle>();
    }

    void SubmitSolution()
    {
        bool Solution = CurrentPuzzle.VerifySolution();
        if (Solution)
        {
            Player.CompletedPuzzle(CurrentPuzzle.GetID(), CurrentPuzzle.GetAttempts());
            CurrentPuzzle.SetCompleted();
        }   
        else
            CurrentPuzzle.SetAttempts();
    }
}
