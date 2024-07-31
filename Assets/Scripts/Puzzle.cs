using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    private int Attempts = 0;
    private bool Done = false;
    private int Prize_ItemID = 0;
    private string Prompt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetID()
    {
        return Prize_ItemID;
    }

    public string GetPrompt()
    {
        return Prompt;
    }

    public int GetAttempts()
    {
        return Attempts;
    }
    public void SetAttempts()
    {
        Attempts++;
    }

    public void SetCompleted()
    {
        Done = true;
    }

    public virtual bool VerifySolution()
    {
        return false;
    }
}