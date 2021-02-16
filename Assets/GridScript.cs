using UnityEngine;

public class GridScript : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] LayerMask unwalkableMask;
    [SerializeField] public int gridSizeX;
    [SerializeField] public int gridSizeY;
    [SerializeField] public float cellSize;

    float cellRadius;
    Node[,] nodeGrid;

    void Start()
    {
        cellRadius = cellSize / 2;
        grid.cellSize = new Vector3(cellSize, cellSize, cellSize);

        nodeGrid = new Node[gridSizeX, gridSizeY];

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPos = grid.CellToWorld(new Vector3Int((-gridSizeX / 2) + x, 0, (-gridSizeY / 2) + y)) + new Vector3(cellRadius, 0, cellRadius);
                bool walkable = !Physics.CheckSphere(worldPos, cellRadius, unwalkableMask);
                nodeGrid[x, y] = new Node(walkable, worldPos);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(cellSize * gridSizeX, cellSize, cellSize * gridSizeY));

        if (nodeGrid != null)
        {
            foreach (Node node in nodeGrid)
            {
                Gizmos.color = node.walkable ? Color.green : Color.red;
                Gizmos.DrawCube(node.worldPosition, new Vector3(cellSize, 0, cellSize));
            }
        }
    }
}
