using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        GameObject PreviousLine = FindChildWithTag("CodeLine");

        GameObject DroppedObject = eventData.pointerDrag;
        Transform InitialParent = DroppedObject.GetComponent<Draggable>().StartingParent;

        if (DroppedObject != null)
        {
            DroppedObject.transform.SetParent(transform);
            DroppedObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            PreviousLine.transform.SetParent(InitialParent);
            PreviousLine.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }

    public void PlaceLines(string Order)
    {
        for (int i = 0; i < 3; i++)
        {
            Transform Place = GameObject.Find("Place_0" + (i + 1)).transform;
            GameObject Line = GameObject.Find("CodeLine_0" + Order[i]);

            Line.transform.SetParent(Place);
            Line.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }

    GameObject FindChildWithTag(string Tag)
    {
        foreach (Transform Child in transform)
        {
            if (Child.CompareTag(Tag))
            {
                return Child.gameObject;
            }
        }
        return null;
    }
}
