using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private List<string> TheoreticalCollection = new List<string>() { "" };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    public void ReturnToCity() 
    {
        SceneManager.LoadScene("Level_01");
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
