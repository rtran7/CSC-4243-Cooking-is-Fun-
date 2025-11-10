using UnityEngine;

public class Victory2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
 private GameObject background;

    //Grab AudioSource
    private AudioSource src;

    //Check if audiosource is played
    private bool played;

    void Awake()
    {
        //Grab the transform of this child object
        Transform childTrans = transform.Find("VictoryBackground");
        // Use transform to get the game object
        background = childTrans.gameObject;
        //Turn off the background
        background.SetActive(false);

        //Grab audios source object
        GameObject audioObj = GameObject.Find("VictorySound");
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
}
