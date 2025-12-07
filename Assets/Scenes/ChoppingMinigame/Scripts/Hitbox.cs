using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private SlicableTomato slicable;
    [SerializeField] private GameObject sliced;
    [SerializeField] private float minimumSliceSpeed = 2f; 

    private Vector3 lastMousePosition;
    private float mouseVelocity;

    void Start()
    {
        lastMousePosition = Input.mousePosition;
    }

    void Update()
    {
       
        Vector3 currentMousePosition = Input.mousePosition;
        float distance = Vector3.Distance(currentMousePosition, lastMousePosition);
        mouseVelocity = distance / Time.deltaTime;
        lastMousePosition = currentMousePosition;
    }

    public void OnMouseExit()
    {
        if (TutorialScreen.tutorialPlaying || PauseScreen.gameIsPaused)
            return;

        
        if (mouseVelocity < minimumSliceSpeed)
            return;

        if (slicable.getSlicedObject() == null)
        {
            slicable.setSlicedObject(sliced);
            slicable.Slice();
        }
    }
}