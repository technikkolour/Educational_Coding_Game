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
        { 10, new(){ new(){""}, new(){""}, new(){""} } },
        { 11, new(){ new(){""}, new(){""}, new(){""} } } };
    private Dictionary<int, List<List<string>>> CodeOrdering = new(){
        { 3, new(){ new(){"Hmm, the code for the gate doesn't work... I think changing the order of the lines will fix it."}, new(){ "for values of x increasing by 1, up to 5", "move 25 cm", "integer x = 1"}, new(){"231"} } } };

    // 
    private Dictionary<int, List<List<string>>> ValueUpdate = new(){
        { 5, new(){ new(){"This gate seems to be locked. Maybe changing a value in the code will open it..."}, new(){"if 'gate is unlocked' = _____\n    open"}, new(){"True"} } },
        { 4, new(){ new(){"The gate to the city is locked. Maybe setting its value to unlocked will help me get through."}, new(){"gate = '_________'"}, new(){"unlocked"} } } };

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
        return CodeOrdering[PuzzleID];
    }

    private void RemoveMessage()
    {
        ErrorMessage.SetActive(false);
    }
}
