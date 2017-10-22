﻿using System;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public Action OnGroupLockedEvent;
    public Action OnLayerClearedEvent;
    public Action OnGameOver;

    public int LayersCleared { get; private set; }
    public int BlocksCleared { get; private set; }
    public int BlocksPlayed { get; private set; }
    public float Score { get; private set; }

    [SerializeField]
    private float timeBetweenDrop = 1.5f;
    [SerializeField]
    private float dropTimeDecreasePerLayerCleared = 0.25f;
    [SerializeField]
    private float dropTimeMin = 0.2f;
    [SerializeField]
    private float pointsForClearingBlock = 5;

    private Level currentLevel;
    private BlockGroup currentBlockGroup;
    private float dropTimer;

    private void Awake() {
        instance = this;
        currentLevel = GetComponent<Level>();
        currentLevel.Init();
        CreateNewBlockGroup();
        dropTimer = timeBetweenDrop;

        currentLevel.OnLayerCleared += OnLayerCleared;
        currentLevel.OnGroupLocked += OnGroupLocked;
    }

    private void Update() {
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
        if (Input.GetKeyDown(KeyCode.E))
            TryRotate(IntVector3.right);
        if (Input.GetKeyDown(KeyCode.X))
            TryRotate(IntVector3.up);
        if (Input.GetKeyDown(KeyCode.F))
            TryRotate(IntVector3.forward);

        if (dropTimer < 0)
            DropBlockGroup();
        else
            dropTimer -= Time.deltaTime;
    }

    private void OnDestroy() {
        currentLevel.OnLayerCleared -= OnLayerCleared;
        currentLevel.OnGroupLocked -= OnGroupLocked;
    }

    private void DropBlockGroup() {
        if(currentLevel.CanMove(currentBlockGroup, IntVector3.down))
            currentLevel.MoveBlockGroup(currentBlockGroup, IntVector3.down);
        else
            CreateNewBlockGroup();
        dropTimer = timeBetweenDrop;
    }

    private bool CanCreateNewBlockGroup() {
        return !currentLevel.Grid.IsOccupied(currentLevel.GetSpawnCoordinate());
    }

    private void CreateNewBlockGroup() {
        if (currentBlockGroup != null)
            currentLevel.LockGroup(currentBlockGroup);
        currentBlockGroup = currentLevel.CreateNewBlockGroup();
    }

    private void TryMoveBlockGroup(IntVector3 input) {
        if (currentLevel.CanMove(currentBlockGroup, input))
            currentLevel.MoveBlockGroup(currentBlockGroup, input);
    }

    private void TryRotate(IntVector3 rotation) {
        if (currentLevel.CanRotate(currentBlockGroup, rotation))
            currentLevel.RotateBlockGroup(currentBlockGroup, rotation);
    }

    private void OnLayerCleared() {
        float blocksInLayer = currentLevel.Size.x * currentLevel.Size.z;
        Score += blocksInLayer * pointsForClearingBlock;
        LayersCleared++;
        if (timeBetweenDrop > dropTimeMin)
            timeBetweenDrop -= dropTimeDecreasePerLayerCleared;
        OnLayerClearedEvent();
        AudioManager.PlayAudioClip(AudioManager.Instance.layerClearedFx);
    }

    private void OnGroupLocked() {
        BlocksPlayed += currentBlockGroup.Blocks.Length;

        if (OnGroupLockedEvent != null)
            OnGroupLockedEvent();

        if (currentLevel.HighestLayer == currentLevel.Size.y - 1) {
            GameOver();
            AudioManager.PlayAudioClip(AudioManager.Instance.gameOverFx);

        } else {
            AudioManager.PlayAudioClip(AudioManager.Instance.groupLockFx);
        }
    }

    private void GameOver() {
        gameObject.SetActive(false);
        AudioManager.PlayAudioClip(AudioManager.Instance.gameOverFx);
        if (OnGameOver != null)
            OnGameOver();
    }
}
