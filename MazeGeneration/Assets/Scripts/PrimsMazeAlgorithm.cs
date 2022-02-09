using System.Collections.Generic;
using UnityEngine;

public class PrimsMazeAlgorithm : BaseMazeAlgorithm
{ 
    //The current row and column that is being checked
    private int currentRow=0, currentColumn = 0;

    private bool hasCompletedCourse = false;

    public PrimsMazeAlgorithm(MazeBlock[,] mazeBlocks) : base(mazeBlocks)
    {
    }
    private List<MazeBlock> visitedMazeBlocks = new List<MazeBlock>();
    private List<MazeBlock> unvisitedMazeBlocks = new List<MazeBlock>();
    private List<MazeBlock> unvisitedAdjacentMazeBlocks = new List<MazeBlock>();


    /// <summary>
    /// Creates a maze structure using Prim's algorithm
    /// </summary>
    public override void CreateMaze()
    {
        //Set the first block as visited
        mazeBlocks[currentRow, currentColumn].hasBeenVisited = true;
        //we add all to this list since none are visited
        for (int r = 0; r < mazeRows; r++)
        {
            for (int c = 0; c < mazeColumns; c++)
            {
                unvisitedMazeBlocks.Add(mazeBlocks[r, c]);
            }
        }
        unvisitedAdjacentMazeBlocks.AddRange(FindAjacentUnvisitedBlocks(currentRow,currentColumn));

        unvisitedMazeBlocks.Remove(mazeBlocks[currentRow, currentColumn]);
        visitedMazeBlocks.Add(mazeBlocks[currentRow, currentColumn]);

        //While there are still maze blocks that have not been visited
        //while (!hasCompletedCourse)
        //{
            CreateMazePathway();
            StartNewPathwayLocation();
        //}
    }

    public void DoItAgain()
    {
        //if (!hasCompletedCourse)
        //{
            //CreateMazePathway();
            StartNewPathwayLocation();
        //}
    }

    private void CreateMazePathway()
    {
        //while (StillRouteAvailable(currentRow,currentColumn))
        //{
        int direction = Random.Range(1, 5);

        //If the direction is north and there is a cell available
        if (direction == 1 && CellIsAvailable(currentRow - 1, currentColumn))
        {
            //We attempt to remove from multiple blocks as not all blocks contain walls to avoid having duplicates
            DestroyWallIfItExists(mazeBlocks[currentRow, currentColumn].northWall);
            DestroyWallIfItExists(mazeBlocks[currentRow - 1, currentColumn].southWall);
            currentRow--;
        }
        //If the direction is south and there is a cell available
        else if (direction == 2 && CellIsAvailable(currentRow + 1, currentColumn))
        {
            //We attempt to remove from multiple blocks as not all blocks contain walls to avoid having duplicates
            DestroyWallIfItExists(mazeBlocks[currentRow, currentColumn].southWall);
            DestroyWallIfItExists(mazeBlocks[currentRow + 1, currentColumn].northWall);
            currentRow++;
        }
        //If the direction is east and there is a cell available
        else if (direction == 3 && CellIsAvailable(currentRow, currentColumn + 1))
        {
            //We attempt to remove from multiple blocks as not all blocks contain walls to avoid having duplicates
            DestroyWallIfItExists(mazeBlocks[currentRow, currentColumn].eastWall);
            DestroyWallIfItExists(mazeBlocks[currentRow, currentColumn + 1].westWall);
            currentColumn++;
        }
        //If the direction is west and there is a cell available
        else if (direction == 4 && CellIsAvailable(currentRow, currentColumn - 1))
        {
            //We attempt to remove from multiple blocks as not all blocks contain walls to avoid having duplicates
            DestroyWallIfItExists(mazeBlocks[currentRow, currentColumn].westWall);
            DestroyWallIfItExists(mazeBlocks[currentRow, currentColumn - 1].eastWall);
            currentColumn--;
        }

        mazeBlocks[currentRow, currentColumn].hasBeenVisited = true;
        visitedMazeBlocks.Add(mazeBlocks[currentRow, currentColumn]);
        unvisitedMazeBlocks.Remove(mazeBlocks[currentRow, currentColumn]);
        Debug.Log("Removed object");
        //}
    }

