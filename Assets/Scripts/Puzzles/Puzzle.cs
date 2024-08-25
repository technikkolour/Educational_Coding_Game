using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Puzzle : MonoBehaviour
{
    public string PuzzleType;
    public int PuzzleID;

    private int Attempts = 1;
    private string Prompt;
    public string Solution;
    private bool Done = false;
    private int Prize_ItemID = 0;

    private DataManager DataManager;
    public string ProposedSolution;

    public TMP_Text PromptObject;
    public TMP_Text AttemptsObject;

    // Start is called before the first frame update
    void Start()
    {
        PuzzleManager PuzzleManager = FindObjectOfType<PuzzleManager>();
        DataManager = FindObjectOfType<DataManager>();

        switch (PuzzleType)
        {
            case "MultipleChoice":
                AssignValues_MC(DataManager.ReturnPuzzleDetails(PuzzleID));
                break;
            case "CodeOrdering":
                AssignValues_CO(DataManager.ReturnPuzzleDetails(PuzzleID));
                break;
            case "ValueUpdate":
                AssignValues_VU(DataManager.ReturnPuzzleDetails(PuzzleID));
                break;
            case "CodeBuilding":
                AssignValues_CB(DataManager.ReturnPuzzleDetails(PuzzleID));
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        AttemptsObject.text = Attempts.ToString();

        if (PuzzleType == "CodeOrdering")
            ComputeOrder();
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

    //####################################################################################################################################################################
    // Multiple Choice;
    public void AssignValues_MC(List<List<string>> Values)
    {
        PromptObject.text = Values[0][0];
        AttemptsObject.text = (Attempts + 1).ToString();

        for (int i = 0; i < 4; i++)
        {
            if (i <= Values[1].Count - 1)
                GameObject.Find("Option_0" + (i + 1)).GetComponentInChildren<UnityEngine.UI.Text>().text = Values[1][i];
            else
                GameObject.Find("Option_0" + (i + 1)).SetActive(false);

        }

        Solution = Values[2][0];
    }
    public void SetAnswer_MC(string OptionName)
    {
        ProposedSolution = GameObject.Find(OptionName).GetComponentInChildren<UnityEngine.UI.Text>().text;
    }
    //####################################################################################################################################################################
    // Multiple Choice - QUIZ Mode;
    public void QuizMode()
    {

    }

    //####################################################################################################################################################################
    // Code Line Ordering;
    public void AssignValues_CO(List<List<string>> Values)
    {
        PromptObject.text = Values[0][0];
        AttemptsObject.text = (Attempts + 1).ToString();

        for (int i = 0; i < 4; i++)
        {
            if (i <= Values[1].Count - 1)
            {
                GameObject ParentCode = GameObject.Find("Place_0" + (i + 1));
                ParentCode.transform.Find("CodeLine_0" + (i + 1)).GetComponentInChildren<TMP_Text>().text = Values[1][i];
            }
            else
            {
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
            GameObject ParentCode = GameObject.Find("Place_0" + (i + 1));
            if (ParentCode != null)
            {
                GameObject CodeLine = ParentCode.GetComponent<DropArea>().FindChildWithTag("CodeLine");
                Order += CodeLine.name.Substring(CodeLine.name.Length - 1);
            }
        }

        ProposedSolution = Order;
    }

    //####################################################################################################################################################################
    // Value Updating;
    public TMP_Text CodeBodyText;
    public TMP_InputField ValueInputField;

    public void AssignValues_VU(List<List<string>> Values)
    {
        PromptObject.text = Values[0][0];
        AttemptsObject.text = (Attempts + 1).ToString();

        GameObject.Find("CodeBody").GetComponentInChildren<TMP_Text>().text = Values[1][0];
        PositionTextBox();

        Solution = Values[2][0];
    }
    public void PositionTextBox()
    {
        // Force TMP to update;
        // Otherwise the text isn't fully loaded and the code doesn't function properly;
        CodeBodyText.ForceMeshUpdate();
        int UnderscoreIndex = CodeBodyText.text.IndexOf("_________");

        if (UnderscoreIndex != -1)
        {
            // Get the world position of the missing value;
            Vector3 MissingValuePosition = CodeBodyText.transform.TransformPoint(CodeBodyText.textInfo.characterInfo[UnderscoreIndex].topRight);

            // Place the InputField in the location of the underscores;
            RectTransform InputFieldRect = ValueInputField.GetComponent<RectTransform>();
            InputFieldRect.position = new Vector3(MissingValuePosition.x + 85, MissingValuePosition.y + 20, InputFieldRect.position.z);
        }
    }
    public void SetAnswer_VU()
    {
        ProposedSolution = ValueInputField.text;
    }

    //####################################################################################################################################################################
    // Code Building
    public GameObject CodeBlockPrefab;
    private GameObject CodeWindow;

    public void AssignValues_CB(List<List<string>> Values)
    {
        PromptObject.text = Values[0][0];
        Solution = Values[1][0];

        CodeWindow = GameObject.Find("CodeScrollView");
    }
    public void SpawnCodeBlock(string Type)
    {
        string BlockText = DataManager.ReturnCodeBlockText(Type);

        GameObject BlockInstance = Instantiate(CodeBlockPrefab);
        BlockInstance.GetComponentInChildren<TMP_Text>().text = BlockText;
        BlockInstance.transform.SetParent(CodeWindow.transform);

        // Set location of block;
    }
    public void DeleteCodeBlock()
    {

    }
    public void MoveBlockUp()
    {

    }
    public void MoveBlockDown()
    {

    }
    public void GenerateSolution()
    {

    }
}
