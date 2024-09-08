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
    // The background will change depending on whether the blocks are nested or not;
    public List<Sprite> BlockBackgrounds = new(2);

    // The optional components;
    public GameObject Dropdown;
    public List<GameObject> Elements = new() {  };
    public TMP_Text Content;

    // The blocks that are nested inside the parent block;
    public bool IsNested = false;
    public bool CanHaveNestedBlocks = false;
    private List<CodeBlock> NestedBlocks = new();
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
        // If neither the block above, nor the current one have any nested blocks, the current block is moved directly above;
        Index = BlockRectTransform.GetSiblingIndex();
        if (Index > 0) BlockRectTransform.SetSiblingIndex(Index - 1);

        // If the block is the first in the nested set, it is moved above the initial parent block;

        // If the block above can have nested blocks, the current one will be attached at the end of the nested set;

        // If the block has nested blocks, the entirety of the set is moved together;

        UpdateButtons();
    }
    public void MoveBlockDown()
    {
        // The block is not nested into into another regardless if the block below it can have nested blocks;
        // This includes the case in which the block below it can have nested blocks, but does not contain any currently;
        IsNested = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = BlockBackgrounds[0];

        // If the block below has nested blocks, the current block should bypass them and be moved directly below the last nested block;

        // If the block has nested blocks, the entirety of the set is moved together;

        Index = BlockRectTransform.GetSiblingIndex();
        if (Index < BlockRectTransform.parent.childCount - 1) BlockRectTransform.SetSiblingIndex(Index + 1);
    }

    // Update which buttons are interactable;
    public void UpdateButtons()
    {
        Index = BlockRectTransform.GetSiblingIndex();

        // If the block is not first, it should be able to move upwards;
        if (Index > 0) UpButton.interactable = true;
        else UpButton.interactable = false;

        // If the block is not last, it should be able to move downwards;
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
                    Elements[i].SetActive(true);
                    LastIndex = PositionTextBox(Elements[i], i, LastIndex);
                }
                break;
            case "Array":
                for (int i = 0; i < 3; i++)
                {
                    Elements[i].SetActive(true);
                    LastIndex = PositionTextBox(Elements[i], i, LastIndex);
                }
                break;
            case "For Loop":
                CanHaveNestedBlocks = true;
                for (int i = 0; i < 3; i++)
                {
                    Elements[i].SetActive(true);
                    LastIndex = PositionTextBox(Elements[i], i, LastIndex);
                }
                break;
            case "Mathematical Operation":
                for (int i = 0; i < 3; i++)
                {
                    Elements[i].SetActive(true);
                    LastIndex = PositionTextBox(Elements[i], i, LastIndex);
                }
                Dropdown.SetActive(true);
                PositionAndPopulateDropdown(Dropdown, new(){ "+", "-", "*", "/" });
                break;
            case "Output":
            case "Attack With Power":            
                Elements[0].SetActive(true);
                PositionTextBox(Elements[0], 0, 0);
                break;
            case "If Statement":
            case "While Loop":
                CanHaveNestedBlocks = true;
                for (int i = 0; i < 2; i++)
                {
                    Elements[i].SetActive(true);
                    LastIndex = PositionTextBox(Elements[i], i, LastIndex);
                }
                Dropdown.SetActive(true);
                PositionAndPopulateDropdown(Dropdown, new() { "=", "<", ">" });
                break;
            case "Move In Direction":
                Elements[0].SetActive(true);
                Dropdown.SetActive(true);
                PositionTextBox(Elements[0], 0, 0);
                PositionAndPopulateDropdown(Dropdown, new() { "Up", "Left", "Right" });
                break;
            case "Assign Key":
                CanHaveNestedBlocks = true;
                Dropdown.SetActive(true);
                PositionAndPopulateDropdown(Dropdown, new() { "Q", "E", "Space" });
                break;
        }

    }
    // Position the input field;
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
    // Position the dropdown;
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
