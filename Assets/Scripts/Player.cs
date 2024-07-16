using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using TMPro;
using System;
using System.Runtime.CompilerServices;

public class Player : MonoBehaviour
{
    private UnityEngine.Vector2 MovementDirection;
    private Rigidbody2D RBComponent;

    string CollidingObject;
    private bool inInteraction = false;
    private string prompt;

    public TMP_Text PromptText;
    public GameObject PromptObject;

    // Start is called before the first frame update
    void Start()
    {
        RBComponent = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GameManager manager;

        MovementDirection.x = Input.GetAxisRaw("Horizontal");
        MovementDirection.y = Input.GetAxisRaw("Vertical");

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

    // Called a set number or times, not depenent on framerate;
    void FixedUpdate()
    {
        float speed = 7;
        RBComponent.MovePosition(RBComponent.position + MovementDirection * speed * Time.fixedDeltaTime);
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
