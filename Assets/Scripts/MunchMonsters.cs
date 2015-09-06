using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MunchMonsters : MonoBehaviour {

  public int[,] board;
  public float tileSize;
  public MonsterManager monsterManager;
  public LeafManager leafManager;
  public GameStateManager gameStateManager;
  public LineRenderer line;
  public ScoreKeeper scoreKeeper;
  public int maxPathLength = 4;
  private bool drawingLine;
  private List<Actor> visitedLeaves;
  private Monster movingMonster;

  public void Restart () {
    foreach(Transform child in leafManager.transform) {
      GameObject.Destroy(child.gameObject);
    }
    foreach(Transform child in monsterManager.transform) {
      GameObject.Destroy(child.gameObject);
    }
    monsterManager.Restart();
    gameStateManager.Restart();
    scoreKeeper.Restart();
    Start();
  }

	// Use this for initialization
	void Start () {
    Screen.orientation = ScreenOrientation.Portrait;

    board = new int[8,8];
    visitedLeaves = new List<Actor>();
    string mon;
    drawingLine = false;
    line.enabled = false;

    for(int i = 0; i < board.GetLength(0); i++) {
      for(int j = 0; j < board.GetLength(1); j++) {
        mon = monsterManager.getMonsterAtStartPosition(i, j);
        if(mon != "") {
          //make a monster
          board[i, j] = 1;
          monsterManager.GenerateNewMonster(i, j, mon);
        } else {
          //make a leaf
          board[i, j] = 0;
          leafManager.GenerateNewLeaf(i, j);
        }
      }
    }//end for loop

    foreach(var m in monsterManager.monsters) {
      //assign previous and next pointers.
      //you can't do this until all the segments have been placed.
      m.prevSegment = monsterManager.getMonsterSegment(m.color, m.numSegment - 1);
      m.nextSegment = monsterManager.getMonsterSegment(m.color, m.numSegment + 1);
    }
	}
	
	// Update is called once per frame
  void Update () {
    if(drawingLine) {
      Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      mp.z = 0;
      if(visitedLeaves.Count < maxPathLength) {
        line.SetPosition(visitedLeaves.Count+1, mp);
      } else {
        line.SetVertexCount(visitedLeaves.Count+1);
      }
    }
  }

  public void restartPath(Monster whichMonster) {
    if(movingMonster == whichMonster && visitedLeaves.Count >= 1) {
      movingMonster = null;
      drawingLine = false;
      line.enabled = false;
      line.SetVertexCount(2);
      visitedLeaves.Clear();
    }
  }

  public void startPath(int r, int c, Monster whichMonster) {
    if(gameStateManager.currentGameState == "ended") {
      return;
    }
    line.enabled = true;
    drawingLine = true;
    movingMonster = whichMonster;
    visitedLeaves.Clear();
    line.SetPosition(0, new Vector3(c*tileSize, r*tileSize, 0));
  }

  public void addToPath(Actor l) {
    //first off, the obvious
    if(!drawingLine) {
      Debug.Log("not moving right now.");
      return;
    }
    //if first leaf, make sure it's adjacent to your head
    if(movingMonster != null && visitedLeaves.Count == 0 && !leafManager.isAdjacent(l, movingMonster.row, movingMonster.col)) {
      Debug.Log("not adjacent to monster head.");
      return;
    }
    if(visitedLeaves.Count > 0 && !leafManager.isAdjacent(l, visitedLeaves[visitedLeaves.Count - 1])) {
      Debug.Log("not adjacent to last leaf.");
      return;
    }
    if(visitedLeaves.Contains(l)) {
      if(l == visitedLeaves[visitedLeaves.Count - 1]) {
        Debug.Log("is already the last leaf.");
        return;
      } else {
        int index = visitedLeaves.IndexOf(l);
        int numToRemove = visitedLeaves.Count - index;
        visitedLeaves.RemoveRange(index, numToRemove);
      }
    }
    if(!drawingLine || visitedLeaves.Count >= maxPathLength) { 
      return; 
    }
    visitedLeaves.Add(l);
    int totalPathLength = visitedLeaves.Count + 2;
    line.SetVertexCount(totalPathLength);
    for(int i = 0; i < totalPathLength - 2; i++) {
      Actor nl = visitedLeaves[i];
      line.SetPosition(i+1, new Vector3(nl.col*tileSize, nl.row*tileSize, 0));
    }
  }

  public void endPath(int r, int c) {
    if(drawingLine) {
      drawingLine = false;
      line.enabled = false;
      StartCoroutine(movingMonster.eat(visitedLeaves));
      line.SetVertexCount(2);
    }
  }
}
