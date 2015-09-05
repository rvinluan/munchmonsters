using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LeafManager : MonoBehaviour {

  public static string[] leafTextures = new string[] { "Tile_A", "Tile_B", "Tile_C", "Tile_D" };

  public List<Leaf> leaves;
  public Leaf leafPrefab;

	// Use this for initialization
	void Start () {
	 leaves = new List<Leaf>();
	}

  public void Restart () {
    leaves = new List<Leaf>();
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
    leaves.Add(leaf);
    return leaf;
  }

  public Leaf GenerateEmptyLeaf(int c, int r) {
    var el = GenerateNewLeaf(c, r);
    el.Spawn(c, r, "Tile_Empty");
    return el;
  }

  public bool isAdjacent(Leaf leaf, int r, int c) {
    int xdiff = Math.Abs(leaf.col - c);
    int ydiff = Math.Abs(leaf.row - r);
    return (xdiff != ydiff) && (xdiff == 0 || ydiff == 0) && (xdiff < 2 && ydiff < 2);
  }

  public bool isAdjacent(Leaf l1, Leaf l2) {
    int xdiff = Math.Abs(l1.col - l2.col);
    int ydiff = Math.Abs(l1.row - l2.row);
    return (xdiff != ydiff) && (xdiff == 0 || ydiff == 0) && (xdiff < 2 && ydiff < 2);
  }
}
