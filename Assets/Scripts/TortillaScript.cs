using UnityEngine;
using UnityEngine.EventSystems;

public class TortillaScript : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped");
    }
}
