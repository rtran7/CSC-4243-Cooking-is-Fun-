using UnityEngine;

public class gameBounds : MonoBehaviour
{
    [SerializeField] private float wallThickness = 5f; // Increased from 3f
    [SerializeField] private float clampPadding = 0.3f;

    private float minX, maxX, minY, maxY;

    void Start()
    {
        CalculateBounds();
        CreateBoundaryWalls();
      
    }

    void CalculateBounds()
    {
        Camera cam = Camera.main;
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        minX = bottomLeft.x + clampPadding;
        maxX = topRight.x - clampPadding;
        minY = bottomLeft.y + clampPadding;
        maxY = topRight.y - clampPadding;
    }

    void CreateBoundaryWalls()
    {
        Camera cam = Camera.main;
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        float screenWidth = topRight.x - bottomLeft.x;
        float screenHeight = topRight.y - bottomLeft.y;

        CreateWall("BottomWall", 
            new Vector2((bottomLeft.x + topRight.x) / 2, bottomLeft.y - wallThickness / 2),
            new Vector2(screenWidth + wallThickness * 2, wallThickness));

        CreateWall("TopWall", 
            new Vector2((bottomLeft.x + topRight.x) / 2, topRight.y + wallThickness / 2),
            new Vector2(screenWidth + wallThickness * 2, wallThickness));

        CreateWall("LeftWall", 
            new Vector2(bottomLeft.x - wallThickness / 2, (bottomLeft.y + topRight.y) / 2),
            new Vector2(wallThickness, screenHeight + wallThickness * 2));

        CreateWall("RightWall", 
            new Vector2(topRight.x + wallThickness / 2, (bottomLeft.y + topRight.y) / 2),
            new Vector2(wallThickness, screenHeight + wallThickness * 2));
    }

    void CreateWall(string wallName, Vector2 position, Vector2 size)
    {
        GameObject wall = new GameObject(wallName);
        wall.transform.parent = transform;
        wall.transform.position = position;
        wall.layer = LayerMask.NameToLayer("Default");
        
        BoxCollider2D collider = wall.AddComponent<BoxCollider2D>();
        collider.size = size;
        collider.isTrigger = false; // Must be solid, not trigger
        
        Rigidbody2D rb = wall.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        
        PhysicsMaterial2D wallMaterial = new PhysicsMaterial2D("WallMaterial");
        wallMaterial.friction = 0.8f; // Increased friction to slow bouncing
        wallMaterial.bounciness = 0.2f; // Small bounce to absorb impact
        collider.sharedMaterial = wallMaterial;
    }
}

