using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChickenCookMinigame : MonoBehaviour
{
    // ---------- Refs ----------
    [Header("Refs")]
    public CookingMeterController meter;
    public GameObject meterGroupUI;
    public GameObject resultsPanel;
    public TMP_Text resultText;
    public Button continueButton;
    public TMP_Text HeaderText;

    // ---------- Gameplay tuning ----------
    [Header("Gameplay")]
    public bool invertValue = true;                // invert clockwise/counterclockwise
    [Range(0, 1f)] public float valueOffset = 0f;  // rotate the 0..1 scale

    // (kept to remain public, but not used by the lap-based verdict)
    [Range(0, 1f)] public float perfectCenter = 0.25f;
    [Range(0, 0.5f)] public float perfectHalfWidth = 0.05f;
    [Range(0, 0.5f)] public float underOverThreshold = 0.20f;
    public bool burntOnClockwiseSide = true;

    [Header("Return")]
    public string returnSceneName = "";

    // ---------- Results UI ----------
    [Header("Result UI")]
    public Image resultImage;        // Image under ResultsPanel to show outcome sprite

    [Header("Result Sprites")]
    public Sprite rawSprite;         // undercooked
    public Sprite perfectSprite;     // perfectly cooked
    public Sprite burntSprite;       // burnt
    public Sprite okaySprite;        // okay-ish

    public System.Action onContinue;

    // ---------- State (kept public where you asked) ----------
    public bool _isCooking;          // currently showing/animating the meter
    public bool _cooking;            // alias if you referenced this elsewhere
    public int _laps;                // how many times we wrapped past 12 o'clock clockwise
    public float _prevV = -1f;       // previous normalized value, -1 = not yet set
    private bool taskCompleted;      // Keep track if the user successfully cooked chicken
    //Track how long it takes for the player to complete the game
    private float elapsedTime;

    //This is used to tell when we should stop the timer
    private bool isRunning;

    //-------------Gameplay manager------------
    //If it exists, grab the gameplay manager
    private GameplayManager gameplayManager;

    //--------- Game Objects ------------------
    private GameObject rawChicken;
    private GameObject tutorialManager;
    private GameObject returnButton;
    void Awake()
    {
        if (resultsPanel) resultsPanel.SetActive(false);
        if (meterGroupUI) meterGroupUI.SetActive(false);
        if (continueButton) continueButton.onClick.AddListener(OnContinue);
        taskCompleted = false; // Assume that the player didn't complete the task

        //Grab the raw chicken
        rawChicken = GameObject.Find("RawChicken");
        rawChicken.SetActive(false);

        //Grab the tutorial manager
        tutorialManager = GameObject.Find("TutorialManager");
        tutorialManager.SetActive(false);

        //Grab the return button
        returnButton = GameObject.Find("ReturnButton");

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
        //Initialize variables
        elapsedTime = 0f;
        isRunning = true;
    }

    //This function is used to return taskCompleted
    public bool checkTask() { return taskCompleted; }

    // Called by your drag script when raw chicken is dropped on the pan
    public void BeginCooking()
    {
        if (meterGroupUI) meterGroupUI.SetActive(true);
        if (HeaderText) HeaderText.gameObject.SetActive(true);
        if (resultsPanel) resultsPanel.SetActive(false);

        _isCooking = true;
        _cooking   = true;
        _laps      = 0;
        _prevV     = -1f;

        if (meter) meter.Unfreeze();
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
            //If player already completed cooking minigame but returns, do this:
            if(gameplayManager.getCookComplete())
            {
                taskCompleted = true;
                tutorialManager.SetActive(false);
                if (HeaderText)    HeaderText.gameObject.SetActive(false);
                if (meterGroupUI)  meterGroupUI.SetActive(false);
                if (resultsPanel)  resultsPanel.SetActive(true);

                if (resultText)    resultText.text = "Perfectly Cooked!";

                if (resultImage)
                {
                    resultImage.enabled        = true;
                    resultImage.sprite         = perfectSprite;
                    resultImage.color          = Color.white;
                    resultImage.preserveAspect = true;
                    resultImage.SetNativeSize();       // optional
                }
            }
            //Else If player completed chop minigame, do this:
            else if(gameplayManager.getChopComplete())
            {
                tutorialManager.SetActive(true);
                rawChicken.SetActive(true);
                
            }
            //Else assume player didn't complete chop minigame
            else
            {
                tutorialManager.SetActive(false);
                rawChicken.SetActive(false);
                returnButton.SetActive(true);
            }
        }
        //If  gameplay manager doesn't exist, do the following
        catch (System.Exception ex)
        {
            tutorialManager.SetActive(true);
            rawChicken.SetActive(true);
        }

        if (!_isCooking) return;

        // --- Read normalized needle value 0..1, apply invert/offset ---
        float v01 = meter != null ? meter.CurrentValue : 0f;
        if (invertValue) v01 = 1f - v01;
        v01 = Mathf.Repeat(v01 + valueOffset, 1f);

        // --- Lap tracking (clockwise wrap across 12 o'clock) ---
        if (_prevV >= 0f)
        {
            float delta = v01 - _prevV;
            if (delta < -0.5f) _laps++;                 // 0.98 -> 0.02 : CW wrap
            else if (delta > 0.5f) _laps = Mathf.Max(0, _laps - 1); // CCW wrap (rare)
        }
        _prevV = v01;

        // --- Stop on Space ---
#if ENABLE_INPUT_SYSTEM
        bool space = (UnityEngine.InputSystem.Keyboard.current != null) &&
                     UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame;
#else
        bool space = Input.GetKeyDown(KeyCode.Space);
#endif
        if (space)
        {
            if (meter) meter.Freeze();
            EvaluateAndShowResult_Laps(v01);   // lap-based verdict
            _isCooking = false;
            _cooking   = false;
        }
    }

    // Lap-based verdict:
    // After the first full revolution, ANY stop = Burnt.
    // Before that:
    //   0.00–0.50  -> Undercooked  (right half)
    //   0.50–0.75  -> Okay-ish     (top-left)
    //   0.75–1.00  -> Perfect      (bottom-left)
    public void EvaluateAndShowResult_Laps(float v01)
    {
        string verdict;
        Sprite verdictSprite = null;

        if (_laps >= 1)
        {
            verdict = "Burnt!";
            verdictSprite = burntSprite;
        }
        else
        {
            if (v01 < 0.5f)
            {
                verdict = "Undercooked!";
                verdictSprite = rawSprite;
            }
            else if (v01 < 0.75f)
            {
                verdict = "Okay-ish";
                verdictSprite = okaySprite;
            }
            else
            {
                verdict = "Perfectly Cooked!";
                verdictSprite = perfectSprite;
                taskCompleted = true; //The task has been completed. Set to true
                isRunning = false;
                //If the gameplay manager exists, signal that the minigame is complete
                try
                {
                    gameplayManager.setCookTime(elapsedTime);
                    gameplayManager.checkCook();
                }
                catch (System.Exception ex)
                {
                    Debug.Log("An error occurred: " + ex.Message);
                }
            }
        }

        // --- UI updates ---
        if (HeaderText)    HeaderText.gameObject.SetActive(false);
        if (meterGroupUI)  meterGroupUI.SetActive(false);
        if (resultsPanel)  resultsPanel.SetActive(true);

        if (resultText)    resultText.text = verdict;

        if (resultImage)
        {
            resultImage.enabled        = true;
            resultImage.sprite         = verdictSprite;
            resultImage.color          = Color.white;
            resultImage.preserveAspect = true;
            resultImage.SetNativeSize();       // optional
        }

        Debug.Log($"Verdict: {verdict} (v={v01:0.000}, laps={_laps})");
    }

    public void OnContinue()
    {
        onContinue?.Invoke();

        if (meterGroupUI) meterGroupUI.SetActive(false);

        if (!string.IsNullOrEmpty(returnSceneName))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(returnSceneName);
        }
        else
        {
            if (resultsPanel) resultsPanel.SetActive(false);
        }
    }
}