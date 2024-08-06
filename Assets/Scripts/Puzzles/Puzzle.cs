using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour
{
    public string PuzzleType;
    public int PuzzleID;

    private int Attempts = 0;
    private string Prompt;
    public string Solution;
    private bool Done = false;
    private int Prize_ItemID = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetID()
    {
        return Prize_ItemID;
    }

    public string GetPrompt()
    {
        return Prompt;
    }

    public int GetAttempts()
    {
        return Attempts;
    }
    public void SetAttempts()
    {
        Attempts++;
    }

    public void SetCompleted()
    {
        Done = true;
    }

    public bool VerifySolution(string ProposedSolution)
    {
        return ProposedSolution == Solution;
    }

    // Multiple Choice;
    public void MultipleChoice(GameObject gameObject)
    {
        Solution = gameObject.GetComponent<Toggle>().GetComponentInChildren<Text>().text;
    }
}
