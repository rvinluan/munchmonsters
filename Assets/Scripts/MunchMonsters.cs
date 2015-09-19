using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MunchMonsters : MonoBehaviour {

  public int[,] board;
  public float tileSize;
  public MonsterManager monsterManager;
  public LeafManager leafManager;
  public GameStateManager gameStateManager;
  public PathDrawer line;
  public ScoreKeeper scoreKeeper;
  public Button backButton;
  public int maxPathLength = 4;
  private bool drawingLine;
  private List<Actor> visitedLeaves;
  private Actor movingActor;

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
    line.shown = false;

    backButton.onClick.AddListener(goBack);

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
    // if(drawingLine) {
    //   Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //   mp.z = 0;
    //   if(visitedLeaves.Count < maxPathLength) {
    //     line.SetPosition(visitedLeaves.Count+1, mp);
    //   } else {
    //     line.SetVertexCount(visitedLeaves.Count+1);
    //   }
    // }
  }

  public void goBack() {
    Application.LoadLevel(0);
  }

  public void restartPath(Actor whichMonster) {
    if(movingActor == whichMonster && visitedLeaves.Count >= 1) {
      movingActor = null;
      drawingLine = false;
      line.shown = false;
      line.positions.Clear();
      visitedLeaves.Clear();
    }
  }

  public void startPath(int r, int c, Actor whichMonster) {
    if(gameStateManager.currentGameState == "ended") {
      return;
    }
    line.shown = true;
    drawingLine = true;
    line.positions.Clear();
    movingActor = whichMonster;
    visitedLeaves.Clear();
    line.SetPosition(0, new Vector3(c*tileSize, r*tileSize, 0));
  }

  public void addToPath(Actor l) {
    //first off, the obvious
    if(!drawingLine) {
      // Debug.Log("not moving right now.");
      return;
    }
    if(movingActor == l) {
      movingActor.restartPathCheck(this);
    }
    if(l.GetType() == typeof(Monster)) {
      return;
    }
    //if first leaf, make sure it's adjacent to your head
    if(movingActor != null && visitedLeaves.Count == 0 && !leafManager.isAdjacent(l, movingActor.row, movingActor.col)) {
      // Debug.Log("not adjacent to monster head.");
      return;
    }
    if(visitedLeaves.Count > 0 && !leafManager.isAdjacent(l, visitedLeaves[visitedLeaves.Count - 1])) {
      // Debug.Log("not adjacent to last leaf.");
      return;
    }
    if(visitedLeaves.Contains(l)) {
      if(l == visitedLeaves[visitedLeaves.Count - 1]) {
        // Debug.Log("is already the last leaf.");
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
    line.clearPositions();
    for(int i = 0; i < visitedLeaves.Count; i++) {
      Actor nl = visitedLeaves[i];
      line.SetPosition(i+1, new Vector3(nl.col*tileSize, nl.row*tileSize, 0));
    }
  }

  public void endPath(int r, int c) {
    if(drawingLine) {
      drawingLine = false;
      line.shown = false;
      StartCoroutine(movingActor.eat(visitedLeaves));
    }
  }
}
