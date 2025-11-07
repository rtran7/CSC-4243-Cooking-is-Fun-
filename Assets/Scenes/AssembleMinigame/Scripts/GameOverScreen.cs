using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverScreen : MonoBehaviour
{
    //Get GameOver game object
    private GameObject background;

    void Awake()
    {
        //Grab the transform of this child object
        Transform childTrans = transform.Find("GameOverBackground");
        // Use transform to get the game object
        background = childTrans.gameObject;
        //Turn off the background
        background.SetActive(false);

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Setup()
    {
        //Pause the game
        Time.timeScale = 0;
        background.SetActive(true);

    }

    //Function for the restart button
    public void restart()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
