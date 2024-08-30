using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnderlinedText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text Text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Underline text only when the cursor is hovering over it;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Text.fontStyle = FontStyles.Underline;  
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Text.fontStyle = FontStyles.Normal;
    }
}
