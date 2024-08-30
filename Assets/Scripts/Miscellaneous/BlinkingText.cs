using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlinkingText : MonoBehaviour
{
    public TMP_Text Underscore;

    // Start is called before the first frame update
    void Start()
    {
         StartCoroutine("Blink");       
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Blink()
    {
        while (true)
        {
            if (Underscore.IsActive()) Underscore.gameObject.SetActive(false);
            else Underscore.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.5f);
        }

    }
}
