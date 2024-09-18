using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
     // Scene management;
    private static string PreviousSceneName;

    // UI Screens;
    public GameObject DeathScreen;

    // Journal management;
    private List<string> TheoreticalCollection = new() { "" };

    // Player spawning properties;
    private Player Player;
    private DataManager DataManager;
    public GameObject AcademySpawn, WarehouseSpawn;
    public GameObject PostBossSpawn_01, PostBossSpawn_02, PostBossSpawn_03;

    // Progression management;
    private GameProgress GameProgress;

    private void Start()
    {
        GameProgress = FindObjectOfType<GameProgress>();
        Player = FindObjectOfType<Player>();
        DataManager = FindObjectOfType<DataManager>();

        PlacePlayer();
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
    private IEnumerator LoadSceneWait(string SceneName, System.Action OnSceneLoaded)
    {
        AsyncOperation AsyncLoadLevel = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);


        while (!AsyncLoadLevel.isDone)
        {
            yield return null;
        }

        yield return new WaitForEndOfFrame();

        OnSceneLoaded?.Invoke();
    }


    //####################################################################################################################################################################
    // GAME RELATED
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
        GameProgress.PreviousScene = SceneManager.GetActiveScene().name;
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
                GameObject SpawnLocation = GameObject.Find("PostBossSpawn_0" + i);                
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
            if (!GameProgress.CurrentInventory.Contains(Item)) GameProgress.CurrentInventory.Add(Item);
        }

    }
    public List<int> GetMedals()
    {
        return GameProgress.Medals;
    }
    public List<Item> GetInventory()
    {
        return GameProgress.CurrentInventory;
    }
    public bool HasItem(int ItemID)
    {
        Item Item = DataManager.ReturnItemForIndex(ItemID);
        return GameProgress.CurrentInventory.Contains(Item);
    }
    public void UseItem(int ItemID)
    {
        GameProgress.CurrentInventory.RemoveAt(ItemID);
    }

    // Handle Robot functionality;
    public void IncreaseHealth()
    {
        GameProgress.RobotHealth += 50f;
    }
}
