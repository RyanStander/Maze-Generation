using UnityEngine;

public static class ExtensionMethods
{
    public static Vector2 CoordinatesOf<T>(this T[,] matrix, T value)
    {
        int w = matrix.GetLength(0); // width
        int h = matrix.GetLength(1); // height

        for (int x = 0; x < w; ++x)
        {
            for (int y = 0; y < h; ++y)
            {
                if (matrix[x, y].Equals(value))
                    return new Vector2(x, y);
            }
        }

        //return a negative value if it did not find a matching value
        return new Vector2(-1, -1);
    }
}
