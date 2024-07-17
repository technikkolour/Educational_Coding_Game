using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    public void LoadNextLevel()
    {
        
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
}