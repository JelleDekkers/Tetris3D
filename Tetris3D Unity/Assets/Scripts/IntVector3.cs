using System;

[Serializable]
public struct IntVector3 {

    public int x, y, z;
    public const int DIMENSIONS = 3;
    /// <summary>
    /// Shorthand for writing IntVector3(0, 0, 0)
    /// </summary>
    public static readonly IntVector3 zero = new IntVector3(0, 0, 0);
    /// <summary>
    /// Shorthand for writing IntVector3(1, 0, 0)
    /// </summary>
    public static readonly IntVector3 right = new IntVector3(1, 0, 0);
    /// <summary>
    /// Shorthand for writing IntVector3(-1, 0, 0)
    /// </summary>
    public static readonly IntVector3 left = new IntVector3(-1, 0, 0);
    /// <summary>
    /// Shorthand for writing IntVector3(0, 0, 1)
    /// </summary>
    public static readonly IntVector3 forward = new IntVector3(0, 0, 1);
    /// <summary>
    /// Shorthand for writing IntVector3(0, 0, -1)
    /// </summary>
    public static readonly IntVector3 back = new IntVector3(0, 0, -1);
    /// <summary>
    /// Shorthand for writing IntVector3(0, 1, 0)
    /// </summary>
    public static readonly IntVector3 up = new IntVector3(0, 1, 0);
    /// <summary>
    /// Shorthand for writing IntVector3(0, -1, 0)
    /// </summary>
    public static readonly IntVector3 down = new IntVector3(0, -1, 0);

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

    public override bool Equals(object obj) {
        return base.Equals(obj);
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }
}
