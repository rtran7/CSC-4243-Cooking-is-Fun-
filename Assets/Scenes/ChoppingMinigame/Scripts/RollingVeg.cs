using UnityEngine;

public class RollingVeg : MonoBehaviour
{
    public float amplitude = 5f;
    public float frequency = 2f;
    public float rotationSpeed = 100f;

    private Vector3 startPos;
    private float phase;
    private bool isSliced = false;



    // Board boundaries
    public float minX, maxX, minY, maxY;

    void Start()
    {
        startPos = transform.position;
        phase = Random.Range(0f, Mathf.PI * 2f);
        rotationSpeed = Random.Range(50f, 150f);
    }

    void Update()
    {
        if (!isSliced)
        {
            // Rolling motion
            float time = Time.time;
            transform.position = new Vector3(
                startPos.x + amplitude * Mathf.Sin(frequency * time + phase),
                startPos.y + amplitude * Mathf.Cos(frequency * time + phase),
                startPos.z
            );
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }

    public void OnSliced()
    {
        isSliced = true;
        rotationSpeed = 0f;
    }

}