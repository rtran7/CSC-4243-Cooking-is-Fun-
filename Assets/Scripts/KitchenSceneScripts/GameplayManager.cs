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
    private Image chopStation;

    //Track whether we should change alpha of stations or not
    private bool cookAlphaChanged;
    private bool assembleAlphaChanged;

    //Track how long it takes for the player to complete the game
    private float elapsedTime;
    private float elapsedChop;
    private float elapsedCook;
    private float elapsedAssemble;

    //This is used to tell when we should stop the timer
    private bool isRunning;

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
        isRunning = true;
        elapsedTime = 0;
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
            chopStation = cuttingObject.GetComponent<Image>();
            //Cutting station starts yellow
            chopStation.color = Color.yellow;
            Color c = chopStation.color;
            c.a = 0.8f;
            chopStation.color = c;

            //If chop is finished
            if(chopComplete && !cookComplete)
            {
                texts[1].color = Color.white;
                texts[0].color = Color.green;

                chopStation.color = Color.green;
                changeImageAlpha(chopStation,0.8f);

                cookStation.color = Color.yellow;
                changeImageAlpha(cookStation,0.8f);

                assembleStation.color = Color.red;
                changeImageAlpha(assembleStation,0.8f);
            }
            //If chop and cook is finished
            else if(chopComplete && cookComplete)
            {
                texts[1].color = Color.green;
                texts[0].color = Color.green;

                chopStation.color = Color.green;
                changeImageAlpha(chopStation,0.8f);

                cookStation.color = Color.green;
                changeImageAlpha(cookStation,0.8f);

                assembleStation.color = Color.yellow;
                changeImageAlpha(assembleStation,0.8f);
            }
            //Assume no games are finished
            else
            {
                texts[1].color = Color.white;
                texts[0].color = Color.white;

                chopStation.color = Color.yellow;
                changeImageAlpha(chopStation,0.8f);

                cookStation.color = Color.red;
                changeImageAlpha(cookStation,0.8f);

                assembleStation.color = Color.red;
                changeImageAlpha(assembleStation,0.8f);
            }
        }
        catch{}

    }

    void Update()
    {

        //Increment timer
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
        }

        //Check if assemble game is complete
        if (assembleComplete && victoryManager != null)
        {
            texts[2].color = Color.green;
            isRunning = false;
            victoryManager.SetActive(true);
            //Report time
            TextMeshProUGUI chopText = victoryManager.transform.Find("ChopTime").GetComponent<TextMeshProUGUI>();
            chopText.text = "Chop time: " + Mathf.Round(elapsedChop) + " seconds";
            TextMeshProUGUI cookText = victoryManager.transform.Find("CookTime").GetComponent<TextMeshProUGUI>();
            cookText.text = "Cook time: " + Mathf.Round(elapsedCook)+ " seconds";
            TextMeshProUGUI assembleText = victoryManager.transform.Find("AssembleTime").GetComponent<TextMeshProUGUI>();
            assembleText.text = "Assemble time: " + Mathf.Round(elapsedAssemble)+ " seconds";
            TextMeshProUGUI totalText = victoryManager.transform.Find("TotalTime").GetComponent<TextMeshProUGUI>();
            totalText.text = "Total time: " + Mathf.Round(elapsedTime)+ " seconds";

        }
        //Check if Cooking game is done
        if(cookComplete)
        {
            //If the text and  assemble station exists, do the following
            try
            {

                if(!assembleAlphaChanged)
                {
                    //Mark the chop text green, mark the assemble station yellow, and cook station green
                    texts[1].color = Color.green;
                    cookStation.color = Color.green;
                    changeImageAlpha(cookStation,0.8f);
                    assembleStation.color = Color.yellow;
                    changeImageAlpha(assembleStation,0.8f);
                    assembleAlphaChanged = true;
                }
            }
            catch{}
        }
        //Check if Chopping game is done
        if(chopComplete)
        {
            //If the text and  cook station exists, do the following
            try
            {
                if(!cookAlphaChanged)
                {
                    //Mark the chop text, cook station yellow,  and chop station green
                    texts[0].color = Color.green;
                    cookStation.color = Color.yellow;
                    changeImageAlpha(cookStation,0.8f);
                    chopStation.color = Color.green;
                    changeImageAlpha(chopStation,0.8f);
                    cookAlphaChanged = true;
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

    public float returnTime(){return elapsedTime;}
    public void setChopTime(float time){elapsedChop = time;}
    public void setCookTime(float time){elapsedCook = time;}
    public void setAssembleTime(float time){elapsedAssemble = time;}
}
