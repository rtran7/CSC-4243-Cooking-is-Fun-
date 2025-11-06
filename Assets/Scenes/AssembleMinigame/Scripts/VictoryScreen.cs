using UnityEngine;

public class VictoryScreen : MonoBehaviour
{
    //Keep track of how many ingredients there are
    private int ingredientCount;

    //Get VictoryBackground game object
    private GameObject background;

    //Other scripts will change ingredientCount with these functions
    public void incrementCount(){ ingredientCount++; }
    public void decrementCount() { ingredientCount--; }

    void Awake()
    {
        //Grab the transform of this child object
        Transform childTrans = transform.Find("VictoryBackground");
        // Use transform to get the game object
        background = childTrans.gameObject;
    
    }

    void Update()
    {
        //If all ingredients have been assembled
        if (ingredientCount == 0)
        {
            //Pause the game
            Time.timeScale = 0;
            // Turn on the victory screen
            background.SetActive(true);

        }

    }
}
