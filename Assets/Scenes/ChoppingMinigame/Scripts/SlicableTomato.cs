using UnityEngine;

public class SlicableTomato : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject unslicedObject;
    private GameObject slicedObject;

    public void setSlicedObject(GameObject slicedObject)
    {
        this.slicedObject = slicedObject;
    }

    public GameObject getSlicedObject()
    {
        return slicedObject;
    }


    public void Slice()
{
    unslicedObject.SetActive(false);
    slicedObject.SetActive(true);
    RollingVeg rv = GetComponent<RollingVeg>();
    if (rv != null)
    {
        rv.OnSliced();
    }
}
}
