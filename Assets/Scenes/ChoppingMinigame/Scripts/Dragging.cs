using UnityEngine;

public class Dragging : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;
    private Rigidbody2D rb;

    public AudioClip plateClip;
    private AudioSource src;

    private bool hasBeenCounted = false;
   
    [SerializeField] private float clickRadius = 1f; 


      void Awake()
    {
        GameObject audioObj = GameObject.Find("PlateSFX");
        src = audioObj.GetComponent<AudioSource>();
       
    }


    void Start()
    {
        mainCamera = Camera.main;

        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic; 
            rb.gravityScale = 0; 
            rb.linearDamping = 5f;
            rb.angularDamping = 3f;
            rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        }
    }

    void Update()
    {
      
        if (Input.GetMouseButtonDown(0) && !isDragging)
        {
            Vector3 mousePos = GetMouseWorldPosition();
            float distance = Vector2.Distance(transform.position, mousePos);
            
           
            if (distance <= clickRadius)
            {
               
                Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(mousePos, clickRadius);
                Dragging closestDraggable = null;
                float closestDistance = float.MaxValue;
                
                foreach (Collider2D col in nearbyColliders)
                {
                    Dragging draggable = col.GetComponent<Dragging>();
                    if (draggable != null && draggable.enabled)
                    {
                        float dist = Vector2.Distance(col.transform.position, mousePos);
                        if (dist < closestDistance)
                        {
                            closestDistance = dist;
                            closestDraggable = draggable;
                        }
                    }
                }
                
                if (closestDraggable == this)
                {
                    StartDragging(mousePos);
                }
            }
        }
        
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            StopDragging();
        }
    }

    void StartDragging(Vector3 mousePos)
    {
        isDragging = true;
        offset = transform.position - mousePos;
        
        if (rb != null)
        {
            rb.WakeUp();
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        
       
        VegMoves movingV = GetComponent<VegMoves>();
        if (movingV != null)
        {
            movingV.enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (isDragging && rb != null)
        {
            Vector3 targetPosition = GetMouseWorldPosition() + offset;
            rb.MovePosition(targetPosition);
            rb.angularVelocity = 0f;
        }
    }

    void StopDragging()
    {
        isDragging = false;
        
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        
        if (hasBeenCounted) return;
        
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


    

   void OnTriggerEnter2D(Collider2D other)
{
   
    if (other.CompareTag("Plate") && !hasBeenCounted && !isDragging)
    {
        hasBeenCounted = true;
        this.enabled = false;


         if (src != null && plateClip != null)
            {
                Debug.Log("hello");
                src.PlayOneShot(plateClip);
            }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.PieceReachedPlate();
        }

        Destroy(gameObject, 0.5f);
    }
}
    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }

    
}
