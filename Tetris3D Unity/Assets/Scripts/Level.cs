using System;
using UnityEngine;

public class Level : MonoBehaviour {

    private static Level instance;
    public static Level Instance {
        get {
            if (instance == null)
                instance = FindObjectOfType<Level>();
            return instance;
        }
    }

    public Action<int> onRowCleared;
    public Action OnGroupLocked;
    public Action OnHeightChanged;
    public Action OnGroupMoved;
    public Action OnGroupMovedFailed;
    public Action OnGroupRotated;
    public Action OnGroupRotatedFailed;

    public Vector3 cubeSize = Vector3.one;
    public IntVector3 Size { get { return size; } }
    public Vector3 StartPos { get; private set; }
    public Block[,,] Grid { get; private set; }
    public int HighestRow {
        get {
            return highestRow;
        }
        private set {
            highestRow = value;
            if(OnHeightChanged != null)
                OnHeightChanged();
        }
    }

    [SerializeField]
    private GameObject blockPrefab;
    [SerializeField]
    private Material outlineMat;
    [SerializeField]
    private IntVector3 size;
    [SerializeField]
    private Color[] layerColors;

    private int highestRow;

    public void Init() {
        instance = this;
        Grid = new Block[Size.x, Size.y, Size.z];
        StartPos = transform.position;
    }

    public bool CanMove(BlockGroup group, IntVector3 direction) {
        for (int i = 0; i < group.Blocks.Length; i++) {
            IntVector3 newCoordinate = group.Blocks[i].Coordinate + direction;
            if (!Grid.CoordinateExistsInGrid(newCoordinate) || Grid.IsOccupied(newCoordinate)) {
                if (OnGroupMovedFailed != null)
                    OnGroupMovedFailed();
                return false;
            }
        }
        return true;
    }

    public void MoveBlockGroup(BlockGroup group, IntVector3 input) {
        for (int i = 0; i < group.Blocks.Length; i++) {
            Block block = group.Blocks[i];
            IntVector3 newCoordinate = block.Coordinate + input;
            block.Coordinate = newCoordinate;
            Vector3 worldPos = new Vector3(newCoordinate.x + cubeSize.x / 2f, newCoordinate.y + cubeSize.y / 2f, newCoordinate.z + cubeSize.z/ 2f);
            block.gObj.transform.position = StartPos + worldPos;
            block.gObj.name = newCoordinate.ToString();
        }

        if(OnGroupMoved != null)
            OnGroupMoved();
    }

    public bool CanRotate(BlockGroup group, IntVector3 rotation) {
        for (int i = 0; i < group.Blocks.Length; i++) {

            Block block = group.Blocks[i];
            if (block == group.RotationPivotBlock)
                continue;

            IntVector3 difFromPivot = block.Coordinate - group.RotationPivotBlock.Coordinate;
            IntVector3 rotationFromPivot = IntVector3.zero;
            if (rotation.x != 0)
                rotationFromPivot = new IntVector3(difFromPivot.z * rotation.x, difFromPivot.y, difFromPivot.x * -rotation.x);
            else if (rotation.y != 0)
                rotationFromPivot = new IntVector3(difFromPivot.y * rotation.y, difFromPivot.x * -rotation.y, difFromPivot.z);
            else if (rotation.z != 0)
                rotationFromPivot = new IntVector3(difFromPivot.x, difFromPivot.z * -rotation.z, difFromPivot.y * rotation.z);
            IntVector3 newCoordinate = group.RotationPivotBlock.Coordinate + rotationFromPivot;

            if (!Grid.CoordinateExistsInGrid(newCoordinate) || Grid.IsOccupied(newCoordinate)) {
                if (OnGroupRotatedFailed != null)
                    OnGroupRotatedFailed();
                return false;
            }
        }
        return true;
    }

