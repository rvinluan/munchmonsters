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

  public override void changeColor(string newColor) {
    this.color = newColor;
    this.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Tile_"+this.color, typeof(Sprite));
  }
}
