using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private static string PreviousSceneName;
    private List<string> TheoreticalCollection = new() { "" };

    // Spawning properties;
    public Player Player;
    public GameObject AcademySpawn, WarehouseSpawn;

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
    // MULTIPLAYER
    public void EnterMultiplayerLobby()
    {
        SceneManager.LoadScene("MultiplayerLobby");
    }


}
