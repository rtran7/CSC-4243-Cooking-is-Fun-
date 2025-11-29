using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI; // Needed for Image

public class StationButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Image img;

    [SerializeField]
    private string scene;

    void Start()
    {
        img = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // When cursor enters the image
        Color c = img.color;
        c.a = 1f; // fully opaque
        img.color = c;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // When cursor leaves the image
        Color c = img.color;
        c.a = 0.8f; // slightly transparent
        img.color = c;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Load the target scene when clicked
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
