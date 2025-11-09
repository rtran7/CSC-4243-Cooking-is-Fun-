using UnityEngine;

public class PauseScreen : MonoBehaviour
{   //Grab menu
    public GameObject menu;

    // Check if we are paused
    private bool isPaused;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //Turn SetActive menu
        menu.SetActive(false);
        //Assume we're not paused
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if ESC key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                pause();
        }
    }

    private void pause()
    {

        //Turn on menu
        menu.SetActive(true);
        //Set time scale to 0
        Time.timeScale = 0f;
        //Set isPaused to true
        isPaused = true;

    }

    public void Resume()
    {
        //Turn on menu
        menu.SetActive(false);
        //Set time scale to 1
        Time.timeScale = 1f;
        //Set isPaused to false
        isPaused = false;
    }
}
