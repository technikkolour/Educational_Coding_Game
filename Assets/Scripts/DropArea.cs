using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropArea : MonoBehaviour, IDropHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Hi!");

        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject != null)
        {
            droppedObject.transform.SetParent(transform);
            droppedObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        PlaceLines("123");
    }

    public void PlaceLines(string Order)
    {
        for (int i = 0; i<3; i++)
        {
            Transform Place = GameObject.Find("Place_0" + (i + 1)).transform;
            GameObject Line = GameObject.Find("CodeLine_0" + Order[i]);

            Line.transform.SetParent(Place);
            Line.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }
}
