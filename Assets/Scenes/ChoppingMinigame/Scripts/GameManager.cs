using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private int totalPieces;
    private int piecesOnPlate = 0;

    public Victory2 victory2;

    //If it exists, grab the gameplay manager
    private GameplayManager gameplayManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
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

    void Start()
    {
        
        totalPieces = 10;
        Debug.Log("Total pieces to collect: " + totalPieces);
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
                gameplayManager.checkChop();
            }
            catch (System.Exception ex)
            {
                Debug.Log("An error occurred: " + ex.Message);
            }

            EndGame();
            victory2.Setup();
        }
    }

    void EndGame()
    {
        Debug.Log("All pieces collected! Game Over!");
    }

}
