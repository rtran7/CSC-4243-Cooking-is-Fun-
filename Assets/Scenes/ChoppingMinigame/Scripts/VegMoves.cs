using UnityEngine;

public class VegMoves : MonoBehaviour
{
    
    public float edgePadding = 1f;
    
    private enum MovementType { Rolling, Bouncing, ZigZag }
    private MovementType currentMovement;
    
    private Vector3 startPos;
    private float phase;
    private bool isSliced = false;
    private float minX, maxX, minY, maxY;
    private float rotationSpeed;
    private float speed;
    private float amplitude;
    private float direction = 1f;
    private float angle;

    void Start()
    {
       
        Camera cam = Camera.main;
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));
        
        minX = bottomLeft.x + edgePadding;
        maxX = topRight.x - edgePadding;
        minY = bottomLeft.y + edgePadding;
        maxY = topRight.y - edgePadding;
        
       
        startPos = transform.position;
        
     
        int movementCount = System.Enum.GetValues(typeof(MovementType)).Length;
        currentMovement = (MovementType)Random.Range(0, movementCount);
        
        
        phase = Random.Range(0f, Mathf.PI * 2f);
        rotationSpeed = Random.Range(50f, 150f);
        speed = Random.Range(2f, 4f); 
        amplitude = Random.Range(8f, 15f); 
        angle = Random.Range(0f, Mathf.PI * 2f);
        
        
    }

    void Update()
    {
        if (isSliced) 
            return;
        
        switch (currentMovement)
        {
            case MovementType.Rolling:
                UpdateRolling();
                break;
            case MovementType.Bouncing:
                UpdateBouncing();
                break;
            case MovementType.ZigZag:
                UpdateZigZag();
                break;
        }
        
       
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
        
       
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    void UpdateRolling()
    {
        float offset = Mathf.Sin(Time.time * speed + phase) * amplitude;
        float newX = startPos.x + offset;
        float newY = startPos.y;
        
        transform.position = new Vector3(newX, newY, startPos.z);
    }

    void UpdateBouncing()
    {
       
        float verticalOffset = Mathf.Abs(Mathf.Sin(Time.time * speed + phase)) * amplitude;
        float horizontalDrift = Mathf.Sin(Time.time * speed * 0.5f + phase) * amplitude * 0.5f;
        
        float newX = startPos.x + horizontalDrift;
        float newY = startPos.y + verticalOffset;
        
        transform.position = new Vector3(newX, newY, startPos.z);
    }


    void UpdateZigZag()
    {
       
        float newX = transform.position.x + direction * speed * 3f * Time.deltaTime;
       
        if (newX <= minX)
        {
            direction = 1f;
            newX = minX;
        }
        else if (newX >= maxX)
        {
            direction = -1f;
            newX = maxX;
        }
        
        float newY = transform.position.y + Mathf.Sin(Time.time * speed) * 0.5f;
        
        transform.position = new Vector3(newX, newY, startPos.z);
    }


    public void OnSliced()
    {
        isSliced = true;
        rotationSpeed = 0f;
    }
}