using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathDrawer : MonoBehaviour {

  public List<Vector3> positions;
  public List<SpriteRenderer> pathCircles;
  public List<SpriteRenderer> pathMiddles;
  public SpriteRenderer circleSprite;
  public SpriteRenderer pathSprite;
  public SpriteRenderer endSprite;
  public bool shown;

	// Use this for initialization
	void Start () {
    positions = new List<Vector3>();
  	pathCircles = new List<SpriteRenderer>();
    int maxPath = GameObject.Find("Managers").GetComponent<MunchMonsters>().maxPathLength;
    for(int i = 0; i < maxPath+1; i++) {
      SpriteRenderer c = Instantiate(
        circleSprite,
        new Vector3(0, 0, 1),
        Quaternion.identity
      ) as SpriteRenderer;
      pathCircles.Add(c);
      c.enabled = false;
      SpriteRenderer p = Instantiate(
        pathSprite,
        new Vector3(0, 0, 1),
        Quaternion.identity
      ) as SpriteRenderer;
      pathMiddles.Add(p);
      p.enabled = false;
    }
    endSprite.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
    for(int i = 0; i < pathCircles.Count; i++) {
      pathCircles[i].enabled = false;
      pathMiddles[i].enabled = false;
    }
    endSprite.enabled = false;
    if(shown) {
      for(int i = 0; i < positions.Count; i++) {
        pathCircles[i].enabled = true;
        pathCircles[i].transform.position = positions[i];
        if(i > 0) {
          bool flip;
          pathMiddles[i].enabled = true;
          pathMiddles[i].transform.position = positionCorrectMiddle(positions[i], positions[i-1], out flip);
          if(flip) {
            pathMiddles[i].transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
          } else {
            pathMiddles[i].transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
          }
        }
        if(i == 4) {
          endSprite.enabled = true;
          endSprite.transform.position = positions[i];
        } else {
          endSprite.enabled = false;
        }
      }
    }
	}

  public Vector3 positionCorrectMiddle(Vector3 pos1, Vector3 pos2, out bool flipped) {
    float tileSize = GameObject.Find("Managers").GetComponent<MunchMonsters>().tileSize;
    if(pos1.y == pos2.y) {
      //then they are horizontally adjacent
      flipped = false;
      if(pos1.x < pos2.x) {
        //then it should go right:
        return new Vector3( pos1.x + tileSize/2, pos1.y, 1 );
      } else {
        return new Vector3( pos1.x - tileSize/2, pos1.y, 1 );
      }
    } else {
      //then they are vertically adjacent
      flipped = true;
      if(pos1.y < pos2.y) {
        //then it should go up:
        return new Vector3( pos1.x, pos1.y + tileSize/2, 1 );
      } else {
        return new Vector3( pos1.x, pos1.y - tileSize/2, 1 );
      }
    }
  }

  public void SetPosition(int index, Vector3 pos) {
    if(positions.Count > index) {
      positions[index] = pos;
    } else {
      positions.Add(pos);
    }
  }

  public void clearPositions() {
    Vector3 temp = positions[0];
    positions.Clear();
    positions.Add(temp);
  }
}
