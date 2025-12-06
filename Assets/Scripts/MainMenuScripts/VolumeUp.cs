using UnityEngine;
using TMPro;

public class VolumeUp : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private TextMeshProUGUI audioText;

    public void volumeUp()
    {
        if(audioSource.volume < 1)
        {
            audioSource.volume += 0.1f;
        }
        else{audioSource.volume = 1f;}

        int numText = Mathf.RoundToInt(audioSource.volume * 100);

        audioText.text = "Volume: " + numText + "%";
    }
}
