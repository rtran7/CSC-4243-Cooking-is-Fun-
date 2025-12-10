using UnityEngine;

public class SlicableTomato : MonoBehaviour
{
    [SerializeField] private GameObject unslicedObject;
    private GameObject slicedObject;
    public AudioClip sliceClip;
    private AudioSource src;

    void Awake()
    {
        GameObject audioObj = GameObject.Find("VegetableSlice");
        src = audioObj.GetComponent<AudioSource>();
    }

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
        VegMoves rv = GetComponent<VegMoves>();
        if (rv != null)
        {
            src.PlayOneShot(sliceClip);
            rv.OnSliced();
        }
    }
}