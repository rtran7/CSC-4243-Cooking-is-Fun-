using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private int totalPieces;
    private int piecesOnPlate = 0;

    public Victory2 victory2;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
            EndGame();
            victory2.Setup();
        }
    }

    void EndGame()
    {
        Debug.Log("All pieces collected! Game Over!");
    }

}
