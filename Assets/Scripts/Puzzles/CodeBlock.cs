using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeBlock : MonoBehaviour
{
    public Button UpButton, DownButton;
    public RectTransform BlockRectTransform;

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
}