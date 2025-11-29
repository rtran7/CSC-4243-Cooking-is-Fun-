using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;

    void Awake()
    {
        // Make sure there's only one MusicManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep it alive across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates if scene reloads
        }
    }

    public BackgroundMusic getInstance(){return instance;}
    public void destroyInstance() => Destroy(instance.gameObject); 
}
