using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameMenu : MonoBehaviour {

    private static GameMenu instance;
    public static GameMenu Instance {
        get { 
            if(instance == null)
                instance = FindObjectOfType<GameMenu>();
            return instance;
        }
    }

    public Action<bool> OnPauseButtonPressed;

    [SerializeField]
    private GameObject gameOverMenu;
    [SerializeField]
    private GameObject highscores;
    [SerializeField]
    private Text finalScoreTxt;
    [SerializeField]
    private GameObject highscorePanel;
    [SerializeField]
    private Text playerName;
    [SerializeField]
    private GameObject submitButton;
    [SerializeField]
    private GameObject pausePanel;

    private bool isPaused;

    private void Start() {
        GameManager.Instance.OnGameOver += ShowGameMenu;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
    }

    private void OnDestroy() {
        GameManager.Instance.OnGameOver -= ShowGameMenu;
    }

    public void PauseGame() {
        isPaused = !isPaused;
        if(OnPauseButtonPressed != null)
        OnPauseButtonPressed(isPaused);
    }

    private void ShowGameMenu() {
        gameOverMenu.SetActive(true);
        float score = GameManager.Instance.Score;
        finalScoreTxt.text = score.ToString();

        if (HighscoreHandler.IsElligibleForHighscore(score)) {
            highscorePanel.SetActive(true);
            submitButton.SetActive(true);
        } else {
            highscorePanel.SetActive(false);
        }
    }

    public void ShowPauseMenu(bool pause) {
        pausePanel.SetActive(pause);
    }
        
    public void ToMenu() {
        SceneManager.LoadScene(0);
    }

    public void Replay() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SubmitHighscore() {
        HighscoreHandler.Save(new Highscore(playerName.text, (int)GameManager.Instance.Score));
        submitButton.SetActive(false);
    }
}
