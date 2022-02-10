using UnityEngine;

/// <summary>
/// This script holds the data of each block in the maze
/// </summary>
public class MazeBlock : MonoBehaviour
{
    public bool hasBeenVisited;
    public GameObject northWall, southWall, eastWall, westWall;
}
