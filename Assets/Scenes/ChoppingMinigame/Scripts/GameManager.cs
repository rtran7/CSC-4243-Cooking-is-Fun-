using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private int totalPieces;
    private int piecesOnPlate = 0;

    public Victory2 victory2;

    //If it exists, grab the gameplay manager
    private GameplayManager gameplayManager;

    //Grab the tutorial manager
    private GameObject tutorial;

    //Track how long it takes for the player to complete the game
    private float elapsedTime;

    //This is used to tell when we should stop the timer
    private bool isRunning;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        tutorial = GameObject.Find("TutorialManager");

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

        //Initialize variables
        elapsedTime = 0f;
        isRunning = true;
    }

    void Start()
    {
        
        totalPieces = 10;
        Debug.Log("Total pieces to collect: " + totalPieces);

        //If gameplay manager exists, do the following
        try
        {
           //If the player already completed this minigame but returns, do this
           if(gameplayManager.getChopComplete())
           {
                tutorial.SetActive(false);
                victory2.Setup();
           }
        }
        catch (System.Exception ex)
        {
            Debug.Log("An error occurred: " + ex.Message);
        }
    }

    void Update()
    {
        //Check if the timer should be running
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    public void PieceReachedPlate()
    {
        piecesOnPlate++;
        Debug.Log("Pieces on plate: " + piecesOnPlate + "/" + totalPieces);

        if (piecesOnPlate == totalPieces)
        {

            //If the gameplay manager exists, signal that the minigame is complete
            try
            {
                gameplayManager.setChopTime(elapsedTime);
                gameplayManager.checkChop();
            }
            catch (System.Exception ex)
            {
                Debug.Log("An error occurred: " + ex.Message);
            }
            isRunning = false;
            EndGame();
            victory2.Setup();
        }
    }

    void EndGame()
    {
        Debug.Log("All pieces collected! Game Over!");
    }

}
