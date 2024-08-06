using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private Player Player;
    private Puzzle CurrentPuzzle;

    public GameObject ErrorMessage;
    public string ProposedSolution;

    // The sets are structured in the order: Prompt, Options (or Lines of Code), Solution;
    private Dictionary<int, List<List<string>>> MultipleChoice = new(){
        { 1, new(){ new(){"Would the number 3 be considered an integer (int)?"}, new(){"Yes", "No"}, new(){"Yes"} } },
        { 2, new(){ new(){""}, new(){""}, new(){""} } } } ;
    private Dictionary<int, List<List<string>>> Quiz = new(){
        { 0, new(){ new(){""}, new(){""}, new(){""} } },
        { 1, new(){ new(){""}, new(){""}, new(){""} } } };
    private Dictionary<int, List<List<string>>> CodeOrdering = new(){
        { 0, new(){ new(){""}, new(){""}, new(){""} } },
        { 1, new(){ new(){""}, new(){""}, new(){""} } } };

    // 
    private Dictionary<int, List<string>> ValueUpdate = new(){
        { 0, new(){ "", ""} },
        { 1, new(){ "", "" } } };

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

    public List<List<string>> ReturnPuzzleDetails(int PuzzleID)
    {
        return MultipleChoice[PuzzleID];
    }

    private void RemoveMessage()
    {
        ErrorMessage.SetActive(false);
    }
}
