using UnityEngine;
using UnityEngine.EventSystems;

public class TortillaScript : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //If an item is placed on this tortilla, change its transform to match
        // Tortilla's transform
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition
                    = GetComponent<RectTransform>().anchoredPosition;
        }
    }
}
