using System.Collections;
using TMPro;
using UnityEngine;

public class TextTyping : MonoBehaviour
{
    public TMP_Text TextBox;    
    
    private float TextSpeed = 0.05f;
    private readonly string LineOfText = "Thank you for playing the game!";

    // Start is called before the first frame update
    void Start()
    {
        TextBox.text = "";        
        StartCoroutine(TypeOutText(LineOfText));
    }

    // Update is called once per frame
    void Update()
    {

    }    

    IEnumerator TypeOutText(string Text)
    {
        foreach (char Character in Text.ToCharArray())
        {
            TextBox.text += Character;
            yield return new WaitForSeconds(TextSpeed);
        }
    }
}
