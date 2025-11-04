using UnityEngine;
using UnityEngine.EventSystems;
public class DragTesting : MonoBehaviour, IDragHandler, IPointerDownHandler, IBeginDragHandler,IEndDragHandler
{
    //Grab the transform, or position, of the item
    private RectTransform rectTransform;
    //Grab the Canvas Group to disble interactibility
    private CanvasGroup canvasGroup;

    //Grab the canvas object in order to grab its scale
    [SerializeField] private Canvas canvas;

    //Triggers when game starts
    private void Awake()
    {
        //Grab the transform
        rectTransform = GetComponent<RectTransform>();
        //Grab the canvas group
        canvasGroup = GetComponent<CanvasGroup>();
    }

    //This function triggers when player starts dragging the mouse
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Start Dragging");
        canvasGroup.blocksRaycasts = false;
    }

    //This function triggers while player is dragging the mouse
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging");
        //Grab the position of item and adjust position based on mouose movement
        //Divide by scale factor so item actually follows the mouse
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    //This function triggers when player stops dragging the mouse
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Stop Dragging");
        canvasGroup.blocksRaycasts = true;
    }

    // This function triggers when player clicks on mousebutton
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked");
    }
}
