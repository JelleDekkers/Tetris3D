using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extenions {

    public static UnityEngine.Vector3 ToVector3(this IntVector3 v) {
        return new UnityEngine.Vector3(v.x, v.y, v.z);
    }

    public static IntVector3 ToIntVector3(this Vector3 v) {
        return new IntVector3((int)v.x, (int)v.y, (int)v.z);
    }

    public static bool CoordinateExistsInGrid<T>(this T[,,] grid, IntVector3 coordinate) {
        if (coordinate.x < 0 || coordinate.x >= grid.GetLength(0))
            return false;
        if (coordinate.y < 0 || coordinate.y >= grid.GetLength(1))
            return false;
        if (coordinate.z < 0 || coordinate.z >= grid.GetLength(2))
            return false;

        return true;
    }

    public static bool IsOccupied<T>(this T[,,] grid, IntVector3 coordinate) {
        return grid[coordinate.x, coordinate.y, coordinate.z] != null;
    }

    public static Block[] Copy(this Block[] blocks) {
        Block[] b = new Block[blocks.Length];
        for (int i = 0; i < blocks.Length; i++)
            b[i] = blocks[i];
        return b;
    }
}
