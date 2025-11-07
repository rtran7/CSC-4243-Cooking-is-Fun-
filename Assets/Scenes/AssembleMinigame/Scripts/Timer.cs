using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    //Public variable to hold an object with the GameOverScreen script
    public GameOverScreen gameOver;

    // Update is called once per frame
    void Update()
    {
        if (remainingTime > 0)
        { remainingTime -= Time.deltaTime; }
        else if (remainingTime < 0)
        { 
            //Make sure timer is always at 0
            remainingTime = 0; 
            // Change text color 
            timerText.color = Color.red;
            // Turn on the  game over screen
            gameOver.Setup();
        }

        int min = Mathf.FloorToInt(remainingTime / 60);
        int second = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}",min,second);
    }
}
