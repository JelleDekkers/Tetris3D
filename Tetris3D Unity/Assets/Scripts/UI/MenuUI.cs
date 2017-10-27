using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UI {

    public class MenuUI : MonoBehaviour {

        [SerializeField]
        private GameObject menu;
        [SerializeField]
        private GameObject highscores;

        public void StartGame() {
            SceneManager.LoadScene(Scenes.GAME_SCENE);
        }

        public void ShowHighScore() {
            menu.SetActive(false);
            highscores.SetActive(true);
        }

        public void BackToMenu() {
            menu.SetActive(true);
            highscores.SetActive(false);
        }

        public void QuitGame() {
            Application.Quit();
        }
    }
}