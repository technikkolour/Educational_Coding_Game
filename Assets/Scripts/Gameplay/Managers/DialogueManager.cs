using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public Dictionary<int, List<string>> Dialogues = new()
    {
        {1, new(){ "",
                    "",
                    "",
                    ""} },
        {2, new(){ "",
                    "",
                    "",
                    ""} },
        {3, new(){ "",
                    "",
                    "",
                    ""} }
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
