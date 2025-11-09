using UnityEngine;

public class Dragging : MonoBehaviour
{
  private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;

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

    void OnMouseUp()
    {
        isDragging = false;
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }

     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Plate"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PieceReachedPlate();
            }

            Destroy(gameObject);
        }
    }

   


}
