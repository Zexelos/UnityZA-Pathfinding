using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    [SerializeField] Transform pather;
    [SerializeField] Transform target;
    [SerializeField] Grid grid;
    [SerializeField] LayerMask unwalkableMask;
    [SerializeField] public int gridSizeX;
    [SerializeField] public int gridSizeY;
    [SerializeField] public float cellSize;

    public List<Node> path = new List<Node>();

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
                nodeGrid[x, y] = new Node(walkable, worldPos, x, y);
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPoint)
    {
        Vector3Int gridPos = grid.WorldToCell(worldPoint);
        return nodeGrid[gridPos.x + gridSizeX / 2, gridPos.z + gridSizeY / 2];
    }

    public List<Node> NodeNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridXPos + x;
                int checkY = node.gridYPos + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    neighbours.Add(nodeGrid[checkX, checkY]);
            }
        }

        return neighbours;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(cellSize * gridSizeX, cellSize, cellSize * gridSizeY));

        if (nodeGrid != null)
        {
            Node patherNode = NodeFromWorldPoint(pather.position);
            Node targetNode = NodeFromWorldPoint(target.position);
            foreach (Node node in nodeGrid)
            {
                Gizmos.color = node.walkable ? Color.white : Color.red;
                if (node == patherNode)
                    Gizmos.color = Color.cyan;
                else if (node == targetNode)
                    Gizmos.color = Color.black;
                else if (path != null)
                    if (path.Contains(node))
                        Gizmos.color = Color.yellow;
                Gizmos.DrawCube(node.worldPosition, new Vector3(cellSize, 0, cellSize));
            }
        }
    }
}
