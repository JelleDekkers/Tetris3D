using UnityEngine;

[ExecuteInEditMode]
public class LevelDrawer : MonoBehaviour {

    private static LevelDrawer instance;
    public static LevelDrawer Instance {
        get {
            if (instance == null)
                instance = FindObjectOfType<LevelDrawer>();
            return instance;
        }
    }

    [SerializeField]
    private Material lineMat;

    private IntVector3 Size { get { return Level.Instance.Size; } }
    private Vector3 StartPos { get { return Level.Instance.StartPos; } }
    private Vector3 horizontalOppositeCorner;
    private Vector3 verticalOppositeCorner;

    private void Awake() {
        instance = this;
        if (GetComponent<Camera>() == null)
            Debug.LogError("No Camera found to render on! Use this script on a gameObject with a Camera Component.");
    }

    private void Start() {
        horizontalOppositeCorner = new Vector3(StartPos.x, StartPos.y, StartPos.z + Size.z * Level.Instance.cubeSize.z);
        verticalOppositeCorner = new Vector3(StartPos.x + Size.x * Level.Instance.cubeSize.x, StartPos.y, StartPos.z);
    }

    void DrawLevel() {
        DrawFloor();
        DrawWallVertical(StartPos);
        DrawWallVertical(verticalOppositeCorner); 
        DrawWallHorizontal(StartPos);
        DrawWallHorizontal(horizontalOppositeCorner);
    }

    private void DrawFloor() {
        // horizontal:
        Vector3 start;
        Vector3 end;
        for (int x = 0; x < Size.x + 1; x++) {
            start = new Vector3(x * Level.Instance.cubeSize.x, 0, 0) + StartPos;
            end = new Vector3(x * Level.Instance.cubeSize.x, 0, Size.z * Level.Instance.cubeSize.z) + StartPos;
            GL.PushMatrix();
            lineMat.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex3(start.x, start.y, start.z);
            GL.Vertex3(end.x, end.y, end.z);
            GL.End();
            GL.PopMatrix();
        }

        // vertical:
        for (int z = 0; z < Size.z + 1; z++) {
            start = new Vector3(0, 0, z * Level.Instance.cubeSize.z) + StartPos;
            end = new Vector3(Size.x * Level.Instance.cubeSize.x, 0, z * Level.Instance.cubeSize.z) + StartPos;
            GL.PushMatrix();
            lineMat.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex3(start.x, start.y, start.z);
            GL.Vertex3(end.x, end.y, end.z);
            GL.End();
            GL.PopMatrix();
        }
    }

    private void DrawWallVertical(Vector3 startCorner) {
        Vector3 start;
        Vector3 end;

        // horizontal:
        for (int y = 1; y < Size.y + 1; y++) {
            start = new Vector3(0, y * Level.Instance.cubeSize.y, 0) + startCorner;
            end = new Vector3(0, y * Level.Instance.cubeSize.y, Size.z * Level.Instance.cubeSize.z) + startCorner;
            GL.PushMatrix();
            lineMat.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex3(start.x, start.y, start.z);
            GL.Vertex3(end.x, end.y, end.z);
            GL.End();
            GL.PopMatrix();
        }

        // vertical:
        for (int z = 0; z < Size.z; z++) {
            start = new Vector3(0, 0, z * Level.Instance.cubeSize.z) + startCorner;
            end = new Vector3(0, Size.y * Level.Instance.cubeSize.y, z * Level.Instance.cubeSize.z) + startCorner;
            GL.PushMatrix();
            lineMat.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex3(start.x, start.y, start.z);
            GL.Vertex3(end.x, end.y, end.z);
            GL.End();
            GL.PopMatrix();
        }
    }

    private void DrawWallHorizontal(Vector3 startCorner) {
        Vector3 start;
        Vector3 end;

        // horizontal:
        for (int y = 1; y < Size.y + 1; y++) {
            start = new Vector3(0, y * Level.Instance.cubeSize.y, 0) + startCorner;
            end = new Vector3(Size.x * Level.Instance.cubeSize.x, y * Level.Instance.cubeSize.y, 0) + startCorner;
            GL.PushMatrix();
            lineMat.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex3(start.x, start.y, start.z);
            GL.Vertex3(end.x, end.y, end.z);
            GL.End();
            GL.PopMatrix();
        }

        // vertical:
        for (int x = 0; x < Size.x + 1; x++) {
            start = new Vector3(x * Level.Instance.cubeSize.x, 0, 0) + startCorner;
            end = new Vector3(x * Level.Instance.cubeSize.x, Size.y * Level.Instance.cubeSize.y, 0) + startCorner;
            GL.PushMatrix();
            lineMat.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex3(start.x, start.y, start.z);
            GL.Vertex3(end.x, end.y, end.z);
            GL.End();
            GL.PopMatrix();
        }
    }

    void OnPostRender() {
        DrawLevel();
    }

    private void OnDrawGizmos() {
        DrawLevel();
    }
}