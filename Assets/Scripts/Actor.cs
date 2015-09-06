using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Actor : MonoBehaviour {
  public int row;
  public int col;
  public string color;

  public void Start() {

  }

  public void Update() {

  }

  public virtual void changeColor(string newColor) {
    this.color = newColor;
  }

  public virtual IEnumerator eat(List<Actor> visited) {
    yield return null;
  }

  public virtual void restartPathCheck(MunchMonsters mm) {

  }

  void OnMouseOver () {
    GameObject.Find("Managers").GetComponent<MunchMonsters>().addToPath(this);
  }

  void OnMouseUp () {
    GameObject.Find("Managers").GetComponent<MunchMonsters>().endPath(this.row, this.col);
  }
}