using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    private static GameplayManager instance;

    // Global state
    private bool chopComplete;
    private bool cookComplete;
    private bool assembleComplete;
    private bool tutorialShown;

    // Scene-specific references
    private GameObject victoryManager; //WARNING: Make sure victoryManagerKitchen is not disabled before the game starts
    private GameObject tutorialManager;


    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }

        // Initialize global state
        chopComplete = false;
        cookComplete = false;
        assembleComplete = false;
        tutorialShown = false;
    }

    void OnEnable()
    {
        // Subscribe to sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe to prevent duplicate calls
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Re-acquire scene-specific objects each time a new scene loads
        victoryManager = GameObject.Find("VictoryManagerKitchen");
        tutorialManager = GameObject.Find("TutorialManagerKitchen");

        if (victoryManager != null) victoryManager.SetActive(false);
        if (tutorialManager != null && tutorialShown) tutorialManager.SetActive(false);
    }

    void Update()
    {
        if (assembleComplete && victoryManager != null)
        {
            victoryManager.SetActive(true);
        }
    }

    // Public methods to update state
    public void checkChop() => chopComplete = true;
    public void checkCook() => cookComplete = true;
    public void checkAssemble() => assembleComplete = true;
    public void checkTutorial() => tutorialShown = true;
    public GameplayManager getInstance(){return instance;}
    public void destroyInstance() => Destroy(instance.gameObject); 
}
