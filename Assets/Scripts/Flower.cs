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

}
