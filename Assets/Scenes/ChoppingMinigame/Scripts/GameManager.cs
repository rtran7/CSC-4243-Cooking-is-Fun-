using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private int totalPieces;
    private int piecesOnPlate = 0;

    public Victory2 victory2;

    [SerializeField]
    private bool inPractice;

    private GameplayManager gameplayManager;
    private GameObject tutorial;
    private float elapsedTime;
    private bool isRunning;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        tutorial = GameObject.Find("TutorialManager");
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (currentScene.Contains("Tutorial") || currentScene.Contains("Practice"))
        {
            inPractice = true;
            Debug.Log("Practice mode enabled");
        }
       
        if (victory2 == null)
        {
            GameObject victoryObj = GameObject.Find("VictoryManager"); 
            if (victoryObj != null)
            {
                victory2 = victoryObj.GetComponent<Victory2>();
               
            }
           
        }

        elapsedTime = 0f;
        isRunning = true;
    }

    void Start()
    {
        totalPieces = 10;
        Debug.Log("Total pieces to collect: " + totalPieces);

        try
        {
           if(gameplayManager != null && gameplayManager.getChopComplete())
           {
                tutorial.SetActive(false);
                if (victory2 != null)
                {
                    victory2.Setup();
                }
           }
        }
        catch (System.Exception ex)
        {
            Debug.Log("An error occurred: " + ex.Message);
        }
    }

    void Update()
    {
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
            isRunning = false;
            
           
            if (!inPractice)
            {
             
                try
                {
                    if (gameplayManager != null)
                    {
                        gameplayManager.setChopTime(elapsedTime);
                        gameplayManager.checkChop();
                        Debug.Log("Progress saved - Time: " + elapsedTime);
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.Log("An error occurred: " + ex.Message);
                }
            }
          
            else if (inPractice)
            {
                Debug.Log("Practice complete - Time: " + elapsedTime + " (not saved)");
            }
            
           
            if (victory2 != null)
            {
                victory2.Setup();
            }
            else
            {
                Debug.LogError("Cannot show victory screen - Victory2 is null!");
            }
        }
    }
}