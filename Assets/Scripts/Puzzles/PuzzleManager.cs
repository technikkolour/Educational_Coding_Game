using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private Player Player;
    private Puzzle CurrentPuzzle;

    private Dictionary<int, List<string>> MultipleChoice = new(){
        { 0, new(){ "", ""} },
        { 1, new(){ "", "" } } } ;
    private Dictionary<int, List<string>> Quiz = new(){
        { 0, new(){ "", ""} },
        { 1, new(){ "", "" } } };
    private Dictionary<int, List<string>> CodeOrdering = new(){
        { 0, new(){ "", ""} },
        { 1, new(){ "", "" } } };
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

    public void SubmitSolution(string CorrectSolution)
    {
        if (CurrentPuzzle.VerifySolution(CorrectSolution))
        {
            Player.CompletedPuzzle(CurrentPuzzle.GetID(), CurrentPuzzle.GetAttempts());
            CurrentPuzzle.SetCompleted();
        }   
        else
            CurrentPuzzle.SetAttempts();
    }
}
