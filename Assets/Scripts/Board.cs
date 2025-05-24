using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    public BoardSquare[,] squares;

    public void CreateBoard(int width, int height, GameObject squarePrefab, Transform parent, Material whiteMaterial, Material blackMaterial)
    {
        squares = new BoardSquare[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                GameObject squareObj = GameObject.Instantiate(squarePrefab, position, Quaternion.identity, parent);

                // Alternar materiales para efecto tablero de ajedrez
                Material mat = ((x + y) % 2 == 0) ? whiteMaterial : blackMaterial;
                squareObj.GetComponentInChildren<Renderer>().material = mat;

                squares[x, y] = new BoardSquare(new Vector2Int(x, y), squareObj, parent, whiteMaterial, blackMaterial);
            }
        }
    }
}

