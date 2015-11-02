using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class LeafManager : MonoBehaviour {

  public static string[] leafTextures = new string[] { "Tile_A", "Tile_B", "Tile_C", "Tile_D" };

  public Leaf leafPrefab;
  public Flower flowerPrefab;
  public Image nextLeafImage;

	// Use this for initialization
	void Start () {
    //renewNextLeaf();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public Leaf GetLeafAt(int c, int r) {
    foreach (Transform child in transform)
    {
      Leaf l = child.GetComponent<Leaf>();
      Debug.Log(l.col);
      Debug.Log(l.row);
    }
    return null;
  }

  public Leaf GenerateNewNextLeaf(int c, int r) {
    float tileSize = transform.parent.GetComponent<MunchMonsters>().tileSize;
    Leaf leaf = Instantiate(
      leafPrefab,
      new Vector3(c*tileSize, r*tileSize, 1),
      Quaternion.identity
    ) as Leaf;
    leaf.Spawn(c, r, nextLeafImage.sprite.name);
    leaf.gameObject.transform.parent = transform;
    return leaf;
  }

  public void renewNextLeaf() {
    List<string> possibleColors = new List<string>(leafTextures);
    string randTile = possibleColors[ (int)(UnityEngine.Random.value * possibleColors.Count) ];
    nextLeafImage.sprite = (Sprite)Resources.Load(randTile, typeof(Sprite));
  }

  public Leaf GenerateNewLeafSpecificColor(string clr, int c, int r) {
    float tileSize = transform.parent.GetComponent<MunchMonsters>().tileSize;
    Leaf leaf = Instantiate(
      leafPrefab,
      new Vector3(c*tileSize, r*tileSize, 1),
      Quaternion.identity
    ) as Leaf;
    leaf.Spawn(c, r, "Tile_"+clr);
    leaf.gameObject.transform.parent = transform;
    return leaf;
  }

  public Leaf GenerateNewLeaf(int c, int r) {
    //exclude no colors
    return GenerateNewLeaf(c, r, "");
  }

  public Leaf GenerateNewLeaf(int c, int r, string excludeColor) {
    float tileSize = transform.parent.GetComponent<MunchMonsters>().tileSize;
    List<string> possibleColors = new List<string>(leafTextures);
    possibleColors.Remove(excludeColor);
    string randTile = possibleColors[ (int)(UnityEngine.Random.value * possibleColors.Count) ];
    Leaf leaf = Instantiate(
      leafPrefab,
      new Vector3(c*tileSize, r*tileSize, 1),
      Quaternion.identity
    ) as Leaf;
    leaf.Spawn(c, r, randTile);
    leaf.gameObject.transform.parent = transform;
    return leaf;
  }

  public Leaf GenerateEmptyLeaf(int c, int r) {
    var el = GenerateNewLeaf(c, r);
    el.Spawn(c, r, "Tile_Empty");
    return el;
  }


  public Flower GenerateNewFlower(int c, int r, string color) {
    float tileSize = transform.parent.GetComponent<MunchMonsters>().tileSize;
    Flower flower = Instantiate(
      flowerPrefab,
      new Vector3(c*tileSize, r*tileSize, 0.9f),
      Quaternion.identity
    ) as Flower;
    flower.Spawn(c, r, color);
    flower.gameObject.transform.parent = transform;
    return flower;
  }

  public bool isAdjacent(Actor leaf, int r, int c) {
    int xdiff = Math.Abs(leaf.col - c);
    int ydiff = Math.Abs(leaf.row - r);
    return (xdiff != ydiff) && (xdiff == 0 || ydiff == 0) && (xdiff < 2 && ydiff < 2);
  }

  public bool isAdjacent(Actor l1, Actor l2) {
    int xdiff = Math.Abs(l1.col - l2.col);
    int ydiff = Math.Abs(l1.row - l2.row);
    return (xdiff != ydiff) && (xdiff == 0 || ydiff == 0) && (xdiff < 2 && ydiff < 2);
  }
}
