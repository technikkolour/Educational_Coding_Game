using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    // UI Screens;
    public GameObject DeathScreen;
    public GameObject ButtonPromptsCanvas;

    // Journal management;
    private List<string> TheoreticalCollection = new() { "" };

    // Player spawning properties;
    private Player Player;
    private DataManager DataManager;
    public GameObject AcademySpawn, WarehouseSpawn;
    public GameObject PostBossSpawn_01, PostBossSpawn_02, PostBossSpawn_03;

    // Progression management;
    private GameProgress GameProgress;

    private void Awake()
    {
        GameProgress = FindObjectOfType<GameProgress>(); 
    }

    private void Start()
    {
        Player = FindObjectOfType<Player>();
        DataManager = FindObjectOfType<DataManager>();

        PlacePlayer();

        if (SceneManager.GetActiveScene().name == "Level_01" && GameProgress.PreviousScene == "")
        {
            DisplayPrompts();
            Invoke("HidePrompts", 5);
        }
    }

    //####################################################################################################################################################################
    // MENU RELATED
    // Start the game from the first level;
    public void StartGame()
    {
        SceneManager.LoadScene("Level_01");
    }
    // Close the game;
    public void ExitGame()
    {
        Application.Quit();
    }
    // Return to the main menu; current game progress is lost;
    public void ReturnToMenu() 
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void EndGame()
    {
        SceneManager.LoadScene("EndGame");
    }


    //####################################################################################################################################################################
    // GAME NAVIGATION RELATED
    public void EnterAcademy()
    {
        GameProgress.PreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Level_02");
    }

    public void EnterWarehouse() 
    {
        GameProgress.PreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Level_03");
    }

    public void EnterBattle()
    {
/*        GameProgress.PreviousScene = SceneManager.GetActiveScene().name;*/
        SceneManager.LoadScene("RobotBattle");
        PauseGame();
    }
    public void DisplayDeathScreen()
    {
        DeathScreen.SetActive(true);
    }

    public void ReturnToCity() 
    {
        GameProgress.PreviousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Level_01");       
    }
    public void PlacePlayer()
    {
        if (SceneManager.GetActiveScene().name == "Level_01" && GameProgress.PreviousScene == "Level_02")
            Player.transform.position = AcademySpawn.transform.position;
        if (SceneManager.GetActiveScene().name == "Level_01" && GameProgress.PreviousScene == "Level_03")
            Player.transform.position = WarehouseSpawn.transform.position;

        for (int i = 2; i >= 0; i--)
            if (SceneManager.GetActiveScene().name == "Level_03" && GameProgress.PreviousScene == "RobotBattle" && GameProgress.BossesCleared[i] == true)
            {
                GameObject SpawnLocation = GameObject.Find("PostBossSpawn_0" + (i + 1));                
                Player.transform.position = SpawnLocation.transform.position;
            }
    }

    // When the game is paused, time no longer passes and the player becomes unable to move;
    public void PauseGame()
    {
        Time.timeScale = 0.0f;
    }
    // When the game is unpaused, the flow of time continues and the player regains their ability to move;
    public void UnpauseGame()
    {
        Time.timeScale = 1.0f;
    }

    // Return the journal entry corresponding to the ID;
    public string GetTheoreticalEntry(int EntryID)
    {
        return TheoreticalCollection[EntryID];
    }

    // Functions for handling button prompts;
    public void DisplayPrompts()
    {
        ButtonPromptsCanvas.SetActive(true);
    }
    public void HidePrompts()
    {
        ButtonPromptsCanvas.SetActive(false);
    }

    //####################################################################################################################################################################
    // MULTIPLAYER RELATED
    public void EnterMultiplayerLobby()
    {
        SceneManager.LoadScene("MultiplayerLobby");
    }


    //####################################################################################################################################################################
    // PROGRESSION RELATED
    public bool IsCleared(int Index)
    {
        Debug.Log(GameProgress.BlockagesCleared.Count);
        return GameProgress.BlockagesCleared[Index];
    }
    public List<bool> GetClearedBlockages()
    {
        return GameProgress.BlockagesCleared;
    }
    public void ClearBlockage(int Index)
    {
        GameProgress.BlockagesCleared[Index] = true;
    }

    // Handle Puzzles;
    public bool IsPuzzleCompleted(int Index)
    {
        return GameProgress.CompletedPuzzles[Index - 1];
    }
    public void CompletedPuzzle(int PuzzleID, int Attempts)
    {
        GameProgress.CompletedPuzzles[PuzzleID - 1] = true;

        if (Attempts <= 3) GameProgress.Medals[Attempts - 1] += 1;
    }

    // Handle Journal entries;
    public List<bool> GetJournalEntriesFound()
    {
        return GameProgress.JournalEntriesFound;
    }
    public void FoundEntry(int EntryID)
    {
        GameProgress.JournalEntriesFound[EntryID - 1] = true;
    }
    public bool IsEntryFound(int EntryID)
    {
        return GameProgress.JournalEntriesFound[EntryID - 1];
    }

    // Handle Inventory;
    public void PickUpItem(int ItemID)
    {
        if (ItemID >= 0)
        {
            Item Item = DataManager.ReturnItemForIndex(ItemID);
            if (!HasItem(ItemID)) GameProgress.CurrentInventory.Add(Item);
        }
    }    
    public bool HasItem(int ItemID)
    {            
        bool Contains = false;

        if (ItemID >= 0)
        {
            Item Item = DataManager.ReturnItemForIndex(ItemID);

            foreach (Item InvItem in GameProgress.CurrentInventory)
            {
                Contains = false;
                if (Item.ItemID == InvItem.ItemID) Contains = true;
            }
        }

        return Contains;
    }
    public void UseItem(int ItemID)
    {
        GameProgress.CurrentInventory.RemoveAt(ItemID);
    }

    public List<int> GetMedals()
    {
        return GameProgress.Medals;
    }
    public List<Item> GetInventory()
    {
        return GameProgress.CurrentInventory;
    }


    // Handle Robot functionality;
    public void IncreaseHealth()
    {
        GameProgress.RobotHealth += 50f;
    }
    public float CurrentTotalHealth()
    {
        return GameProgress.RobotHealth;
    }

    // Handle Boss completion;
    public void DefeatedBoss(int Index)
    {
        GameProgress.BossesCleared[Index] = true;
    }
    public bool IsDefeated(int Index)
    {
        return GameProgress.BossesCleared[Index];
    }
}
