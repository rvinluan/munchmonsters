using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStateManager : MonoBehaviour {

  public GameObject modal;
  public string currentGameState;
  public Button restartButton;
  public Text endGameText;
  public ScoreKeeper scoreKeeper;

  public void Restart() {
    currentGameState = "playing";
    modal.SetActive(false);
  }

	// Use this for initialization
	void Start () {
    currentGameState = "playing";
    modal.SetActive(false);
    restartButton.onClick.AddListener(restartGame);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public void restartGame() {
    transform.parent.GetComponent<MunchMonsters>().Restart();
  }

  public void displayModal() {
    currentGameState = "ended";
    endGameText.text = scoreKeeper.getLowestScore().ToString();
    modal.SetActive(true);
  }
}
