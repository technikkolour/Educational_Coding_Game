using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // The lists are structured in the order: Prompt, Options (or Lines of Code), Solution;
    private readonly Dictionary<int, List<List<string>>> MultipleChoice = new(){
        { 1, new(){ new(){"How would you store your age?"}, new(){"As a variable.", "As a constant."}, new(){ "As a variable." } } },
        { 2, new(){ new(){"Would the number 3.5 be considered an integer?"}, new(){"Yes", "No"}, new(){"No"} } },
        { 3, new(){ new(){"Can you compare two variables of different types, such as a string and a boolean?"}, new(){ "Yes, but the result will always be false.", "Yes.", "No, it will result in an error." }, new(){ "No, it will result in an error." } }}
    };
    private readonly Dictionary<int, List<List<string>>> Quiz = new(){
        { 13, new(){ new(){"What is a variable?"}, new(){"A number", "A value that can change.", "A string."}, new(){"A value that changes."} } },
        { 14, new(){ new(){"How many values can a boolean have?"}, new(){"Just one.", "An infinite amount.", "Just two."}, new(){ "Just two." } } },
        { 15, new(){ new(){"What should I use if I would like to see the final result of a program?"}, new(){"An output block.", "A declarative block."}, new(){ "An output block." } } },
        { 16, new(){ new(){"If you would like to keep track of your favourite teacher's name, what would you store it as?"}, new(){"An integer.", "An list of characters.", "A string.", "A boolean."}, new(){"A string."} } },
        { 17, new(){ new(){"Is a variable the opposite of a constant?"}, new(){"True", "False"}, new(){"True"} } },
        { 18, new(){ new(){"Select the correct line of code."}, new(){"integer x = 12.5", "float x = false", "string = \"Hello!\""}, new(){ "string = \"Hello!\"" } } },
        { 19, new(){ new(){"What is absolutely necessary in a while loop?"}, new(){"A condition.", "An output statement.", "An if statement."}, new(){ "A condition." } } }
    };
    private readonly Dictionary<int, List<List<string>>> CodeOrdering = new(){
        { 4, new(){ new(){"Oh, it seems that the code for the chest is all mixed up. I should reorganise it!"}, new(){"open", "if password = 'correct'"}, new(){"21"} } },
        { 5, new(){ new(){"Hmm, this appears to be an automated gate and the code doesn't work... I think changing the order of the lines will fix it."}, new(){"for values of x increasing by 1, up to 5", "move 25 cm", "integer x = 1"}, new(){"312"} } }
    };

    // The lists are structured in the order: Prompt, Code (with missing values marked as underscores), Solution;
    private readonly Dictionary<int, List<List<string>>> ValueUpdate = new(){
        { 6, new(){ new(){"The gate to the city is locked. Maybe setting its value to unlocked will help me get through."}, new(){"gate = ' _________ '"}, new(){"unlocked"} } },
        { 7, new(){ new(){"This chest seems to be locked. Maybe changing a value in the code will open it..."}, new(){ "if 'chest is unlocked' = _________ \n    open" }, new(){"true"} } },
        { 8, new(){ new(){"This robot seems to have been programmed not to let anyone move past it. I should change its code."}, new(){ "if 'humans are not allowed' = _______ \n    let them past" }, new(){ "false" } } },
        { 9, new(){ new(){"This automated gate seems like it's fully functional. I should have a deeper look at the code..."}, new(){ "if 'gate can open' = true \n    _______ 35 cm" }, new(){""} } }
    };

    // The lists are structured in the order: Prompt, Possible Solutions;
    private readonly Dictionary<int, List<List<string>>> CodeBuilding = new(){
        { 10, new(){ new(){"Please write a program that compares two numbers a = 10, b = 32, and outputs whether a is greater than b."}, new(){"false"} } },
        { 11, new(){ new(){"Please write a function that adds up the numbers from 1 to 100 and prints out the result."}, new(){"5050"} } },
        { 12, new(){ new(){"Please write a program that computes the average of the numbers between 11 and 27."}, new(){"19"} } },
        { 999, new(){ new(){ "Here you get to build your own robot to control in the battles! The robot can already move left and right using the same controls as the human character! Your Strength stat is limited to 250." }, new(){ "" } } }
    };

    // Code Building Blocks;
    private readonly Dictionary<string, string> CodeBuildingBlocks = new()
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

    // Get the details corresponding to the puzzle identified by its ID;
    public List<List<string>> ReturnPuzzleDetails(int PuzzleID)
    {
        List<List<string>> Puzzle = new();

        switch (PuzzleID)
        {
            case int i when i >= 1 && i <= 3:
                Puzzle = MultipleChoice[PuzzleID];
                break;
            case int i when i >= 4 && i <= 5:
                Puzzle = CodeOrdering[PuzzleID];
                break;
            case int i when i >= 6 && i <= 9:
                Puzzle = ValueUpdate[PuzzleID];
                break;
            case int i when (i >= 10 && i <= 12) || i == 999:
                Puzzle = CodeBuilding[PuzzleID];
                break;
            case int i when i >= 13 && i <= 19:
                Puzzle = Quiz[PuzzleID];
                break;
        }

        return Puzzle;
    }
}
