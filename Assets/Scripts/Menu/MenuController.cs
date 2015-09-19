using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {

  public Button StartButton;
  public Button QuitButton;
  public Button CombinedButton;
  public Button LowestButton;
  public Button BackButton;
  public Text HighScoreLowestText;
  public Text HighScoreCombinedText;
  public RectTransform HighlightA;
  public RectTransform HighlightB;
  public RectTransform HighlightC;
  public RectTransform HighlightD;
  public int currentScreen = 1;
  public Prefs prefs;

  void Awake () {
    DontDestroyOnLoad(prefs);
  }

	// Use this for initialization
	void Start () {
    StartButton.onClick.AddListener(progressToPickMode);
    QuitButton.onClick.AddListener(quitGame);
  	CombinedButton.onClick.AddListener(startCombinedGame);
    LowestButton.onClick.AddListener(startLowestGame);
    BackButton.onClick.AddListener(goBack);
    //set high scores
    HighScoreCombinedText.text = "Best:" + prefs.getHighScore(Prefs.GameMode.Combined);
    HighScoreLowestText.text = "Best:" + prefs.getHighScore(Prefs.GameMode.Lowest);
	}
	
  //this happens when Press Start is played
	void progressToPickMode () {
    StartCoroutine(animateHighlight(HighlightA, StartButton));
  }

  void quitGame () {
    StartCoroutine(animateHighlight(HighlightD, QuitButton));
  }

  void startCombinedGame () {
    StartCoroutine(animateHighlight(HighlightA, CombinedButton));
  }

  void startLowestGame () {
    StartCoroutine(animateHighlight(HighlightB, LowestButton));
  }

  void goBack () {
    transitionScreenBack(currentScreen - 1);
  }

  public IEnumerator animateHighlight(RectTransform whichHighlight, Button whichButton) {
    float t = 0;
    float startX = whichHighlight.anchoredPosition.x;
    whichHighlight.anchoredPosition = new Vector2(startX, whichButton.GetComponent<RectTransform>().anchoredPosition.y);
    while(t <= 2.0) {
      t += Time.deltaTime/0.5f;
      float amt = 0;
      if(t <= 1.0) {
        amt = Mathf.Lerp(770, 0, Animations.QuintEaseIn(t, 0f, 1f, 1));
      } else {
        float u = t - 1.0f;
        amt = Mathf.Lerp(0, -770, Animations.QuintEaseIn(u, 0f, 1f, 1));
      }
      whichHighlight.anchoredPosition = new Vector2(amt, whichHighlight.anchoredPosition.y);
      yield return null;
    }
    //move back to start
    whichHighlight.anchoredPosition = new Vector2(startX, whichHighlight.anchoredPosition.y);
    loadNextScreen(whichButton);
  }

  public IEnumerator animateScreenObjects(RectTransform parent, int finalPosition) {
    float t = 0;
    float parentInitialPosition = parent.anchoredPosition.x;
    while(t <= 1.0) {
      t += Time.deltaTime/0.5f;
      float amt = Mathf.Lerp(parentInitialPosition, finalPosition, Animations.QuintEaseIn(t, 0f, 1f, 1));
      parent.anchoredPosition = new Vector2(amt, parent.anchoredPosition.y);
      yield return null;
    }
  }

  void transitionScreen(int screenNo) {
    RectTransform oldScreen = GameObject.Find("Screen"+currentScreen).GetComponent<RectTransform>();
    RectTransform newScreen = GameObject.Find("Screen"+screenNo).GetComponent<RectTransform>();
    StartCoroutine(animateScreenObjects(oldScreen, -770));
    StartCoroutine(animateScreenObjects(newScreen, 0));
    currentScreen = screenNo;
  }

  void transitionScreenBack(int screenNo) {
    RectTransform oldScreen = GameObject.Find("Screen"+currentScreen).GetComponent<RectTransform>();
    RectTransform newScreen = GameObject.Find("Screen"+screenNo).GetComponent<RectTransform>();
    StartCoroutine(animateScreenObjects(oldScreen, 770));
    StartCoroutine(animateScreenObjects(newScreen, 0));
    currentScreen = screenNo;
  }


  void loadNextScreen(Button which) {
    switch(which.gameObject.name) {
      case "PlayButton":
        transitionScreen(2);
      break;
      case "Quit":
        Application.Quit();
      break;
      case "Combined":
        prefs.gameMode = Prefs.GameMode.Combined;
        Application.LoadLevel(1);
      break;
      case "Lowest":
        prefs.gameMode = Prefs.GameMode.Lowest;
        Application.LoadLevel(1);
      break;
    }
  }

}
