using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private SlicableTomato slicable;
    [SerializeField] private GameObject sliced;
    [SerializeField] private float minimumSliceSpeed = 100f;
    
    private Vector3 lastMousePosition;
    private float mouseVelocity;
    private bool alreadySliced = false;

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

    private void OnMouseOver()
    {
        if (TutorialScreen.tutorialPlaying || PauseScreen.gameIsPaused)
            return;

        if (alreadySliced)
            return;

      
        if (mouseVelocity >= minimumSliceSpeed)
        {
           
            PerformSlice();
        }
    }

    private void PerformSlice()
    {
        if (slicable.getSlicedObject() == null)
        {
            slicable.setSlicedObject(sliced);
            slicable.Slice();
            alreadySliced = true;
            
        }
    }
}