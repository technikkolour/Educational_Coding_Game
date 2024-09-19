using System.Collections.Generic;
using TMPro;
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
    public GameObject MultipleChoiceUI_Prefab, CodeOrderingUI_Prefab, ValueUpdatingUI_Prefab, CodeBuildingUI_Prefab;

    // Quiz properties;
    public float TimeLimit = 180f, StartTime = 0f, TimeRemaining;
    public int InitialID = 13;
    public int Score = 0;
    public TMP_Text Timer;

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
        if (Player != null)
            Spawner = Player.InteractingWith;

        if (CurrentPuzzle != null && CurrentPuzzle.PuzzleType == "Quiz")
        {            
            TimeRemaining = TimeLimit - (Time.unscaledTime - StartTime);
            DisplayTime();

            if (TimeRemaining <= 0)
            {
                ClosePuzzle();    
                Timer.transform.parent.gameObject.SetActive(false);
                QuizCompleted();
            }

        }
    }

    public void SubmitSolution()
    {
        if (CurrentPuzzle != null)
        {
            if (CurrentPuzzle.PuzzleType == "Quiz")
            {
                if (CurrentPuzzle.VerifySolution()) 
                    Score++;

                int CurrentID = CurrentPuzzle.PuzzleID + 1;
                Player.InteractingWith = null;

                ClosePuzzle();

                if (CurrentID <= 19)
                {
                    SpawnPuzzle("Quiz", CurrentID);
                }
                else QuizCompleted();
                
            }
            else if (!CurrentPuzzle.RobotBuilding)
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

                    if (CurrentPuzzle.Spawner.name.Contains("Computer")) GameManager.IncreaseHealth();

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
                GameManager.UnpauseGame();
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
                Puzzle = Instantiate(MultipleChoiceUI_Prefab);
                break;
            case "Quiz":
                Puzzle = Instantiate(MultipleChoiceUI_Prefab);
                if (StartTime == 0f) StartTime = Time.unscaledTime;
                break;
            case "CodeOrdering":
                Puzzle = Instantiate(CodeOrderingUI_Prefab);
                break;
            case "ValueUpdate":
                Puzzle = Instantiate(ValueUpdatingUI_Prefab);
                break;
            case "CodeBuilding":
                Puzzle = Instantiate(CodeBuildingUI_Prefab);
                break;
        }

        PuzzleUI.SetActive(true);
        Puzzle.transform.SetParent(PuzzleUI.transform);

        Puzzle PuzzleComponent = Puzzle.GetComponent<Puzzle>();

        if (PuzzleComponent != null && PuzzleComponent.Spawner != null)
        {
            if (PuzzleComponent.PuzzleType != "Quiz") { }
                PuzzleComponent.SetAttempts(Spawner.Attempts);
            PuzzleComponent.SetItemID(Spawner.ItemID);
        }
        PuzzleComponent.PuzzleType = PuzzleType;
        PuzzleComponent.PuzzleID = PuzzleID;

        // Pause the game when the puzzle is spawned in;
        GameManager.PauseGame();
    }

    // Remove the puzzle from the scene once it is closed;
    public void ClosePuzzle()
    {
        // Store the attempts number inside the spawner;
        Puzzle Puzzle = FindObjectOfType<Puzzle>();

        if (Puzzle != null)
        {
            if (!(Puzzle.PuzzleType == "Quiz"))
                Puzzle.SetAttempts(Puzzle.GetAttempts());
            Player.FinishedInteraction();
            Destroy(Puzzle.gameObject);     
        }
    }

    private void QuizCompleted()
    {
        if (Score >= 5)
        {            
            // Set the dialogue phase for the NPC spawner;
            PuzzleSpawner NPC = FindObjectOfType<PuzzleSpawner>();
            NPC.gameObject.GetComponent<NPC>().DialoguePhase = 1;

            // Mark the puzzle as completed and get the Robot Licence;
            GameManager.CompletedPuzzle(InitialID, 4);
            GameManager.PickUpItem(NPC.ItemID);

            Player.FinishedInteraction();

            PuzzleUI.SetActive(false);
            GameManager.UnpauseGame();
        }
        else if (Score < 5)
        {
            SuccessMessage.SetActive(true);
            Score = 0;
        }
    }

    public void DisplayTime()
    {
        int Minutes = Mathf.FloorToInt(TimeRemaining / 60f);
        int Seconds = Mathf.FloorToInt(TimeRemaining - Minutes * 60);

        Timer.text = string.Format("{0:0}:{1:00}", Minutes, Seconds);
    }
}
