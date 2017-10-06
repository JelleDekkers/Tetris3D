using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {

    public IntVector3 Coordinate { get; set; }
    public GameObject gObj;

    public Block(IntVector3 coordinate) {
        Coordinate = coordinate;
    }

    public void SetGameObject(GameObject gObj) {
        this.gObj = gObj;
    }

    public void ChangeColor(Color c) {
        gObj.GetComponent<Renderer>().material.color = c;
    }
}
