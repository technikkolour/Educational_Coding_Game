using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Scene management;
    private static string PreviousSceneName;

    // UI Screens;
    public GameObject DeathScreen;

    // Journal management;
    private List<string> TheoreticalCollection = new() { "" };

    // Player spawning properties;
    public Player Player;
    public GameObject AcademySpawn, WarehouseSpawn;

    // Progression management;
    private List<bool> BlockagesCleared = new(Enumerable.Repeat(false, 7));
    private List<int> Medals = new() { 0, 0, 0 };
    private List<bool> CompletedPuzzles = new(Enumerable.Repeat(false, 10));
    private List<bool> JournalEntriesFound = new(Enumerable.Repeat(false, 11));
    private List<Item> CurrentInventory = new();

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
        SceneManager.LoadScene("Level_02");
    }

    public void EnterWarehouse() 
    {
        SceneManager.LoadScene("Level_03");
    }

    public void EnterBattle()
    {
        SceneManager.LoadScene("RobotBattle");
        PauseGame();
    }
    public void DisplayDeathScreen()
    {
        DeathScreen.SetActive(true);
    }

    public void ReturnToCity() 
    {
        PreviousSceneName = SceneManager.GetActiveScene().name;

        StartCoroutine(LoadSceneWait("Level_01", () =>
        {
            Player = FindObjectOfType<Player>();
            AcademySpawn = GameObject.Find("AcademySpawn");
            WarehouseSpawn = GameObject.Find("WarehouseSpawn");

            Debug.Log(PreviousSceneName);

            if (PreviousSceneName == "Level_02") Player.transform.position = AcademySpawn.transform.position;
            else if (PreviousSceneName == "Level_03") Player.transform.position = WarehouseSpawn.transform.position;
        }));
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
    public void ClearBlockage(int Index)
    {
        BlockagesCleared[Index] = true;

    }

    public bool IsPuzzleCompleted(int Index)
    {
        return CompletedPuzzles[Index];
    }
    public void SetPuzzleCompleted(int Index)
    {
        CompletedPuzzles[Index] = true;
    }
}
