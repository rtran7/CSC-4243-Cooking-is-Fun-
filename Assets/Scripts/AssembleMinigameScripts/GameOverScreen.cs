using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverScreen : MonoBehaviour
{
    //Get GameOver game object
    private GameObject background;

    //Grab AudioSource
    private AudioSource src;

    //Check if audiosource is played
    private bool played;

    void Awake()
    {
        //Grab the transform of this child object
        Transform childTrans = transform.Find("GameOverBackground");
        // Use transform to get the game object
        background = childTrans.gameObject;
        //Turn off the background
        background.SetActive(false);

        //Grab audios source object
        GameObject audioObj = GameObject.Find("GameOverSound");
        //Grab audio source
        src = audioObj.GetComponent<AudioSource>();

        //Initialize played
        played = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Setup()
    {
        //Pause the game
        Time.timeScale = 0;
        background.SetActive(true);
        //Play audio source if not played
        if (!played) { src.Play(); played = true;}

    }

    //Function for the restart button
    public void restart()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
