using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI {

    public class InfoPanelUI : MonoBehaviour {

        [SerializeField]
        private Text highScore;
        [SerializeField]
        private Text cubesPlayed;
        [SerializeField]
        private Text score;
        [SerializeField]
        private Text pitSize;

        private void Start() {
            GameManager.Instance.OnGroupLockedEvent += UpdateCubesPlayed;
            GameManager.Instance.OnLayerClearedEvent += UpdateScore;
            pitSize.text = Level.Instance.Size.x + " x " + Level.Instance.Size.y + " x " + Level.Instance.Size.z;
        }

        private void OnDestroy() {
            GameManager.Instance.OnGroupLockedEvent -= UpdateCubesPlayed;
        }

        private void UpdateCubesPlayed() {
            cubesPlayed.text = GameManager.Instance.BlocksPlayed.ToString();
        }

        private void UpdateScore() {
            score.text = GameManager.Instance.Score.ToString();
        }
    }
}