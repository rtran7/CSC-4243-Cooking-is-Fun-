using UnityEngine;
using UnityEngine.EventSystems;
public class DragTesting : MonoBehaviour, IDragHandler, IPointerDownHandler, IBeginDragHandler,IEndDragHandler
{
    //Grab the transform, or position, of the ingredient
    private RectTransform rectTransform;
    //Grab the Canvas Group to disble interactibility
    private CanvasGroup canvasGroup;
    //Grab the Tortilla object
    private GameObject tortilla;
    //Grab the script from the victory screen
    private VictoryScreen victoryScript;

    //Grab the canvas object in order to grab its scale
    [SerializeField] private Canvas canvas;

    //Triggers when game starts
    private void Awake()
    {
        //Grab the transform
        rectTransform = GetComponent<RectTransform>();
        //Grab the canvas group
        canvasGroup = GetComponent<CanvasGroup>();
        //Grab the Tortilla object
        tortilla = GameObject.Find("Tortilla");
        //Set the gameobject's layer with this script to ingredients
        gameObject.layer = LayerMask.NameToLayer("Ingredients");

        //Grab the victoryManager game object
        GameObject victoryManager = GameObject.Find("VictoryManager");
        //Initialize victoryScript
        victoryScript = victoryManager.GetComponent<VictoryScreen>();
        //Increment ingredientCount
        victoryScript.incrementCount();
    }

    //This function triggers when player starts dragging the mouse
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    //This function triggers while player is dragging the mouse
    public void OnDrag(PointerEventData eventData)
    {
        //Grab the position of item and adjust position based on mouose movement
        //Divide by scale factor so item actually follows the mouse
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    //This function triggers when player stops dragging the mouse
    public void OnEndDrag(PointerEventData eventData)
    {
        RectTransform tortillaTrans = tortilla.GetComponent<RectTransform>();

        //Check if the object is on the tortilla
        if (IsOverlapping(rectTransform, tortillaTrans))
        {
            //Lock the ingredient position to tortilla
            rectTransform.anchoredPosition = tortillaTrans.anchoredPosition;
            //decrement ingredientCount
            victoryScript.decrementCount();
        }

        else { canvasGroup.blocksRaycasts = true; }
    }

    // This function triggers when player clicks on mousebutton
    public void OnPointerDown(PointerEventData eventData) {}

    //This function compares this object's transform to the tortilla object
    // If both of these objects overlap, lock this object's transform to the
    // tortilla object
    public bool IsOverlapping(RectTransform rectA, RectTransform rectB)
    {
        //Check if any of the parameters are null
        if (rectA == null || rectB == null) { return false; }

        //Create two arrays that will store corners for each rectangle
        Vector3[] cornerA = new Vector3[4];
        Vector3[] cornerB = new Vector3[4];

        // Grab the world coordinates. Not local
        rectA.GetWorldCorners(cornerA);
        rectB.GetWorldCorners(cornerB);

        //Create two new rectangles
        Rect rect1 = new Rect(cornerA[0], cornerA[2] - cornerA[0]);
        Rect rect2 = new Rect(cornerB[0], cornerB[2] - cornerB[0]);

        return rect1.Overlaps(rect2);

    }
}
