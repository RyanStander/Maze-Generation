using UnityEngine;

/// <summary>
/// This script will manage the generation of the maze. This algorithm is based on Randomized Prim's algorithm found from wikipedia.
/// </summary>
public class MazeGenerator : MonoBehaviour
{
    [Tooltip("The amount of blocks that will be created in respective direction")]
    [Range(1,100)]public int mazeRows, mazeColumns;
    [Tooltip("The prefab for the walls")]
    public GameObject wallPrefab;
    [Tooltip("The prefab for the floor")]
    public GameObject floorPrefab;
    [Tooltip("What the size of the maze should be, this is related to the scale of a cube (scale of 6 would be a size of 6)")]
    public float size=2f;

    //2d array of maze blocks
    private MazeBlock[,] mazeBlocks;

    PrimsMazeAlgorithm primsMazeAlgorithm;

    private void Start()
    {
        InitializeMaze();

        primsMazeAlgorithm = new PrimsMazeAlgorithm(mazeBlocks);

        primsMazeAlgorithm.CreateMaze();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed");
            primsMazeAlgorithm.DoItAgain();
        }   
    }

    /// <summary>
    /// Handles the creation of the initial maze, this will just be a grid of maze blocks with no pathways
    /// </summary>
    private void InitializeMaze()
    {
        mazeBlocks = new MazeBlock[mazeRows, mazeColumns];

        //Create a new row
        for (int r = 0; r < mazeRows; r++)
        {
            //Create a new column
            for (int c = 0; c < mazeColumns; c++)
            {
                //Create floor piece
                GameObject mazeBlock = Instantiate(floorPrefab, new Vector3(r * size, -(size / 2f), c * size), Quaternion.identity);
                //Initialize a new maze block in the 2d array
                mazeBlocks[r, c] = mazeBlock.AddComponent<MazeBlock>();
                //Set the name of floor piece to what its row and column values are
                mazeBlock.name = "Floor " + r + "," + c;

                //If first column of the row
                if (c==0)
                {
                    //Create west wall
                    mazeBlocks[r, c].westWall = Instantiate(wallPrefab, new Vector3(r * size, 0, (c * size) - (size / 2f)), Quaternion.identity);
                    //Set the name of wall piece to what its row and column values are
                    mazeBlocks[r, c].westWall.name = "West wall " + r + "," + c;
                }

                //Create east wall
                mazeBlocks[r, c].eastWall = Instantiate(wallPrefab, new Vector3(r * size, 0, (c * size) + (size / 2f)), Quaternion.identity);
                //Set the name of wall piece to what its row and column values are
                mazeBlocks[r, c].eastWall.name = "East wall " + r + "," + c;

                //If first row
                if (r==0)
                {
                    //Create north wall
                    mazeBlocks[r, c].northWall = Instantiate(wallPrefab, new Vector3((r*size)-(size/2f),0,c*size), Quaternion.identity);
                    //Set the name of wall piece to what its row and column values are
                    mazeBlocks[r, c].northWall.name = "North wall " + r + "," + c;
                    //Rotate the wall
                    mazeBlocks[r, c].northWall.transform.Rotate(Vector3.up * 90);
                }

                //Create south wall
                mazeBlocks[r, c].southWall = Instantiate(wallPrefab, new Vector3((r * size) + (size / 2f), 0, c * size ), Quaternion.identity);
                //Set the name of wall piece to what its row and column values are
                mazeBlocks[r, c].southWall.name = "South wall " + r + "," + c;
                //Rotate the wall
                mazeBlocks[r, c].southWall.transform.Rotate(Vector3.up * 90);
            }
        }
    }
}
