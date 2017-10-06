using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private Level currentLevel;
    private BlockGroup currentBlockGroup;
    private float timer;
    private float timeBetweenHeightDecrease = 1.5f;

    public float score;

    private void Start() {
        currentLevel = Level.Instance;
        currentLevel.Init();
        CreateNewBlockGroup();
        timer = timeBetweenHeightDecrease;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            TryMoveBlockGroup(Vector3.left);
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            TryMoveBlockGroup(Vector3.forward);
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            TryMoveBlockGroup(Vector3.right);
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            TryMoveBlockGroup(Vector3.back);
        else if (Input.GetKeyDown(KeyCode.Space))
            TryMoveBlockGroup(Vector3.down);
        else if (Input.GetKeyDown(KeyCode.Q))
            TryRotate(-1, 0);
        else if (Input.GetKeyDown(KeyCode.E))
            TryRotate(1, 0);
        else if (Input.GetKeyDown(KeyCode.X))
            TryRotate(0, 1);
        else if (Input.GetKeyDown(KeyCode.Z))
            TryRotate(0, -1);

        if (timer < 0)
            MoveBlockDown();
        else
            timer -= Time.deltaTime;
    }

    private void MoveBlockDown() {
        if(CanMove(Vector3.down))
            currentLevel.MoveBlockGroup(currentBlockGroup, Vector3.down);
        else
            CreateNewBlockGroup();
        timer = timeBetweenHeightDecrease;
    }

    private void CreateNewBlockGroup() {
        if (currentBlockGroup != null)
            currentLevel.StoreGroupInGrid(currentBlockGroup);
        currentBlockGroup = currentLevel.CreateNewBlockGroup();
    }

    private void TryMoveBlockGroup(Vector3 input) {
        if (CanMove(input))
            currentLevel.MoveBlockGroup(currentBlockGroup, input);
    }

    private bool CanMove(Vector3 direction) {
        for (int i = 0; i < currentBlockGroup.Blocks.Length; i++) {
            IntVector3 newCoordinate = currentBlockGroup.Blocks[i].Coordinate + direction.ToIntVector3();
            if (!currentLevel.Grid.CoordinateExistsInGrid(newCoordinate) || currentLevel.Grid.IsOccupied(newCoordinate))
                return false;
        }
        return true;
    }

    private void TryRotate(int horizontal, int vertical) {
        if (CanRotate(horizontal, vertical))
            currentLevel.RotateBlockGroup(currentBlockGroup, horizontal, vertical);
    }

    private bool CanRotate(int horizontal, int vertical) {
        for (int i = 0; i < currentBlockGroup.Blocks.Length; i++) {

            Block block = currentBlockGroup.Blocks[i];
            if (block == currentBlockGroup.RotationPivotBlock)
                continue;

            IntVector3 difFromPivot = block.Coordinate - currentBlockGroup.RotationPivotBlock.Coordinate;
            IntVector3 rotatedFromPivot = new IntVector3(difFromPivot.z * horizontal, difFromPivot.y, difFromPivot.x * -horizontal);
            IntVector3 newCoordinate = currentBlockGroup.RotationPivotBlock.Coordinate + rotatedFromPivot;

            if (!currentLevel.Grid.CoordinateExistsInGrid(newCoordinate) || currentLevel.Grid.IsOccupied(newCoordinate)) 
                return false;
        }
        return true;
    }
}
