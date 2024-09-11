using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.Mathematics;
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
    public List<Sprite> BlockBackgrounds = new();

    // The optional Game Objects;
    public GameObject Dropdown;
    public List<GameObject> Elements = new() {  };
    public TMP_Text Content;

    // The values of the optional Game Objects;
    public int ElementIndex = -1;
    public List<string> Values = new();

    // The blocks that are nested inside the parent block;
    public bool IsNested = false;
    public bool CanHaveNestedBlocks = false;
    public CodeBlock Parent = null;
    public List<CodeBlock> NestedBlocks = new();

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
        if (NestedBlocks.Count != 0) ResetNestedBlocks();
        if (IsNested) Parent.NestedBlocks.Remove(this);
        Destroy(gameObject);
    }

    // Change the order in which blocks are organised;
    public void MoveBlockUp()
    {
        Index = BlockRectTransform.GetSiblingIndex();

        // Get the code block above;
        CodeBlock BlockAbove = BlockRectTransform.parent.GetChild(Index - 1).GetComponent<CodeBlock>();

        if (Index > 0)
        {
            if (BlockAbove.CanHaveNestedBlocks)
            {
                // If the block above can have nested blocks, the current one will be attached at the end of the nested set;
                if (!IsNested)
                {
                    IsNested = true;
                    Parent = BlockAbove;
                    BlockAbove.NestedBlocks.Add(this);
                    transform.Find("BlockBackground").GetComponent<Image>().sprite = BlockBackgrounds[1];
                }
                else if (IsNested)
                {
                    IsNested = false;
                    Parent.NestedBlocks.Remove(this);
                    Parent = null;
                    BlockRectTransform.SetSiblingIndex(Index - 1);
                    transform.Find("BlockBackground").GetComponent<Image>().sprite = BlockBackgrounds[0];
                }
            }
            else if (BlockAbove.IsNested)
            {
                // If the block above is nested and the current one is also nested, move above;
                if (IsNested) BlockRectTransform.SetSiblingIndex(Index - 1);
                else
                {
                    IsNested = true;
                    Parent = BlockAbove.Parent;
                    Parent.NestedBlocks.Add(this);
                    transform.Find("BlockBackground").GetComponent<Image>().sprite = BlockBackgrounds[1];
                }
            }
            else if (NestedBlocks.Count != 0)
            {
                // If the block has nested blocks, the entirety of the set is moved together;
                BlockRectTransform.SetSiblingIndex(Index - 1);

                foreach (CodeBlock Block in NestedBlocks)
                {
                    int BlockIndex = Block.BlockRectTransform.GetSiblingIndex();
                    Block.BlockRectTransform.SetSiblingIndex(BlockIndex - 1);
                }
            }
            else
            {
                // If neither the block above, nor the current one have any nested blocks, the current block is moved directly above;
                // If the block is the first in the nested set, it is moved above the initial parent block;
                IsNested = false;
                Parent = null;
                BlockRectTransform.SetSiblingIndex(Index - 1);
                transform.Find("BlockBackground").GetComponent<Image>().sprite = BlockBackgrounds[0];
            }

        }

        UpdateButtons();
    }
    public void MoveBlockDown()
    {
        Index = BlockRectTransform.GetSiblingIndex();

        if (Index < BlockRectTransform.parent.childCount - 1)
        {
            // Get the code block below;
            CodeBlock BlockBelow = BlockRectTransform.parent.GetChild(Index + 1).GetComponent<CodeBlock>();

            if (IsNested)
            {        
                // If the block is nested into another, and the block below is nested too, it should simply move down;
                if (BlockBelow.IsNested) BlockRectTransform.SetSiblingIndex(Index + 1);
                else
                {
                    // If the block is nested into another, and the block below is not, it should no longer be nested when moved down, but should remain in the same position;
                    // Get the block it was nested into;
                    IsNested = false;
                    Parent.NestedBlocks.Remove(this);
                    Parent = null;
                    transform.Find("BlockBackground").GetComponent<Image>().sprite = BlockBackgrounds[0];
                }
            }
            else if (BlockBelow.CanHaveNestedBlocks && BlockBelow.NestedBlocks.Count != 0)
            {
                // If the block below has nested blocks, the current block should bypass them and be moved directly below the last nested block;
                Index = BlockBelow.NestedBlocks[^1].Index;
                BlockRectTransform.SetSiblingIndex(Index);
            }
            else if (NestedBlocks.Count != 0)
            {        
                // If the block has nested blocks, the entirety of the set is moved together;
                BlockRectTransform.SetSiblingIndex(Index + NestedBlocks.Count + 1);

                foreach (CodeBlock Block in NestedBlocks)
                {
                    int BlockIndex = Block.BlockRectTransform.GetSiblingIndex();
                    Block.BlockRectTransform.SetSiblingIndex(BlockIndex + NestedBlocks.Count + 1);
                }
            }
            else
            {
                // When moving downwards, the block is not nested into another regardless of the block below it having the possibility of nesting blocks;
                // This includes the case in which the block below it can have nested blocks, but does not contain any currently;
                IsNested = false;
                BlockRectTransform.SetSiblingIndex(Index + 1);
                transform.Find("BlockBackground").GetComponent<Image>().sprite = BlockBackgrounds[0];
            }
        }
        else if (Index == BlockRectTransform.parent.childCount - 1 && IsNested)
        {
            // If the block is the last one in the window and is nested, it should be able to move down, outside of the nested set;
            IsNested = false;
            Parent.NestedBlocks.Remove(this);
            Parent = null;
            transform.Find("BlockBackground").GetComponent<Image>().sprite = BlockBackgrounds[0];
        }

    }

    // Reset the nested blocks to not nested;
    public void ResetNestedBlocks()
    {
        foreach (CodeBlock Block in NestedBlocks)
        {
            Block.IsNested = false;
            Block.Parent.NestedBlocks.Remove(this);
            Block.Parent = null;
            Block.transform.Find("BlockBackground").GetComponent<Image>().sprite = BlockBackgrounds[0];
        }    
    }

    // Update which buttons are interactable;
    public void UpdateButtons()
    {
        Index = BlockRectTransform.GetSiblingIndex();

        // If the block is not first, it should be able to move upwards;
        if (Index > 0) UpButton.interactable = true;
        else UpButton.interactable = false;

        // If the block is not last, it should be able to move downwards;
        if (Index < BlockRectTransform.parent.childCount - 1 || IsNested) DownButton.interactable = true;
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

    public void SetIndex(int NewIndex)
    {
        ElementIndex = NewIndex;
    }
    public void OnValueChanged(GameObject CurrentObject)
    {
        string Value;     
        
        switch (ElementIndex)
        {
            case int i when i >= 0 && i <= 2:
                Value = CurrentObject.GetComponent<TMP_InputField>().text;        
                Values[ElementIndex] = Value;
                break;
            case int i when i == 3:
                TMP_Dropdown Dropdown = CurrentObject.GetComponent<TMP_Dropdown>();
                Value = Dropdown.options[Dropdown.value].text;
                Values[ElementIndex] = Value;
                break;
        }

        SetIndex(-1);
    }
}
