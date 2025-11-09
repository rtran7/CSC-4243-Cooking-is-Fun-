using UnityEngine;

public class QuitButton : MonoBehaviour
{
    //public function to close game
    public void Quit()
    {
        Application.Quit();

        //Ensures we also quit in unity editor
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
