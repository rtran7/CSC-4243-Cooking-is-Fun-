using UnityEngine;

public class RollingVeg : MonoBehaviour
{

     public float amplitude = 5f;
    public float speed = 2f;
    public float rotationSpeed = 100f;
    public float edgePadding = 0.5f;

    private Vector3 startPos;
    private float phase;
    private bool isSliced = false;

    private float minX, maxX, minY, maxY;

    void Start()
    {
 
    
    startPos.x = Mathf.Clamp(startPos.x, minX, maxX);
    startPos.y = Mathf.Clamp(startPos.y, minY, maxY);
    transform.position = startPos;
    
    phase = Random.Range(0f, Mathf.PI * 2f);
    rotationSpeed = Random.Range(50f, 150f);
        
       
        Camera cam = Camera.main;
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));
        
        minX = bottomLeft.x + edgePadding;
        maxX = topRight.x - edgePadding;
        minY = bottomLeft.y + edgePadding;
        maxY = topRight.y - edgePadding;
    }

    void Update()
    {
        if (!isSliced)
        {
           
            float offset = Mathf.Sin(Time.time * speed + phase) * amplitude;
            float newX = startPos.x + offset;
            float newY = startPos.y;
            
          
            newX = Mathf.Clamp(newX, minX, maxX);
            newY = Mathf.Clamp(newY, minY, maxY);
            
            transform.position = new Vector3(newX, newY, startPos.z);
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }

    public void OnSliced()
    {
        isSliced = true;
        rotationSpeed = 0f;
    }
    
}