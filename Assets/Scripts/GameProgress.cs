using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance { get; private set; }

    public List<bool> BlockagesCleared = new(Enumerable.Repeat(false, 7));
    public List<int> Medals = new() { 0, 0, 0 };
    public List<bool> CompletedPuzzles = new(Enumerable.Repeat(false, 16));
    public List<bool> JournalEntriesFound = new(Enumerable.Repeat(false, 11));
    public List<Item> CurrentInventory = new();
    public float RobotHealth = 150f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
