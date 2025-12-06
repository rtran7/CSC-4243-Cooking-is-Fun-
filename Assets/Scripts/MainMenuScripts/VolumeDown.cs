using UnityEngine;
using TMPro;

public class VolumeDown : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private TextMeshProUGUI audioText;

    public void volumeDown()
    {
        if(audioSource.volume > 0)
        {
            audioSource.volume -= 0.1f;
        }
        else{audioSource.volume = 0f;}

        int numText = Mathf.RoundToInt(audioSource.volume * 100);

        audioText.text = "Volume: " + numText + "%";
    }
}
