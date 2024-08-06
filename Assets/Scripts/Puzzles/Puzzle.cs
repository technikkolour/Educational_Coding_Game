using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
        PuzzleManager PuzzleManager = FindObjectOfType<PuzzleManager>();
        if (PuzzleType == "MultipleChoice") AssignValues_MC(PuzzleManager.ReturnPuzzleDetails(PuzzleID));
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
    public TMP_Text PromptObject;
    public TMP_Text AttemptsObject;

    public void AssignValues_MC(List<List<string>> Values)
    {
        PromptObject.text = Values[0][0];
        AttemptsObject.text = (Attempts + 1).ToString();

        for (int i = 0; i < 4; i++)
        {
            if (i <= Values[1].Count - 1)
                GameObject.Find("Option_0" + (i + 1)).GetComponentInChildren<Text>().text = Values[1][i];
            else
                GameObject.Find("Option_0" + (i + 1)).SetActive(false);

        }

        Solution = Values[2][0];
    }
    public void MultipleChoice(GameObject gameObject)
    {
        Solution = gameObject.GetComponent<Toggle>().GetComponentInChildren<Text>().text;
    }
}
