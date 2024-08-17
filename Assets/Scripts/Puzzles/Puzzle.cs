using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour
{
    public string PuzzleType;
    public int PuzzleID;

    private int Attempts = 1;
    private string Prompt;
    public string Solution;
    private bool Done = false;
    private int Prize_ItemID = 0;

    public string ProposedSolution;

    public TMP_Text PromptObject;
    public TMP_Text AttemptsObject;

    // Start is called before the first frame update
    void Start()
    {
        PuzzleManager PuzzleManager = FindObjectOfType<PuzzleManager>();
        if (PuzzleType == "MultipleChoice") AssignValues_MC(PuzzleManager.ReturnPuzzleDetails(PuzzleID));
        if (PuzzleType == "CodeOrdering") AssignValues_CO(PuzzleManager.ReturnPuzzleDetails(PuzzleID));
    }

    // Update is called once per frame
    void Update()
    {
        AttemptsObject.text = Attempts.ToString();
    }

    public int GetID()
    {
        return PuzzleID;
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

    public bool VerifySolution()
    {
        return ProposedSolution == Solution;
    }

    // Multiple Choice;
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
    public void SetAnswer(string OptionName)
    {
        ProposedSolution = GameObject.Find(OptionName).GetComponentInChildren<Text>().text;
    }

    // Code Line Ordering;
    public void AssignValues_CO(List<List<string>> Values)
    {
        PromptObject.text = Values[0][0];
        AttemptsObject.text = (Attempts + 1).ToString();

        for (int i = 0; i < 4; i++)
        {
            if (i <= Values[1].Count - 1)
                GameObject.Find("CodeLine_0" + (i + 1)).GetComponentInChildren<TMP_Text>().text = Values[1][i];
            else
            {
                GameObject.Find("CodeLine_0" + (i + 1)).SetActive(false);
                GameObject.Find("Place_0" + (i + 1)).SetActive(false);
            }

        }

        Solution = Values[2][0];
    }
    private void ComputeOrder()
    {
        string Order = ""; 

        for (int i = 0; i < 4; i++)
        {
            /*Order += new(CollidingObject.name.Split("_"))[-1];*/
        }

        ProposedSolution = Order;
    }
    public void MoveLine(Collider2D MovedLine)
    {
        Vector2 IntialPosition = MovedLine.transform.position;
    }

    // Value Updating;
    public void AssignValues_VU(List<List<string>> Values)
    {
        PromptObject.text = Values[0][0];
        AttemptsObject.text = (Attempts + 1).ToString();

        for (int i = 0; i < 4; i++)
        {
            if (i <= Values[1].Count - 1)
                GameObject.Find("CodeLine_0" + (i + 1)).GetComponentInChildren<TMP_Text>().text = Values[1][i];
            else
            {
                GameObject.Find("CodeLine_0" + (i + 1)).SetActive(false);
                GameObject.Find("Place_0" + (i + 1)).SetActive(false);
            }

        }

        Solution = Values[2][0];
    }
}
