using UnityEngine;

public class Dragging : MonoBehaviour
{
  private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;

    private bool hasBeenCounted = false;

    void Start()
    {
        mainCamera = Camera.main;
    }

   


    void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPosition();
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Plate") && !hasBeenCounted)
        {
            hasBeenCounted = true;
            this.enabled = false;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.PieceReachedPlate();
            }

            Destroy(gameObject, 0.5f);
        }
    }



    void OnMouseUp()
{
    isDragging = false;
    
    if (hasBeenCounted) return; // Already counted
    
    Collider2D[] overlaps = Physics2D.OverlapCircleAll(transform.position, 0.5f);
    foreach (Collider2D col in overlaps)
    {
        if (col.CompareTag("Plate"))
        {
            hasBeenCounted = true;
            this.enabled = false;
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PieceReachedPlate();
            }
            
            Destroy(gameObject, 0.5f);
            return;
        }
    }
}




    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }

   


}
