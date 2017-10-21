using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {

    public class HighscoreItem : MonoBehaviour {

        [SerializeField]
        private Text playerNameTxt;
        [SerializeField]
        private Text scoreTxt;

        public void UpdateValues(Highscore score) {
            playerNameTxt.text = score.Name;
            scoreTxt.text = score.Score.ToString();
        }

        public void Clear() {
            playerNameTxt.text = "";
            scoreTxt.text = "";
        }
    }
}