using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform OptionRectTransform;
    private CanvasGroup OptionCanvasGroup;

    public Transform StartingParent;
    public Vector3 StartPosition;

    // Start is called before the first frame update
    void Start()
    {
        OptionRectTransform = GetComponent<RectTransform>();
        OptionCanvasGroup = GetComponent<CanvasGroup>();
        StartingParent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        StartPosition = transform.position;
        StartingParent = transform.parent;
        OptionCanvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        OptionRectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == StartingParent)
            transform.position = StartPosition;

        OptionCanvasGroup.blocksRaycasts = true;
        StartingParent = null;
    }
}
