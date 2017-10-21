using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {

    public class HighscoreUI : MonoBehaviour {

        [SerializeField]
        private Transform grid;

        private HighscoreItem[] highscoreItems;
        private List<Highscore> highscores;

        private void Awake() {
            highscoreItems = grid.GetComponentsInChildren<HighscoreItem>();
        }

        private void OnEnable() {
            UpdateGrid();
        }

        private void UpdateGrid() {
            highscores = HighscoreHandler.Load();
            if(highscores == null) {
                Debug.LogError("something went wrong loading highscores");
                return;
            }

            foreach (HighscoreItem item in highscoreItems)
                item.Clear();

            for(int i = 0; i < highscoreItems.Length; i++) {
                if (i > highscores.Count - 1)
                    return;
                highscoreItems[i].UpdateValues(highscores[i]);
            }
        }
    }
}