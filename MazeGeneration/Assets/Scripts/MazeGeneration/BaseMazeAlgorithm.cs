using UnityEngine;

public abstract class BaseMazeAlgorithm
{
    //The 2d array of maze blocks
    protected MazeBlock[,] mazeBlocks;
    //The total maze rows and columns
    protected int mazeRows, mazeColumns;

    public BaseMazeAlgorithm(MazeBlock[,] mazeBlocks) : base()
    {
        //Set the mazeblocks to be that of the constructor
        this.mazeBlocks = mazeBlocks;
        //Set the rows to be that of the first dimension of the maze blocks
        mazeRows = mazeBlocks.GetLength(0);
        //Set the columsn to be that of the second dimension of the maze blocks
        mazeColumns = mazeBlocks.GetLength(1);
    }

    public abstract void CreateMaze();
}
