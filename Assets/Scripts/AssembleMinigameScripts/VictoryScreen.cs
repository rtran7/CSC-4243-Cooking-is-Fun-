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

    //Keep track that the ingredients are in order
    private bool inOrder;

    //Public variable to hold an object with the GameOverScreen script
    public GameOverScreen gameOver;

    //If it exists, grab the gameplay manager
    private GameplayManager gameplayManager;

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
        //Grab the transform of this child object
        Transform childTrans = transform.Find("VictoryBackground");
        // Use transform to get the game object
        background = childTrans.gameObject;

        //Grab audios source object
        GameObject audioObj = GameObject.Find("VictorySound");
        //Grab audio source
        src = audioObj.GetComponent<AudioSource>();

        //Initialize played
        played = false;

        //Initialize objects
        GameObject[] tempObjects = GameObject.FindGameObjectsWithTag("Ingredients");
        //Sort by position in hierarchy
        GameObject[] sortedObjects = tempObjects.OrderBy(go => go.transform.GetSiblingIndex()).ToArray();
        objects = new List<GameObject>(sortedObjects);

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
        //If all ingredients have been assembled
        if (ingredientCount == 0 && inOrder)
        {

            //If the gameplay manager exists, signal that the minigame is complete
            try
            {
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

        else if (ingredientCount == 0 && !inOrder)
        {
            // Turn on the  game over screen
            gameOver.Setup();
        }

    }
}
