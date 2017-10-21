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
            SceneManager.LoadScene(1);
        }

        public void ShowHighScore() {
            menu.SetActive(false);
            highscores.SetActive(true);
        }

        public void BackToMenu() {
            menu.SetActive(true);
            highscores.SetActive(false);
        }
    }
}