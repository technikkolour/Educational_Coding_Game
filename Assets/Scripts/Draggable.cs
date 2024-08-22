using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform OptionRectTransform;  
    
    public CanvasGroup Canvas;

    // Start is called before the first frame update
    void Start()
    {
        OptionRectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 
        Canvas.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        OptionRectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Canvas.blocksRaycasts = true;
    }
}
