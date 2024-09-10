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
    public GameObject PuzzleUI;
    public string ProposedSolution;

    public GameObject MultipleChoiceUI_Prefab, CodeOrderingUI_Prefab, ValueUpdatingUI_prefab, CodeBuildingUI_Prefab;

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
        if (!CurrentPuzzle.RobotBuilding)
        {
            if (CurrentPuzzle.PuzzleType == "CodeBuilding")
            {
                List<CodeBlock> PuzzleBlocks = CurrentPuzzle.GenerateBlockList();
                CurrentPuzzle.ComputeSolution(PuzzleBlocks);
            }

            if (CurrentPuzzle.VerifySolution())
            {
                Player.CompletedPuzzle(CurrentPuzzle.GetID(), CurrentPuzzle.GetAttempts());
                CurrentPuzzle.SetCompleted();
            }
            else
            {
                CurrentPuzzle.SetAttempts();
                ErrorMessage.SetActive(true);
                Invoke(nameof(RemoveMessage), 5);
            }
        }
        else
        {
            List<CodeBlock> PuzzleBlocks = CurrentPuzzle.GenerateBlockList();
            if (CurrentPuzzle.RobotBuilding) CurrentPuzzle.AssignKeyBindings(PuzzleBlocks);
        }
    }

    private void RemoveMessage()
    {
        ErrorMessage.SetActive(false);
    }

    // Instantiate the puzzle;
    public void SpawnPuzzle(string PuzzleType, int PuzzleID)
    {
        GameObject Puzzle = new();
        
        switch (PuzzleType)
        {
            case "MultipleChoice":
                Puzzle = Instantiate(MultipleChoiceUI_Prefab);
                break;
            case "CodeOrdering":
                Puzzle = Instantiate(CodeOrderingUI_Prefab);
                break;
            case "ValueUpdate":
                Puzzle = Instantiate(ValueUpdatingUI_prefab);
                break;
            case "CodeBuilding":
                Puzzle = Instantiate(CodeBuildingUI_Prefab);
                break;
        }

        PuzzleUI.SetActive(true);
        Puzzle.transform.SetParent(PuzzleUI.transform);

        Puzzle.GetComponent<Puzzle>().PuzzleType = PuzzleType;
        Puzzle.GetComponent<Puzzle>().PuzzleID = PuzzleID;
    }        
    
    // Remove the puzzle from the scene once it is closed;
    public void ClosePuzzle()
    {
        Puzzle Puzzle = GameObject.FindObjectOfType<Puzzle>();
        Destroy(Puzzle.gameObject);
    }
}
