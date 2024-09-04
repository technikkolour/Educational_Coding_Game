using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // The lists are structured in the order: Prompt, Options (or Lines of Code), Solution;
    private Dictionary<int, List<List<string>>> MultipleChoice = new(){
        { 1, new(){ new(){"Would the number 3 be considered an integer?"}, new(){"Yes", "No"}, new(){"Yes"} } },
        { 2, new(){ new(){""}, new(){""}, new(){""} } }
    };
    private Dictionary<int, List<List<string>>> Quiz = new(){
        { 10, new(){ new(){""}, new(){""}, new(){""} } },
        { 11, new(){ new(){""}, new(){""}, new(){""} } },
        { 12, new(){ new(){""}, new(){""}, new(){""} } },
        { 13, new(){ new(){""}, new(){""}, new(){""} } },
        { 14, new(){ new(){""}, new(){""}, new(){""} } },
        { 15, new(){ new(){""}, new(){""}, new(){""} } },
        { 16, new(){ new(){""}, new(){""}, new(){""} } }
    };
    private Dictionary<int, List<List<string>>> CodeOrdering = new(){
        { 3, new(){ new(){"Hmm, the code for the gate doesn't work... I think changing the order of the lines will fix it."}, new(){ "for values of x increasing by 1, up to 5", "move 25 cm", "integer x = 1"}, new(){"312"} } }
    };

    // The lists are structured in the order: Prompt, Code (with missing values marked as underscores), Solution;
    private Dictionary<int, List<List<string>>> ValueUpdate = new(){
        { 4, new(){ new(){"The gate to the city is locked. Maybe setting its value to unlocked will help me get through."}, new(){"gate = ' _________ '"}, new(){"unlocked"} } },
        { 5, new(){ new(){"This gate seems to be locked. Maybe changing a value in the code will open it..."}, new(){ "if 'gate is unlocked' = _________ \n    open" }, new(){"True"} } },
        { 6, new(){ new(){ "" }, new(){ "" }, new(){ "" } } } 
    };

    // The lists are structured in the order: Prompt, Possible Solutions;
    private Dictionary<int, List<List<string>>> CodeBuilding = new(){
        { 7, new(){ new(){"I want to add up all of the numbers up to 100. Could you write a function that does that for me?"}, new(){""} } },
        { 8, new(){ new(){"This robot just needs to move backwrds 10 steps so I can pass. I should change its code so it does that."}, new(){""} } },
        { 9, new(){ new(){""}, new(){""} } }
    };

    // Code Building Blocks;
    private Dictionary<string, string> CodeBuildingBlocks = new()
    {
        { "Integer", "integer _______ = _______" },
        { "Float", "float _______ = _______" },
        { "Boolean", "boolean _______ = _______" },
        { "String", "string _______ = _______" },
        { "Array", "array _______ = [_______, _______]" },
        { "Mathematical Operation", " _______ = _______ ------ _______"},
        { "Output", "print _______"},
        { "If Statement", "if _______ ------ _______ do" },
        { "For Loop", "for values of _______ increasing by _______, up to _______, do" },
        { "While Loop", "while _______ ------ _______ do" },
        { "Move In Direction", "move ------ with speed _______" },
        { "Attack With Power", "attack with _______ power" },
        { "Assign Key", "if ------ is pressed down do"}
    };

    // Inventory Data;
    public List<Item> Items = new();
    public List<Sprite> Sprites = new(4);

    // Start is called before the first frame update
    void Start()
    {
        Items.Add(Item.CreateInstance("Robot Licence", Sprites[0], "With this I can build my own robot!", 0));
        Items.Add(Item.CreateInstance("Rusty Circuit Board", Sprites[1], "Wow! I've never seen one in real life until now!", 1));
        Items.Add(Item.CreateInstance("Multicolour Pen", Sprites[2], "It's so cool! I've always wanted one!", 2));
        Items.Add(Item.CreateInstance("Smartphone", Sprites[3], "Hmm... Apparently this is the phone of the future!", 3));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Get the item corresponding to the given index;
    public Item ReturnItemForIndex(int Index)
    {
        return Items[Index];
    }

    // Get the code block for the 
    public string ReturnCodeBlockText(string Type)
    {
        return CodeBuildingBlocks[Type];
    }

    // Get the details correspomding to the puzzle identified by its ID;
    public List<List<string>> ReturnPuzzleDetails(int PuzzleID)
    {
        List<List<string>> Puzzle = new();

        switch (PuzzleID)
        {
            case int i when i >= 1 && i <= 2:
                Puzzle = MultipleChoice[PuzzleID];
                break;
            case int i when i == 3:
                Puzzle = CodeOrdering[PuzzleID];
                break;
            case int i when i >= 4 && i <= 6:
                Puzzle = ValueUpdate[PuzzleID];
                break;
            case int i when i >= 7 && i <= 9:
                Puzzle = CodeBuilding[PuzzleID];
                break;
            case int i when i >= 10 && i <= 11:
                Puzzle = Quiz[PuzzleID];
                break;
        }

        return Puzzle;
    }
}
