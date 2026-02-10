using UnityEngine;

public class GridTesting : MonoBehaviour
{
    public int x = 10;
    public int y = 1;
    public int z = 10;
    public float cellSize = 10;
    public bool drawAbove = false;
    public Transform parent;
    public void Start()
    {
        Grid grid = new Grid(x, y, z, cellSize, drawAbove, parent);
    }
}