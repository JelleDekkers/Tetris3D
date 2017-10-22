using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UI {

    public class GameOverMenu : MonoBehaviour {

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

        private void Start() {
            GameManager.Instance.OnGameOver += ShowGameMenu;
        }

        private void OnDestroy() {
            GameManager.Instance.OnGameOver -= ShowGameMenu;
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
}