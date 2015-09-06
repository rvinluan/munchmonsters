using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster : Actor {

  public static string[] monsterTextures = new string[] { 
    "Monster_Head", "Monster_Body_Straight", "Monster_Body_Curved" 
  };
  public const int NOSEGMENT = -1;
  public const int NORTH     = 0;
  public const int EAST      = 1;
  public const int SOUTH     = 2;
  public const int WEST      = 3;

  public int numSegment;
  public Monster prevSegment;
  public Monster nextSegment;

  private MonsterManager manager;

  public void Spawn (int c, int r, string mon) {
    col = c;
    row = r;
    color = mon;
    manager = transform.parent.GetComponent<MonsterManager>();
    numSegment = manager.getSegmentAtStartPosition(c, r);
  }

  private int getDirectionOfSegment (Monster seg) {
    if(seg == null) {
      return Monster.NOSEGMENT;
    }
    if(this.col == seg.col) {
      if(this.row < seg.row) { 
        return Monster.NORTH; 
      }
      else { 
        return Monster.SOUTH; 
      }
    } else {
      if(this.col < seg.col) { 
        return Monster.EAST; 
      }
      else { 
        return Monster.WEST; 
      }
    }
  }

  private Sprite determineSprite (int prevFacing, int nextFacing) {
    int rotation = 0;
    string textureName = "";
    Sprite result;

    if(prevFacing == Monster.NOSEGMENT || nextFacing == Monster.NOSEGMENT) {
      if(nextFacing == Monster.NOSEGMENT) {
        textureName = "Monster_Tail";
      } else {
        textureName = "Monster_Head_" + color;
      }
      int otherFacing = prevFacing == Monster.NOSEGMENT ? nextFacing : prevFacing;
      switch (otherFacing) {
        case Monster.NORTH :
          rotation = 180;
          break;
        case Monster.EAST :
          rotation = 90;
          break;
        case Monster.WEST :
          rotation = -90;
          break;
      }
      transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
      result = (Sprite)Resources.Load(textureName, typeof(Sprite));
      return result;
    }//end head

    if(prevFacing == Monster.NORTH || nextFacing == Monster.NORTH) {
      textureName = "Monster_Body_Curved";
      int otherFacing = prevFacing == Monster.NORTH ? nextFacing : prevFacing;
      switch (otherFacing) {
        case Monster.SOUTH :
          textureName = "Monster_Body_Straight";
          rotation = 0;
          break;
        case Monster.EAST :
          rotation = 90;
          break;
        case Monster.WEST :
          rotation = 180;
          break;
      }
      transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
      result = (Sprite)Resources.Load(textureName, typeof(Sprite));
      return result;
    }//end north

    if(prevFacing == Monster.SOUTH || nextFacing == Monster.SOUTH) {
      textureName = "Monster_Body_Curved";
      int otherFacing = prevFacing == Monster.SOUTH ? nextFacing : prevFacing;
      switch (otherFacing) {
        case Monster.EAST :
          rotation = 0;
          break;
        case Monster.WEST :
          rotation = -90;
          break;
      }
      transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
      result = (Sprite)Resources.Load(textureName, typeof(Sprite));
      return result;
    }//end south

    if(prevFacing == Monster.EAST || nextFacing == Monster.EAST) {
      textureName = "Monster_Body_Straight";
      int otherFacing = prevFacing == Monster.EAST ? nextFacing : prevFacing;
      switch (otherFacing) {
        case Monster.WEST :
          rotation = 90;
          break;
      }
      transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
      result = (Sprite)Resources.Load(textureName, typeof(Sprite));
      return result;
    }//last case: east/west

    //default
    transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
    result = (Sprite)Resources.Load("Monster_Body_Straight", typeof(Sprite));
    return result;    
  }

  public IEnumerator eat(List<Actor> foodArray) {
    int correctColorCount = 0;
    if(foodArray.Count == 0) {
      //this was probably an accidental swipe
      yield break;
    }
    for(int h = 0; h < foodArray.Count; h++) {
      if(foodArray[h].color == this.color) {
        correctColorCount++;
      }
    }
    for(int i = 0; i < foodArray.Count; i++) {
      Actor eatenLeaf = foodArray[i];
      if(i == 0 && correctColorCount == 4) {
        moveForwardOnce(eatenLeaf.row, eatenLeaf.col, eatenLeaf, true);
      } else {
        moveForwardOnce(eatenLeaf.row, eatenLeaf.col, eatenLeaf, false);
      }
      yield return new WaitForSeconds(0.2f);
    }
    foodArray.Clear();
    if(correctColorCount == 0) {
      transform.parent.GetComponent<MonsterManager>().monsterStarve(this);
    } else {
      if(correctColorCount == 4) {
        //combo!
        GameObject.Find("scoreKeeper").GetComponent<ScoreKeeper>().addMultiplier(color);
      }
      GameObject.Find("scoreKeeper").GetComponent<ScoreKeeper>().addPoints(color, correctColorCount);
    }
  }

  public void moveForwardOnce(int newRow, int newCol, Actor eaten, bool flower) {
    int oldRow = this.row;
    int oldCol = this.col;
    this.row = newRow;
    this.col = newCol;
    if(nextSegment != null) {
      nextSegment.moveForwardOnce(oldRow, oldCol, eaten, flower);
    } else {
      if(flower) {
        GameObject.Find("leafManager").GetComponent<LeafManager>().GenerateNewFlower(oldCol, oldRow, "Flower_"+this.color);
        Object.Destroy(eaten.gameObject);
      } else if(eaten.color == color) {
        //put a new leaf in this place
        string clr = "Tile_" + this.color;
        GameObject.Find("leafManager").GetComponent<LeafManager>().GenerateNewLeaf(oldCol, oldRow, clr);
        Object.Destroy(eaten.gameObject);
      } else {
        //don't make a new leaf
        GameObject.Find("leafManager").GetComponent<LeafManager>().GenerateEmptyLeaf(oldCol, oldRow);
        Object.Destroy(eaten.gameObject);
      }
    }
  }
	
	// Update is called once per frame
	new void Update () {
    float tileSize = GameObject.Find("Managers").GetComponent<MunchMonsters>().tileSize;
    transform.position = new Vector3(this.col*tileSize, this.row*tileSize, 1);
    this.GetComponent<SpriteRenderer>().sprite = determineSprite(getDirectionOfSegment(prevSegment), getDirectionOfSegment(nextSegment));
	}

  void OnMouseOver () {
    if(this.numSegment == 0)
      GameObject.Find("Managers").GetComponent<MunchMonsters>().restartPath(this);
  }

  void OnMouseDown () {
    if(this.numSegment == 0)
      GameObject.Find("Managers").GetComponent<MunchMonsters>().startPath(this.row, this.col, this);
  }
  void OnMouseUp () {
    GameObject.Find("Managers").GetComponent<MunchMonsters>().endPath(this.row, this.col);
  }
}
