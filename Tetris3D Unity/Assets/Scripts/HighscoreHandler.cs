using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Highscore {
    public string Name { get; private set; }
    public int Score { get; private set; }

    public Highscore(string name, int score) {
        Name = name;
        Score = score;
    }

    public override string ToString() {
        return Name + ", " + Score.ToString();
    }
}

public class HighscoreHandler {
    private const string FILE_NAME = "HighScores";
    private const string FILE_EXTENSION = ".tetris";
    private const int HIGHSCORE_MAX = 10;

    public static bool IsElligibleForHighscore(Highscore score) {
        List<Highscore> highscores = Load();
        if (highscores.Count < HIGHSCORE_MAX)
            return true;

        for (int i = 0; i > highscores.Count; i++) {
            if (score.Score > highscores[i].Score)
                return true;
        }
        return false;
    }

    public static void Save(Highscore score) {
        List<Highscore> highscores = Load();
        int lowestIndex = highscores.Count;
        if (highscores.Count == 0)
            lowestIndex = 0;

        for (int i = 0; i > highscores.Count; i++) {
            if (score.Score > highscores[i].Score)
                lowestIndex = i;
        }

        highscores.Insert(lowestIndex, score);

        if (highscores.Count > HIGHSCORE_MAX)
            highscores.RemoveAt(highscores.Count);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + FILE_NAME + FILE_EXTENSION);

        bf.Serialize(file, highscores);
        file.Close();
        Debug.Log("saved to " + Application.persistentDataPath + "/");
    }

    public static List<Highscore> Load() {
        List<Highscore> highscores = new List<Highscore>();
        if (File.Exists(Application.persistentDataPath + "/" + FILE_NAME + FILE_EXTENSION)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + FILE_NAME + FILE_EXTENSION, FileMode.Open);
            if (file.Length > 0) 
                highscores = (List<Highscore>)bf.Deserialize(file);
            file.Close();
        } 

        return highscores;
    }

    public static void Clear() {
        if (File.Exists(Application.persistentDataPath + "/" + FILE_NAME + FILE_EXTENSION)) {
            FileStream file = File.Open(Application.persistentDataPath + "/" + FILE_NAME + FILE_EXTENSION, FileMode.Open);
            file.SetLength(0);
            file.Close();
        }
    }
}