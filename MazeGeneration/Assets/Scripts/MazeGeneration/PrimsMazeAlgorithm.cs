using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimsMazeAlgorithm : BaseMazeAlgorithm
{ 
    //The current row and column that is being checked
    private int currentRow=0, currentColumn = 0;
    private List<MazeBlock> unvisitedAdjacentMazeBlocks = new List<MazeBlock>();
    public PrimsMazeAlgorithm(MazeBlock[,] mazeBlocks) : base(mazeBlocks){}

    /// <summary>
    /// Creates a maze structure using Prim's algorithm
    /// </summary>
    public override void CreateMaze()
    {
        //Set the first block as visited
        mazeBlocks[currentRow, currentColumn].hasBeenVisited = true;
        //Find all adjacent blocks to the first block
        unvisitedAdjacentMazeBlocks.AddRange(FindAjacentUnvisitedBlocks(currentRow, currentColumn));
        SetupMazeStructure();
    }

    /// <summary>
    /// Finds all unvisited blocks and removes walls to connect to existing maze structure
    /// </summary>
    private void SetupMazeStructure()
    {
        //While there are still unvisited maze blocks
        while (unvisitedAdjacentMazeBlocks.Count > 0)
        {

            //Choose a random block from the list of unvisited maze blocks adjacent to the created maze
            int randomBlock = Random.Range(0, unvisitedAdjacentMazeBlocks.Count);
            //Get the coordinates of the unvisited block in relation to the entire maze structure
            Vector2 coords = ExtensionMethods.CoordinatesOf(mazeBlocks, unvisitedAdjacentMazeBlocks[randomBlock]);
            //Perform a check to ensure that the block being worked with has an adjacent block to it that has been visited
            if (HasAdjacentVisitedBlock((int)coords.x, (int)coords.y))
            {
                //Set the applicable block as the one we are currently working with
                currentRow = (int)coords.x;
                currentColumn = (int)coords.y;
                //Destroy the wall between these 2 blocks
                DestroyAdjacentWall(currentRow, currentColumn);
                //Set the block we just visited to be visited
                mazeBlocks[currentRow, currentColumn].hasBeenVisited = true;

                //Add adjacent unvisted blocks to the list of unvisitedAdjacentMazeBlocks
                foreach (MazeBlock mazeBlock in FindAjacentUnvisitedBlocks(currentRow, currentColumn))
                {
                    //Make sure the list does not already contain the maze block, we dont want duplicates in the list
                    if (!unvisitedAdjacentMazeBlocks.Contains(mazeBlock))
                    {
                        //Add it to the list if it matches the requirements
                        unvisitedAdjacentMazeBlocks.Add(mazeBlock);
                    }
                }
                //Remove this block from the unvisited list
                unvisitedAdjacentMazeBlocks.RemoveAt(randomBlock);
            }
        }
    }

    /// <summary>
    /// Checks if there are any blocks that have been visited adjacent to the checked block
    /// </summary>
    private bool HasAdjacentVisitedBlock(int row, int column)
    {
        int visitedBlocks = 0;

        //If the block north of the one being checked has been visited and current block is not the top most row
        if (row>0 && mazeBlocks[row-1,column].hasBeenVisited)
        {
            visitedBlocks++;
        }

        //If the block south of the one being checked has been visited and current block is not the bottom most row
        if (row < (mazeRows-2) && mazeBlocks[row + 1, column].hasBeenVisited)
        {
            visitedBlocks++;
        }

        //If the block west of the one being checked has been visited and current block is not the left most row
        if (column > 0 && mazeBlocks[row, column-1].hasBeenVisited)
        {
            visitedBlocks++;
        }

        //If the block east of the one being checked has been visited and current block is not the right most row
        if (column < (mazeColumns - 2) && mazeBlocks[row, column+1].hasBeenVisited)
        {
            visitedBlocks++;
        }

        return visitedBlocks > 0;
    } 

    private void DestroyWallIfItExists(GameObject wall)
    {
        //Only destroy the wall if it is not null
        if (wall != null)
        {
            Object.Destroy(wall);
        }
    }

    private void DestroyAdjacentWall(int row, int column)
    {
        bool hasDestroyedWall = false;

        //Keep checking until a wall has been destroyed
        while (!hasDestroyedWall)
        {
            int direction = Random.Range(1, 5);
            //If the direction is north and there is a cell available
            if (direction == 1 && row > 0 && mazeBlocks[row - 1, column].hasBeenVisited)
            {
                //We attempt to remove from multiple blocks as not all blocks contain walls to avoid having duplicates
                DestroyWallIfItExists(mazeBlocks[row, column].northWall);
                DestroyWallIfItExists(mazeBlocks[row - 1, column].southWall);
                hasDestroyedWall = true;
            }
            //If the direction is south and there is a cell available
            else if (direction == 2 && row < (mazeRows - 2) && mazeBlocks[row + 1, column].hasBeenVisited)
            {
                //We attempt to remove from multiple blocks as not all blocks contain walls to avoid having duplicates
                DestroyWallIfItExists(mazeBlocks[row, column].southWall);
                DestroyWallIfItExists(mazeBlocks[row + 1, column].northWall);
                hasDestroyedWall = true;
            }
            //If the direction is east and there is a cell available
            else if (direction == 3 && column > 0 && mazeBlocks[row, column - 1].hasBeenVisited)
            {
                //We attempt to remove from multiple blocks as not all blocks contain walls to avoid having duplicates
                DestroyWallIfItExists(mazeBlocks[row, column].westWall);
                DestroyWallIfItExists(mazeBlocks[row, column - 1].eastWall);
                hasDestroyedWall = true;
            }
            //If the direction is west and there is a cell available
            else if (direction == 4 && column < (mazeColumns - 2) && mazeBlocks[row, column + 1].hasBeenVisited)
            {
                //We attempt to remove from multiple blocks as not all blocks contain walls to avoid having duplicates
                DestroyWallIfItExists(mazeBlocks[row, column].eastWall);
                DestroyWallIfItExists(mazeBlocks[row, column + 1].westWall);
                hasDestroyedWall = true;
            }
        }
    }

    private List<MazeBlock> FindAjacentUnvisitedBlocks(int row, int column)
    {
        //Create new list of maze blocks for keeping adjacent wall blocks
        List<MazeBlock> adjacentWallBlocks = new List<MazeBlock>();

        //Get north wall | if we're at row 0, we don't want to get the wall because it would be an outer wall
        if (row != 0)
        {
            if (!mazeBlocks[row - 1, column].hasBeenVisited)
                adjacentWallBlocks.Add(mazeBlocks[row - 1, column]);
        }
        //Get east wall | if we're at the last column, we don't want to get the wall because it would be an outer wall
        if (column!=mazeColumns-1)
        {
            if (!mazeBlocks[row, column + 1].hasBeenVisited)
                adjacentWallBlocks.Add(mazeBlocks[row, column + 1]);
        }
        //Get south wall | if we're at the last row, we don't want to get the wall because it would be an outer wall
        if (row!=mazeRows-1)
        {
            if (!mazeBlocks[row + 1, column].hasBeenVisited)
                adjacentWallBlocks.Add(mazeBlocks[row + 1, column]);
        }
        //Get west wall | if we're at column 0, we don't want to get the wall because it would be an outer wall
        if (column!=0)
        {
            if (!mazeBlocks[row, column - 1].hasBeenVisited)
                adjacentWallBlocks.Add(mazeBlocks[row, column - 1]);
        }

        return adjacentWallBlocks;

    }
}
