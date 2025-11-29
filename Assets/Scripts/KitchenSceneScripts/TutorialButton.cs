using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    private GameObject tutorialManager;
    //If it exists, grab the gameplay manager
    private GameplayManager gameplayManager;

    void Awake()
    {
        tutorialManager = GameObject.Find("TutorialManagerKitchen");

        //Grab the gameplay manager if it exist. If it doesn't exist, move on 
        try
        {
            GameObject tempObj = GameObject.Find("GameplayManager");
            gameplayManager = tempObj.GetComponent<GameplayManager>();
        }
        catch (System.Exception ex)
        {
            Debug.Log("An error occurred: " + ex.Message);
        }
    }

    public void tutorialShown()
    {
    //If the gameplay manager exists, signal that the minigame is complete
            try
            {
                gameplayManager.checkTutorial();
            }
            catch (System.Exception ex)
            {
                Debug.Log("An error occurred: " + ex.Message);
            }
            tutorialManager.SetActive(false);
    }
}
