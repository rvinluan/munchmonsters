using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Seed : Actor {

  public Transform seedSprite;
  public Transform leafSprite_A;
  public Transform leafSprite_B;
  public Transform leafSprite_C;
  public Transform leafSprite_D;
  public Seed seedPrefab;
  private int score;
  private Dictionary<string, bool> eaten;
  private bool allEaten;

	// Use this for initialization
	new void Start () {
    this.color = "E";
    this.allEaten = false;
    this.eaten = new Dictionary<string, bool>();
    eaten.Add("A", false);
    leafSprite_A.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    eaten.Add("B", false);
    leafSprite_B.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    eaten.Add("C", false);
    leafSprite_C.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    eaten.Add("D", false);
    leafSprite_D.gameObject.GetComponent<SpriteRenderer>().enabled = true;
	}

  new void Update () {
    transform.Rotate(0,0,0.5f);
  }

  public void Spawn (int c, int r) {
    this.resetPosition(c, r);
  }

  public bool hasEaten(string C) {
    return eaten[C];
  }

  public void eatLeaves(string C) {
    if(C == "E") {
      return;
    }
    if(!this.hasEaten(C)) {
      this.eaten[C] = false;
      switch(C) {
        case "A":
          leafSprite_A.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        break;
        case "B":
          leafSprite_B.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        break;
        case "C":
          leafSprite_C.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        break;
        case "D":
          leafSprite_D.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        break;
      }
      this.eaten[C] = true;
      if(this.eaten["A"] &&
        this.eaten["B"] &&
        this.eaten["C"] &&
        this.eaten["D"]) {
          float tileSize = GameObject.Find("Managers").GetComponent<MunchMonsters>().tileSize;
          GameObject[] allLeaves = GameObject.FindGameObjectsWithTag("Leaf");
          Leaf rl = allLeaves[Random.Range(0, allLeaves.Length)].GetComponent<Leaf>();
          Seed newSeed = Instantiate(
            seedPrefab,
            new Vector3(rl.col*tileSize, rl.row*tileSize, 1),
            Quaternion.identity
          ) as Seed;
          newSeed.Spawn(rl.col, rl.row);
          newSeed.transform.parent = GameObject.Find("SeedParent").transform;
          Object.Destroy(rl.gameObject);
          this.allEaten = true;
          GameObject.Find("scoreKeeper").GetComponent<ScoreKeeper>().addSeedPoints(1);
      }
    }
  }

  public void resetPosition() {
    GameObject[] allLeaves = GameObject.FindGameObjectsWithTag("Leaf");
    Leaf rl = allLeaves[Random.Range(0, allLeaves.Length)].GetComponent<Leaf>();
    this.resetPosition(rl.col, rl.row);
    Object.Destroy(rl.gameObject);
  }

  public void resetPosition(int c, int r) {
    // Debug.Log("original seed c: " + this.col);
    // Debug.Log("set to c: " + c);
    this.col = c;
    this.row = r;
    float tileSize = GameObject.Find("Managers").GetComponent<MunchMonsters>().tileSize;
    seedSprite.position = new Vector3(c*tileSize, r*tileSize, 1);
  }

  public override void OnMouseOver () {
    if(!this.allEaten) {
      GameObject.Find("Managers").GetComponent<MunchMonsters>().addToPath(this);
    }
  }

  // void OnMouseDown () {
  //   if(!this.allEaten) {
  //     GameObject.Find("Managers").GetComponent<MunchMonsters>().startPath(this.row, this.col, this);
  //   }
  // }

  // public override void restartPathCheck(MunchMonsters mm) {
  //     mm.restartPath(this);
  // }

  // public override IEnumerator eat(List<Actor> visited) {
  //   if(this.score <= 0) {
  //     yield return null;
  //   } else {
  //     this.score = this.score - 1;
  //   }
  //   foreach(Actor l in visited) {
  //     l.changeColor(this.color);
  //   }
  //   this.updateColor("E");
  //   yield return null;
  // }
}
