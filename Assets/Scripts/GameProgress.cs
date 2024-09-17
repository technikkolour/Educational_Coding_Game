using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance { get; private set; }

    public List<int> Medals = new() { 0, 0, 0 };
    public List<bool> JournalEntriesFound = new(Enumerable.Repeat(false, 11));
    public List<Item> CurrentInventory = new();

    public List<bool> BossesCleared = new(Enumerable.Repeat(false, 3));    
    public List<bool> BlockagesCleared = new(Enumerable.Repeat(false, 100));    
    public List<bool> CompletedPuzzles = new(Enumerable.Repeat(false, 16));
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
