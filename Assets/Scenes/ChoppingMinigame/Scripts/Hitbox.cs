using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created
   [SerializeField] private SlicableTomato slicable;
  [SerializeField] private GameObject sliced;

  public void OnMouseExit()
    {

    if(slicable.getSlicedObject() == null)
    {
      slicable.setSlicedObject(sliced);
      slicable.Slice();
      }
   
        
    }
}