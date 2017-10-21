using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class highscoreTest : MonoBehaviour {

    public string playerName = "test_user";
    public int playerScore = 10;

    void OnGUI() {
        if (GUI.Button(new Rect(10, 10, 200, 100), "save"))
            Save();
        if (GUI.Button(new Rect(10, 210, 200, 100), "load"))
            Load();
        if (GUI.Button(new Rect(10, 310, 200, 100), "clear"))
            HighscoreHandler.Clear();
    }

    void Save() {
        Highscore score = new Highscore(playerName, playerScore);
        if (HighscoreHandler.IsElligibleForHighscore(score))
            HighscoreHandler.Save(score);
    }

    private void Load() {
        List<Highscore> highscores = HighscoreHandler.Load();
        foreach(Highscore score in highscores) {
            print(score);
        }
    }
}
