using UnityEngine;
using System.Collections;

public class Leaf : Actor {

  public void Spawn (int c, int r, string clr) {
    col = c;
    row = r;
    //texture
    this.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(clr, typeof(Sprite));
    color = clr[clr.Length - 1].ToString();
    if(clr == "Tile_Empty") {
      color = "E";
      return;
    }
    //flip?
    if (Random.value > 0.5) {
      Vector3 temp = transform.localScale;
      temp.x *= -1;
      transform.localScale = temp;
    }
  }
  
  void OnMouseOver () {
    GameObject.Find("Managers").GetComponent<MunchMonsters>().addToPath(this);
  }

  void OnMouseUp () {
    GameObject.Find("Managers").GetComponent<MunchMonsters>().endPath(this.row, this.col);
  }
}
