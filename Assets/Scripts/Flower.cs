using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flower : Actor {

  public void Spawn (int c, int r, string clr) {
    col = c;
    row = r;
    //texture
    this.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(clr, typeof(Sprite));
    color = clr[clr.Length - 1].ToString();
  }

  new void Update () {
    transform.Rotate(0,0,0.5f);
  }

  void OnMouseDown () {
    GameObject.Find("Managers").GetComponent<MunchMonsters>().startPath(this.row, this.col, this);
  }

  public override void restartPathCheck(MunchMonsters mm) {
      mm.restartPath(this);
  }

  public override IEnumerator eat(List<Actor> visited) {
    foreach(Actor l in visited) {
      l.changeColor(this.color);
    }
    GameObject.Destroy(gameObject);
    yield return null;
  }

}
