using UnityEngine;

public class TutorialScreen : MonoBehaviour
{

    //Get Tutorial game object
    private GameObject background;

    public static bool tutorialPlaying=false;

    void Awake()
    {
        //Grab the transform of this child object
        Transform childTrans = transform.Find("TutorialScreen");
        // Use transform to get the game object
        background = childTrans.gameObject;
        //Turn on the background
        background.SetActive(true);
        //Pause the game
        Time.timeScale = 0;
        tutorialPlaying=true;

    }

    public void startGame()
    {
        background.SetActive(false);
        tutorialPlaying = false;
        Time.timeScale = 1;
    }
    public void startTutorial()
    {
        background.SetActive(true);
        tutorialPlaying = true;
        Time.timeScale = 0;
    }
}
