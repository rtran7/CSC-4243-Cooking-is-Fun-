using UnityEngine;

public class HelpButton : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorial;

    public void helpButton()
    {
        //Pause game
        Time.timeScale = 0f;
        //Set tutorial active
        tutorial.SetActive(true);
    }
}
