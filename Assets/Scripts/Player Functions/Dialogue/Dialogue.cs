using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Display the lines of dialogue;
    private float TextSpeed = 0.25f;
    private int CurrentLineIndex;

    public TMP_Text DialogueTextBox;
    public void DisplayLine(string Line)
    {
        CurrentLineIndex = 0;
    }
    IEnumerator TypeOutLine(string Line)
    {
        foreach (char Character in Line.ToCharArray())
        {
            DialogueTextBox.text += Character;
            yield return new WaitForSeconds(TextSpeed);
        }
    }
}
