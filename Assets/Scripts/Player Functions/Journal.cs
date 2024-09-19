using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Journal : MonoBehaviour
{
    private Dictionary<int, List<string>> AllEntries = new() {
        {0, new(){ "Variables", "Variables are values that can change. For example, your age!" } },
        {1, new(){ "Constants", "Constants are values that cannot change, such as gravity, or your eye colour." } },
        {2, new(){ "String", "Used to store text; a string represents a series of characters. It can be a name, an address, even a phone number!" } },
        {3, new(){ "Output", "The values of variables are hidden after they are initialised and used inside a program. To view their current values we can use output statements." } },
        {4, new(){ "Integers and Floats", "Integeers are used to store whole numbers, like the number of people present in a room.\nFloats are used to store decimals, such as your height." } },
        {5, new(){ "Boolean", "Used to store true or false values." } },
        {6, new(){ "Arrays", "They can store multiple values of the same type together. For example, they can be used for storing a student's grades (floats) or a list of the students enrolled in a class (strings)." } },
        {7, new(){ "If Statements", "Used to perform some actions based on one or more conditions." } },
        {8, new(){ "For", "A set of actions is performed for every time a value is updated. The starting and final values, as well as the step size, are defined." } },
        {9, new(){ "While", "A set of actions is performed as long as a condition is true." } },
        {10, new(){ "Functions", "They are independent portions of code. They can be used to tell the program to perform a set of actions at any point." } },
        {11, new(){ "", "" } } };
    private List<List<string>> Entries = new() { new() { "", "" }, new() { "", "" } };

    public TMP_Text LeftTitle, RightTitle;
    public TMP_Text LeftText, RightText;
    public GameObject JournalUI;
    public Button BackwardButton, ForwardButton;

    private GameManager GameManager;
    private Player Player;
    private int left = 0, right = 1; // Store the current entry displayed on each page;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEntries();

        if (right == (Entries.Count / 2) * 2 - 1) ForwardButton.gameObject.SetActive(false);
        else if (Entries.Count > 3) ForwardButton.gameObject.SetActive(true);
        if (left == 0) BackwardButton.gameObject.SetActive(false);

        DisplayText();
    }

    // Journal UI interaction functions;
    public void OpenJournal()
    {
        JournalUI.SetActive(true);
        GameManager.PauseGame();
    }
    public void CloseJournal()
    {
        JournalUI.SetActive(false);
        GameManager.UnpauseGame();
    }
    public void DisplayText()
    {
        LeftTitle.text = Entries[left][0];
        LeftText.text = Entries[left][1];

        RightTitle.text = Entries[right][0];
        RightText.text = Entries[right][1];
    }

    // Journal navigation functions;
    public void Forward()
    {
        if (right < Entries.Count - 2)
        {
            left += 2;
            right += 2;

            BackwardButton.gameObject.SetActive(true);
        }
    }
    public void Backward()
    {
        if (left > 0)
        {
            left -= 2;
            right -= 2;

            ForwardButton.gameObject.SetActive(true);
        }
    }

    // Build the content of the player's journal;
    void UpdateEntries()
    {
        for (int i = 0; i < GameManager.GetJournalEntriesFound().Count; i++)
        {
            if (GameManager.GetJournalEntriesFound()[i] == true && !Entries.Contains(AllEntries[i]))
            {
                if (Entries[0][1] == "" && Entries[1][1] == "") Entries.RemoveAt(0);
                Entries.Insert(Entries.Count - 1, AllEntries[i]);
            }
        }
    }
}
