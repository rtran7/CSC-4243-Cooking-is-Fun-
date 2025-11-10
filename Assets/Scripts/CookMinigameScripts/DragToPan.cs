using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class DragToPan : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Refs")]
    public RectTransform rawChicken;            // this object
    public RectTransform panTarget;             // Canvas/PanTarget (UI Image)
    public Canvas canvas;                       // the parent Canvas
    public ChickenCookMinigame cookLogic;       // reference on GameHandler (or same Canvas)
    public CookingMeterController meter;        // the meter script
    public RectTransform meterGroup;
    public TMP_Text HeaderText;// Canvas/MeterGroupUI (to SetActive)

    CanvasGroup _cg;
    Vector2 _startPos;

    void Awake()
    {
        _cg = GetComponent<CanvasGroup>();
        if (rawChicken == null) rawChicken = (RectTransform)transform;
    }

    void Start()
    {
        _startPos = rawChicken.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData e)
    {
        _cg.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData e)
    {
        if (canvas == null) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, e.position, e.pressEventCamera, out var local);
        rawChicken.anchoredPosition = local;
    }

    public void OnEndDrag(PointerEventData e)
    {

     if (HeaderText) HeaderText.gameObject.SetActive(true);
        _cg.blocksRaycasts = true;

        bool overPan = RectTransformUtility.RectangleContainsScreenPoint(
            panTarget, e.position, e.pressEventCamera);



        if (overPan)
        {
            // snap to the pan
            rawChicken.anchoredPosition = panTarget.anchoredPosition;

            // begin cooking
            rawChicken.gameObject.SetActive(false);
            if (meterGroup) meterGroup.gameObject.SetActive(true);
            if (HeaderText) HeaderText.gameObject.SetActive(true);


            if (cookLogic) cookLogic.BeginCooking();
            if (meter) meter.Unfreeze();
        }
        else
        {
            // return to start
            rawChicken.anchoredPosition = _startPos;
        }
    }
}
