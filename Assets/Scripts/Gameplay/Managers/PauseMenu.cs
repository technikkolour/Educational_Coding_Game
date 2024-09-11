using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private GameManager GameManager;
    public GameObject MenuUI;

    // Start is called before the first frame update;
    void Start()
    {
       GameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (MenuUI.activeSelf == false)
            {
                MenuUI.SetActive(true);
                GameManager.PauseGame();
            }
            else 
            {
                MenuUI.SetActive(false);
                GameManager.UnpauseGame();
            }
    }

}