    public void RotateBlockGroup(BlockGroup group, IntVector3 rotation) {
        for (int i = 0; i < group.Blocks.Length; i++) {

            Block block = group.Blocks[i];
            if (block == group.RotationPivotBlock)
                continue;

            IntVector3 difFromPivot = block.Coordinate - group.RotationPivotBlock.Coordinate;
            IntVector3 rotationFromPivot = IntVector3.zero;
            if (rotation.x != 0) 
                rotationFromPivot = new IntVector3(difFromPivot.z * rotation.x, difFromPivot.y, difFromPivot.x * -rotation.x);
            else if (rotation.y != 0)
                rotationFromPivot = new IntVector3(difFromPivot.y * rotation.y, difFromPivot.x * -rotation.y, difFromPivot.z);
            else if (rotation.z != 0)
                rotationFromPivot = new IntVector3(difFromPivot.x, difFromPivot.z * -rotation.z, difFromPivot.y * rotation.z);
            IntVector3 newCoordinate = group.RotationPivotBlock.Coordinate + rotationFromPivot;
            block.Coordinate = newCoordinate;
            Vector3 worldPos = new Vector3(newCoordinate.x + cubeSize.x / 2f, newCoordinate.y + cubeSize.y / 2f, newCoordinate.z + cubeSize.z / 2f);
            block.gObj.transform.position = worldPos;
        }

        if (OnGroupRotated != null)
            OnGroupRotated();
    }

    private bool RowIsFull(int row) {
        for (int x = 0; x < Grid.GetLength(0); x++) {
            for (int z = 0; z < Grid.GetLength(2); z++) {
                if (Grid[x, row, z] == null)
                    return false;
            }
        }
        return true;
    }

    private void ClearRow(int row) {
        for(int x = 0; x < Grid.GetLength(0); x++) {
            for(int z = 0; z < Grid.GetLength(2); z++) {
                Destroy(Grid[x, row, z].gObj);
                Grid[x, row, z] = null;
            }
        }
        MoveRowsDown(row);
        HighestRow--;
        if (onRowCleared != null)
            onRowCleared(row);
    }

    private void MoveRowsDown(int fromRowNr) {
        for(int y = fromRowNr; y < HighestRow + 1; y++) {
            for(int x = 0; x < Size.x; x++) {
                for (int z = 0; z < Size.z; z++) {
                    if(Grid[x, y, z] != null) {
                        Grid[x, y - 1, z] = Grid[x, y, z];
                        Grid[x, y - 1, z].gObj.transform.position += Vector3.down; 
                        Grid[x, y, z] = null;
                    }
                }
            }
        }
    }

    public BlockGroup CreateNewBlockGroup() {
        Block[] blocks = BlockGroupTypes.GetRandom().Copy(); 

        for (int i = 0; i < blocks.Length; i++) {
            IntVector3 coordinate = GetSpawnCoordinate() + blocks[i].Coordinate;
            Vector3 worldCoordinate = new Vector3(coordinate.x + cubeSize.x / 2f, coordinate.y + cubeSize.y / 2f, coordinate.z + cubeSize.z / 2f);
            Block block = new Block(coordinate);
            block.SetGameObject(Instantiate(blockPrefab));
            block.gObj.name = coordinate.ToString();
            block.gObj.GetComponent<MeshRenderer>().enabled = false;
            block.gObj.transform.localScale = new Vector3(cubeSize.x, cubeSize.y, cubeSize.z);
            block.Coordinate = coordinate;
            block.gObj.transform.localPosition = worldCoordinate;
            blocks[i] = block;
        }

        BlockGroup group = new BlockGroup(blocks);
        return group;
    }

    public void LockGroup(BlockGroup group) {
        int row = 0;
        foreach (Block block in group.Blocks) {
            Grid[block.Coordinate.x, block.Coordinate.y, block.Coordinate.z] = block;
            if (row < block.Coordinate.y)
                row = block.Coordinate.y;
            Destroy(block.gObj.GetComponent<BlockOutlineDrawer>());
            block.gObj.GetComponent<MeshRenderer>().enabled = true;
            block.gObj.GetComponent<MeshRenderer>().material.color = GetCorrespondingRowColor(block.Coordinate.y);
        }

        for(int i = 0; i < row + 1; i++) {
            if (RowIsFull(i)) {
                ClearRow(i);
                return;
            }
        }

        if (row + 1 > highestRow)
            HighestRow = row + 1;
        if (OnGroupLocked != null)
            OnGroupLocked();
    }

    public Color GetCorrespondingRowColor(int height) {
        return layerColors[(height + GameManager.Instance.rowsCleared) % layerColors.Length];
    }

    public IntVector3 GetSpawnCoordinate() {
        IntVector3 coordinate = new IntVector3((int)((Size.x - 1) / 2), Size.y - 1, (int)((Size.z - 1) / 2));
        return coordinate;
    }
}
