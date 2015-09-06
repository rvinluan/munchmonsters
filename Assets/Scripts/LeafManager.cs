using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LeafManager : MonoBehaviour {

  public static string[] leafTextures = new string[] { "Tile_A", "Tile_B", "Tile_C", "Tile_D" };

  public Leaf leafPrefab;
  public Flower flowerPrefab;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
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
