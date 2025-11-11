using UnityEngine;
using UnityEngine.SceneManagement;
public class ResultButton : MonoBehaviour
{

    [SerializeField]
    private ChickenCookMinigame gameHandler;

    public string scene;

    public void continueButton()
    { 
    
        bool taskCompleted = gameHandler.checkTask();
        if (taskCompleted)
        {
            SceneManager.LoadScene(scene);
        }

        else { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }

        }
}
