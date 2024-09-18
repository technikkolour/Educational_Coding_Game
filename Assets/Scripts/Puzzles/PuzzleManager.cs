using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    // Properties relating to the puzzle;
    private Player Player;
    private Puzzle CurrentPuzzle;
    private PuzzleSpawner Spawner;
    private GameManager GameManager;

    // Properties for validating the solution to the puzzle;
    public GameObject ErrorMessage, SuccessMessage;
    public GameObject PuzzleUI;
    public string ProposedSolution;

    // The prefabs for the different types of puzzles;
    public GameObject MultipleChoiceUI_Prefab, CodeOrderingUI_Prefab, ValueUpdatingUI_prefab, CodeBuildingUI_Prefab;

    // Start is called before the first frame update;
    void Start()
    {
        Player = FindObjectOfType<Player>();
        GameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame;
    void Update()
    {
        CurrentPuzzle = FindObjectOfType<Puzzle>();
        Spawner = Player.InteractingWith;
    }

    public void SubmitSolution()
    {
        if (CurrentPuzzle != null)
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
                    GameManager.CompletedPuzzle(CurrentPuzzle.GetID(), CurrentPuzzle.GetAttempts());
                    GameManager.PickUpItem(CurrentPuzzle.GetItemID());
                    ClosePuzzle();
                    DisplaySuccessMessage();
                }
                else
                {
                    CurrentPuzzle.IncreaseAttempts();
                    ErrorMessage.SetActive(true);
                    Invoke("RemoveMessage", 5);
                }
            }
            else
            {
                List<CodeBlock> PuzzleBlocks = CurrentPuzzle.GenerateBlockList();
                if (CurrentPuzzle.RobotBuilding) CurrentPuzzle.AssignKeyBindings(PuzzleBlocks);
                Destroy(GameObject.Find("PuzzleCanvas"));
            }
        }

    }

    // Set the error message as inactive;
    private void RemoveMessage()
    {
        ErrorMessage.SetActive(false);
    }

    // Set the success message as active;
    public void DisplaySuccessMessage()
    {
        SuccessMessage.SetActive(true);
    }
    // Set the success message as inactive;
    public void RemoveSuccessMessage()
    {
        SuccessMessage.SetActive(false);
    }

    // Instantiate the puzzle depending on the type;
    public void SpawnPuzzle(string PuzzleType, int PuzzleID)
    {
        GameObject Puzzle = new();

        switch (PuzzleType)
        {
            case "MultipleChoice":
            case "Quiz":
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

        Puzzle PuzzleComponent = Puzzle.GetComponent<Puzzle>();

        if (PuzzleComponent.Spawner != null && PuzzleComponent.Spawner != null)
        {
            PuzzleComponent.SetAttempts(Spawner.Attempts);
            PuzzleComponent.SetItemID(Spawner.ItemID);
            PuzzleComponent.PuzzleType = PuzzleType;
            PuzzleComponent.PuzzleID = PuzzleID;
        }

        // Pause the game when the puzzle is spawned in;
        GameManager.PauseGame();
    }

    // Remove the puzzle from the scene once it is closed;
    public void ClosePuzzle()
    {
        // Store the attempts number inside the spawner;
        Puzzle Puzzle = GameObject.FindObjectOfType<Puzzle>();

        if (Puzzle != null)
        {
            Puzzle.SetAttempts(Puzzle.GetAttempts());
            Player.FinishedInteraction();
            Destroy(Puzzle.gameObject);     
        }
  
    }
}
