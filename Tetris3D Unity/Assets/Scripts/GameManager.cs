using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private Level currentLevel;
    private BlockGroup currentBlockGroup;
    private float timer;
    [SerializeField]
    private float timeBetweenHeightDecrease = 1.5f;

    public float score;

    private bool gameOver;

    private void Start() {
        currentLevel = Level.Instance;
        currentLevel.Init();
        CreateNewBlockGroup();
        timer = timeBetweenHeightDecrease;
    }

    private void Update() {
        if (gameOver)
            return;

        // movement:
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            TryMoveBlockGroup(IntVector3.left);
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            TryMoveBlockGroup(IntVector3.forward);
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            TryMoveBlockGroup(IntVector3.right);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            TryMoveBlockGroup(IntVector3.back);
        if (Input.GetKeyDown(KeyCode.Space))
            TryMoveBlockGroup(IntVector3.down);

        // rotation:
        if (Input.GetKeyDown(KeyCode.Q))
            TryRotate(IntVector3.left);
        if (Input.GetKeyDown(KeyCode.E))
            TryRotate(IntVector3.right);
        if (Input.GetKeyDown(KeyCode.X))
            TryRotate(IntVector3.up);
        if (Input.GetKeyDown(KeyCode.Z))
            TryRotate(IntVector3.down);
        if (Input.GetKeyDown(KeyCode.F))
            TryRotate(IntVector3.forward);
        if (Input.GetKeyDown(KeyCode.G))
            TryRotate(IntVector3.back);

        if (timer < 0)
            MoveBlockDown();
        else
            timer -= Time.deltaTime;
    }

    private void GameOver() {
        print("Game Over!");
    }

    private void MoveBlockDown() {
        if(CanMove(IntVector3.down))
            currentLevel.MoveBlockGroup(currentBlockGroup, IntVector3.down);
        else
            CreateNewBlockGroup();
        timer = timeBetweenHeightDecrease;
    }

    private void CreateNewBlockGroup() {
        if (currentBlockGroup != null)
            currentLevel.StoreGroupInGrid(currentBlockGroup);
        if (CanCreateNewBlockGroup())
            currentBlockGroup = currentLevel.CreateNewBlockGroup();
        else
            GameOver();
    }

    private bool CanCreateNewBlockGroup() {
        return !currentLevel.Grid.IsOccupied(currentLevel.GetSpawnCoordinate());
    }

    private void TryMoveBlockGroup(IntVector3 input) {
        if (CanMove(input))
            currentLevel.MoveBlockGroup(currentBlockGroup, input);
    }

    private bool CanMove(IntVector3 direction) {
        for (int i = 0; i < currentBlockGroup.Blocks.Length; i++) {
            IntVector3 newCoordinate = currentBlockGroup.Blocks[i].Coordinate + direction;
            if (!currentLevel.Grid.CoordinateExistsInGrid(newCoordinate) || currentLevel.Grid.IsOccupied(newCoordinate))
                return false;
        }
        return true;
    }

    private void TryRotate(IntVector3 rotation) {
        if (CanRotate(rotation))
            currentLevel.RotateBlockGroup(currentBlockGroup, rotation);
    }

    private bool CanRotate(IntVector3 rotation) {
        for (int i = 0; i < currentBlockGroup.Blocks.Length; i++) {

            Block block = currentBlockGroup.Blocks[i];
            if (block == currentBlockGroup.RotationPivotBlock)
                continue;

            IntVector3 difFromPivot = block.Coordinate - currentBlockGroup.RotationPivotBlock.Coordinate;
            IntVector3 rotationFromPivot = IntVector3.zero;
            if (rotation.x != 0)
                rotationFromPivot = new IntVector3(difFromPivot.z * rotation.x, difFromPivot.y, difFromPivot.x * -rotation.x);
            else if (rotation.y != 0)
                rotationFromPivot = new IntVector3(difFromPivot.y * rotation.y, difFromPivot.x * -rotation.y, difFromPivot.z);
            else if (rotation.z != 0)
                rotationFromPivot = new IntVector3(difFromPivot.x, difFromPivot.z * -rotation.z, difFromPivot.y * rotation.z);
            IntVector3 newCoordinate = currentBlockGroup.RotationPivotBlock.Coordinate + rotationFromPivot;

            if (!currentLevel.Grid.CoordinateExistsInGrid(newCoordinate) || currentLevel.Grid.IsOccupied(newCoordinate)) 
                return false;
        }
        return true;
    }
}
