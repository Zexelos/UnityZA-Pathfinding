using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] Transform pather;
    [SerializeField] Transform target;

    GridScript gridScript;

    void Start()
    {
        gridScript = GetComponent<GridScript>();
    }

    void Update()
    {
        FindPath(pather.position, target.position);
        foreach (Node node in gridScript.path)
            Debug.Log($"x:{node.gridXPos}, y:{node.gridYPos}");
    }

    public void FindPath(Vector3 startPos, Vector3 endPos)
    {
        Node startNode = gridScript.NodeFromWorldPoint(startPos);
        Node endNode = gridScript.NodeFromWorldPoint(endPos);

        List<Node> openSet = new List<Node>();
        List<Node> closedSet = new List<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost)
                {
                    if (openSet[i].hCost < currentNode.hCost)
                        currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                RetracePath(startNode, endNode);
                return;
            }

            foreach (Node neighbour in gridScript.NodeNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                    continue;

                int movementToNeighbourCost = currentNode.gCost + GetDistance(currentNode, neighbour);

                if (movementToNeighbourCost < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = movementToNeighbourCost;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        gridScript.path = path;
    }

    int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.gridXPos - b.gridXPos);
        int dstY = Mathf.Abs(a.gridYPos - b.gridYPos);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
