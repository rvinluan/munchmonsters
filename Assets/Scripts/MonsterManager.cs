using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterManager : MonoBehaviour {

  public List<Monster> monsters;

  public Monster monsterPrefab;
  private struct MonsterSegment {
    public string monsterName;
    public int numSegment;
  }

  public string getMonsterAtStartPosition (int x, int y) {
    return getStartingMonsterHere(x, y).monsterName;
  }

  public int getSegmentAtStartPosition (int x, int y) {
    return getStartingMonsterHere(x, y).numSegment;
  }

  private MonsterSegment getStartingMonsterHere (int x, int y) {
    MonsterSegment ms;
    ms.monsterName = "";
    ms.numSegment = -1;
    string[,] visualArray = new string[,] {
      {"00", "00", "00", "00", "00", "00", "00", "00"}, //this is the top left
      {"00", "D3", "00", "00", "00", "A3", "00", "00"},
      {"00", "D2", "D1", "00", "00", "A2", "A1", "00"},
      {"00", "00", "D0", "00", "00", "00", "A0", "00"},
      {"00", "C3", "00", "00", "00", "B3", "00", "00"},
      {"00", "C2", "C1", "00", "00", "B2", "B1", "00"},
      {"00", "00", "C0", "00", "00", "00", "B0", "00"},
      {"00", "00", "00", "00", "00", "00", "00", "00"}  //this is the top right
    };
    string key = visualArray[x, y];
    if(key == "00") {
      return ms;
    }
    ms.monsterName = key[0].ToString();
    ms.numSegment = int.Parse(key[1].ToString());
    return ms;
  }

  public Monster getMonsterSegment (string monsterName, int segment) {
    if(segment > 3 || segment < 0) {
      return null;
    }
    foreach(var m in monsters) {
      if(m.GetComponent<Monster>().monsterName == monsterName 
        && m.GetComponent<Monster>().numSegment == segment) {
        return m;
      }
    }
    return null;
  }

  public Monster GenerateNewMonster (int c, int r, string mon) {
    float tileSize = transform.parent.GetComponent<MunchMonsters>().tileSize;
    Monster m = Instantiate(
      monsterPrefab,
      new Vector3(c*tileSize, r*tileSize, 1),
      Quaternion.identity
    ) as Monster;
    m.gameObject.transform.parent = transform;
    m.Spawn(c, r, mon);
    monsters.Add(m);
    return m;
  }

  public void monsterStarve(Monster m) {
    GameObject.Find("gameStateManager").GetComponent<GameStateManager>().displayModal();
  }

	// Use this for initialization
	void Start () {
    monsters = new List<Monster>();
	}

  public void Restart () {
    monsters = new List<Monster>();
  }
	
	// Update is called once per frame
	void Update () {
	
	}
}
