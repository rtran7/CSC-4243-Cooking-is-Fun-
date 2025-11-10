using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour
{
    //Function for the restart button
    public void restart()
    {
        TutorialScreen.tutorialPlaying = false;
        PauseScreen.gameIsPaused = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
