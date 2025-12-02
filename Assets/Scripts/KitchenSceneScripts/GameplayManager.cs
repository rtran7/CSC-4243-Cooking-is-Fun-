using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
    private GameObject tutorialManager;//WARNING: Make sure tutorialManagerKitchen is not disabled before the game starts

    //Grab the task panel
    private GameObject panel;
    private TextMeshProUGUI[] texts;

    //Grab the  stations
    private Image cookStation;
    private Image assembleStation;

    //Track whether we should change alpha of stations or not
    private bool cookAlphaChanged;
    private bool assembleAlphaChanged;

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
        cookAlphaChanged = false;
        assembleAlphaChanged = false;
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

        //Do the following if the panel and station buttons exist
        try
        {
            //Grab the panel and its children
            panel = GameObject.Find("TaskPanel");
            texts = panel.GetComponentsInChildren<TextMeshProUGUI>();

            //Grab the stations and change the color to red
            GameObject cookingObject = GameObject.Find("CookingStation");
            cookStation = cookingObject.GetComponent<Image>();

            GameObject assembleObject = GameObject.Find("AssembleStation");
            assembleStation = assembleObject.GetComponent<Image>();

            GameObject cuttingObject = GameObject.Find("CuttingStation");
            Image tempImage = cuttingObject.GetComponent<Image>();
            //Cutting station is always green
            tempImage.color = Color.green;
            Color c = tempImage.color;
            c.a = 0.8f;
            tempImage.color = c;

            

            //Check if the cooking minigame is done
            if(cookComplete)
            {
                //Mark the cook text and cook station green
                texts[1].color = Color.green;
                assembleStation.color = Color.green;
                changeImageAlpha(assembleStation,0.8f);
            }
            else
            {
                texts[1].color = Color.white;
                assembleStation.color = Color.red;
                changeImageAlpha(assembleStation,0.8f);
            }
            //Check if the cutting minigame is done
            if(chopComplete)
            {
                //Mark the chop text and cook station green
                texts[0].color = Color.green;
                cookStation.color = Color.green;
                changeImageAlpha(cookStation,0.8f);
            }
            else
            {
                texts[0].color = Color.white;
                cookStation.color = Color.red;
                changeImageAlpha(cookStation,0.8f);
            }
        }
        catch{}

    }

    void Update()
    {
        if (assembleComplete && victoryManager != null)
        {
            texts[2].color = Color.green;
            victoryManager.SetActive(true);
        }

        //Check if Chopping game is done
        if(chopComplete)
        {
            //If the text and  cook station exists, do the following
            try
            {
                if(!cookAlphaChanged)
                {
                    //Mark the chop text and cook station green
                    texts[0].color = Color.green;
                    cookStation.color = Color.green;
                    changeImageAlpha(cookStation,0.8f);
                    cookAlphaChanged = true;
                }
            }
            catch{}
        }

        //Check if Cooking game is done
        if(cookComplete)
        {
            //If the text and  assemble station exists, do the following
            try
            {

                if(!assembleAlphaChanged)
                {
                    //Mark the chop text and cook station green
                    texts[1].color = Color.green;
                    assembleStation.color = Color.green;
                    changeImageAlpha(assembleStation,0.8f);
                    assembleAlphaChanged = true;
                }
            }
            catch{}
        }
    }

    // Public methods to update state
    public void checkChop() => chopComplete = true;
    public bool getChopComplete(){return chopComplete;}

    public void checkCook() => cookComplete = true;
    public bool getCookComplete(){return cookComplete;}

    public void checkAssemble() => assembleComplete = true;
    public void checkTutorial() => tutorialShown = true;

    public GameplayManager getInstance(){return instance;}
    public void destroyInstance() => Destroy(instance.gameObject); 

    private void changeImageAlpha(Image img, float value)
    {
        Color c = img.color;
        c.a = value;
        img.color = c;
    }
}
