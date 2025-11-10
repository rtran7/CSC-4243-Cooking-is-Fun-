using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsPanel : MonoBehaviour
{
    [Header("UI Refs")]
    public GameObject root;            // this panel
    public TMP_Text resultText;        // ResultBox/ResultText
    public Button continueButton;      // ContinueButton

    [Header("Return")]
    public string returnSceneName = ""; // optional

    void Awake()
    {
        if (continueButton)
            continueButton.onClick.AddListener(OnContinue);
    }

    public void Show(string message)
    {
        if (resultText) resultText.text = message;
        if (root) root.SetActive(true);
    }

    void OnContinue()
    {
        if (!string.IsNullOrEmpty(returnSceneName))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(returnSceneName);
        }
        else
        {
            if (root) root.SetActive(false);
        }
    }
}
