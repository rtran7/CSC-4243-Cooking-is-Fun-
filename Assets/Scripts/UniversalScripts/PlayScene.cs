using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayScene : MonoBehaviour
{
    public string scene;

    //Function for the restart button
    public void playScene()
    {

        SceneManager.LoadScene(scene);

    }
}
