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

    [SerializeField][Range(4, 100)]
    private IntVector3 size;

    public IntVector3 Size { get { return size; } }
    public Vector3 StartPos { get; private set; } 

    public int CubeSizeX = 1;
    public int CubeSizeY = 1;
    public int CubeSizeZ = 1;

    public Block[,,] Grid { get; private set; }

    public void Init() {
        instance = this;
        Grid = new Block[size.x, size.y, size.z];
        StartPos = transform.position;
        LevelDrawer.Instance.Setup(size, StartPos);
    }

    public void MoveBlockGroup(BlockGroup group, Vector3 input) {
        for (int i = 0; i < group.Blocks.Length; i++) {
            IntVector3 newCoordinate = group.Blocks[i].Coordinate + input.ToIntVector3();
            group.Blocks[i].Coordinate = newCoordinate;
            Vector3 worldPos = new Vector3(newCoordinate.x + CubeSizeX / 2f, newCoordinate.y + CubeSizeY / 2f, newCoordinate.z + CubeSizeZ / 2f);
            group.Blocks[i].gObj.transform.position = StartPos + worldPos;
        }
    }

    private void ClearRow(int gridIndex) {

    }

    public BlockGroup CreateNewBlockGroup() {
        Block[] blocks = BlockGroupTypes.GetRandom();

        for (int i = 0; i < blocks.Length; i++) {
            IntVector3 coordinate = GetSpawnCoordinate() + blocks[i].Coordinate;
            Vector3 worldCoordinate = new Vector3(coordinate.x + CubeSizeX / 2f, coordinate.y + CubeSizeY / 2f, coordinate.z + CubeSizeZ / 2f);
            Block block = new Block(coordinate);
            block.SetGameObject(GameObject.CreatePrimitive(PrimitiveType.Cube));
            block.Coordinate = coordinate;
            block.gObj.transform.localPosition = worldCoordinate;
            blocks[i] = block;
            Grid[coordinate.x, coordinate.y, coordinate.z] = block;
        }

        BlockGroup group = new BlockGroup(blocks);
        return group;
    }

    private IntVector3 GetSpawnCoordinate() {
        IntVector3 coordinate = new IntVector3((int)((size.x - 1) / 2), size.y - 1, (int)((size.z - 1) / 2));
        return coordinate;
    }
}
