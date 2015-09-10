using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScoreKeeper : MonoBehaviour {

  public Text scoreForA;
  public Text scoreForB;
  public Text scoreForC;
  public Text scoreForD;
  public Text multiplierForA;
  public Text multiplierForB;
  public Text multiplierForC;
  public Text multiplierForD;
  public RectTransform sizerA;
  public RectTransform sizerB;
  public RectTransform sizerC;
  public RectTransform sizerD;
  private Dictionary<string, int> scores;
  private Dictionary<string, int> multipliers;
  private Dictionary<string, RectTransform> sizers;
  private string lowestScore;
  private Prefs prefs;
  private static string[] colors = new string[4] {"A", "B", "C", "D"};

  public void Restart() {
    lowestScore = "A";
    scores["A"] = 0;
    scores["B"] = 0;
    scores["C"] = 0;
    scores["D"] = 0;
    multipliers["A"] = 1;
    multipliers["B"] = 1;
    multipliers["C"] = 1;
    multipliers["D"] = 1;
    multiplierForA.enabled = false;
    multiplierForB.enabled = false;
    multiplierForC.enabled = false;
    multiplierForD.enabled = false;
    if(prefs.gameMode == Prefs.GameMode.Lowest) {
      updateScoreIndicator(lowestScore);
    }
  }

	// Use this for initialization
	void Start () {
    lowestScore = "A";
    scores = new Dictionary<string, int>();
    scores.Add("A", 0);
    scores.Add("B", 0);
    scores.Add("C", 0);
    scores.Add("D", 0);
    multipliers = new Dictionary<string, int>();
    multipliers.Add("A", 1);
    multipliers.Add("B", 1);
    multipliers.Add("C", 1);
    multipliers.Add("D", 1);
    multiplierForA.enabled = false;
    multiplierForB.enabled = false;
    multiplierForC.enabled = false;
    multiplierForD.enabled = false;
    sizers = new Dictionary<string, RectTransform>();
    sizers.Add("A", sizerA);
    sizers.Add("B", sizerB);
    sizers.Add("C", sizerC);
    sizers.Add("D", sizerD);
    prefs = GameObject.Find("Prefs").GetComponent<Prefs>();
	}
	
	// Update is called once per frame
	void Update () {
    scoreForA.text = scores["A"].ToString();
    scoreForB.text = scores["B"].ToString();
    scoreForC.text = scores["C"].ToString();
    scoreForD.text = scores["D"].ToString();
	}

  public void addMultiplier(string mon) {
    multipliers[mon]++;
    Text[] mults = new Text[4] { multiplierForA, multiplierForB, multiplierForC, multiplierForD};
    for(int i = 0; i < 4; i++) {
      Text m = mults[i];
      if(multipliers[colors[i]] == 1) {
        m.enabled = false;
      } else {
        m.text = "x" + multipliers[colors[i]].ToString();
        m.enabled = true;
      }
    }
  }

  public IEnumerator sizeSizer(RectTransform which, int finalSize) {
    float startSize = which.sizeDelta.x;
    float t = 0;
    while(t <= 1.0) {
      t += Time.deltaTime;
      float amt = Mathf.Lerp(startSize, finalSize, Animations.CubicEaseInOut(t, 0f, 1f, 0.3f));
      which.sizeDelta = new Vector2(amt, amt);
      yield return null;
    }
  }

  public void updateScoreIndicator(string lowestScore) {
    for(int i = 0; i < 4; i++) {
      string cur = colors[i];
      if(cur == lowestScore) {
        StartCoroutine( sizeSizer(sizers[cur], 125) );
      } else {
        StartCoroutine( sizeSizer(sizers[cur], 75) );
      }
    }
  }

  public void addPoints(string mon, int numPoints) {
    scores[mon] += numPoints * multipliers[mon];
    string tempLowestScore = "A";
    foreach(KeyValuePair<string, int> sc in scores) {
      if(sc.Value < scores[tempLowestScore]) {
        tempLowestScore = sc.Key;
      }
    }
    lowestScore = tempLowestScore;
    if(prefs.gameMode == Prefs.GameMode.Lowest) {
      updateScoreIndicator(lowestScore);
    }
  }

  public int getLowestScore() {
    return scores[lowestScore];
  }
  public int getCombinedScore() {
    int total = 0;
    foreach(KeyValuePair<string, int> sc in scores) {
      total += sc.Value;
    }
    return total;
  }
}
