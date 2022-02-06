using UnityEngine;

[CreateAssetMenu(menuName = "Maze/Wall")]
public class MazeWallData : ScriptableObject
{
    [Tooltip("The name of the wall piece")]
    public string wallName;
    [Tooltip("The prefab that will be created for the wall")]
    public GameObject wallPrefab;
    [Tooltip("The width of the wall prefab")]
    public float wallWidth;
    [Tooltip("The length of the wall prefab")]
    public float wallLength;
}
