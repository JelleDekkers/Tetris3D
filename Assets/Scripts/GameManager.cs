using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private Level currentLevel;
    private BlockGroup currentBlockGroup;

    private void Start() {
        currentLevel = Level.Instance;
        currentLevel.Init();
        CreateNewBlockGroup();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            TryMoveBlockGroup(Vector3.left);
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            TryMoveBlockGroup(Vector3.forward);
        else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            TryMoveBlockGroup(Vector3.right);
        else if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            TryMoveBlockGroup(Vector3.back);
        else if(Input.GetKeyDown(KeyCode.Space))
            TryMoveBlockGroup(Vector3.down);
    }

    private void CreateNewBlockGroup() {
        currentBlockGroup = currentLevel.CreateNewBlockGroup();
    }

    private void TryMoveBlockGroup(Vector3 input) {
        if (CanMove(input))
            currentLevel.MoveBlockGroup(currentBlockGroup, input);
    }

    private bool CanMove(Vector3 input) {
        for (int i = 0; i < currentBlockGroup.Blocks.Length; i++) {
            IntVector3 newCoordinate = currentBlockGroup.Blocks[i].Coordinate + input.ToIntVector3();
            if (!currentLevel.Grid.CoordinateExistsInGrid(newCoordinate)) // en is niet occupied
                return false;
        }
        return true;
    }
}