    /// <summary>
    /// Once there is no unvisited paths to persue, start looking for unvisited cells that fit requirements
    /// </summary>
    private void StartNewPathwayLocation()
    {
        //Seeing that this was called due to lack of path, assume that all pathways are found unless proven otherwise by the below algorithm
        hasCompletedCourse = true;

        //While there are still unvisited maze blocks
        while (unvisitedAdjacentMazeBlocks.Count>0)
        {
            int randomBlock = Random.Range(0, unvisitedAdjacentMazeBlocks.Count);
            Vector2 coords = ExtensionMethods.CoordinatesOf(mazeBlocks, unvisitedAdjacentMazeBlocks[randomBlock]);
            if (HasAdjacentVisitedBlock((int)coords.x, (int)coords.y))
            {
                //Since a block has been found that is suitable, set the course to not be complted
                hasCompletedCourse = false;
                //Set the applicable block as the one we are currently working with
                currentRow = (int)coords.x;
                currentColumn = (int)coords.y;
                //Destroy the wall between these 2 blocks
                DestroyAdjacentWall(currentRow, currentColumn);
                //Set the block we just visited to be visited
                mazeBlocks[currentRow, currentColumn].hasBeenVisited = true;

                foreach (MazeBlock mazeBlock in FindAjacentUnvisitedBlocks(currentRow,currentColumn))
                {
                    if (!unvisitedAdjacentMazeBlocks.Contains(mazeBlock))
                    {
                        unvisitedAdjacentMazeBlocks.Add(mazeBlock);
                    }
                }

                visitedMazeBlocks.Add(unvisitedAdjacentMazeBlocks[randomBlock]);
                unvisitedMazeBlocks.RemoveAt(randomBlock);
                unvisitedAdjacentMazeBlocks.RemoveAt(randomBlock);
            } 
        }
    }

    private bool StillRouteAvailable(int row, int column)
    {
        int availableRoutes = 0;

        //if the row count is larger than 0 and the row before has been visited, increase available routes
        if (row>0&&!mazeBlocks[row-1,column].hasBeenVisited)
            availableRoutes++;

        //if the current row count is larger than the total amount of rows and the row after has been visited, increase available routes
        if (row < mazeRows - 1 && !mazeBlocks[row + 1, column].hasBeenVisited)
            availableRoutes++;

        //if the column count is larger than 0 and the column before has been visited, increase available routes
        if (column > 0 && !mazeBlocks[row, column - 1].hasBeenVisited)
            availableRoutes++;

        //if the current column count is larger than the total amount of column and the column after has been visited, increase available routes
        if (column < mazeColumns - 1 && !mazeBlocks[row, column + 1].hasBeenVisited)
            availableRoutes++;

        //return whether there is more than 0 available routes
        return availableRoutes > 0;
    }

    /// <summary>
    /// Performs a check to ensure the cell being moved to is available
    /// </summary>
    private bool CellIsAvailable(int row, int column)
    {
        //make a check to ensure that the cell that is being moved to is available
        if (row >= 0 && row < mazeRows && column >= 0 && column < mazeColumns && !mazeBlocks[row, column].hasBeenVisited)
            return true;
        else
            return false;
    }

    private bool HasAdjacentVisitedBlock(int row, int column)
    {
        int visitedBlocks = 0;

        if (row>0 && mazeBlocks[row-1,column].hasBeenVisited)
        {
            visitedBlocks++;
        }

        if (row < (mazeRows-2) && mazeBlocks[row + 1, column].hasBeenVisited)
        {
            visitedBlocks++;
        }

        if (column > 0 && mazeBlocks[row, column-1].hasBeenVisited)
        {
            visitedBlocks++;
        }

        if (column < (mazeColumns - 2) && mazeBlocks[row, column+1].hasBeenVisited)
        {
            visitedBlocks++;
        }

        return visitedBlocks > 0;
    } 

    private void DestroyWallIfItExists(GameObject wall)
    {
        if (wall != null)
        {
            Object.Destroy(wall);
        }
    }

    private void DestroyAdjacentWall(int row, int column)
    {
        bool hasDestroyedWall = false;

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
