using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{

    private GameplayManager managerInstance;
    private BackgroundMusic musicInstance;
    private GameplayManager gameplayManager;
    private BackgroundMusic backMusic;
    void Start()
    {
        try
        {
            GameObject tempObj = GameObject.Find("GameplayManager");
            gameplayManager = tempObj.GetComponent<GameplayManager>();
            managerInstance = gameplayManager.getInstance();

            tempObj = GameObject.Find("BackgroundMusic");
            backMusic = tempObj.GetComponent<BackgroundMusic>();
            musicInstance = backMusic.getInstance();
        }
        catch (System.Exception ex)
        {
            Debug.Log("An error occurred: " + ex.Message);
        }
    }

    public void ResetGame()
    {
        // Destroy the persistent GameplayManager 
        if (managerInstance != null)
        {
            gameplayManager.destroyInstance();
        }

        // Destroy the persistent BackgroundMusic 
        if (musicInstance != null)
        {
            backMusic.destroyInstance();
        }

        // Load the MainMenu scene
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
