using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridXPos;
    public int gridYPos;

    public int gCost;
    public int hCost;

    public Node parent;

    public int fCost => gCost + hCost;

    public Node(bool _walkable, Vector3 _worldPosition, int gridXPos, int gridYPos)
    {
        walkable = _walkable;
        worldPosition = _worldPosition;
        this.gridXPos = gridXPos;
        this.gridYPos = gridYPos;
    }
}
