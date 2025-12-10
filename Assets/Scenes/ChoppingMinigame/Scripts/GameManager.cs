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
        }

        GameObject tempObj = GameObject.Find("GameplayManager");
        if (tempObj != null)
        {
            gameplayManager = tempObj.GetComponent<GameplayManager>();
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

        if(gameplayManager != null && gameplayManager.getChopComplete())
        {
            tutorial.SetActive(false);
            if (victory2 != null)
            {
                victory2.Setup();
            }
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

        if (piecesOnPlate == totalPieces)
        {
            isRunning = false;
            
            if (!inPractice)
            {
                if (gameplayManager != null)
                {
                    gameplayManager.setChopTime(elapsedTime);
                    gameplayManager.checkChop();
                }
            }
            
            if (victory2 != null)
            {
                victory2.Setup();
            }
        }
    }
}