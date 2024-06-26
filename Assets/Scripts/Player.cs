using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using TMPro;
using System;

public class Player : MonoBehaviour
{
    string CollidingObject;
    private bool inInteraction = false;
    private string prompt;

    public TMP_Text PromptText;
    public GameObject PromptObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int speed = 5;

        if(Input.GetKey(KeyCode.D))
                transform.Translate(speed * Time.deltaTime * UnityEngine.Vector2.right);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(speed * Time.deltaTime * UnityEngine.Vector2.left);
        if (Input.GetKey(KeyCode.W))
            transform.Translate(speed * Time.deltaTime * UnityEngine.Vector2.up);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(speed * Time.deltaTime * UnityEngine.Vector2.down);

        if (Input.GetKeyDown(KeyCode.E) && CollidingObject != "")
        {
            switch (inInteraction)
            {
                case false:
                    inInteraction = true;
                    PromptText.text = prompt;
                    PromptObject.SetActive(true);
                    Time.timeScale = 0.0f;
                    break;
                case true:
                    inInteraction = false;
                    PromptObject.SetActive(false);
                    Time.timeScale = 1.0f;
                    break;
            }
        }

    }

    void OnTriggerStay2D(Collider2D col)
    {
        CollidingObject = col.gameObject.name;
        
        prompt = CollidingObject switch
        {
            "NPC" => "Hi! You must be new here!",
            _ => "Hmm... Nothing to see here!",
        };
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        CollidingObject = "";
        prompt = "";
    }
}
