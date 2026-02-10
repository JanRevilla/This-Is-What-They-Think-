using UnityEngine;
public class Grid
{
    private int width;
    private int height;
    private int depth;
    private float cellSize;
    private int[,,] gridArray;
    private Vector3[,,] gridCenterWorldPositions;
    private bool drawAbove;
    private Transform parent;

    public Grid(int width, int height, int depth, float cellSize, bool drawAbove, Transform parent)
    {
        this.width = width;
        this.height = height;
        this.depth = depth;
        this.cellSize = cellSize;
        this.drawAbove = drawAbove;
        this.parent = parent;

        gridArray = new int[width, height, depth];
        gridCenterWorldPositions = new Vector3[width, height, depth];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                for (int z = 0; z < gridArray.GetLength(2); z++)
                {
                    gridCenterWorldPositions[x, y, z] = GetWorldPosition(x, y, z) + (drawAbove ? Vector3.one : (Vector3.right + Vector3.forward)) * cellSize / 2;
                    CreateWorldText(gridArray[x, y, z].ToString(), parent, gridCenterWorldPositions[x, y, z], new Vector3(90.0f, 0.0f, 0.0f), (int)(10 * cellSize), Color.red, TextAnchor.MiddleCenter);
                }
            }
        }

        for (int y = 0; y < gridArray.GetLength(1) + (drawAbove ? 1 : 0); y++)
        {
            for (int x = 0; x < gridArray.GetLength(0) + 1; x++)
            {
                DrawDebugLine(GetWorldPosition(x, y, 0), GetWorldPosition(x, y, depth)); 
            }
            for (int z = 0; z < gridArray.GetLength(2) + 1; z++)
            {
                DrawDebugLine(GetWorldPosition(0, y, z), GetWorldPosition(width, y, z)); 
            }
        }

        for (int x = 0; x < gridArray.GetLength(0) + 1; x++)
        {
            for (int z = 0; z < gridArray.GetLength(2) + 1; z++)
            {
                DrawDebugLine(GetWorldPosition(x, 0, z), GetWorldPosition(x, height - (drawAbove ? 0 : 1), z)); 
            }
        }
    }

    private Vector3 GetWorldPosition(int x, int y, int z)
    {
        return (new Vector3(x, y, z)) * cellSize + parent.position;
    }
    private Vector3 GetGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - parent.position).x / cellSize);
        int y = Mathf.FloorToInt((worldPosition - parent.position).y / cellSize);
        int z = Mathf.FloorToInt((worldPosition - parent.position).z / cellSize);

        return new Vector3(x, y, z);
    }

    private void DrawDebugLine(Vector3 start, Vector3 end)
    {
        Debug.DrawLine(start, end, Color.white, 1000.0f);
    }

    private TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), Vector3 localRotation = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(text, parent, localPosition, localRotation, fontSize, (Color)color, textAnchor, textAlignment);
    }

    private TextMesh CreateWorldText(string text, Transform parent, Vector3 position, Vector3 localRotation, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment)
    {
        GameObject gameObject = new GameObject("WorldText", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.position = position;
        transform.localRotation = Quaternion.Euler(localRotation);
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = 5000;
        return textMesh;
    }
}
