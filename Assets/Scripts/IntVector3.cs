using System;

[Serializable]
public struct IntVector3 {

    public int x, y, z;
    public const int DIMENSIONS = 3;

    public static IntVector3 Zero = new IntVector3(0, 0, 0);

    public IntVector3(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override string ToString() {
        return "(" + x.ToString() + ", " + y.ToString() + ", " + z.ToString() + ")";
    }

    public static IntVector3 operator +(IntVector3 one, IntVector3 two) {
        one.x += two.x;
        one.y += two.y;
        one.z += two.z;
        return one;
    }

    public static IntVector3 operator -(IntVector3 one, IntVector3 two) {
        one.x -= two.x;
        one.y -= two.y;
        one.z -= two.z;
        return one;
    }

    public static bool operator ==(IntVector3 one, IntVector3 two) {
        return (one.x == two.x && one.y == two.y && one.z == two.z);
    }

    public static bool operator !=(IntVector3 one, IntVector3 two) {
        return (one.x != two.x || one.y != two.y || one.z != two.z);
    }

    public static explicit operator UnityEngine.Vector3(IntVector3 v) {
        return new UnityEngine.Vector3(v.x, v.y, v.z);
    }
}
