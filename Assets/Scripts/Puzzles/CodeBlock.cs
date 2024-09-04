using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeBlock : MonoBehaviour
{
    public Button UpButton, DownButton;
    public RectTransform BlockRectTransform;
    public string Type;

    // The blocks that
    private List<CodeBlock> NestedBlocks = new List<CodeBlock>();
    private int Index;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateButtons();
    }

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
