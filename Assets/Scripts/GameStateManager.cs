using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStateManager : MonoBehaviour {

  public GameObject modal;
  public string currentGameState;
  public Button restartButton;
  public Text endGameText;
  public Text highScoreText;
  public ScoreKeeper scoreKeeper;
  private Prefs prefs;

  public void Restart() {
    currentGameState = "playing";
    modal.SetActive(false);
  }

	// Use this for initialization
	void Start () {
    currentGameState = "playing";
    modal.SetActive(false);
    restartButton.onClick.AddListener(restartGame);
    // prefs = GameObject.Find("Prefs").GetComponent<Prefs>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void restartGame() {
    PlayerPrefs.Save();
    transform.parent.GetComponent<MunchMonsters>().Restart();
  }

  public void displayModal() {
    currentGameState = "ended";
    int finalScore = 0;
    // if(prefs.gameMode == Prefs.GameMode.Lowest) {
    //   finalScore = scoreKeeper.getLowestScore();
    // } else {
    //   finalScore = scoreKeeper.getCombinedScore();
    // }
    if(finalScore > prefs.getHighScore(prefs.gameMode)) {
      //new high score! message this somewhere.
      Debug.Log(prefs.getHighScore(prefs.gameMode));
      highScoreText.text = "High Score!";
      prefs.setHighScore(finalScore);
    } else {
      highScoreText.text = "Game Over";
    }
    endGameText.text = finalScore.ToString();
    modal.SetActive(true);
  }
}
