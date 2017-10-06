using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    private float pointsForClearingRow = 5;

    [SerializeField]
    private IntVector3 size;
    public IntVector3 Size { get { return size; } }
    public Vector3 StartPos { get; private set; }
    public int currentHeight = 0;

    public int CubeSizeX = 1;
    public int CubeSizeY = 1;
    public int CubeSizeZ = 1;

    public Block[,,] Grid { get; private set; }

    public void Init() {
        instance = this;
        Grid = new Block[Size.x, Size.y, Size.z];
        StartPos = transform.position;
        LevelDrawer.Instance.Setup(Size, StartPos);
    }

    public void MoveBlockGroup(BlockGroup group, IntVector3 input) {
        for (int i = 0; i < group.Blocks.Length; i++) {
            Block block = group.Blocks[i];
            IntVector3 newCoordinate = block.Coordinate + input;
            block.Coordinate = newCoordinate;
            Vector3 worldPos = new Vector3(newCoordinate.x + CubeSizeX / 2f, newCoordinate.y + CubeSizeY / 2f, newCoordinate.z + CubeSizeZ / 2f);
            block.gObj.transform.position = StartPos + worldPos;
            block.gObj.name = newCoordinate.ToString();
        }
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
            Vector3 worldPos = new Vector3(newCoordinate.x + CubeSizeX / 2f, newCoordinate.y + CubeSizeY / 2f, newCoordinate.z + CubeSizeZ / 2f);
            block.gObj.transform.position = worldPos;
        }
    }

    public bool RowIsFull(int row) {
        for (int x = 0; x < Grid.GetLength(0); x++) {
            for (int z = 0; z < Grid.GetLength(2); z++) {
                if (Grid[x, row, z] == null)
                    return false;
            }
        }
        return true;
    }

    public void ClearRow(int row) {
        for(int x = 0; x < Grid.GetLength(0); x++) {
            for(int z = 0; z < Grid.GetLength(2); z++) {
                Destroy(Grid[x, row, z].gObj);
                Grid[x, row, z] = null;
            }
        }
        MoveAllLayersDown();
        GetComponent<GameManager>().score += pointsForClearingRow;
    }

    private void MoveAllLayersDown() {
        for(int y = 0; y < currentHeight + 1; y++) {
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
        currentHeight--;
    }

    public BlockGroup CreateNewBlockGroup() {
        Block[] blocks = BlockGroupTypes.Type_L.Copy(); 

        for (int i = 0; i < blocks.Length; i++) {
            IntVector3 coordinate = GetSpawnCoordinate() + blocks[i].Coordinate;
            Vector3 worldCoordinate = new Vector3(coordinate.x + CubeSizeX / 2f, coordinate.y + CubeSizeY / 2f, coordinate.z + CubeSizeZ / 2f);
            Block block = new Block(coordinate);
            block.SetGameObject(GameObject.CreatePrimitive(PrimitiveType.Cube));
            block.gObj.name = coordinate.ToString();
            block.Coordinate = coordinate;
            block.gObj.transform.localPosition = worldCoordinate;
            blocks[i] = block;
        }

        BlockGroup group = new BlockGroup(blocks);
        return group;
    }

    public void StoreGroupInGrid(BlockGroup group) {
        int rows = 0;
        foreach (Block block in group.Blocks) {
            Grid[block.Coordinate.x, block.Coordinate.y, block.Coordinate.z] = block;
            if (rows < block.Coordinate.y)
                rows = block.Coordinate.y;
        }

        for(int i = 0; i < rows + 1; i++) {
            if (RowIsFull(i))
                ClearRow(i);
        }

        currentHeight = rows + 1;
    }

    private IntVector3 GetSpawnCoordinate() {
        IntVector3 coordinate = new IntVector3((int)((Size.x - 1) / 2), Size.y - 1, (int)((Size.z - 1) / 2));
        return coordinate;
    }
}
