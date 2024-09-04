using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodeBlock : MonoBehaviour
{
    public Button UpButton, DownButton;
    public RectTransform BlockRectTransform;
    public string Type;

    // The optional elements;
    public GameObject Dropdown;
    public List<GameObject> Elements = new() {  };

    public TMP_Text Content;

    // The blocks that are nested inside the parent block;
    private List<CodeBlock> NestedBlocks = new List<CodeBlock>();
    private int Index;

    // Start is called before the first frame update
    void Start()
    {
        UpdateElements();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateButtons();
    }

    // Code for removing the code block;
    public void DeleteCodeBlock()
    {
        Destroy(gameObject);
    }

    // Change the order in which blocks are organised;
    public void MoveBlockUp()
    {
        Index = BlockRectTransform.GetSiblingIndex();
        if (Index > 0) BlockRectTransform.SetSiblingIndex(Index - 1);
        UpdateButtons();
    }
    public void MoveBlockDown()
    {
        Index = BlockRectTransform.GetSiblingIndex();
        if (Index < BlockRectTransform.parent.childCount - 1) BlockRectTransform.SetSiblingIndex(Index + 1);
    }

    // Update which buttons are interactable;
    public void UpdateButtons()
    {
        Index = BlockRectTransform.GetSiblingIndex();

        if (Index > 0) UpButton.interactable = true;
        else UpButton.interactable = false;

        if (Index < BlockRectTransform.parent.childCount - 1) DownButton.interactable = true;
        else DownButton.interactable = false;
    }

    // Place the UI elements in the correct spots;
    public void UpdateElements()
    {
        switch (Type)
        {
            case "Integer":
            case "Float":            
            case "Boolean":
            case "String":
                for (int i = 0; i<2; i++)
                {
                    Elements[i].gameObject.SetActive(true);
                    PositionTextBox(Elements[i]);
                }
                break;
            case "Array":            
            case "For Loop":
                for (int i = 0; i < 3; i++)
                {
                    Elements[i].gameObject.SetActive(true);
                    PositionTextBox(Elements[i]);
                }
                break;
            case "Mathematical Operation":
                for (int i = 0; i < 3; i++)
                {
                    Elements[i].gameObject.SetActive(true);
                    PositionTextBox(Elements[i]);
                }
                Dropdown.SetActive(true);
                PositionAndPopulateDropdown(Dropdown, new(){ "+", "-", "*", "/" });
                break;
            case "Output":
            case "Attack With Power":            
                Elements[0].SetActive(true);
                PositionTextBox(Elements[0]);
                break;
            case "If Statement":
            case "While Loop":
                for (int i = 0; i < 2; i++)
                {
                    Elements[i].gameObject.SetActive(true);
                    PositionTextBox(Elements[i]);
                }
                Dropdown.SetActive(true);
                PositionAndPopulateDropdown(Dropdown, new() { "=", "<", ">" });
                break;
            case "Move In Direction":
                Elements[0].SetActive(true);
                Dropdown.SetActive(true);
                PositionTextBox(Elements[0]);
                PositionAndPopulateDropdown(Dropdown, new() { "Up", "Left", "Right" });
                break;
            case "Assign Key":
                Dropdown.SetActive(true);
                PositionAndPopulateDropdown(Dropdown, new() { "Q", "E", "Space" });
                break;
        }

    }
    // Code for positioning the input field;
    public void PositionTextBox(GameObject Element)
    {
        Content.ForceMeshUpdate();
        int UnderscoreIndex = Content.text.IndexOf("_______");

        Debug.Log(Content.text);

        if (UnderscoreIndex != -1)
        {
            Vector3 MissingValuePosition = Content.transform.TransformPoint(Content.textInfo.characterInfo[UnderscoreIndex].topRight);

            // Position the input field;
            RectTransform InputFieldRect = Element.GetComponent<RectTransform>();
            InputFieldRect.position = new Vector3(MissingValuePosition.x + 65, MissingValuePosition.y + 10, InputFieldRect.position.z);
        }
    }
    public void PositionAndPopulateDropdown(GameObject Dropdown, List<string> Options)
    {
        Content.ForceMeshUpdate();
        int UnderscoreIndex = Content.text.IndexOf("------");

        if (UnderscoreIndex != -1)
        {
            Vector3 DropdownPosition = Content.transform.TransformPoint(Content.textInfo.characterInfo[UnderscoreIndex].topRight);

            // Position the dropdown;
            RectTransform DropdownRect = Dropdown.GetComponent<RectTransform>();
            DropdownRect.position = new Vector3(DropdownPosition.x + 40, DropdownPosition.y + 5, DropdownRect.position.z);

            // Remove old options and populate the dropdown;
            Dropdown.GetComponent<TMP_Dropdown>().AddOptions(Options);
        }
    }

    // Block types;
    public void DeclarativeBlock(string Type, string Name, string Value)
    {
        switch (Type)
        {
            case "Integer":
                break;
            case "Float":
                break;
            case "Boolean":
                break;
            case "String":
                break;
            case "Array":
                break; 
        }
    }
    public void AssignmentBlock(string Variable, string Element_01, string Operation, string Element_02)
    {

    }
    public void OutputBlock(string Output)
    {

    }
    public void ConditionalBlock(string Variable_01, string Condition, string Variable_02)
    {

    }
    public void ForLoopBlock(string Variable, string StartValue, string StepValue)
    {

    }
    public void WhileLoopBlock(string Variable_01, string Condition, string Variable_02)
    {

    }

}
