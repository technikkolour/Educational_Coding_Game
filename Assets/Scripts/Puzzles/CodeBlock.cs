using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CodeBlock : MonoBehaviour
{
    // The visual elements;
    public Button UpButton, DownButton;
    public RectTransform BlockRectTransform;
    public string Type;
    public List<Sprite> BlockBackgrounds = new List<Sprite>(2);

    // The optional components;
    public GameObject Dropdown;
    public List<GameObject> Elements = new() {  };
    public TMP_Text Content;

    // The blocks that are nested inside the parent block;
    public bool IsNested = false;
    public bool CanHaveNestedBlocks = false;
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
        // The block being moved down does not nest it into another;
        IsNested = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = BlockBackgrounds[0];

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
        int LastIndex = 0;

        switch (Type)
        {
            case "Integer":
            case "Float":            
            case "Boolean":
            case "String":
                for (int i = 0; i<2; i++)
                {
                    Elements[i].gameObject.SetActive(true);
                    LastIndex = PositionTextBox(Elements[i], i, LastIndex);
                }
                break;
            case "Array":
                for (int i = 0; i < 3; i++)
                {
                    Elements[i].gameObject.SetActive(true);
                    LastIndex = PositionTextBox(Elements[i], i, LastIndex);
                }
                break;
            case "For Loop":
                CanHaveNestedBlocks = true;
                for (int i = 0; i < 3; i++)
                {
                    Elements[i].gameObject.SetActive(true);
                    LastIndex = PositionTextBox(Elements[i], i, LastIndex);
                }
                break;
            case "Mathematical Operation":
                for (int i = 0; i < 3; i++)
                {
                    Elements[i].gameObject.SetActive(true);
                    LastIndex = PositionTextBox(Elements[i], i, LastIndex);
                }
                Dropdown.SetActive(true);
                PositionAndPopulateDropdown(Dropdown, new(){ "+", "-", "*", "/" });
                break;
            case "Output":
            case "Attack With Power":            
                Elements[0].SetActive(true);
                LastIndex = PositionTextBox(Elements[0], 0, 0);
                break;
            case "If Statement":
            case "While Loop":
                CanHaveNestedBlocks = true;
                for (int i = 0; i < 2; i++)
                {
                    Elements[i].gameObject.SetActive(true);
                    LastIndex = PositionTextBox(Elements[i], i, LastIndex);
                }
                Dropdown.SetActive(true);
                PositionAndPopulateDropdown(Dropdown, new() { "=", "<", ">" });
                break;
            case "Move In Direction":
                Elements[0].SetActive(true);
                Dropdown.SetActive(true);
                LastIndex = PositionTextBox(Elements[0], 0, 0);
                PositionAndPopulateDropdown(Dropdown, new() { "Up", "Left", "Right" });
                break;
            case "Assign Key":
                CanHaveNestedBlocks = true;
                Dropdown.SetActive(true);
                PositionAndPopulateDropdown(Dropdown, new() { "Q", "E", "Space" });
                break;
        }

    }
    // Code for positioning the input field;
    public int PositionTextBox(GameObject Element, int ElementIndex, int LastIndex)
    {
        Content.ForceMeshUpdate();
        int UnderscoreIndex = Content.text.IndexOf("_______", LastIndex + 1);

        if (UnderscoreIndex != -1)
        {
            Vector3 MissingValuePosition = Content.transform.TransformPoint(Content.textInfo.characterInfo[UnderscoreIndex].topRight);

            // Position the input field;
            RectTransform InputFieldRect = Element.GetComponent<RectTransform>();
            InputFieldRect.position = new Vector3(MissingValuePosition.x + 60, MissingValuePosition.y + 10, InputFieldRect.position.z);

            LastIndex = UnderscoreIndex;
        }

        return LastIndex;
    }
    public void PositionAndPopulateDropdown(GameObject Dropdown, List<string> Options)
    {
        Content.ForceMeshUpdate();
        int DashIndex = Content.text.IndexOf("------");

        if (DashIndex != -1)
        {
            Vector3 DropdownPosition = Content.transform.TransformPoint(Content.textInfo.characterInfo[DashIndex].topRight);

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
