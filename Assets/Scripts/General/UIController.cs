using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public GameObject dashActive;
    public GameObject dashInactive;
    public GameObject pauseButtons;
    public Text tutorialText;
    public Text counterText;
    public Text finishText;
    public Text scoreText;
    public Text highScoreText;
    public string endLevelText = "YOU DID IT!";
    public float seconds;
    public float startTime = 500;
    private bool _isPaused;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        pauseButtons.SetActive(false);
        _isPaused = false;
        DashIcon(true); // Set active dash icon, use false to use inactive icon, to use in another script: UIController.instance.DashIcon(true);
        counterText.text = startTime.ToString("000");
        MainGameManager.Main.timeComponent.secondTick += UpdateTimer;
        MainGameManager.Main.timeComponent.timeIsUp += RetryLevel;
        MainGameManager.Main.scoreComponent.scoreHasUpdated += UpdateScore;

        Actor.Component.DashController dc = MainGameManager.Player.GetComponent<Actor.Component.DashController>();

        if (dc)
            dc.onDashReady += DashIcon;
    }

    public void UpdateTimer(int newTime)
    {
        counterText.text = newTime.ToString("000");
    }

    public void UpdateScore(int newScore)
    {
        scoreText.text = newScore.ToString("00000000");
    }

    public void UpdateTutorialText( string tip )
    {
        tutorialText.text = tip;
    }

    public void LevelFinish( float seconds )
    {
        Time.timeScale = 0;
        int timeSec = MainGameManager.Main.timeComponent.startTime - (int)MainGameManager.Main.timeComponent.Timer;
        highScoreText.text = timeSec.ToString() + " seconds";
        finishText.text = endLevelText;
    }

    public void PauseGame(Actor.Player.PlayerInput playerInput)
    {
        if (_isPaused)
        {
            Time.timeScale = 1;
            _isPaused = false;
            finishText.text = "";
            pauseButtons.SetActive(false);
            playerInput.UnpauseComponents();
        }
        else
        {
            finishText.text = "PAUSE";
            pauseButtons.SetActive(true);
            Time.timeScale = 0;
            _isPaused = true;
            playerInput.PauseComponents();
        }
        
    }

    public void DashIcon(bool dash)
    {
        if (dash)
        {
            dashActive.SetActive(true);
            dashInactive.SetActive(false);
        }
        else
        {
            dashActive.SetActive(false);
            dashInactive.SetActive(true);
        }
    }

    public void RetryLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

}
