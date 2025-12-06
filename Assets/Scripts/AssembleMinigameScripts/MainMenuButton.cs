using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    //public function to close game
    public void Quit()
    {
        Application.Quit();

        //Ensures we also quit in unity editor
        //UnityEditor.EditorApplication.isPlaying = false;
    }
}
