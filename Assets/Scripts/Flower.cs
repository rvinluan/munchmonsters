using UnityEngine;
using System.Collections;

public class Flower : Actor {

  public void Spawn (int c, int r, string clr) {
    col = c;
    row = r;
    //texture
    this.GetComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load("Flower_B", typeof(Sprite));
    // color = clr[clr.Length - 1].ToString();
    // if(clr == "Tile_Empty") {
    //   color = "E";
    //   return;
    // }
    // //flip?
    // if (Random.value > 0.5) {
    //   Vector3 temp = transform.localScale;
    //   temp.x *= -1;
    //   transform.localScale = temp;
    // }
  }

  new void Update () {
    transform.Rotate(0,0,0.5f);
  }

  void OnMouseOver () {
    Debug.Log("hey, flower over here!");
    GameObject.Find("Managers").GetComponent<MunchMonsters>().addToPath(this);
  }

  void OnMouseUp () {
    GameObject.Find("Managers").GetComponent<MunchMonsters>().endPath(this.row, this.col);
  }

}
