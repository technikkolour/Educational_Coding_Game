using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private Player Player;
    private Puzzle CurrentPuzzle;

    public GameObject ErrorMessage;
    public string ProposedSolution;

    // Start is called before the first frame update;
    void Start()
    {
        Player = FindObjectOfType<Player>();
    }

    // Update is called once per frame;
    void Update()
    {
        CurrentPuzzle = FindObjectOfType<Puzzle>();
    }

    public void SubmitSolution()
    {
        if (CurrentPuzzle.VerifySolution())
        {
            Player.CompletedPuzzle(CurrentPuzzle.GetID(), CurrentPuzzle.GetAttempts());
            CurrentPuzzle.SetCompleted();
        }   
        else
        {
            CurrentPuzzle.SetAttempts();
            ErrorMessage.SetActive(true);
            Invoke("RemoveMessage", 5);
        }
    }

    private void RemoveMessage()
    {
        ErrorMessage.SetActive(false);
    }
}
