using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class VictoryScreen : MonoBehaviour
{
    //Keep track of how many ingredients there are
    private int ingredientCount;

    //Get VictoryBackground game object
    private GameObject background;

    //Grab AudioSource
    private AudioSource src;

    //Check if audiosource is played
    private bool played;

    //Keep track of order of ingredients
    private List<GameObject> objects = new List<GameObject>();
    private GameObject chicken;

    //Keep track that the ingredients are in order
    private bool inOrder;

    //Public variable to hold an object with the GameOverScreen script
    public GameOverScreen gameOver;

    //Grab the return button
    private GameObject returnButton;

    //Grab the tutorial manager
    private GameObject tutorialManager;

    //Grab the script in the tutorial manager
    private TutorialScreen tutorialScript;

    //If it exists, grab the gameplay manager
    private GameplayManager gameplayManager;

    //This variable keeps track if we're in the practice mode or not.
    //If we are, this script will behave slightly differently
    [SerializeField]
    private bool inPractice;

    //Keetp track if the tutorial has been played
    private bool tutorialPlayed;

    //Track how long it takes for the player to complete the game
    private float elapsedTime;

    //This is used to tell when we should stop the timer
    private bool isRunning;

    //Other scripts will change ingredientCount with these functions
    public void incrementCount(){ ingredientCount++; }
    public void decrementCount(GameObject obj)
    {
        //Decrement ingredientCount
        ingredientCount--;

        // Pop from front (queue-style)
        GameObject first = objects[0];
        objects.RemoveAt(0);

        if (first != obj)
        { inOrder = false; }
    }

    void Awake()
    {
        //Initialize variables
        elapsedTime = 0f;
        isRunning = true;

        //Grab the transform of this child object
        Transform childTrans = transform.Find("VictoryBackground");
        // Use transform to get the game object
        background = childTrans.gameObject;

        //Grab audios source object
        GameObject audioObj = GameObject.Find("VictorySound");
        //Grab audio source
        src = audioObj.GetComponent<AudioSource>();

        //Grab the return button
        returnButton = GameObject.Find("ReturnButton");

        //Grab the tutorial manager
        tutorialManager = GameObject.Find("TutorialManager");
        tutorialScript = tutorialManager.GetComponent<TutorialScreen>();

        //Initialize boolean variables
        played = false;
        tutorialPlayed = false;

        //Initialize objects
        GameObject[] tempObjects = GameObject.FindGameObjectsWithTag("Ingredients");
        //Sort by position in hierarchy
        GameObject[] sortedObjects = tempObjects.OrderBy(go => go.transform.GetSiblingIndex()).ToArray();
        objects = new List<GameObject>(sortedObjects);

        //Grab the  chicken
        chicken = GameObject.Find("Diced Chicken");

        //Assume objects are in order
        inOrder = true;

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

    void Update()
    {
        //Check if the timer should be running
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
        }
        
        //If gameplayManger exists, do the following:
        try
        {
            //If the cooking minigame is complete and we're not in practice mode, do this:
            if(gameplayManager.getCookComplete() && !inPractice && !tutorialPlayed)
            {
                tutorialScript.startTutorial();
                //Iterate through all ingredients and disable dragScript
                foreach (GameObject obj in objects)
                {
                    //Enable ingredients
                    obj.SetActive(true);
                    //Get Script in each ingredient
                    IngredientsScript temp = obj.GetComponent<IngredientsScript>();
                    //Enable it
                    temp.enabled = true;
                }
                //The tutorial has been showed
                tutorialPlayed = true;
            }
            //Else if we are in the practice mode, do this:
            else if(gameplayManager.getCookComplete() && inPractice && !tutorialPlayed)
            {
                //Turn on the tutorial
                tutorialScript.startTutorial();
                //Iterate through all ingredients and enable dragScript
                foreach (GameObject obj in objects)
                {
                    //Enable ingredients
                    obj.SetActive(true);
                    //Get Script in each ingredient
                    IngredientsScript temp = obj.GetComponent<IngredientsScript>();
                    //Enable it
                    temp.enabled = true;
                }
                //The tutorial has been showed
                tutorialPlayed = true;
            }
            // Else just assume the player tries to play minigame
            // without doing previous minigames
            else if(!gameplayManager.getChopComplete())
            {
                tutorialManager.SetActive(false);
                //Set time scale to 0
                Time.timeScale = 0f;
                //Iterate through all ingredients and disable dragScript
                foreach (GameObject obj in objects)
                {
                    //Get Script in each ingredient
                    IngredientsScript temp = obj.GetComponent<IngredientsScript>();
                    //Disable it
                    temp.enabled = false;
                    //Disable ingredients
                    obj.SetActive(false);
                }
            }
            else if(!gameplayManager.getCookComplete())
            {
                tutorialManager.SetActive(false);
                //Set time scale to 0
                Time.timeScale = 0f;
                //Iterate through all ingredients and disable dragScript
                foreach (GameObject obj in objects)
                {
                    //Enable ingredients
                    obj.SetActive(true);
                    //Get Script in each ingredient
                    IngredientsScript temp = obj.GetComponent<IngredientsScript>();
                    //Disable it
                    temp.enabled = false;
                }
                //Disable chicken
                chicken.SetActive(false);
            }
        }
        //Do this if gameplay manager doesn't exist
        catch (System.Exception ex)
        {
            //Debug.Log("An error occurred: " + ex.Message);
            
            //If tutorial not played, do this:
            if(!tutorialPlayed)
            {
                //Play tutorial
                tutorialScript.startTutorial();
                //Iterate through all ingredients and disable dragScript
                foreach (GameObject obj in objects)
                {
                    //Get Script in each ingredient
                    IngredientsScript temp = obj.GetComponent<IngredientsScript>();
                    //Enable it
                    temp.enabled = true;
                }
                //The tutorial has been showed
                tutorialPlayed = true;
            }
        }

        //If all ingredients have been assembled
        if (ingredientCount == 0 && inOrder && !inPractice)
        {
            isRunning = false;
            //If the gameplay manager exists, signal that the minigame is complete
            try
            {
                gameplayManager.setAssembleTime(elapsedTime);
                gameplayManager.checkAssemble();
            }
            catch (System.Exception ex)
            {
                Debug.Log("An error occurred: " + ex.Message);
            }
            //Pause the game
            Time.timeScale = 0;
            // Turn on the victory screen
            background.SetActive(true);
            //Play audio source if not played
            if (!played) { src.Play(); played = true; }

        }
        //Else if we're in practice and player won, do this:
        else if (ingredientCount == 0 && inOrder && inPractice)
        {
            //Pause the game
            Time.timeScale = 0;
            // Turn on the victory screen
            background.SetActive(true);
            //Play audio source if not played
            if (!played) { src.Play(); played = true; }

        }
        //Else assume that the player failed the game
        else if (ingredientCount == 0 && !inOrder)
        {
            // Turn on the  game over screen
            gameOver.Setup();
        }

    }
}
