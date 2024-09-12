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
        if (AttemptsObject != null)
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
                Order += CodeLine.name[^1..];
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
    public bool RobotBuilding;

    private GameObject CodeWindow;

    public void AssignValues_CB(List<List<string>> Values)
    {
        PromptObject.text = Values[0][0];
        Solution = Values[1][0];

        CodeWindow = GameObject.Find("CodeScrollView/Viewport/Content");

        if (!RobotBuilding)
        {
            GameObject.Find("RobotTab").SetActive(false);
        }
        else
        {
            GameObject.Find("VariablesTab").SetActive(false);
            GameObject.Find("FunctionsTab").SetActive(false);
        }
    }
    // Instantiate the code block;
    public void SpawnCodeBlock(string Type)
    {
        string BlockText = DataManager.ReturnCodeBlockText(Type);

        GameObject BlockInstance = Instantiate(CodeBlockPrefab);
        BlockInstance.transform.Find("BlockText").GetComponent<TMP_Text>().text = BlockText;
        BlockInstance.transform.SetParent(CodeWindow.transform);
        BlockInstance.GetComponent<CodeBlock>().Type = Type;

        // Set whether the block can have nested blocks;
        List<string> NestingTypes = new() { "If Statement", "For Loop", "While Loop", "Assign Key" };
        if (NestingTypes.Contains(Type)) BlockInstance.GetComponent<CodeBlock>().CanHaveNestedBlocks = true;
    }
    // Obtain a list of the Code Blocks that can nest other blocks present in the code window;
    public List<CodeBlock> GenerateBlockList()
    {
        List<CodeBlock> BlockList = new();

        foreach (Transform BlockTransform in CodeWindow.transform)
        {
            CodeBlock Block = BlockTransform.GetComponent<CodeBlock>();
            if (Block.CanHaveNestedBlocks || Block.Parent == null)
            BlockList.Add(Block);
        }
        return BlockList;
    }
    // Compute the solution string based on the blocks in the code window;
    public void ComputeSolution(List<CodeBlock> CodeBlocks)
    {
        Dictionary<string, string> VariablesAndValues = new();
        string Output = "";
        bool ErrorsPresent = false;

        // Put the code together;
        foreach (CodeBlock Block in CodeBlocks) {
            // If there are any errors present in the implementation, mark ErrorsPresent as true; 
            switch (Block.Type)
            {
                case "Integer":
                    // Verify whether the types match up;
                    if (string.IsNullOrEmpty(Block.Values[0]) || string.IsNullOrEmpty(Block.Values[1])) ErrorsPresent = true;
                    else if (int.TryParse(Block.Values[1], out _) && !VariablesAndValues.ContainsKey(Block.Values[0])) 
                        VariablesAndValues.Add(Block.Values[0], Block.Values[1]);
                    else ErrorsPresent = true;
                    break;

                case "Float":
                    // Verify whether the types match up;
                    if (string.IsNullOrEmpty(Block.Values[0]) || string.IsNullOrEmpty(Block.Values[1])) ErrorsPresent = true;
                    else if (float.TryParse(Block.Values[1], out _) && !VariablesAndValues.ContainsKey(Block.Values[0]))
                        VariablesAndValues.Add(Block.Values[0], Block.Values[1]);
                    else ErrorsPresent = true;
                    break;

                case "Boolean":
                    // Verify whether the types match up;
                    if (string.IsNullOrEmpty(Block.Values[0]) || string.IsNullOrEmpty(Block.Values[1])) ErrorsPresent = true;
                    else if (bool.TryParse(Block.Values[1], out _) && !VariablesAndValues.ContainsKey(Block.Values[0]))
                        VariablesAndValues.Add(Block.Values[0], Block.Values[1]);
                    else ErrorsPresent = true;
                    break;

                case "String":
                    // Verify whether the types match up;
                    if (string.IsNullOrEmpty(Block.Values[0]) || string.IsNullOrEmpty(Block.Values[1])) ErrorsPresent = true;
                    else if ((Block.Values[1].StartsWith("\"") && Block.Values[1].EndsWith("\"")) && !VariablesAndValues.ContainsKey(Block.Values[0]))
                        VariablesAndValues.Add(Block.Values[0], Block.Values[1]);
                    else ErrorsPresent = true;
                    break;

                case "Array":
                    // Verify whether the types match up;
                    if (string.IsNullOrEmpty(Block.Values[0]) || string.IsNullOrEmpty(Block.Values[1]) || string.IsNullOrEmpty(Block.Values[2])) ErrorsPresent = true;
                    else if (!AreSameType(Block.Values[1], Block.Values[2])) ErrorsPresent = true;
                    else ErrorsPresent = true;
                    
                    // If there are no issues with the array, add it to the variable list;
                    if (!ErrorsPresent)
                        VariablesAndValues.Add(Block.Values[0], "[" + Block.Values[1] + ", " + Block.Values[2] + "]");
                    break;

                case "Mathematical Operation":
                    // Verify whether the types match up;
                    // Verify if the variables referenced exist;
                    if (string.IsNullOrEmpty(Block.Values[0]) || string.IsNullOrEmpty(Block.Values[1]) || string.IsNullOrEmpty(Block.Values[2])) ErrorsPresent = true;
                    else if (!AreSameType(Block.Values[1], Block.Values[2]) || (!AreSameType(Block.Values[0], Block.Values[1]))) ErrorsPresent = true;
                    else if (!VariablesAndValues.ContainsKey(Block.Values[0]) || !VariablesAndValues.ContainsKey(Block.Values[1]) || !VariablesAndValues.ContainsKey(Block.Values[2])) ErrorsPresent = true;
                    
                    // Compute the result of the operation;
                    if (!ErrorsPresent)
                    {
                        if (VariablesAndValues.ContainsKey(Block.Values[0]) || VariablesAndValues.ContainsKey(Block.Values[1]) || VariablesAndValues.ContainsKey(Block.Values[2]))
                            VariablesAndValues[Block.Values[0]] = MathematicalOperation(VariablesAndValues[Block.Values[1]], VariablesAndValues[Block.Values[2]], Block.Values[3]);
                        else VariablesAndValues[Block.Values[0]] = MathematicalOperation(Block.Values[1], Block.Values[2], Block.Values[3]);
                    }
                    break;

                case "Output":
                    // Verify whether the variables referenced exist;
                    Debug.Log(ErrorsPresent);
                    if (string.IsNullOrEmpty(Block.Values[0])) ErrorsPresent = true;
                    else if (!float.TryParse(Block.Values[0], out _) && !(Block.Values[0].StartsWith("\"") && Block.Values[0].EndsWith("\"")) && !VariablesAndValues.ContainsKey(Block.Values[0])) ErrorsPresent = true;
                    Debug.Log(ErrorsPresent);

                    // If there are no issues with the code, add the resulting output to the Output string;
                    if (!ErrorsPresent)
                        if (VariablesAndValues.ContainsKey(Block.Values[0]))
                        {
                            Output += VariablesAndValues[Block.Values[0]];
                        }
                        else
                        {
                            Output += Block.Values[0];
                        }

                    break;

                case "If Statement":
                    // Verify whether the types match up and the condition is possible;
                    // Verify whether the variables inside the declaration exist;
                    if (string.IsNullOrEmpty(Block.Values[0]) || string.IsNullOrEmpty(Block.Values[1])) ErrorsPresent = true;
                    else if (!AreSameType(Block.Values[1], Block.Values[2])) ErrorsPresent = true;

                    // Process the nested blocks;
                    if (!ErrorsPresent)
                    {
                        if (VariablesAndValues.ContainsKey(Block.Values[0]) || VariablesAndValues.ContainsKey(Block.Values[1]))
                            if (ConditionalBlock(VariablesAndValues[Block.Values[0]], VariablesAndValues[Block.Values[1]], Block.Values[3]))
                            {
                                ComputeSolution(Block.NestedBlocks);
                            }
                        else if (ConditionalBlock(Block.Values[0], Block.Values[1], Block.Values[3]))
                            {
                                ComputeSolution(Block.NestedBlocks);
                            }
                    }
                    break;

                case "For Loop":
                    // Verify whether the variable inside the loop declaration exists;
                    if (string.IsNullOrEmpty(Block.Values[0]) || string.IsNullOrEmpty(Block.Values[1]) || string.IsNullOrEmpty(Block.Values[2])) ErrorsPresent = true;
                    else if (!AreSameType(Block.Values[0], Block.Values[1]) || !AreSameType(Block.Values[1], Block.Values[2])) ErrorsPresent = true;
                    // Process the nested blocks;

                    if (!ErrorsPresent)
                    {
                        int Start = int.Parse(VariablesAndValues[Block.Values[0]]);
                        int End = int.Parse(Block.Values[1]);
                        int Step = int.Parse(Block.Values[2]);

                        for (int i = Start; i <= End; i += Step)
                        {
                            ComputeSolution(Block.NestedBlocks);
                        }
                    }
                    break;

                case "While Loop":
                    // Verify whether the variable inside the loop declaration exists;
                    if (string.IsNullOrEmpty(Block.Values[0]) || string.IsNullOrEmpty(Block.Values[1])) ErrorsPresent = true;
                    else if (!AreSameType(Block.Values[0], Block.Values[1])) ErrorsPresent = true;

                    // Process the nested blocks;
                    if (!ErrorsPresent)
                    {
                        while (ConditionalBlock(VariablesAndValues[Block.Values[0]], Block.Values[1], Block.Values[3]))
                        {
                            ComputeSolution(Block.NestedBlocks);
                        }
                    }
                    break;
            }
            if (Block.CanHaveNestedBlocks && Block.NestedBlocks.Count == 0) ErrorsPresent = true;

            List<string> BlockContents = new() { Block.Type };
        }

        // If there are any error with the code, the solution is reset to an empty set;
        if (ErrorsPresent) Output = "";

        // The proposed solution is equal to the expected output or the list of elements;
        ProposedSolution = Output;
    }
    // Helper function for assessing whether the parameters are of the same type;
    public bool AreSameType(string Variable_01, string Variable_02)
    {
        if ((int.TryParse(Variable_01, out _) == int.TryParse(Variable_02, out _)) ||
            (float.TryParse(Variable_01, out _) == float.TryParse(Variable_02, out _)) ||
            (bool.TryParse(Variable_01, out _) == bool.TryParse(Variable_02, out _)) ||
            (Variable_01.StartsWith("\"") && Variable_02.StartsWith("\"") && Variable_01.EndsWith("\"") && Variable_02.EndsWith("\""))) return true;
        return false;
    }
    // Method simulating the behaviour of the Mathematical Operation block;
    public string MathematicalOperation(string Variable_01, string Variable_02, string Operand)
    {
        float Result = 0;

        // Parse the variables to float;
        float Value_01 = float.Parse(Variable_01);
        float Value_02 = float.Parse(Variable_02);

        switch (Operand)
        {
            case "+":
                Result = Value_01 + Value_02;
                break;
            case "-":
                Result = Value_01 - Value_02;
                break;
            case "*":
                Result = Value_01 * Value_02;
                break;
            case "/":
                Result = Value_01 / Value_02;
                break;
        }

        return Result.ToString(); 
    }
    // Method simulating the behaviour of the Mathematical Operation block;
    public bool ConditionalBlock(string Variable_01, string Variable_02, string Condition)
    {
        float Value_01, Value_02;

        switch (Condition)
        {
            case "=":
                return (Variable_01 == Variable_02);
            case "<":
                Value_01 = float.Parse(Variable_01);
                Value_02 = float.Parse(Variable_02);
                return (Value_01 < Value_02);
            case ">":
                Value_01 = float.Parse(Variable_01);
                Value_02 = float.Parse(Variable_02);
                return (Value_01 > Value_02);
        }

        return false;
    }
    //####################################################################################################################################################################
    // Code Building - ROBOT Mode;
    public void AssignKeyBindings(List<CodeBlock> CodeBlocks)
    {
        Robot Robot = GameObject.Find("Robot").GetComponent<Robot>();

        foreach (CodeBlock ParentBlock in CodeBlocks)
        {
            // Check whether the parent block has nested blocks, if it does not, do not add a mapping for it;
            if (ParentBlock.CanHaveNestedBlocks && ParentBlock.NestedBlocks.Count != 0)
            {
                // Get the key selected inside the Key Binding block;
                TMP_Dropdown KeyDropdown = ParentBlock.GetComponentInChildren<TMP_Dropdown>();
                string Key = KeyDropdown.options[KeyDropdown.value].text;

                // Add the mapping to the corresponding key inside the KeyBindings list;
                foreach (CodeBlock Block in ParentBlock.NestedBlocks)
                {
                    Robot.KeyBindings[Key].Add(Block);
                }
            }
        }
    }
}
