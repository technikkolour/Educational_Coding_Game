using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject MenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (MenuUI.activeSelf == false)
            {
                MenuUI.SetActive(true);
                Time.timeScale = 0.0f;
            }
            else 
            {
                MenuUI.SetActive(false);
                Time.timeScale = 1.0f;
            }
    }


}
