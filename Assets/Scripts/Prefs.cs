using UnityEngine;
using System.Collections;

public class Prefs : MonoBehaviour {

  public enum GameMode {
    Combined,
    Lowest
  }
  public GameMode gameMode = GameMode.Combined;

	public int getHighScore(GameMode gm) {
    return PlayerPrefs.GetInt("HighScore_"+gm.ToString(), 0);
  }

  public void setHighScore(int finalScore) {
    PlayerPrefs.SetInt("HighScore_"+gameMode.ToString(), finalScore);
  }
}
