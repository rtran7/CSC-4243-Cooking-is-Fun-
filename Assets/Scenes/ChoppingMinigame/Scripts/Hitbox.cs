using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
  
   [SerializeField] private SlicableTomato slicable;
  [SerializeField] private GameObject sliced;

  public void OnMouseExit()
  {
      if (TutorialScreen.tutorialPlaying || PauseScreen.gameIsPaused)
           return;

    if(slicable.getSlicedObject() == null)
    {
      slicable.setSlicedObject(sliced);
      slicable.Slice();
      }
   
        
    }
}